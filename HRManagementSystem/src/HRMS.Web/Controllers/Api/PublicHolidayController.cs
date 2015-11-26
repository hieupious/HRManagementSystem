using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using HRMS.Web.Models;
using HRMS.Web.ViewModels;
using Microsoft.Data.Entity;
using AutoMapper;
using Newtonsoft.Json;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMS.Web.Controllers.Api
{
    [Route("api/[controller]")]
    public class PublicHolidayController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public PublicHolidayController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        /// <summary>
        /// Get all public holidays
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<PublicHolidayViewModel> GetAllPublicHoliday()
        {
            var publicHolidays = dbContext.PublicHolidays.Include(x => x.VNPublicHoliday).AsEnumerable();
            var publicHolidayViewModels = Mapper.Map<IEnumerable<PublicHolidayViewModel>>(publicHolidays);

            return publicHolidayViewModels;
        }

        /// <summary>
        /// Get public holiday by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetPublicHoliday")]
        public IActionResult GetById(int id)
        {
            var item = dbContext.PublicHolidays.FirstOrDefault(p => p.Id == id);
            if (item == null)
            {
                return HttpNotFound(id);
            }

            return new ObjectResult(item);
        }

        /// <summary>
        /// Get all public holidays by year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        // GET: api/PublicHoliday/year
        [HttpGet("GetByYear/{year}")]
        public IActionResult GetByYear(int year)
        {
            var publicHolidays = dbContext.PublicHolidays.Include(x => x.VNPublicHoliday).Where(p => p.Date.Year == year).AsEnumerable();
            var publicHolidayViewModels = Mapper.Map<IEnumerable<PublicHolidayViewModel>>(publicHolidays);

            return new ObjectResult(publicHolidayViewModels);
        }

        /// <summary>
        /// Create public holiday
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] PublicHoliday model)
        {
            if (model == null || !ModelState.IsValid)
                return HttpBadRequest(ModelState);

            dbContext.PublicHolidays.Add(model);
            dbContext.SaveChanges();

            var viewModel = Mapper.Map<PublicHolidayViewModel>(model);
            return CreatedAtRoute("GetPublicHoliday", new { controller = "PublicHoliday", id = model.Id }, viewModel);
        }

        /// <summary>
        /// Update public holiday
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] PublicHoliday model)
        {
            if (model == null || model.Id != id)
            {
                return HttpBadRequest();
            }

            var holiday = dbContext.PublicHolidays.FirstOrDefault(p => p.Id == id);
            if (holiday == null)
            {
                return HttpNotFound();
            }

            holiday.Date = model.Date;
            dbContext.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// Delete public holiday
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var holiday = dbContext.PublicHolidays.FirstOrDefault(p => p.Id == id);
            if (holiday == null)
            {
                return HttpNotFound();
            }

            dbContext.PublicHolidays.Remove(holiday);
            dbContext.SaveChanges();

            return new NoContentResult();
        }

        /// <summary>
        /// Get all vietnamese public holidays
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetVietnameseHolidays")]
        public IActionResult GetVietnameseHolidays()
        {
            var vietnameseHolidays = dbContext.VietnamesePublicHolidays.AsEnumerable();
            return new ObjectResult(vietnameseHolidays);
        }
    }
}
