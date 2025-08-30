
using BuildingBlocks.Contracts;
using BuildingBlocks.Infrastructure.Messaging;
using Modules.Patients.Domain;
using Modules.Patients.Infrastructure;

namespace Modules.Patients.Application;

public class PatientService
{
    private readonly PatientRepository _repo;
    private readonly IEventBus _bus;

    public PatientService(PatientRepository repo, IEventBus bus)
    {
        _repo = repo;
        _bus = bus;
    }

    public async Task RegisterAsync(Patient patient)
    {
        await _repo.AddAsync(patient);
        var evt = new PatientRegisteredEvent(patient.Id, patient.FullName, patient.Dob, patient.Email);
        await _bus.PublishAsync("patients.registered", evt);
    }
}
