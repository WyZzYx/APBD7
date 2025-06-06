using Domain;

namespace BusinessLogic.Services;

public interface IDeviceService {
    Task<IEnumerable<Device>> GetAll();
    Task<Device?> GetById(string id);
    Task<Device?> Create(Device? device);
    Task Update(Device? device);
    Task Delete(string id);
}