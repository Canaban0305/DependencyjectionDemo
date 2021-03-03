using DependencyjectionDemo.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyjectionDemo.Services
{
    public class GenericService<T>: IGenericService<T>
    {
        public T Data { get; private set; }
        public GenericService(T data)
        {
            this.Data = data;
        }
    }
}
