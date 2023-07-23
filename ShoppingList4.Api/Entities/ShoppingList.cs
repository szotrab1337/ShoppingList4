using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618

namespace ShoppingList4.Api.Entities
{
    public class ShoppingList
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual List<Entry> Entries { get; set; } = new();
    }
}
