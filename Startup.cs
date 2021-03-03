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
            #region ע����� ��ͬ�������ڵķ���
            // ����ע��
            services.AddSingleton<IMySingletonService, MySingletonService>();   // ����
            services.AddScoped<IMyScopedService, MyScopedService>();    // �����򣨷�Χ��
            services.AddTransient<IMyTransientService, MyTransientService>();   // ˲ʱ
            #endregion

            #region ��ʽע��
            services.AddSingleton<IOrderService>(new OrderService());   // ֱ��ע��ʵ��
            //services.AddSingleton<IOrderService, OrderServiceEx>();
            //services.AddSingleton<IOrderService>(serviceProvider =>       // ����ģʽ
            //{
            //    return new OrderServiceEx();
            //});
            // ����ģʽע�������ڵ���\˲ʱ\������
            //services.AddScoped<IOrderService>(serviceProvider =>
            //{
            //    return new OrderService();
            //});
            #endregion

            #region ����ע��
            // ����ӿ�����һ�£�����ע��
            //services.TryAddSingleton<IOrderService, OrderServiceEx>();
            // ����ӿ�����һ�£���ʵ�ֽӿڵ����Ͳ�һ�£������ע��
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IOrderService, OrderServiceEx>());
            #endregion

            #region �Ƴ����滻
            // �滻��һ��ע��ķ���
            //services.Replace(ServiceDescriptor.Singleton<IOrderService, OrderServiceEx>());
            // �Ƴ����и����͵�ע�����
            //services.RemoveAll<IOrderService>();
            #endregion

            #region ע�᷺��ģ��
            services.AddSingleton(typeof(IGenericService<>), typeof(GenericService<>));
            #endregion

            #region ������������ͷ���Ϊ
            // ����ʵ����IDisposableʵ���Ķ��������Ḻ�������������ڽ��й������磺
            //services.AddTransient<IOrderService, DisposableOrderService>();
            //services.AddScoped<IOrderService>(service => new DisposableOrderService());
            // ע���ֶ������Ķ���ʱ�����������ٰ�æ�������ͷŶ������磺
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
