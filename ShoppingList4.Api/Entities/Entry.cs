using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618

namespace ShoppingList4.Api.Entities
{
    public class Entry
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }

        public int ShoppingListId { get; set; }
        public virtual ShoppingList ShoppingList { get; set; }
    }
}
