using Domain;

namespace BusinessLogic.Services;

public interface IDeviceService
{
    Task<IEnumerable<Device>> GetAllAsync();
    Task<Device?> GetByIdAsync(string id);
    Task CreateAsync(Device device);
    Task UpdateAsync(Device device);
    Task DeleteAsync(string id);
}