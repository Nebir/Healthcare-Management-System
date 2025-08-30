
using Microsoft.AspNetCore.Mvc;
using Modules.Patients.Application;
using Modules.Patients.Domain;

namespace ModularMonolith.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly PatientService _svc;
    public PatientsController(PatientService svc) => _svc = svc;

    [HttpPost]
    public async Task<IActionResult> Register(Patient patient)
    {
        await _svc.RegisterAsync(patient);
        return Ok(new { message = "Patient registered. Orchestrator workflow started." });
    }
}
