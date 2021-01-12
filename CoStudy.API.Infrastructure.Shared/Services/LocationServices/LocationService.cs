using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var a = provinceRepository.GetAll().AsEnumerable();
            if(!String.IsNullOrEmpty(name))
            {
                a = a.Where(x => x.Name.Contains(name));
            }
            return a;
        }
        

        public async Task<Province> GetProvinceByCode(string code)
        {
            var builder = Builders<Province>.Filter.Eq("code", code);
            var province = await provinceRepository.FindAsync(builder);
            return province;
        }

        public async Task<IEnumerable<District>> GetDistrictByProvince(string province)
        {
            var builder = Builders<District>.Filter.Eq("province_code", province);
            var result = (await districtRepository.FindListAsync(builder)).AsEnumerable();
            return result;
        }

        public async Task<District> GetDistrictByCode(string code)
        {
            var builder = Builders<District>.Filter.Eq("code", code);
            return (await districtRepository.FindAsync(builder));
        }

        public async Task<IEnumerable<Ward>> GetWardByDistrict(string district)
        {
            var builder = Builders<Ward>.Filter.Eq("district_code", district);
            var result = (await wardRepository.FindListAsync(builder)).AsEnumerable();
            return result;
        }

        public async  Task<Ward> GetWardByCode(string code)
        {
            var builder = Builders<Ward>.Filter.Eq("code", code);
            return (await wardRepository.FindAsync(builder));
        }
    }
}
