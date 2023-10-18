using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameRenderer.Engine
{
    internal class Engine : IEngine
    {
        public IEngine AddSystem<TSystem>(TSystem system) where TSystem : Delegate
        {
            return this;
        }
    }
}
