using System;
using System.Diagnostics;
using Newtonsoft.Json;
using PQLines.Models;

namespace PQLines.Services.DataDeserializer
{
    // Newtonsoft.Json deserializer

    public class JsonDeserializer : IDeserializer
    {
        public T Deserialize<T>(string data) where T : IRootModel
        {
            T root;
            try
            {
                root = JsonConvert.DeserializeObject<T>(data);
            }
            catch (Exception)
            {
                Debug.WriteLine("ERROR: Unable to deserialise JSON - check validity of the JSON source and the Models' structure");
                throw;
            }

            return root;
        }
    }
}