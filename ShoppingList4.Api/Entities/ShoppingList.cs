using System.ComponentModel.DataAnnotations;

namespace ShoppingList4.Api.Entities
{
    public class ShoppingList
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }

        public ShoppingList(string name, DateTime createdOn)
        {
            Name = name;
            CreatedOn = createdOn;
        }

        public virtual List<Entry> Entris { get; set; } = new();
    }
}
