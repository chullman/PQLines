using System;
using System.Collections.Generic;
using System.Linq;
using Acr.UserDialogs;
using PQLines.Models;
using Xamarin.Forms;

namespace PQLines.ViewModels.ExclusionZones
{
    public class ExclusionZonesViewModel : IContentViewModel
    {
        private readonly IList<string> _buttonTextsChildData = new List<string>();
        private readonly IList<string> _buttonTextsChildIDs = new List<string>();
        private readonly int _childIDsCount;
        private readonly ExclContent _deserializedContents;
        
        public ExclusionZonesViewModel(IRootModel deserializedContents)
        {
            _deserializedContents = deserializedContents as ExclContent;

            // Find and grab the relevant "ID" fields from the Models as these will be used for the buttons' text
            // Also find and grab relevant "Data" fields from the Models as these will be used for the popup displays
            if (_deserializedContents != null)
            {
                foreach (var phaseVoltage in _deserializedContents.PhaseVoltages)
                {
                    _buttonTextsChildIDs.Add(phaseVoltage.ID);
                    _buttonTextsChildData.Add(phaseVoltage.Data);
                }

                _childIDsCount = _deserializedContents.PhaseVoltages.Count;
            }
            else
            {
                throw new ArgumentNullException("deserializedContents", String.Format("{0} is referencing null deserialized contents", GetType()));
            }


            Title = "Phase Voltage";
        }

        public string Title { get; set; }

        // Used by the View to know how many buttons to display
        public int GetIDsCount()
        {
            if (_childIDsCount != 0)
            {
                return _childIDsCount;
            }
            throw new NullReferenceException(string.Format("ERROR: The queried data appears to be empty - check the query within View Model class {0}", GetType()));
        }

        // The command that is processed when a popup button is clicked
        public Command AlertDataPopup(int clickedButtonNum)
        {
            // On iOS, the popup box actually looks better if the message is set to the title parameter of AlertAsync, and vice-versa for Android
            return Device.OnPlatform(
                
                iOS: new Command(async () => await UserDialogs.Instance.AlertAsync(
                    _deserializedContents.Description.ToUpper() + " - " + _buttonTextsChildIDs[clickedButtonNum],
                    _buttonTextsChildData[clickedButtonNum],
                    "Close")),
                
                Android: new Command(async () => await UserDialogs.Instance.AlertAsync(
                    _buttonTextsChildData[clickedButtonNum],
                    _deserializedContents.Description.ToUpper() + " - " + _buttonTextsChildIDs[clickedButtonNum],
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