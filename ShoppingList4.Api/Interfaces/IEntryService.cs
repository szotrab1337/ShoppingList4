using ShoppingList4.Api.Entities;
using ShoppingList4.Api.Models;

namespace ShoppingList4.Api.Interfaces;

public interface IEntryService
{
    int Create(CreateEntryDto dto);
    void Delete(int id);
    Entry GetById(int id);
    void Update(int id, UpdateEntryDto dto);
}