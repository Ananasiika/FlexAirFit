namespace FlexAirFit.Core.Models;

public class Client
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Guid IdMembership { get; set; }
    public DateTime MembershipEnd { get; set; }
    public int? RemainFreezing { get; set; }
    public DateTime?[][]? FreezingIntervals { get; set; }
    public bool IsMembershipActive(DateTime currentDate = default) {
        if (currentDate == default)
        {
            currentDate = DateTime.Today;
        }
        return !FreezingIntervals.Any(interval => interval[0] <= currentDate && currentDate <= interval[1]) && currentDate <= MembershipEnd;
    }


    public Client(Guid id,
        string name,
        string gender,
        DateTime dateOfBirth,
        Guid idMembership,
        DateTime membershipEnd,
        int? remainFreezing,
        DateTime?[][]? freezingIntervals)
    {
        Id = id;
        Name = name;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        IdMembership = idMembership;
        MembershipEnd = membershipEnd;
        RemainFreezing = remainFreezing;
        FreezingIntervals = freezingIntervals;
    }
}