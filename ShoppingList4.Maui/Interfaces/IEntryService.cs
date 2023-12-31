﻿namespace ShoppingList4.Maui.Interfaces;
using Entry = ShoppingList4.Maui.Entity.Entry;

public interface IEntryService
{
    Task<List<Entry>> GetAsync(int shoppingListId);
    Task<bool> UpdateAsync(Entry entry);
    Task<bool> DeleteAsync(int id);
    Task<bool> DeleteMultipleAsync(List<int> ids);
    Task<bool> AddAsync(string name, int shoppingListId);
}