using AutoMapper;
using DataAccessLayer;
using Services.Model;


namespace WebAPI.AutoMapper
{
    public class Mapper: Profile
    {
        public Mapper()
        {
                CreateMap<CommandDTO,Command>().ReverseMap();
                CreateMap<LanguageDTO,Language>().ReverseMap();
        }
    }
}
