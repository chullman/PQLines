using System;
using System.Collections.Generic;

namespace PQLines.Services.ContentProvider
{
    internal interface IContentProvider<T>
    {
        T Fetch();
        T Find(object id);
        IList<T> GetAll(Func<T, bool> predicate);
    }
}