using InventoryApi.Model.DTO;
using InventoryApi.Model.Entities;
using InventoryApi.Model.Interfaces;

namespace InventoryApi.Business.Services
{
    public class FilterService : IFilterService
    {
        private readonly IFilterRepository _repository;

        public FilterService(IFilterRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<FilterDTO>> GetAllAsync()
        {
            var filters = await _repository.GetAllAsync();
            return filters.Select(f => new FilterDTO
            {
                FilterType = f.filterType,
                Column = f.Column,
                Value = f.Value
            });
        }

        public async Task<FilterDTO?> GetByIdAsync(int id)
        {
            var filter = await _repository.GetByIdAsync(id);
            if (filter is null) return null;

            return new FilterDTO
            {
                FilterType = filter.filterType,
                Column = filter.Column,
                Value = filter.Value
            };
        }

        public async Task AddAsync(FilterDTO dto)
        {
            var filter = new Filter
            {
                FilterType = dto.FilterType,
                Column = dto.Column,
                Value = dto.Value
            };

            await _repository.AddAsync(filter);
        }

        public async Task UpdateAsync(int id, FilterDTO dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null) return;

            existing.FilterType = dto.FilterType;
            existing.Column = dto.Column;
            existing.Value = dto.Value;

            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}