using PQLines.Models;

namespace PQLines.Services.DataDeserializer
{
    public interface IDeserializer
    {
        T Deserialize<T>(string data) where T : IRootModel;
    }
}