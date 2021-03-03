using DependencyjectionDemo.IServices;
using DependencyjectionDemo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyjectionDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 注册服务 不同生命周期的服务
            // 泛型注册
            services.AddSingleton<IMySingletonService, MySingletonService>();   // 单例
            services.AddScoped<IMyScopedService, MyScopedService>();    // 作用域（范围）
            services.AddTransient<IMyTransientService, MyTransientService>();   // 瞬时
            #endregion

            #region 花式注册
            services.AddSingleton<IOrderService>(new OrderService());   // 直接注入实例
            //services.AddSingleton<IOrderService, OrderServiceEx>();
            //services.AddSingleton<IOrderService>(serviceProvider =>       // 工厂模式
            //{
            //    return new OrderServiceEx();
            //});
            // 工厂模式注册适用于单例\瞬时\作用域
            //services.AddScoped<IOrderService>(serviceProvider =>
            //{
            //    return new OrderService();
            //});
            #endregion

            #region 尝试注册
            // 如果接口类型一致，则不再注册
            //services.TryAddSingleton<IOrderService, OrderServiceEx>();
            // 如果接口类型一致，但实现接口的类型不一致，则可以注册
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IOrderService, OrderServiceEx>());
            #endregion

            #region 移除和替换
            // 替换第一个注册的服务
            //services.Replace(ServiceDescriptor.Singleton<IOrderService, OrderServiceEx>());
            // 移除所有该类型的注册服务
            //services.RemoveAll<IOrderService>();
            #endregion

            #region 注册泛型模板
            services.AddSingleton(typeof(IGenericService<>), typeof(GenericService<>));
            #endregion

            #region 作用域与对象释放行为
            // 对于实现了IDisposable实例的对象，容器会负责对其的生命周期进行管理，例如：
            //services.AddTransient<IOrderService, DisposableOrderService>();
            //services.AddScoped<IOrderService>(service => new DisposableOrderService());
            // 注册手动创建的对象时，容器将不再帮忙创建与释放对象，例如：
            //var service = new DisposableOrderService();
            //services.AddSingleton<IOrderService>(service);
            #endregion

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
