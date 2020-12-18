using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Interfaces.Repository;
using EVendas.Aplication;
using EVendas.Aplication.Filters;
using EVendas.Aplication.Interfaces;
using EVendas.Aplication.ServiceBus;
using EVendas.Aplication.Services;
using Infra.Reposiory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Vendas_AzureServiceBus
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

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EVendas  - Modulo de Estoque WebApi ASP.NET CORE 3.1", Description = "Documentação da API Modulo de Estoque com Utilização de fila Azure ServiceBus", Version = "1.0" });
            });
            #endregion

            #region DbContext
            services.AddDbContext<RepositoryContext>(options => options.UseInMemoryDatabase(databaseName: "ProdutoEstoqueDb"));
            #endregion

            #region Dependency Injection

            
            services.AddScoped<IProdutoService, ProdutoService>();
                       
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<RepositoryContext>();

            services.AddScoped<IServiceBusSender, ServiceBusSender>();
            services.AddScoped<IServiceBusConsumer, ServiceBusConsumer>();

            #endregion

            #region AutoMapper
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Mapping());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ErrorResponseFilter));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Modulo de Estoque - Versão 1.0");
            });

            #endregion

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Bus Queue
            var scope = app.ApplicationServices.CreateScope();
            var service = scope.ServiceProvider.GetService<IServiceBusConsumer>();
            service.RegisterOnMessageHandler_ProdutoVendido();
        }
    }
}
