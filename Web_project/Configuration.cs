using Microsoft.Extensions.DependencyInjection;
using Web_project.Models.Interfaces;
using Web_project.Models.SqlRepository;

namespace Web_project
{
    public static class Configuration
    {
        public static void UseServicesEtablissements(this IServiceCollection services)
        {
            services.AddHttpClient<IEtablissementRepository, SQLEtablissementRepository>();
        }
        public static void UseServicesHoraires(this IServiceCollection services)
        {
            services.AddHttpClient<IHoraireRepository, SQLHoraireRepository>();
        }
        public static void UseServicesPhotos(this IServiceCollection services)
        {
            services.AddHttpClient<IPhotosRepository, SQLPhotosRepository>();
        }
        public static void UseServicesNews(this IServiceCollection services)
        {
            services.AddHttpClient<INewsRepository, SQLNewsRepository>();
        }
        public static void UseServicesVAT(this IServiceCollection services)
        {
            services.AddHttpClient<IVatLayerRepository, SQLVatLayerRepository>();
        }
        public static void UseServicesMapQuest(this IServiceCollection services)
        {
            services.AddHttpClient<IMapQuestRepository, SQLMapQuestRepository>();
        }
        public static void UseServicesShortUrl(this IServiceCollection services)
        {
            services.AddHttpClient<IShortUrlRepository, SQLShortUrlRepository>();
        }
    }
}
