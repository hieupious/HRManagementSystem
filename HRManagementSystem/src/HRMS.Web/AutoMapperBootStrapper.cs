using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HRMS.Web.Models;
using HRMS.Web.ViewModels;


namespace HRMS.Web
{
    public class AutoMapperBootStrapper
    {
        public static void ConfigureAutoMapper()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<PublicHoliday, PublicHolidayViewModel>()
                    .ForMember(nameof(PublicHolidayViewModel.Name), opt => opt.MapFrom(p => p.VNPublicHoliday.Name))
                    .ForMember(nameof(PublicHolidayViewModel.Description), opt => opt.MapFrom(p => p.VNPublicHoliday.Description))
                    //.ForMember(nameof(PublicHolidayViewModel.Date),opt=>opt.MapFrom(p=>p.Date.ToShortDateString()))
                    .ForMember(nameof(PublicHolidayViewModel.DayOfWeek),opt=>opt.MapFrom(p=>p.Date.DayOfWeek));
            });
        }
    }
}
