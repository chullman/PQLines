using System.Collections.Generic;

namespace PQLines.Models
{
    public class ExclContent : IRootModel
    {
        public ExclContent()
        {
            PhaseVoltages = new List<PhaseVoltage>();
        }

        public List<PhaseVoltage> PhaseVoltages { get; set; }
        public string Description { get; set; }
    }

    public class PhaseVoltage
    {
        public string ID { get; set; }
        public string Data { get; set; }
    }
}