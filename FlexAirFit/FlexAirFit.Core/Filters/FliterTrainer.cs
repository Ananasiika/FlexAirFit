namespace FlexAirFit.Core.Filters;

public class FilterTrainer
{
    public string? Name { get; init; }
    public string? Gender { get; init; }
    public string? Specialization { get; init; }
    public int? MinExperience { get; init; }
    public int? MaxExperience { get; init; }
    public int? MinRating { get; init; }
    public int? MaxRating { get; init; }
    
    public FilterTrainer(string? name, 
        string? gender, 
        string? specialization, 
        int? minExperience, 
        int? maxExperience,
        int? minRating, 
        int? maxRating)
    {
        Name = name;
        Gender = gender;
        Specialization = specialization;
        MinExperience = minExperience;
        MaxExperience = maxExperience;
        MinRating = minRating;
        MaxRating = maxRating;
    }
}