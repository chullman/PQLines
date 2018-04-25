using System.Collections.Generic;

namespace PQLines.Models
{
    public class MADContent : IRootModel
    {
        public MADContent()
        {
            Categories = new List<Category>();
        }

        public List<Category> Categories { get; set; }
        public string Description { get; set; }
    }

    public class Category
    {
        public Category()
        {
            MADPhaseVoltages = new List<MADPhaseVoltage>();
        }

        public string ID { get; set; }
        public List<MADPhaseVoltage> MADPhaseVoltages { get; set; }
    }

    public class MADPhaseVoltage
    {
        public string ID { get; set; }
        public string Data { get; set; }
    }
}