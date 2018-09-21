using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.InvokeRoute
{
    public interface IDataProvider
    {
        object GetBody(int index, Type type);
    }
}
