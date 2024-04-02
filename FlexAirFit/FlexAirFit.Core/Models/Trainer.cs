namespace FlexAirFit.Core.Models;

public class Trainer
{
    public Guid Id { get; set; }
    public Guid IdUser { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public string Specialization { get; set; }
    public int Experience { get; set; }
    public int Rating { get; set; }

    public Trainer(Guid id,
        Guid idUser,
        string name,
        string gender,
        string specialization,
        int experience,
        int rating)
    {
        Id = id;
        IdUser = idUser;
        Name = name;
        Gender = gender;
        Specialization = specialization;
        Experience = experience;
        Rating = rating;
    }
}