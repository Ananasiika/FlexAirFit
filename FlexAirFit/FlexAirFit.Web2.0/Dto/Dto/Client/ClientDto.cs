using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FlexAirFit.Core.Enums;

namespace FlexAirFit.Web2._0.Dto.Dto;

public enum GenderType
{
    male,
    female
}

[DataContract(Name = "client")]
public class ClientDto
{
    [DataMember(Name = "id")]
    public Guid Id { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }
    
    [DataMember(Name = "gender")]
    [EnumDataType(typeof(GenderType))]
    public string Gender { get; set; }
    
    [DataMember(Name = "dateOfBirth")]
    public DateTime DateOfBirth { get; set; }
    
    [DataMember(Name = "idMembership")]
    public Guid IdMembership { get; set; }
    
    [IgnoreDataMember]
    public string? NameMembership { get; set; }

    [DataMember(Name = "membershipEnd")]
    public DateTime MembershipEnd { get; set; }
    
    [DataMember(Name = "remainFreezing")]
    public int? RemainFreezing { get; set; }
    
    [DataMember(Name = "freezingIntervals")]
    public DateTime?[][]? FreezingIntervals { get; set; }
    
    public ClientDto (Guid id,
        string name,
        string gender,
        DateTime dateOfBirth,
        Guid idMembership,
        string nameMembership,
        DateTime membershipEnd,
        int? remainFreezing,
        DateTime?[][]? freezingIntervals)
    {
        Id = id;
        Name = name;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        IdMembership = idMembership;
        NameMembership = nameMembership;
        MembershipEnd = membershipEnd;
        RemainFreezing = remainFreezing;
        FreezingIntervals = freezingIntervals;
    }
}