using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using PQLines.Models;
using PQLines.Services.DataDeserializer;
using PQLines.Services.DataReader;

namespace PQLines.Services.ContentProvider
{
    // Handles the reading and extracting of all our JSON files into their respective model classes
    // IMPORTANT: For this to work, the name of the root level model object (e.g. class CondContent) must match the json file name (i.e. CondContent.json)

    public class JsonResourceContentProvider<T> : IContentProvider<T> where T : IRootModel
    {
        private readonly IStreamProvider _dataStreamHelper;
        private readonly IDeserializer _jsonDeserializer;

        public JsonResourceContentProvider(IDeserializer jsonDeserializer, IStreamProvider dataStreamHelper)
        {
            _jsonDeserializer = jsonDeserializer;
            _dataStreamHelper = dataStreamHelper;
        }

        // Deserialise the JSON into our models by using our EmbeddedStreamProvider service to read the JSON contents
        public T Fetch()
        {
            using (var dataStream = _dataStreamHelper.GetResourceStream<T>("json"))
            using (var readDataStream = new StreamReader(dataStream))
            {
                return _jsonDeserializer.Deserialize<T>(readDataStream.ReadToEnd());
            }
        }

        public T Find(object id)
        {
            throw new NotImplementedException();
        }

        public IList<T> GetAll(Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}