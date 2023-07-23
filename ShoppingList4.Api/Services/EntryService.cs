using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingList4.Api.Entities;
using ShoppingList4.Api.Exceptions;
using ShoppingList4.Api.Interfaces;
using ShoppingList4.Api.Models;

namespace ShoppingList4.Api.Services
{
    public class EntryService : IEntryService
    {
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ShoppingListDbContext _dbContext;
        private readonly ILogger<EntryService> _logger;

        public EntryService(IMapper mapper, IDateTimeProvider dateTimeProvider, ShoppingListDbContext dbContext, ILogger<EntryService> logger)
        {
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
            _dbContext = dbContext;
            _logger = logger;
        }

        public int Create(CreateEntryDto dto)
        {
            var entry = _mapper.Map<Entry>(dto);
            entry.CreatedOn = _dateTimeProvider.GetNow();

            _dbContext.Entries.Add(entry);
            _dbContext.SaveChanges();

            return entry.Id;
        }

        public void Delete(int id)
        {
            var entry = GetById(id);

            _dbContext.Entries.Remove(entry);
            _dbContext.SaveChanges();
        }

        public Entry GetById(int id)
        {
            var entry = _dbContext
                .Entries
                .FirstOrDefault(x => x.Id == id);

            if (entry is null)
            {
                throw new NotFoundException("Entry not found");
            }

            return entry;
        }

        public void Update(int id, UpdateEntryDto dto)
        {
            var entry = GetById(id);
            entry.Name = dto.Name;

            _dbContext.SaveChanges();
        }
    }
}
