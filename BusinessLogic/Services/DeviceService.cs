using DataAccess.Repositories;
using Domain;

namespace BusinessLogic.Services;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _repo;

    public DeviceService(IDeviceRepository repo)
    {
        _repo = repo;
    }

    public Task<IEnumerable<Device>> GetAllAsync()
    {
        return _repo.GetAllAsync();
    }

    public Task<Device?> GetByIdAsync(string id)
    {
        return _repo.GetByIdAsync(id);
    }

    public Task CreateAsync(Device device)
    {
        if (string.IsNullOrWhiteSpace(device.Id)
            || string.IsNullOrWhiteSpace(device.Name))
        {
            throw new ArgumentException("Id and Name are required.");
        }

        return _repo.CreateAsync(device);
    }

    public Task UpdateAsync(Device device)
    {
        if (string.IsNullOrWhiteSpace(device.Id))
        {
            throw new ArgumentException("Id is required.");
        }

        return _repo.UpdateAsync(device);
    }

    public Task DeleteAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Id is required.");
        }

        return _repo.DeleteAsync(id);
    }
}