using DependencyjectionDemo.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyjectionDemo.Services
{
    public class DisposableOrderService : IOrderService, IDisposable
    {
        public void Dispose()
        {
            Console.WriteLine($"DisposableOrderService Disposable:{this.GetHashCode()}");
        }
    }
}
