using System;
using System.Collections.Generic;
using System.Linq;
using Acr.UserDialogs;
using PQLines.Models;
using Xamarin.Forms;

namespace PQLines.ViewModels.PlantAndVehicleApprDist.ApprTypes.ObserverStates
{
    public class ObserverStatesViewModel : IContentViewModel
    {
        private readonly IList<string> _buttonTextsChildData = new List<string>();
        private readonly IList<string> _buttonTextsChildIDs = new List<string>();
        private readonly int _childIDsCount;
        private readonly string _level1ID;
        private readonly string _level2ID;
        
        public ObserverStatesViewModel(IRootModel deserializedContents, string level1ID, string level2ID)
        {
            var deserializedContents1 = deserializedContents as PAVContent;
            _level1ID = level1ID;
            _level2ID = level2ID;

            // Find and grab the relevant "ID" fields from the Models as these will be used for the buttons' text
            // Also find and grab relevant "Data" fields from the Models as these will be used for the popup displays
            if (deserializedContents1 != null)
            {
                var measurements = (from a in deserializedContents1.ApprPhaseVoltages
                    where string.Compare(a.ID, _level1ID, StringComparison.CurrentCultureIgnoreCase) == 0
                    from b in a.ApprTypes
                    where string.Compare(b.ID, _level2ID, StringComparison.CurrentCultureIgnoreCase) == 0
                    from c in b.ObserverStates
                    select c).ToList();

                foreach (var measurement in measurements)
                {
                    _buttonTextsChildIDs.Add(measurement.ID);
                    _buttonTextsChildData.Add(measurement.Data);
                }

                _childIDsCount = measurements.Count;
            }
            else
            {
                throw new ArgumentNullException("deserializedContents", String.Format("{0} is referencing null deserialized contents", GetType()));
            }


            Title = _level2ID;

            QuestionText = String.Format("Are you a {0} {1} observer?", _level1ID, _level2ID.ToLower());
        }

        public string Title { get; set; }
        public string QuestionText { get; set; }

        // Used by the View to know how many buttons to display
        public int GetIDsCount()
        {
            if (_childIDsCount != 0)
            {
                return _childIDsCount;
            }
            throw new NullReferenceException(string.Format("ERROR: The queried data appears to be empty - check the query within View Model class {0}", GetType()));
        }

        // The command that is processed when a popups button is clicked
        public Command AlertDataPopup(int clickedButtonNum)
        {
            // On iOS, the popup box actually looks better if the message is set to the title parameter of AlertAsync, and vice-versa for Android
            return Device.OnPlatform(
                
                iOS: new Command(async () => await UserDialogs.Instance.AlertAsync(
                _level2ID.ToUpper() + " - " + _buttonTextsChildIDs[clickedButtonNum],
                _buttonTextsChildData[clickedButtonNum],
                "Close")),
                
                Android: new Command(async () => await UserDialogs.Instance.AlertAsync(
                    _buttonTextsChildData[clickedButtonNum],
                    _level2ID.ToUpper() + " - " + _buttonTextsChildIDs[clickedButtonNum],
                    "Close")),
                    
                WinPhone: new Command(() => { throw new NotImplementedException(); })
                );
        }

        public string GetChildIDText(int buttonNum)
        {
            if (_buttonTextsChildIDs.Any())
            {
                return _buttonTextsChildIDs[buttonNum];
            }
            throw new NullReferenceException(string.Format("ERROR: No relevant 'ID' strings were found - check the query within View Model class {0}", GetType()));
        }
    }
}