using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autenticacao
{

    public static class GlobalClass {

        public static List<Login> User = new List<Login>();

    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Login us = new Login();
            us.id = 1;
            us.user = "santiago@teste.com";
            us.senha = "123";
            us.useAutenticacao = "N";

            Login us2 = new Login();
            us2.id = 2;
            us2.user = "pereira@teste.com";
            us2.senha = "123";
            us2.useAutenticacao = "N";

            GlobalClass.User.Add(us);
            GlobalClass.User.Add(us2);

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();



             app.UseEndpoints(endpoints =>
             {
                 endpoints.MapControllers();

             });
           
        }
    }
}
