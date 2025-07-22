using Recruiva.Core.Entities;

namespace Recruiva.Web.Services;

public class AddressService
{
    public AddressService(AddressRepository repository)
    {
        _repository = repository;
    }

    private readonly AddressRepository _repository;

    public Task AddAsync(Address address) => _repository.AddAsync(address);

    public Task DeleteAsync(Guid id) => _repository.DeleteAsync(id);

    public Task<List<Address>> GetAllAsync() => _repository.GetAllAsync();

    public Task<Address?> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

    public Task UpdateAsync(Address address) => _repository.UpdateAsync(address);
}