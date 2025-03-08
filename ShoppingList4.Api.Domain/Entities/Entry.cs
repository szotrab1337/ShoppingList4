﻿namespace ShoppingList4.Api.Domain.Entities
{
    public class Entry
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsBought { get; set; }
        public int ShoppingListId { get; set; }

        public ShoppingList ShoppingList { get; set; } = null!;
    }
}
