
using MongoDB.Driver;
using Modules.Patients.Domain;

namespace Modules.Patients.Infrastructure;

public class PatientRepository
{
    private readonly IMongoCollection<Patient> _col;
    public PatientRepository(IMongoClient client, string dbName)
    {
        _col = client.GetDatabase(dbName).GetCollection<Patient>("patients");
    }

    public Task AddAsync(Patient p) => _col.InsertOneAsync(p);
}
