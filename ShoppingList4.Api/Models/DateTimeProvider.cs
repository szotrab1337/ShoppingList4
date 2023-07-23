using ShoppingList4.Api.Interfaces;

namespace ShoppingList4.Api.Models
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetNow() => DateTime.Now;
    }
}
