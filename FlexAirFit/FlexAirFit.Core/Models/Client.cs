namespace FlexAirFit.Core.Models;

public class Client
{
    public Guid Id { get; set; }
    public Guid IdUser { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Guid IdMembership { get; set; }
    public DateOnly MembershipEnd { get; set; }
    public int? RemainFreezing { get; set; }
    public bool IsFreezing { get; set; }
    public List<Tuple<DateOnly, DateOnly>> FreezingIntervals { get; set; }

    public Client(Guid id,
        Guid idUser,
        string name,
        string gender,
        DateOnly dateOfBirth,
        Guid idMembership,
        DateOnly membershipEnd,
        int? remainFreezing,
        bool isFreezing,
        List<Tuple<DateOnly, DateOnly>> freezingIntervals)
    {
        Id = id;
        IdUser = idUser;
        Name = name;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        IdMembership = idMembership;
        MembershipEnd = membershipEnd;
        RemainFreezing = remainFreezing;
        IsFreezing = isFreezing;
        FreezingIntervals = freezingIntervals;
    }
}