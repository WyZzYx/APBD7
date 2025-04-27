using DataAccess.Repositories;
using Domain;

namespace BusinessLogic.Services;

public class DeviceService : IDeviceService {
    private readonly IDeviceRepository _repo;
    public DeviceService(IDeviceRepository repo) => _repo = repo;

    public Task<IEnumerable<Device>> GetAll() => _repo.GetAllAsync();

    public Task<Device?> GetById(string id) => _repo.GetByIdAsync(id);

    public async Task<Device?> Create(Device? d) {
        d.Id = Guid.NewGuid().ToString();
        DeviceValidator.Validate(d);
        await _repo.CreateAsync(d);
        return d;
    }

    public Task Update(Device? d) {
        DeviceValidator.Validate(d);
        return _repo.UpdateAsync(d);
    }

    public Task Delete(string id) {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException();
        return _repo.DeleteAsync(id);
    }
}