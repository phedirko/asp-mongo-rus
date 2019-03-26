using AspMongo.Handler;
using AspMongo.Services.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspMongo
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //Регистрация репозиториев в контейнер зависимостей
            services.AddScoped<UserRepository>();
            services.AddScoped<NoteRepository>();


            //Регистрация обрабротчика авторизации
            services
                .AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, SimpleAuthHandler>("BasicAuthentication", null);


            // Создание индексов в коллекции User
            services
                .BuildServiceProvider()
                .GetRequiredService<UserRepository>()
                .CreateIndexes();

            // Создание индексов в коллекции Note
            services
                .BuildServiceProvider()
                .GetRequiredService<NoteRepository>()
                .CreateIndexes();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Добавление аутентификации в конвеер обработки запроса
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
