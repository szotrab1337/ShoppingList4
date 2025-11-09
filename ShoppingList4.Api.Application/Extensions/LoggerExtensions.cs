using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using ShoppingList4.Api.Domain.Entities;

namespace ShoppingList4.Api.Application.Extensions
{
    public static class LoggerExtensions
    {
        public static void AddLogger(this ILoggingBuilder logging, IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .AddDestructure()
                .CreateLogger();

            logging.ClearProviders();
            logging.AddSerilog(logger);
        }

        private static LoggerConfiguration AddDestructure(this LoggerConfiguration configuration)
        {
            configuration.Destructure.ByTransforming<ShoppingList>(x => new
            {
                x.Id,
                x.Name,
                x.CreatedOn
            });

            configuration.Destructure.ByTransforming<Entry>(x => new
            {
                x.Id,
                x.Name,
                x.CreatedOn,
                x.ShoppingListId
            });

            configuration.Destructure.ByTransforming<User>(x => new
            {
                x.Id,
                x.Email,
                x.Name
            });

            return configuration;
        }
    }
}