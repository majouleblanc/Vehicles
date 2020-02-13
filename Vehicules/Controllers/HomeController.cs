using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vehicules.Entities;
using Vehicules.Models;
using Vehicules.Services;
using Vehicules.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vehicules.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IVehiculeRepository repository;

        public HomeController(IVehiculeRepository repository)
        {
            this.repository = repository;
        }

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Vehicule> model = repository.GetAll();
            return Ok(model);
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            Vehicule model = repository.Get(id);
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpPost]
        public IActionResult Create([FromBody] VehiculeCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newVehicule = new Vehicule()
            {
                Make = model.Make,
                Model = model.Model,
                Color = model.Color,
                Type = model.Type,
                VIN = model.VIN
            };
            repository.add(newVehicule);
            return CreatedAtAction(nameof(Detail), new { newVehicule.Id }, newVehicule);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var vehicule = repository.Get(id);
            if (vehicule == null)
            {
                return NotFound();
            }
            repository.Delete(vehicule);
            return NoContent();
        }

        [HttpPut]
        public IActionResult Update(int id, [FromBody] VehiculeUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicule = repository.Get(id);
            if (vehicule == null)
            {
                return NotFound();
            }

            var updatedVehicule = new Vehicule
            {
                Id = id,
                Make = model.Make,
                Model = model.Model,
                Color = model.Color,
                Type = model.Type,
                VIN = model.VIN
            };
            repository.Update(updatedVehicule);
            return NoContent();
        }
    }
}
