using System.Text.Json;
using Domain;
using Microsoft.Data.SqlClient;


namespace DataAccess.Repositories;


public class DeviceRepository : IDeviceRepository {
    private readonly string _conn;
    public DeviceRepository(string connectionString) => _conn = connectionString;

    public async Task<IEnumerable<Device>> GetAllAsync() {
        var list = new List<Device>();
        const string sql = "SELECT Id, Name, IsEnabled, DeviceType, TypeValue FROM Devices";
        await using var c = new SqlConnection(_conn);
        await using var cmd = new SqlCommand(sql, c);
        await c.OpenAsync();
        await using var rdr = await cmd.ExecuteReaderAsync();
        while (await rdr.ReadAsync()) {
            var type = rdr.GetString(3);
            var tv   = rdr.GetString(4);
            list.Add(DeviceFactory.FromRecord(type, rdr.GetString(0), rdr.GetString(1), rdr.GetBoolean(2), tv));
        }
        return list;
    }

    public async Task<Device?> GetByIdAsync(string id) {
        const string sql = "SELECT Id, Name, IsEnabled, DeviceType, TypeValue FROM Devices WHERE Id=@id";
        await using var c = new SqlConnection(_conn);
        await using var cmd = new SqlCommand(sql, c);
        cmd.Parameters.AddWithValue("@id", id);
        await c.OpenAsync();
        await using var rdr = await cmd.ExecuteReaderAsync();
        if (!await rdr.ReadAsync()) return null;
        return DeviceFactory.FromRecord(
            rdr.GetString(3), rdr.GetString(0), rdr.GetString(1), rdr.GetBoolean(2), rdr.GetString(4)
        );
    }

    public async Task CreateAsync(Device? d) {
        const string sql = @"
            INSERT INTO Devices
              ( Id, Name, IsEnabled, DeviceType, TypeValue )
            VALUES
              ( @id, @name, @en, @dt, @tv )";
        
        await using var c   = new SqlConnection(_conn);
        await using var cmd = new SqlCommand(sql, c);

        cmd.Parameters.AddWithValue("@id",   d.Id);
        cmd.Parameters.AddWithValue("@name", d.Name);
        cmd.Parameters.AddWithValue("@en",   d.IsEnabled);
        cmd.Parameters.AddWithValue("@dt",   d.DeviceType);
        cmd.Parameters.AddWithValue("@tv",   JsonSerializer.Serialize(d, d.GetType()));

        await c.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateAsync(Device? d) {
        const string sql = @"
            UPDATE Devices
               SET Name       = @name,
                   IsEnabled  = @en,
                   DeviceType = @dt,
                   TypeValue  = @tv
             WHERE Id = @id";

        await using var c   = new SqlConnection(_conn);
        using var tx  = c.BeginTransaction();

        await using var cmd = new SqlCommand(sql, c);

        cmd.Parameters.AddWithValue("@id",   d.Id);
        cmd.Parameters.AddWithValue("@name", d.Name);
        cmd.Parameters.AddWithValue("@en",   d.IsEnabled);
        cmd.Parameters.AddWithValue("@dt",   d.DeviceType);
        cmd.Parameters.AddWithValue("@tv",   JsonSerializer.Serialize(d, d.GetType()));

        await c.OpenAsync();
        var rows = await cmd.ExecuteNonQueryAsync();
        if (rows == 0) throw new KeyNotFoundException($"Device '{d.Id}' not found");
        tx.Commit();

    }

    public async Task DeleteAsync(string id) {
        const string sql = "DELETE FROM Devices WHERE Id=@id";
        await using var c = new SqlConnection(_conn);
        await using var cmd = new SqlCommand(sql, c);
        using var tx  = c.BeginTransaction();

        cmd.Parameters.AddWithValue("@id", id);
        await c.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
        tx.Commit();
    }
}