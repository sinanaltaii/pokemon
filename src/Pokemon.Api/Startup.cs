﻿using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Pokemon.Api.Mapper;
using Pokemon.Core.Services;
using Pokemon.Infrastructure.Repositories;
using System.IO;

namespace Pokemon.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IWebHostEnvironment HostingEnvironment { get;}
        public IConfiguration Configuration { get; }
        private SqliteConnection inMemorySqlite;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            //Sql lite in-memory DB
            inMemorySqlite = new SqliteConnection("Data Source=:memory:");
            inMemorySqlite.Open();
            services.AddDbContext<Pokemon.Infrastructure.Data.AppDbContext>(options => {
                options.UseSqlite(inMemorySqlite);
            });

            DbContextOptions<Pokemon.Infrastructure.Data.AppDbContext> opts = new DbContextOptionsBuilder<Pokemon.Infrastructure.Data.AppDbContext>()
                    .UseSqlite(inMemorySqlite)
                    .Options;

            //TODO: Move to service class. Error handling, logging.
            var path = Path.GetFullPath(Path.Combine(HostingEnvironment.ContentRootPath, @"..\..\")) + "\\appdata\\json\\pokemon";
            var files = Directory.GetFiles(path);

            using (var context = new Pokemon.Infrastructure.Data.AppDbContext(opts))
            {
                // Create the schema in the database
                context.Database.EnsureCreated();
                foreach (var file in files)
                {
                    var jsonString = System.IO.File.ReadAllText(file);
                    var pokemonDto = JsonConvert.DeserializeObject<Models.PokemonDto>(jsonString);
                    var pokemon = mapper.Map(pokemonDto, new Core.Entities.Pokemon());
                    context.Pokemon.Add(mapper.Map(pokemonDto, pokemon));
                }
                context.SaveChanges();

            }


            services.AddScoped<IPokemonRepository, Infrastructure.Data.PokemonRepository>();
            services.AddScoped<IPokemonService, PokemonService>();

            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddMvc()
                .AddNewtonsoftJson();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
