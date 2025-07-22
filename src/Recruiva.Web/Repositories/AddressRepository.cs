using Recruiva.Core.Entities;

namespace Recruiva.Web.Repositories;

public class AddressRepository
{
    public AddressRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    private readonly ApplicationDbContext _context;

    public async Task AddAsync(Address address)
    {
        _context.Addresses.Add(address);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var address = await _context.Addresses.FindAsync(id);
        if (address != null)
        {
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Address>> GetAllAsync()
    {
        return await _context.Addresses.ToListAsync();
    }

    public async Task<Address?> GetByIdAsync(Guid id)
    {
        return await _context.Addresses.FindAsync(id);
    }

    public async Task UpdateAsync(Address address)
    {
        _context.Addresses.Update(address);
        await _context.SaveChangesAsync();
    }
}