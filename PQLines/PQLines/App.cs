using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using PQLines.Bootstrapping;
using PQLines.Bootstrapping.Extensions;
using PQLines.Models;
using PQLines.Services.ContentProvider;
using PQLines.Services.PageNavigation;
using PQLines.ViewModels;
using PQLines.ViewModels.ConductorInfo;
using PQLines.ViewModels.ConductorInfo.Conductors;
using PQLines.ViewModels.ConductorInfo.Conductors.Measurements;
using PQLines.ViewModels.ExclusionZones;
using PQLines.ViewModels.LiveLineMinApprDist.Categories;
using PQLines.ViewModels.PlantAndVehicleApprDist;
using PQLines.ViewModels.PlantAndVehicleApprDist.ApprTypes;
using PQLines.ViewModels.PlantAndVehicleApprDist.ApprTypes.ObserverStates;
using PQLines.Views;
using PQLines.Views.ConductorInfo;
using PQLines.Views.ConductorInfo.Conductors;
using PQLines.Views.ConductorInfo.Conductors.Measurements;
using PQLines.Views.ExclusionZones;
using PQLines.Views.LiveLineMinApprDist;
using PQLines.Views.LiveLineMinApprDist.Categories;
using PQLines.Views.LiveLineWorkingLdLmts;
using PQLines.Views.PlantAndVehicleApprDist;
using PQLines.Views.PlantAndVehicleApprDist.ApprTypes;
using PQLines.Views.PlantAndVehicleApprDist.ApprTypes.ObserverStates;
using Xamarin.Forms;

namespace PQLines
{
    // Our application's COMPOSITE ROOT for Xamarin.Forms!
    
    // Note that this app follows an MVVM architecture

    // IMPORTANT: For any given View Model, call any .AddTo methods in the same order that the button's will be displayed 
    // (i.e. most likely the same order as written in the respective JSONs)

    public class App : Application
    {
        private CondContent _condContent;
        private ConductorsViewModel _conductorsVM;
        private ExclContent _exclContent;
        private ExclusionZonesViewModel _exclusionZonesVM;
        private HomePageViewModel _homePageVM;
        private MADContent _madContent;
        private MADViewModel _MADVM;
        private PAVContent _pavContent;
        private PAVViewModel _PAVVM;
        private ApplicationRuntime_Shared _runtime;
        private IViewFactory _viewFactory;
        private IViewModelsICanNavigateTo _VMsICanNavigateTo;
        private WLLContent _wllContent;
        private WLLViewModel _WLLVM;

        // Called first by the device
        public App()
        {
            StartApp();
        }

        private void StartApp()
        {
            // Bootstrap together our Autofac container, including module registrations
            _runtime = DefaultApplication_Shared.Build().Bootstrap();

            _viewFactory = _runtime.Container.Resolve<IViewFactory>();
            _VMsICanNavigateTo = _runtime.Container.Resolve<IViewModelsICanNavigateTo>();

            // Deserialise all our JSONs into their respective Models
            _condContent = _runtime.Container.Resolve<IContentProvider<CondContent>>().Fetch();
            _exclContent = _runtime.Container.Resolve<IContentProvider<ExclContent>>().Fetch();
            _madContent = _runtime.Container.Resolve<IContentProvider<MADContent>>().Fetch();
            _wllContent = _runtime.Container.Resolve<IContentProvider<WLLContent>>().Fetch();
            _pavContent = _runtime.Container.Resolve<IContentProvider<PAVContent>>().Fetch();

            // The Home Page View Model, specifically, requires all of the above deserialised contents (just so we generate the text for the 5 buttons on the home page),
            // so we'll collect these into "allContents" for passing into HomePageViewModel
            var allContents = new List<IRootModel>
            {
                _condContent,
                _exclContent,
                _madContent,
                _wllContent,
                _pavContent
            };

            // Map all of our View Models to their respective Views
            _viewFactory.Map<HomePageViewModel, HomePage>();

            _viewFactory.Map<ConductorsViewModel, ConductorsPage>();
            _viewFactory.Map<ConductorViewModel, ConductorPage>();
            _viewFactory.Map<MeasurementsViewModel, MeasurementsPage>();

            _viewFactory.Map<ExclusionZonesViewModel, ExclusionZonesPage>();

            _viewFactory.Map<MADViewModel, MADPage>();
            _viewFactory.Map<CategoriesViewModel, CategoriesPage>();

            _viewFactory.Map<WLLViewModel, WLLPage>();

            _viewFactory.Map<PAVViewModel, PAVPage>();
            _viewFactory.Map<ApprTypesViewModel, ApprTypesPage>();
            _viewFactory.Map<ObserverStatesViewModel, ObserverStatesPage>();

            // Resolve the Home Page View Model from the Autofac container
            _homePageVM = _runtime.Container.ResolveWithParameters<HomePageViewModel>(new Dictionary<string, object>
            {
                {"deserializedContents", allContents}
            });

            // Asycnhronsouly resolve ALL other View Models required for this appliation.
            // In case this takes a while, display a loading activity indicator until this is complete
            Task.Run(() => LoadAppViewModels());

            // Displays the Home Page
            MainPage = _viewFactory.CreateThemedNavPage(_homePageVM);

        }

        private void LoadAppViewModels()
        {
            // To tell the app to display a loading activity indicator
            _homePageVM.IsBusy = true;

            // Uncomment this to simulate a timed delay to test the loading activity (for TESTING/DEBUGGING purposes only)
            //SIMULATEDELAY(10);

            // Resolve all View Models used by the application
            // This includes setting which View Models all of these View Models are able to navigate to
            ResolveAllConductorInfoViewModels();
            ResolveAllExclusionZonesViewModels();
            ResolveAllMADViewModels();
            ResolveAllWLLViewModels();
            ResolveAllPAVViewModels();

            // for the Home Page View Model, tell it what View Models it is able to navigate to when each button is clicked
            // Remember, these .AddTo methods must be called in the same order that the buttons will be displayed 
            _VMsICanNavigateTo.AddTo<HomePageViewModel>(_conductorsVM);
            _VMsICanNavigateTo.AddTo<HomePageViewModel>(_exclusionZonesVM);
            _VMsICanNavigateTo.AddTo<HomePageViewModel>(_MADVM);
            _VMsICanNavigateTo.AddTo<HomePageViewModel>(_WLLVM);
            _VMsICanNavigateTo.AddTo<HomePageViewModel>(_PAVVM);

            // After we call the .AddTos above, pass this collection into the Home Page View Models
            _homePageVM.SetViewModelsICanNavigateTo(_VMsICanNavigateTo.GetAllFrom<HomePageViewModel>());

            // To tell the app to hide the loading activity indicator on completion of the above
            _homePageVM.IsBusy = false;
        }


        private void ResolveAllConductorInfoViewModels()
        {
            // Resolve ALL of our Conductor Info View Models (1 View Model for every View/page),
            // passing in the deserialised Model data, as well as passing in strings that indicate to each View Model where in the Model structure it's finding the data

            // Due to the one-to-many relationships of the models (e.g. one conductor to many measurements) and because I haven't yet implemented mapping checks on the JSON heirachy (e.g. telling the app that "Neon" belongs only under "AAAC/1120"),
            // we need new instances of ViewModelsICanNavigateTo for each conductor, hence the below new lifetime scope creations (wrapped inside "using" blocks).
            using (var AAAC1120Scope = _runtime.Container.BeginLifetimeScope())
            {
                var neonMeasurementsVM =
                    AAAC1120Scope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"AAAC/1120"},
                        {"level2ID", @"Neon"}
                    });

                var nitrogenMeasurementsVM =
                    AAAC1120Scope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"AAAC/1120"},
                        {"level2ID", @"Nitrogen"}
                    });
                var oxygenMeasurementsVM =
                    AAAC1120Scope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"AAAC/1120"},
                        {"level2ID", @"Oxygen"}
                    });

                var phosphorusMeasurementsVM =
                    AAAC1120Scope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"AAAC/1120"},
                        {"level2ID", @"Phosphorus"}
                    });

                var sulphurMeasurementsVM =
                    AAAC1120Scope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"AAAC/1120"},
                        {"level2ID", @"Sulphur"}
                    });


                var AAAC1120VMsICanNavigateTo = AAAC1120Scope.Resolve<IViewModelsICanNavigateTo>();
                AAAC1120VMsICanNavigateTo.AddTo<ConductorViewModel>(neonMeasurementsVM);
                AAAC1120VMsICanNavigateTo.AddTo<ConductorViewModel>(nitrogenMeasurementsVM);
                AAAC1120VMsICanNavigateTo.AddTo<ConductorViewModel>(oxygenMeasurementsVM);
                AAAC1120VMsICanNavigateTo.AddTo<ConductorViewModel>(phosphorusMeasurementsVM);
                AAAC1120VMsICanNavigateTo.AddTo<ConductorViewModel>(sulphurMeasurementsVM);

                var conductorAAAC1120VM =
                    AAAC1120Scope.ResolveWithParameters<ConductorViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"viewModelsICanNavigateTo", AAAC1120VMsICanNavigateTo.GetAllFrom<ConductorViewModel>()},
                        {"level1ID", @"AAAC/1120"}
                    });
                _VMsICanNavigateTo.AddTo<ConductorsViewModel>(conductorAAAC1120VM);
            }

            using (var AAAC6201Scope = _runtime.Container.BeginLifetimeScope())
            {
                var opalMeasurementsVM =
                    AAAC6201Scope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"AAAC/6201"},
                        {"level2ID", @"Opal"}
                    });

                var pearlMeasurementsVM =
                    AAAC6201Scope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"AAAC/6201"},
                        {"level2ID", @"Pearl"}
                    });

                var AAAC6201VMsICanNavigateTo = AAAC6201Scope.Resolve<IViewModelsICanNavigateTo>();
                AAAC6201VMsICanNavigateTo.AddTo<ConductorViewModel>(opalMeasurementsVM);
                AAAC6201VMsICanNavigateTo.AddTo<ConductorViewModel>(pearlMeasurementsVM);

                var conductorAAAC6201VM =
                    AAAC6201Scope.ResolveWithParameters<ConductorViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"viewModelsICanNavigateTo", AAAC6201VMsICanNavigateTo.GetAllFrom<ConductorViewModel>()},
                        {"level1ID", @"AAAC/6201"}
                    });
                _VMsICanNavigateTo.AddTo<ConductorsViewModel>(conductorAAAC6201VM);
            }


            using (var AAACSCACScope = _runtime.Container.BeginLifetimeScope())
            {
                var opgwbicc12MeasurementsVM =
                    AAACSCACScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"AAAC+SC/AC"},
                        {"level2ID", @"OPGW BICC12"}
                    });

                var opgwbicc24MeasurementsVM =
                    AAACSCACScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"AAAC+SC/AC"},
                        {"level2ID", @"OPGW BICC24"}
                    });


                var opgwsumi24MeasurementsVM =
                    AAACSCACScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"AAAC+SC/AC"},
                        {"level2ID", @"OPGW SUMI24"}
                    });

                var opgwpirelli24MeasurementsVM =
                    AAACSCACScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"AAAC+SC/AC"},
                        {"level2ID", @"OPGW Pirelli24"}
                    });


                var AAACSCACVMsICanNavigateTo = AAACSCACScope.Resolve<IViewModelsICanNavigateTo>();
                AAACSCACVMsICanNavigateTo.AddTo<ConductorViewModel>(opgwbicc12MeasurementsVM);
                AAACSCACVMsICanNavigateTo.AddTo<ConductorViewModel>(opgwbicc24MeasurementsVM);
                AAACSCACVMsICanNavigateTo.AddTo<ConductorViewModel>(opgwsumi24MeasurementsVM);
                AAACSCACVMsICanNavigateTo.AddTo<ConductorViewModel>(opgwpirelli24MeasurementsVM);

                var conductorAAACSCACVM =
                    AAACSCACScope.ResolveWithParameters<ConductorViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"viewModelsICanNavigateTo", AAACSCACVMsICanNavigateTo.GetAllFrom<ConductorViewModel>()},
                        {"level1ID", @"AAAC+SC/AC"}
                    });
                _VMsICanNavigateTo.AddTo<ConductorsViewModel>(conductorAAACSCACVM);
            }


            using (var AACScope = _runtime.Container.BeginLifetimeScope())
            {
                var uranusMeasurementsVM =
                    AACScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"AAC"},
                        {"level2ID", @"Uranus"}
                    });

                var venusMeasurementsVM =
                    AACScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"AAC"},
                        {"level2ID", @"Venus"}
                    });


                var virgoMeasurementsVM =
                    AACScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"AAC"},
                        {"level2ID", @"Virgo"}
                    });


                var AACVMsICanNavigateTo = AACScope.Resolve<IViewModelsICanNavigateTo>();
                AACVMsICanNavigateTo.AddTo<ConductorViewModel>(uranusMeasurementsVM);
                AACVMsICanNavigateTo.AddTo<ConductorViewModel>(venusMeasurementsVM);
                AACVMsICanNavigateTo.AddTo<ConductorViewModel>(virgoMeasurementsVM);

                var conductorAACVM = AACScope.ResolveWithParameters<ConductorViewModel>(new Dictionary<string, object>
                {
                    {"deserializedContents", _condContent},
                    {"viewModelsICanNavigateTo", AACVMsICanNavigateTo.GetAllFrom<ConductorViewModel>()},
                    {"level1ID", @"AAC"}
                });
                _VMsICanNavigateTo.AddTo<ConductorsViewModel>(conductorAACVM);
            }


            using (var AACSRZ1120Scope = _runtime.Container.BeginLifetimeScope())
            {
                var grapeMeasurementsVM =
                    AACSRZ1120Scope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"AACSR/GZ 1120"},
                        {"level2ID", @"Grape"}
                    });

                var mangoMeasurementsVM =
                    AACSRZ1120Scope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"AACSR/GZ 1120"},
                        {"level2ID", @"Mango"}
                    });


                var pawpawMeasurementsVM =
                    AACSRZ1120Scope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"AACSR/GZ 1120"},
                        {"level2ID", @"Paw Paw"}
                    });


                var AACSRZ1120VMsICanNavigateTo = AACSRZ1120Scope.Resolve<IViewModelsICanNavigateTo>();
                AACSRZ1120VMsICanNavigateTo.AddTo<ConductorViewModel>(grapeMeasurementsVM);
                AACSRZ1120VMsICanNavigateTo.AddTo<ConductorViewModel>(mangoMeasurementsVM);
                AACSRZ1120VMsICanNavigateTo.AddTo<ConductorViewModel>(pawpawMeasurementsVM);

                var conductorAACSRZ1120VM =
                    AACSRZ1120Scope.ResolveWithParameters<ConductorViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"viewModelsICanNavigateTo", AACSRZ1120VMsICanNavigateTo.GetAllFrom<ConductorViewModel>()},
                        {"level1ID", @"AACSR/GZ 1120"}
                    });
                _VMsICanNavigateTo.AddTo<ConductorsViewModel>(conductorAACSRZ1120VM);
            }


            using (var ACSRACScope = _runtime.Container.BeginLifetimeScope())
            {
                var divingMeasurementsVM =
                    ACSRACScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"ACSR/AC"},
                        {"level2ID", @"Diving"}
                    });

                var tennisMeasurementsVM =
                    ACSRACScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"ACSR/AC"},
                        {"level2ID", @"Tennis"}
                    });


                var volleyballMeasurementsVM =
                    ACSRACScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"ACSR/AC"},
                        {"level2ID", @"Volleyball"}
                    });


                var ACSRACVMsICanNavigateTo = ACSRACScope.Resolve<IViewModelsICanNavigateTo>();
                ACSRACVMsICanNavigateTo.AddTo<ConductorViewModel>(divingMeasurementsVM);
                ACSRACVMsICanNavigateTo.AddTo<ConductorViewModel>(tennisMeasurementsVM);
                ACSRACVMsICanNavigateTo.AddTo<ConductorViewModel>(volleyballMeasurementsVM);

                var conductorACSRACVM =
                    ACSRACScope.ResolveWithParameters<ConductorViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"viewModelsICanNavigateTo", ACSRACVMsICanNavigateTo.GetAllFrom<ConductorViewModel>()},
                        {"level1ID", @"ACSR/AC"}
                    });
                _VMsICanNavigateTo.AddTo<ConductorsViewModel>(conductorACSRACVM);
            }


            using (var ACSRGZScope = _runtime.Container.BeginLifetimeScope())
            {
                var grapeMeasurementsVM =
                    ACSRGZScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"ACSR/GZ"},
                        {"level2ID", @"Grape"}
                    });

                var lemonMeasurementsVM =
                    ACSRGZScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"ACSR/GZ"},
                        {"level2ID", @"Lemon"}
                    });


                var limeMeasurementsVM =
                    ACSRGZScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"ACSR/GZ"},
                        {"level2ID", @"Lime"}
                    });

                var mangoMeasurementsVM =
                    ACSRGZScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"ACSR/GZ"},
                        {"level2ID", @"Mango"}
                    });

                var orangeMeasurementsVM =
                    ACSRGZScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"ACSR/GZ"},
                        {"level2ID", @"Orange"}
                    });

                var pawpawMeasurementsVM =
                    ACSRGZScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"ACSR/GZ"},
                        {"level2ID", @"Paw Paw"}
                    });


                var ACSRGZVMsICanNavigateTo = ACSRGZScope.Resolve<IViewModelsICanNavigateTo>();
                ACSRGZVMsICanNavigateTo.AddTo<ConductorViewModel>(grapeMeasurementsVM);
                ACSRGZVMsICanNavigateTo.AddTo<ConductorViewModel>(lemonMeasurementsVM);
                ACSRGZVMsICanNavigateTo.AddTo<ConductorViewModel>(limeMeasurementsVM);
                ACSRGZVMsICanNavigateTo.AddTo<ConductorViewModel>(mangoMeasurementsVM);
                ACSRGZVMsICanNavigateTo.AddTo<ConductorViewModel>(orangeMeasurementsVM);
                ACSRGZVMsICanNavigateTo.AddTo<ConductorViewModel>(pawpawMeasurementsVM);

                var conductorACSRGZVM =
                    ACSRGZScope.ResolveWithParameters<ConductorViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"viewModelsICanNavigateTo", ACSRGZVMsICanNavigateTo.GetAllFrom<ConductorViewModel>()},
                        {"level1ID", @"ACSR/GZ"}
                    });
                _VMsICanNavigateTo.AddTo<ConductorsViewModel>(conductorACSRGZVM);
            }

            using (var ACSRIScope = _runtime.Container.BeginLifetimeScope())
            {
                var bearMeasurementsVM =
                    ACSRIScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"ACSR/I"},
                        {"level2ID", @"Bear"}
                    });

                var camelMeasurementsVM =
                    ACSRIScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"ACSR/I"},
                        {"level2ID", @"Camel"}
                    });


                var dogMeasurementsVM =
                    ACSRIScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"ACSR/I"},
                        {"level2ID", @"Dog"}
                    });

                var goatMeasurementsVM =
                    ACSRIScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"ACSR/I"},
                        {"level2ID", @"Goat"}
                    });

                var martinMeasurementsVM =
                    ACSRIScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"ACSR/I"},
                        {"level2ID", @"Martin"}
                    });

                var pantherMeasurementsVM =
                    ACSRIScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"ACSR/I"},
                        {"level2ID", @"Panther"}
                    });

                var tigerMeasurementsVM =
                    ACSRIScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"ACSR/I"},
                        {"level2ID", @"Tiger"}
                    });

                var wolfMeasurementsVM =
                    ACSRIScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"ACSR/I"},
                        {"level2ID", @"Wolf"}
                    });


                var ACSRIVMsICanNavigateTo = ACSRIScope.Resolve<IViewModelsICanNavigateTo>();
                ACSRIVMsICanNavigateTo.AddTo<ConductorViewModel>(bearMeasurementsVM);
                ACSRIVMsICanNavigateTo.AddTo<ConductorViewModel>(camelMeasurementsVM);
                ACSRIVMsICanNavigateTo.AddTo<ConductorViewModel>(dogMeasurementsVM);
                ACSRIVMsICanNavigateTo.AddTo<ConductorViewModel>(goatMeasurementsVM);
                ACSRIVMsICanNavigateTo.AddTo<ConductorViewModel>(martinMeasurementsVM);
                ACSRIVMsICanNavigateTo.AddTo<ConductorViewModel>(pantherMeasurementsVM);
                ACSRIVMsICanNavigateTo.AddTo<ConductorViewModel>(tigerMeasurementsVM);
                ACSRIVMsICanNavigateTo.AddTo<ConductorViewModel>(wolfMeasurementsVM);

                var conductorACSRIVM =
                    ACSRIScope.ResolveWithParameters<ConductorViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"viewModelsICanNavigateTo", ACSRIVMsICanNavigateTo.GetAllFrom<ConductorViewModel>()},
                        {"level1ID", @"ACSR/I"}
                    });
                _VMsICanNavigateTo.AddTo<ConductorsViewModel>(conductorACSRIVM);
            }


            using (var SCACScope = _runtime.Container.BeginLifetimeScope())
            {
                var scac7325MeasurementsVM =
                    SCACScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"SC/AC"},
                        {"level2ID", @"7/3.25"}
                    });

                var scac7375MeasurementsVM =
                    SCACScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"SC/AC"},
                        {"level2ID", @"7/3.75"}
                    });

                var alumoweldMeasurementsVM =
                    SCACScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"SC/AC"},
                        {"level2ID", @"Alumoweld"}
                    });


                var SCACVMsICanNavigateTo = SCACScope.Resolve<IViewModelsICanNavigateTo>();
                SCACVMsICanNavigateTo.AddTo<ConductorViewModel>(scac7325MeasurementsVM);
                SCACVMsICanNavigateTo.AddTo<ConductorViewModel>(scac7375MeasurementsVM);
                SCACVMsICanNavigateTo.AddTo<ConductorViewModel>(alumoweldMeasurementsVM);


                var conductorSCACVM = SCACScope.ResolveWithParameters<ConductorViewModel>(new Dictionary<string, object>
                {
                    {"deserializedContents", _condContent},
                    {"viewModelsICanNavigateTo", SCACVMsICanNavigateTo.GetAllFrom<ConductorViewModel>()},
                    {"level1ID", @"SC/AC"}
                });
                _VMsICanNavigateTo.AddTo<ConductorsViewModel>(conductorSCACVM);
            }

            using (var SCGZScope = _runtime.Container.BeginLifetimeScope())
            {
                var scgz19200MeasurementsVM =
                    SCGZScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"SC/GZ"},
                        {"level2ID", @"19/2.00"}
                    });


                var SCGZVMsICanNavigateTo = SCGZScope.Resolve<IViewModelsICanNavigateTo>();
                SCGZVMsICanNavigateTo.AddTo<ConductorViewModel>(scgz19200MeasurementsVM);


                var conductorSCGZVM = SCGZScope.ResolveWithParameters<ConductorViewModel>(new Dictionary<string, object>
                {
                    {"deserializedContents", _condContent},
                    {"viewModelsICanNavigateTo", SCGZVMsICanNavigateTo.GetAllFrom<ConductorViewModel>()},
                    {"level1ID", @"SC/GZ"}
                });
                _VMsICanNavigateTo.AddTo<ConductorsViewModel>(conductorSCGZVM);
            }

            using (var SCGZIScope = _runtime.Container.BeginLifetimeScope())
            {
                var scgz19200MeasurementsVM =
                    SCGZIScope.ResolveWithParameters<MeasurementsViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"level1ID", @"SC/GZ/I"},
                        {"level2ID", @"19/2.03"}
                    });


                var SCGZIVMsICanNavigateTo = SCGZIScope.Resolve<IViewModelsICanNavigateTo>();
                SCGZIVMsICanNavigateTo.AddTo<ConductorViewModel>(scgz19200MeasurementsVM);


                var conductorSCGZIVM =
                    SCGZIScope.ResolveWithParameters<ConductorViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _condContent},
                        {"viewModelsICanNavigateTo", SCGZIVMsICanNavigateTo.GetAllFrom<ConductorViewModel>()},
                        {"level1ID", @"SC/GZ/I"}
                    });
                _VMsICanNavigateTo.AddTo<ConductorsViewModel>(conductorSCGZIVM);
            }


            _conductorsVM = _runtime.Container.ResolveWithParameters<ConductorsViewModel>(new Dictionary<string, object>
            {
                {"deserializedContents", _condContent},
                {"viewModelsICanNavigateTo", _VMsICanNavigateTo.GetAllFrom<ConductorsViewModel>()}
            });

        }

        private void ResolveAllExclusionZonesViewModels()
        {
            _exclusionZonesVM =
                _runtime.Container.ResolveWithParameters<ExclusionZonesViewModel>(new Dictionary<string, object>
                {
                    {"deserializedContents", _exclContent}
                });

        }

        private void ResolveAllMADViewModels()
        {
            var BarehandCategoryVM =
                _runtime.Container.ResolveWithParameters<CategoriesViewModel>(new Dictionary<string, object>
                {
                    {"deserializedContents", _madContent},
                    {"level1ID", @"Barehand"}
                });

            var HotStickCategoryVM =
                _runtime.Container.ResolveWithParameters<CategoriesViewModel>(new Dictionary<string, object>
                {
                    {"deserializedContents", _madContent},
                    {"level1ID", @"Hot Stick"}
                });

            var PhaseToPhaseCategoryVM =
                _runtime.Container.ResolveWithParameters<CategoriesViewModel>(new Dictionary<string, object>
                {
                    {"deserializedContents", _madContent},
                    {"level1ID", @"Phase to Phase"}
                });

            _VMsICanNavigateTo.AddTo<MADViewModel>(BarehandCategoryVM);
            _VMsICanNavigateTo.AddTo<MADViewModel>(HotStickCategoryVM);
            _VMsICanNavigateTo.AddTo<MADViewModel>(PhaseToPhaseCategoryVM);

            _MADVM = _runtime.Container.ResolveWithParameters<MADViewModel>(new Dictionary<string, object>
            {
                {"deserializedContents", _madContent},
                {"viewModelsICanNavigateTo", _VMsICanNavigateTo.GetAllFrom<MADViewModel>()}
            });
        }

        private void ResolveAllWLLViewModels()
        {
            _WLLVM = _runtime.Container.ResolveWithParameters<WLLViewModel>(new Dictionary<string, object>
            {
                {"deserializedContents", _wllContent}
            });
        }

        private void ResolveAllPAVViewModels()
        {
            using (var the1KVScope = _runtime.Container.BeginLifetimeScope())
            {
                var the1KVPlantPhaseVoltageVM =
                    the1KVScope.ResolveWithParameters<ObserverStatesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"level1ID", @"1kV"},
                        {"level2ID", @"Plant"}
                    });

                var the1KVVehiclePhaseVoltageVM =
                    the1KVScope.ResolveWithParameters<ObserverStatesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"level1ID", @"1kV"},
                        {"level2ID", @"Vehicle"}
                    });


                var the1KVVMsICanNavigateTo = the1KVScope.Resolve<IViewModelsICanNavigateTo>();
                the1KVVMsICanNavigateTo.AddTo<ApprTypesViewModel>(the1KVPlantPhaseVoltageVM);
                the1KVVMsICanNavigateTo.AddTo<ApprTypesViewModel>(the1KVVehiclePhaseVoltageVM);


                var the1KVPhaseVoltageVM =
                    the1KVScope.ResolveWithParameters<ApprTypesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"viewModelsICanNavigateTo", the1KVVMsICanNavigateTo.GetAllFrom<ApprTypesViewModel>()},
                        {"level1ID", @"1kV"}
                    });
                _VMsICanNavigateTo.AddTo<PAVViewModel>(the1KVPhaseVoltageVM);
            }


            using (var the33KVScope = _runtime.Container.BeginLifetimeScope())
            {
                var the33KVPlantPhaseVoltageVM =
                    the33KVScope.ResolveWithParameters<ObserverStatesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"level1ID", @"33kV"},
                        {"level2ID", @"Plant"}
                    });

                var the33KVVehiclePhaseVoltageVM =
                    the33KVScope.ResolveWithParameters<ObserverStatesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"level1ID", @"33kV"},
                        {"level2ID", @"Vehicle"}
                    });


                var the33KVVMsICanNavigateTo = the33KVScope.Resolve<IViewModelsICanNavigateTo>();
                the33KVVMsICanNavigateTo.AddTo<ApprTypesViewModel>(the33KVPlantPhaseVoltageVM);
                the33KVVMsICanNavigateTo.AddTo<ApprTypesViewModel>(the33KVVehiclePhaseVoltageVM);


                var the33KVPhaseVoltageVM =
                    the33KVScope.ResolveWithParameters<ApprTypesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"viewModelsICanNavigateTo", the33KVVMsICanNavigateTo.GetAllFrom<ApprTypesViewModel>()},
                        {"level1ID", @"33kV"}
                    });
                _VMsICanNavigateTo.AddTo<PAVViewModel>(the33KVPhaseVoltageVM);
            }

            using (var the66KVScope = _runtime.Container.BeginLifetimeScope())
            {
                var the66KVPlantPhaseVoltageVM =
                    the66KVScope.ResolveWithParameters<ObserverStatesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"level1ID", @"66kV"},
                        {"level2ID", @"Plant"}
                    });

                var the66KVVehiclePhaseVoltageVM =
                    the66KVScope.ResolveWithParameters<ObserverStatesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"level1ID", @"66kV"},
                        {"level2ID", @"Vehicle"}
                    });


                var the66KVVMsICanNavigateTo = the66KVScope.Resolve<IViewModelsICanNavigateTo>();
                the66KVVMsICanNavigateTo.AddTo<ApprTypesViewModel>(the66KVPlantPhaseVoltageVM);
                the66KVVMsICanNavigateTo.AddTo<ApprTypesViewModel>(the66KVVehiclePhaseVoltageVM);


                var the66KVPhaseVoltageVM =
                    the66KVScope.ResolveWithParameters<ApprTypesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"viewModelsICanNavigateTo", the66KVVMsICanNavigateTo.GetAllFrom<ApprTypesViewModel>()},
                        {"level1ID", @"66kV"}
                    });
                _VMsICanNavigateTo.AddTo<PAVViewModel>(the66KVPhaseVoltageVM);
            }


            using (var the110KVScope = _runtime.Container.BeginLifetimeScope())
            {
                var the110KVPlantPhaseVoltageVM =
                    the110KVScope.ResolveWithParameters<ObserverStatesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"level1ID", @"110kV"},
                        {"level2ID", @"Plant"}
                    });

                var the110KVVehiclePhaseVoltageVM =
                    the110KVScope.ResolveWithParameters<ObserverStatesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"level1ID", @"110kV"},
                        {"level2ID", @"Vehicle"}
                    });


                var the110KVVMsICanNavigateTo = the110KVScope.Resolve<IViewModelsICanNavigateTo>();
                the110KVVMsICanNavigateTo.AddTo<ApprTypesViewModel>(the110KVPlantPhaseVoltageVM);
                the110KVVMsICanNavigateTo.AddTo<ApprTypesViewModel>(the110KVVehiclePhaseVoltageVM);


                var the110KVPhaseVoltageVM =
                    the110KVScope.ResolveWithParameters<ApprTypesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"viewModelsICanNavigateTo", the110KVVMsICanNavigateTo.GetAllFrom<ApprTypesViewModel>()},
                        {"level1ID", @"110kV"}
                    });
                _VMsICanNavigateTo.AddTo<PAVViewModel>(the110KVPhaseVoltageVM);
            }


            using (var the132KVScope = _runtime.Container.BeginLifetimeScope())
            {
                var the132KVPlantPhaseVoltageVM =
                    the132KVScope.ResolveWithParameters<ObserverStatesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"level1ID", @"132kV"},
                        {"level2ID", @"Plant"}
                    });

                var the132KVVehiclePhaseVoltageVM =
                    the132KVScope.ResolveWithParameters<ObserverStatesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"level1ID", @"132kV"},
                        {"level2ID", @"Vehicle"}
                    });


                var the132KVVMsICanNavigateTo = the132KVScope.Resolve<IViewModelsICanNavigateTo>();
                the132KVVMsICanNavigateTo.AddTo<ApprTypesViewModel>(the132KVPlantPhaseVoltageVM);
                the132KVVMsICanNavigateTo.AddTo<ApprTypesViewModel>(the132KVVehiclePhaseVoltageVM);


                var the132KVPhaseVoltageVM =
                    the132KVScope.ResolveWithParameters<ApprTypesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"viewModelsICanNavigateTo", the132KVVMsICanNavigateTo.GetAllFrom<ApprTypesViewModel>()},
                        {"level1ID", @"132kV"}
                    });
                _VMsICanNavigateTo.AddTo<PAVViewModel>(the132KVPhaseVoltageVM);
            }

            using (var the275KVScope = _runtime.Container.BeginLifetimeScope())
            {
                var the275KVPlantPhaseVoltageVM =
                    the275KVScope.ResolveWithParameters<ObserverStatesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"level1ID", @"275kV"},
                        {"level2ID", @"Plant"}
                    });

                var the275KVVehiclePhaseVoltageVM =
                    the275KVScope.ResolveWithParameters<ObserverStatesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"level1ID", @"275kV"},
                        {"level2ID", @"Vehicle"}
                    });


                var the275KVVMsICanNavigateTo = the275KVScope.Resolve<IViewModelsICanNavigateTo>();
                the275KVVMsICanNavigateTo.AddTo<ApprTypesViewModel>(the275KVPlantPhaseVoltageVM);
                the275KVVMsICanNavigateTo.AddTo<ApprTypesViewModel>(the275KVVehiclePhaseVoltageVM);


                var the275KVPhaseVoltageVM =
                    the275KVScope.ResolveWithParameters<ApprTypesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"viewModelsICanNavigateTo", the275KVVMsICanNavigateTo.GetAllFrom<ApprTypesViewModel>()},
                        {"level1ID", @"275kV"}
                    });
                _VMsICanNavigateTo.AddTo<PAVViewModel>(the275KVPhaseVoltageVM);
            }

            using (var the330KVScope = _runtime.Container.BeginLifetimeScope())
            {
                var the330KVPlantPhaseVoltageVM =
                    the330KVScope.ResolveWithParameters<ObserverStatesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"level1ID", @"330kV"},
                        {"level2ID", @"Plant"}
                    });

                var the330KVVehiclePhaseVoltageVM =
                    the330KVScope.ResolveWithParameters<ObserverStatesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"level1ID", @"330kV"},
                        {"level2ID", @"Vehicle"}
                    });


                var the330KVVMsICanNavigateTo = the330KVScope.Resolve<IViewModelsICanNavigateTo>();
                the330KVVMsICanNavigateTo.AddTo<ApprTypesViewModel>(the330KVPlantPhaseVoltageVM);
                the330KVVMsICanNavigateTo.AddTo<ApprTypesViewModel>(the330KVVehiclePhaseVoltageVM);


                var the330KVPhaseVoltageVM =
                    the330KVScope.ResolveWithParameters<ApprTypesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"viewModelsICanNavigateTo", the330KVVMsICanNavigateTo.GetAllFrom<ApprTypesViewModel>()},
                        {"level1ID", @"330kV"}
                    });
                _VMsICanNavigateTo.AddTo<PAVViewModel>(the330KVPhaseVoltageVM);
            }


            using (var theAbove330KVScope = _runtime.Container.BeginLifetimeScope())
            {
                var theAbove330KVPlantPhaseVoltageVM =
                    theAbove330KVScope.ResolveWithParameters<ObserverStatesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"level1ID", @"Above 330kV"},
                        {"level2ID", @"Plant"}
                    });

                var theAbove330KVVehiclePhaseVoltageVM =
                    theAbove330KVScope.ResolveWithParameters<ObserverStatesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"level1ID", @"Above 330kV"},
                        {"level2ID", @"Vehicle"}
                    });


                var theAbove330KVVMsICanNavigateTo = theAbove330KVScope.Resolve<IViewModelsICanNavigateTo>();
                theAbove330KVVMsICanNavigateTo.AddTo<ApprTypesViewModel>(theAbove330KVPlantPhaseVoltageVM);
                theAbove330KVVMsICanNavigateTo.AddTo<ApprTypesViewModel>(theAbove330KVVehiclePhaseVoltageVM);


                var theAbove330KVPhaseVoltageVM =
                    theAbove330KVScope.ResolveWithParameters<ApprTypesViewModel>(new Dictionary<string, object>
                    {
                        {"deserializedContents", _pavContent},
                        {"viewModelsICanNavigateTo", theAbove330KVVMsICanNavigateTo.GetAllFrom<ApprTypesViewModel>()},
                        {"level1ID", @"Above 330kV"}
                    });
                _VMsICanNavigateTo.AddTo<PAVViewModel>(theAbove330KVPhaseVoltageVM);
            }


            _PAVVM = _runtime.Container.ResolveWithParameters<PAVViewModel>(new Dictionary<string, object>
            {
                {"deserializedContents", _pavContent},
                {"viewModelsICanNavigateTo", _VMsICanNavigateTo.GetAllFrom<PAVViewModel>()}
            });
        }

        // For TESTING/DEBUGGING purposes - to simulate a timed delay
        private void SIMULATEDELAY(int seconds)
        {
            var startDt = DateTime.Now;

            while (true)
            {
                if ((DateTime.Now - startDt).TotalSeconds >= seconds)
                    break;
            }
        }

    }
}