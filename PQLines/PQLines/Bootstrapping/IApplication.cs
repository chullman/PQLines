using System;

namespace PQLines.Bootstrapping
{
    public interface IApplication<out T> where T : IDisposable
    {
        T Bootstrap();
    }
}