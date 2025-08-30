
namespace Modules.Patients.Domain;

public class Patient // Aggregate Root
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string FullName { get; set; } = string.Empty;
    public DateTime Dob { get; set; }
    public string Email { get; set; } = string.Empty;
}
