using CoStudy.API.Domain.Entities.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.LocationServices
{
    public interface ILocationService
    {
        IEnumerable<Province> GetAllProvinces(string name);
        Task<Province> GetProvinceByCode(string code);

        Task<IEnumerable<District>> GetDistrictByProvince(string province);
        Task<District> GetDistrictByCode(string code);

        Task<IEnumerable<Ward>> GetWardByDistrict(string district);
        Task<Ward> GetWardByCode(string code);
    }
}
