using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Web_project.Models.Interfaces;
using Web_project.Models.SqlRepository;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using MesContexts.Models;
using System.Net.Http;
using FluentValidation;
using Web_project.Validator;
using Web_project.ViewsModels;
using Microsoft.AspNetCore.Http;

namespace Web_project
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false).AddFluentValidation();
            services.AddControllersWithViews();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme= "Cookies";
                    options.Authority = "https://localhost:5001";
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "mvc";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code id_token";
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.SaveTokens = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                    options.Scope.Add("MonApi");
                    options.Scope.Add("role");
                });

            services.UseServicesEtablissements();
            services.UseServicesHoraires();
            services.UseServicesPhotos();
            services.UseServicesNews();
            services.UseServicesVAT();
            services.UseServicesMapQuest();
            services.UseServicesShortUrl();
            services.AddTransient<IValidator<EtablissementCreateViewModel>, EtablissementCreateValidator>();
            services.AddTransient<IValidator<EtablissementEditViewModel>, EtablissementEditValidator>();
            services.AddTransient<IValidator<IFormFile>, PhotosValidator>();
            services.AddTransient<IValidator<IFormFile>, LogoValidator>();
            services.AddSingleton<HttpClient>(new HttpClient());
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
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvcWithDefaultRoute();
        }
    }
}
