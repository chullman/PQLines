﻿using PQLines.ViewModels;
using PQLines.ViewModels.ConductorInfo.Conductors;
using Xamarin.Forms;

namespace PQLines.Views.ConductorInfo.Conductors
{
    public partial class ConductorPage : ThemedContentPage//, IPageContentBuilder
    {
        public ConductorPage(IContentViewModel vmContext)
        {
            InitializeComponent();

            BindingContext = vmContext;

            XamlBindedScrollView.Content = CreateGridOfButtons(vmContext as ConductorViewModel);
        }

        // We want to dynamically generate the buttons and their text based on the number of "IDs" gathered in the View Model
        // Unfortunately, XAML doesn't support looping statements of any kind, so I had to move the parent XAML code into this code-behind class
        // This feels messy, but I'm sure there's a better way by creating a custom control?
        private Grid CreateGridOfButtons(ConductorViewModel vmContext)
        {
            var tileGrid = new Grid
            {
                Padding = ButtonGridPadding,
                VerticalOptions = ButtonGridVertOptions,
                HorizontalOptions = ButtonGridHorzOptions,
                RowSpacing = ButtonGridRowSpacing,
                ColumnSpacing = ButtonGridColumnSpacing,
                RowDefinitions = CreateRowDefinitions(vmContext),
                ColumnDefinitions = CreateColumnDefinitions(vmContext)
            };

            for (var i = 0; i < vmContext.GetIDsCount(); i++)
            {
                tileGrid.Children.Add(new ThemedNavigationButton
                {
                    Text = vmContext.GetParentIDText(i),
                    Command = vmContext.NavigateToViewModel(i),
                    BackgroundColor = ThemedNavigationButton.ButtonBackgroundColor,
                    TextColor = ThemedNavigationButton.ButtonTextColor,
                    FontSize = ThemedNavigationButton.ButtonFontSize,
                    FontAttributes = ThemedNavigationButton.ButtonFontAttributes
                }, 0, i);
            }

            return tileGrid;
        }

        private RowDefinitionCollection CreateRowDefinitions(ConductorViewModel vmContext)
        {
            var rowDefinitionCollection = new RowDefinitionCollection();

            for (var i = 0; i < vmContext.GetIDsCount(); i++)
            {
                rowDefinitionCollection.Add(new RowDefinition {Height = ButtonGridRowDefHeight});
            }

            return rowDefinitionCollection;
        }

        private ColumnDefinitionCollection CreateColumnDefinitions(ConductorViewModel vmContext)
        {
            var columnDefinitionCollection = new ColumnDefinitionCollection();

            columnDefinitionCollection.Add(new ColumnDefinition {Width = ButtonGridColumnDefWidth});

            return columnDefinitionCollection;
        }
    }
}