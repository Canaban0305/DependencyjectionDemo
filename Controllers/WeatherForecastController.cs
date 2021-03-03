using DependencyjectionDemo.IServices;
using DependencyjectionDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyjectionDemo.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private IOrderService _orderService;
        private IGenericService<IMySingletonService> _genericService;
        //private IGenericService<IOrderService> _genericService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IOrderService orderService, IGenericService<IMySingletonService> genericService)
        {
            _logger = logger;
            _genericService = genericService;
            _orderService = orderService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        public int GetService([FromServices]IMySingletonService singleton1, 
                                [FromServices]IMySingletonService singleton2,
                                [FromServices]IMyScopedService scoped1,
                                [FromServices]IMyScopedService scoped2,
                                [FromServices]IMyTransientService transient1,
                                [FromServices]IMyTransientService transient2)
        {
            // 打印哈希
            Console.WriteLine($"singleton1:{singleton1.GetHashCode()}");
            Console.WriteLine($"singleton2:{singleton2.GetHashCode()}");

            Console.WriteLine($"scoped1:{scoped1.GetHashCode()}");
            Console.WriteLine($"scoped2:{scoped2.GetHashCode()}");

            Console.WriteLine($"transient1:{transient1.GetHashCode()}");
            Console.WriteLine($"transient2:{transient2.GetHashCode()}");

            Console.WriteLine("==请求结束==");

            return 1;
        }

        [HttpGet]
        public int GetServiceList([FromServices]IEnumerable<IOrderService> orderServices)
        {
            foreach (var item in orderServices)
            {
                Console.WriteLine($"获取到的服务实例：{item.ToString()}:{item.GetHashCode()}");
            }

            Console.WriteLine("==请求结束==");
            return 1;
        }

        [HttpGet]
        public int Disposable([FromServices]IOrderService orderService1,
                                [FromServices]IOrderService orderService2)
        {
            Console.WriteLine($"orderService1:{orderService1.GetHashCode()}");
            Console.WriteLine($"orderService2:{orderService2.GetHashCode()}");
            #region 
            Console.WriteLine("======1=======");
            using (IServiceScope scope = HttpContext.RequestServices.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<IOrderService>();
                var service2 = scope.ServiceProvider.GetService<IOrderService>();
            }
            Console.WriteLine("======2=======");
            #endregion
            Console.WriteLine("==接口请求结束==");
            return 1;
        }
    }
}
