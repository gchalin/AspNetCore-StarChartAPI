using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            //This method should return NotFound if there is no CelestialObject with
            //an Id property that matches the parameter.
            var CelestialObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);

            if (CelestialObject == null) return NotFound();
           
            List<CelestialObject> mysat = new List<CelestialObject>();

            foreach (var cObject in _context.CelestialObjects.Where(c => c.OrbitedObjectId == id))
            {
                mysat.Add(new CelestialObject() { Id = cObject.Id, Name = cObject.Name });
                cObject.Satellites = mysat;
             };
            CelestialObject.Satellites = mysat;
            return Ok(CelestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            //This method should return NotFound if there is no CelestialObject with
            //an Id property that matches the parameter.
            var CelestialObject = _context.CelestialObjects.FirstOrDefault(c => c.Name == name);

            if (CelestialObject == null) return NotFound();

            List<CelestialObject> mysat = new List<CelestialObject>();

            foreach (var cObject in _context.CelestialObjects.Where(c => c.OrbitedObjectId == CelestialObject.Id))
            {
                mysat.Add(new CelestialObject() { Id = cObject.Id, Name = cObject.Name });
                cObject.Satellites = mysat;
            };
            CelestialObject.Satellites = mysat;
            return Ok(CelestialObject);
        }
    }
}
