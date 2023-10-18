using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameRenderer.Engine
{
    internal interface IEngine
    {
        IEngine AddSystem<TSystem>(TSystem system) where TSystem : Delegate;
    }
}
