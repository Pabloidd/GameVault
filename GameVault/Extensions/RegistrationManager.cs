using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using GameVault.Options;

namespace GameVault.Extensions
{
    /// <summary>
    /// Статический класс для методов расширения ,
    /// предназначенных для регистрации сервисов приложения.
    /// </summary>
    public class RegistrationManager
    {
        /// <summary>
        /// Метод расширения для регистрации сервисов приложения.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="configuration">Конфигурация приложения.</param>
        /// <returns>Коллекция сервисов.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MariaDbOptions>(configuration.GetSection("MariaDB"));
            return services;
        }
    }
}