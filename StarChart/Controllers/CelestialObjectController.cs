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
            /*
            if (_context.CelestialObjects.Count() == 0)
            {
                _context.CelestialObjects.Add(new CelestialObject() { Id = 1, Name = "sun" });
                _context.CelestialObjects.Add(new CelestialObject() { Id = 2, Name = "earth", OrbitedObjectId = 1 });
                _context.CelestialObjects.Add(new CelestialObject() { Id = 3, Name = "mars", OrbitedObjectId = 1 });
                _context.CelestialObjects.Add(new CelestialObject() { Id = 4, Name = "mercury", OrbitedObjectId = 1 });
                _context.SaveChanges();
            }
            */
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
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
           IEnumerable<CelestialObject> cObjects = _context.CelestialObjects.Where(c => c.Name == name);

            if (cObjects.Count() == 0) return NotFound();
            foreach (var item in cObjects)
            {
                List<CelestialObject> mysat = new List<CelestialObject>();

                foreach (var cObject in _context.CelestialObjects.Where(c => c.OrbitedObjectId == item.Id))
                {
                    mysat.Add(new CelestialObject() { Id = cObject.Id, Name = cObject.Name });
                    cObject.Satellites = mysat;
                };
                item.Satellites = mysat;

            }

            return Ok(cObjects);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<CelestialObject> cObjects = _context.CelestialObjects;

            if (cObjects.Count() == 0) return NotFound();
            foreach (var item in cObjects)
            {
                List<CelestialObject> mysat = new List<CelestialObject>();

                foreach (var cObject in _context.CelestialObjects.Where(c => c.OrbitedObjectId == item.Id))
                {
                    mysat.Add(new CelestialObject() { Id = cObject.Id, Name = cObject.Name });
                    cObject.Satellites = mysat;
                };
                item.Satellites = mysat;

            }

            return Ok(cObjects);
        }
        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject cObject)
        {
            CelestialObject createdRsource= _context.CelestialObjects.Add(cObject).Entity;
            _context.SaveChanges();
            var routeValues = new { id = createdRsource.Id };
            return CreatedAtRoute(
                routeName: "GetById",
                routeValues: routeValues,
                value: createdRsource);
        }

        [HttpPut("{id}")]
      public IActionResult Update(int id, CelestialObject pcObject)
        {
            var CelesObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);
            if (CelesObject == null) return NotFound();

            CelesObject.Name = pcObject.Name;
            CelesObject.OrbitalPeriod = pcObject.OrbitalPeriod;
            CelesObject.OrbitedObjectId = pcObject.OrbitedObjectId;
            _context.CelestialObjects.Update(CelesObject);
            _context.SaveChanges();
            return NoContent();

        }
    }
}
