using ShoppingList4.Api.Middlewares;

namespace ShoppingList4.Api.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void AddMiddlewares(this IServiceCollection services)
        {
            services.AddScoped<ErrorHandlingMiddleware>();
        }
    }
}
