using System.Collections.Generic;

namespace PQLines.Models
{
    public class WLLContent : IRootModel
    {
        public WLLContent()
        {
            Items = new List<Item>();
        }

        public List<Item> Items { get; set; }
        public string Description { get; set; }
    }

    public class Item
    {
        public string ID { get; set; }
        public string Data { get; set; }
    }
}