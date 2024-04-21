﻿namespace FlexAirFit.Core.Models;

public class Client
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Guid IdMembership { get; set; }
    public DateOnly MembershipEnd { get; set; }
    public int? RemainFreezing { get; set; }
    public List<Tuple<DateOnly, DateOnly>>? FreezingIntervals { get; set; }

    public bool IsMembershipActive(DateOnly currentDate = default) {
        if (currentDate == default)
        {
            currentDate = DateOnly.FromDateTime(DateTime.Today);
        }
        return !FreezingIntervals.Any(interval => interval.Item1 <= currentDate && currentDate <= interval.Item2) && currentDate <= MembershipEnd;
    }


    public Client(Guid id,
        string name,
        string gender,
        DateOnly dateOfBirth,
        Guid idMembership,
        DateOnly membershipEnd,
        int? remainFreezing,
        List<Tuple<DateOnly, DateOnly>> freezingIntervals)
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