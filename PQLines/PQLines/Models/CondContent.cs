using System.Collections.Generic;

namespace PQLines.Models
{
    public class CondContent : IRootModel
    {
        public CondContent()
        {
            Conductors = new List<Conductor>();
        }

        public List<Conductor> Conductors { get; set; }
        public string Description { get; set; }
    }

    public class Conductor
    {
        public Conductor()
        {
            Names = new List<ConductorName>();
        }

        public string ID { get; set; }
        public List<ConductorName> Names { get; set; }
    }

    public class ConductorName
    {
        public ConductorName()
        {
            Measurements = new List<Measurement>();
        }

        public string ID { get; set; }
        public List<Measurement> Measurements { get; set; }
    }

    public class Measurement
    {
        public string ID { get; set; }
        public string Data { get; set; }
    }
}