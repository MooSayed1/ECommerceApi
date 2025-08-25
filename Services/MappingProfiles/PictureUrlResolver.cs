using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Shared;

namespace Services.MappingProfiles;

public class PictureUrlResolver(IConfiguration configuration) : IValueResolver<Product,ProductResultDto,string> 
{
    public string Resolve(Product source, ProductResultDto destination, string destMember, ResolutionContext context)
    {
        if(string.IsNullOrEmpty(source.PictureUrl))
        {
            return string.Empty;
        }
        return $"{configuration["JwtOptions:Issuer"]}{source.PictureUrl}";
    }
}