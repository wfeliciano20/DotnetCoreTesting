using System;
using Entities;

namespace ServiceContracts.DTO;

/// <summary>
/// DTO class that is used as return type 
/// for most CountryService methods
/// </summary>
public class CountryResponse
{
    public Guid CountryId { get; set; }
    public string? CountryName { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (obj.GetType() != typeof(CountryResponse))
            return false;

        CountryResponse objectToCompare = (CountryResponse)obj;
        return this.CountryId == objectToCompare.CountryId && this.CountryName == objectToCompare.CountryName;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public static class CountryExtensions
{
    public static CountryResponse ToCountryResponse
    (this Country country)
    {
        return new CountryResponse()
        {
            CountryId = country.CountryId,
            CountryName = country.CountryName
        };
    }

}
