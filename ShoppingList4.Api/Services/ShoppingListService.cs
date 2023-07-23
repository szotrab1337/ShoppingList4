using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingList4.Api.Entities;
using ShoppingList4.Api.Exceptions;
using ShoppingList4.Api.Interfaces;
using ShoppingList4.Api.Models;

namespace ShoppingList4.Api.Services
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly ShoppingListDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ShoppingListService> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ShoppingListService(ShoppingListDbContext dbContext, IMapper mapper,
            ILogger<ShoppingListService> logger, IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
        }

        public IEnumerable<ShoppingList> GetAll()
        {
            var shoppingLists = _dbContext.ShoppingLists
                .Include(x => x.Entries)
                .ToList();

            return shoppingLists;
        }

        public int Create(ShoppingListDto dto)
        {
            var shoppingList = _mapper.Map<ShoppingList>(dto);
            shoppingList.CreatedOn = _dateTimeProvider.GetNow();

            _dbContext.ShoppingLists.Add(shoppingList);
            _dbContext.SaveChanges();

            _logger.LogInformation("Added new shopping list: {@shoppingList}.", shoppingList);

            return shoppingList.Id;
        }

        public ShoppingList GetById(int id)
        {
            var shoppingList = _dbContext
                .ShoppingLists
                .Include(x => x.Entries)
                .FirstOrDefault(x => x.Id == id);

            if (shoppingList is null)
            {
                throw new NotFoundException("Shopping list not found");
            }

            return shoppingList;
        }

        public void Delete(int id)
        {
            var shoppingList = GetByIdWithoutRelationships(id);

            _dbContext.ShoppingLists.Remove(shoppingList);
            _dbContext.SaveChanges();
        }

        private ShoppingList GetByIdWithoutRelationships(int id)
        {
            var shoppingList = _dbContext.ShoppingLists.FirstOrDefault(x => x.Id == id);

            if (shoppingList is null)
            {
                throw new NotFoundException("Shopping list not found");
            }

            return shoppingList;
        }

        public void Update(int id, ShoppingListDto dto)
        {
            var shoppingList = GetByIdWithoutRelationships(id);
            shoppingList.Name = dto.Name;

            _dbContext.SaveChanges();
        }
    }
}
