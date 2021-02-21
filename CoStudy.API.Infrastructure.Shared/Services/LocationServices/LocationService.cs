using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoStudy.API.Infrastructure.Shared.Services.LocationServices
{
    public class LocationService : ILocationService
    {
        IProvinceRepository provinceRepository;
        IDistrictRepository districtRepository;
        IWardRepository wardRepository;

        public LocationService(IProvinceRepository provinceRepository, IDistrictRepository districtRepository, IWardRepository wardRepository)
        {
            this.provinceRepository = provinceRepository;
            this.districtRepository = districtRepository;
            this.wardRepository = wardRepository;
        }

        public IEnumerable<Province> GetAllProvinces(string name)
        {
            IEnumerable<Province> a = provinceRepository.GetAll().AsEnumerable();
            if (!String.IsNullOrEmpty(name))
            {
                a = a.Where(x => x.Name.Contains(name));
            }
            return a;
        }


        public async Task<Province> GetProvinceByCode(string code)
        {
            FilterDefinition<Province> builder = Builders<Province>.Filter.Eq("code", code);
            Province province = await provinceRepository.FindAsync(builder);
            return province;
        }

        public async Task<IEnumerable<District>> GetDistrictByProvince(string province)
        {
            FilterDefinition<District> builder = Builders<District>.Filter.Eq("province_code", province);
            IEnumerable<District> result = (await districtRepository.FindListAsync(builder)).AsEnumerable();
            return result;
        }

        public async Task<District> GetDistrictByCode(string code)
        {
            FilterDefinition<District> builder = Builders<District>.Filter.Eq("code", code);
            return (await districtRepository.FindAsync(builder));
        }

        public async Task<IEnumerable<Ward>> GetWardByDistrict(string district)
        {
            FilterDefinition<Ward> builder = Builders<Ward>.Filter.Eq("district_code", district);
            IEnumerable<Ward> result = (await wardRepository.FindListAsync(builder)).AsEnumerable();
            return result;
        }

        public async Task<Ward> GetWardByCode(string code)
        {
            FilterDefinition<Ward> builder = Builders<Ward>.Filter.Eq("code", code);
            return (await wardRepository.FindAsync(builder));
        }
    }
}
