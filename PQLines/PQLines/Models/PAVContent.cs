using System.Collections.Generic;

namespace PQLines.Models
{
    public class PAVContent : IRootModel
    {
        public PAVContent()
        {
            ApprPhaseVoltages = new List<ApprPhaseVoltage>();
        }

        public List<ApprPhaseVoltage> ApprPhaseVoltages { get; set; }
        public string Description { get; set; }
    }

    public class ApprPhaseVoltage
    {
        public ApprPhaseVoltage()
        {
            ApprTypes = new List<ApprType>();
        }

        public string ID { get; set; }
        public List<ApprType> ApprTypes { get; set; }
    }

    public class ApprType
    {
        public ApprType()
        {
            ObserverStates = new List<ObserverState>();
        }

        public string ID { get; set; }
        public List<ObserverState> ObserverStates { get; set; }
    }

    public class ObserverState
    {
        public string ID { get; set; }
        public string Data { get; set; }
    }
}