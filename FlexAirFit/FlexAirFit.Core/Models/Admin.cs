namespace FlexAirFit.Core.Models;

public class Admin
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }

    public Admin(Guid id,
        string name,
        DateTime dateOfBirth,
        string gender)
    {
        Id = id;
        Name = name;
        DateOfBirth = dateOfBirth;
        Gender = gender;
    }
    
}