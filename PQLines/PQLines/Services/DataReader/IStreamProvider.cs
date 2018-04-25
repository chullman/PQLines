using System.IO;
using PQLines.Models;

namespace PQLines.Services.DataReader
{
    public interface IStreamProvider
    {
        Stream GetResourceStream<T>(string fileExtension) where T : IRootModel;
    }
}