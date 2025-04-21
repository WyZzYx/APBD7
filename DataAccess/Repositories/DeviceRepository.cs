using Domain;
using Microsoft.Data.SqlClient;


namespace DataAccess.Repositories;


public class DeviceRepository : IDeviceRepository
{
    private readonly string _connectionString;

    public DeviceRepository(string connectionString)
    {
        _connectionString = connectionString 
                            ?? throw new ArgumentNullException(nameof(connectionString));
    }

        public async Task<IEnumerable<Device>> GetAllAsync()
        {
            var devices = new List<Device>();
            const string sql = "SELECT Id, Name, IsEnabled FROM Devices";

            await using var conn = new SqlConnection(_connectionString);
            await using var cmd  = new SqlCommand(sql, conn);
            await conn.OpenAsync();

            await using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                devices.Add(new Device
                {
                    Id        = rdr.GetString(0),
                    Name      = rdr.GetString(1),
                    IsEnabled = rdr.GetBoolean(2)
                });
            }

            return devices;
        }

        public async Task<Device?> GetByIdAsync(string id)
        {
            const string sql = "SELECT Id, Name, IsEnabled FROM Devices WHERE Id = @id";

            await using var conn = new SqlConnection(_connectionString);
            await using var cmd  = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            await conn.OpenAsync();

            await using var rdr = await cmd.ExecuteReaderAsync();
            if (!await rdr.ReadAsync()) return null;

            return new Device
            {
                Id        = rdr.GetString(0),
                Name      = rdr.GetString(1),
                IsEnabled = rdr.GetBoolean(2)
            };
        }

        public async Task CreateAsync(Device device)
        {
            const string sql =
                "INSERT INTO Devices (Id, Name, IsEnabled) VALUES (@id, @name, @enabled)";

            await using var conn = new SqlConnection(_connectionString);
            await using var cmd  = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id",      device.Id);
            cmd.Parameters.AddWithValue("@name",    device.Name);
            cmd.Parameters.AddWithValue("@enabled", device.IsEnabled);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Device device)
        {
            const string sql =
                "UPDATE Devices SET Name = @name, IsEnabled = @enabled WHERE Id = @id";

            await using var conn = new SqlConnection(_connectionString);
            await using var cmd  = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id",      device.Id);
            cmd.Parameters.AddWithValue("@name",    device.Name);
            cmd.Parameters.AddWithValue("@enabled", device.IsEnabled);
            await conn.OpenAsync();

            var rows = await cmd.ExecuteNonQueryAsync();
            if (rows == 0)
            {
                throw new KeyNotFoundException($"Device with Id '{device.Id}' not found.");
            }
        }

        public async Task DeleteAsync(string id)
        {
            const string sql = "DELETE FROM Devices WHERE Id = @id";

            await using var conn = new SqlConnection(_connectionString);
            await using var cmd  = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
