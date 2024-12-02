using ComputerApiHetfo.Models;
using ComputerAPInet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerApiHetfo.Controllers
{
    [Route("osystem")]
    [ApiController]
    public class OsystemController : ControllerBase
    {
        private readonly ComputerContext computerContext;
        public OsystemController(ComputerContext computerContext)
        {
            this.computerContext = computerContext;
        }

        [HttpPost]
        public async Task<ActionResult<Osystem>> Post(CreateOsDto createOsDto)
        {
            var os = new Osystem
            {
                Id = Guid.NewGuid(),
                Name = createOsDto.Name
            };

            if (os != null)
            {
                await computerContext.Osystems.AddAsync(os);
                await computerContext.SaveChangesAsync();
                return StatusCode(201, os);
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult<Osystem>> Get()
        {
            return Ok(await computerContext.Osystems.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Osystem>> GetById(Guid id)
        {
            var os = await computerContext.Osystems.FirstOrDefaultAsync(os => os.Id == id);
            return os != null ? Ok(os) : NotFound(new { message = "Nincs ilyen találat." });

            /*if (os != null)
            {
                return Ok(os);
            }

            return NotFound(new { message = "Nincs ilyen találat." });*/
        }
        /*
        [HttpGet("WithComp")]
        public async Task<ActionResult> GetWithComp(Guid id)
        {
            var os = await computerContext.Osystems
                .AsNoTracking()
                .Where(os => os.Id == id)
                .Select(os => new
                {
                    os.Id,
                    os.Name,
                    Comps = os.Comps.Select(c => new
                    {
                        c.Id,
                        c.Brand,
                        c.Type,
                        c.Display,
                        c.Memory,
                        c.CreatedTime,
                        c.OsId
                    })
                })
                .FirstOrDefaultAsync();

            if (os != null)
            {
                return Ok(os);
            }

            return NotFound();
        }
        */

        [HttpPut("{id}")]
        public async Task<ActionResult<Osystem>> Put(Guid id, UpdateOsDto updateOsDto)
        {
            var existingOs = await computerContext.Osystems.FirstOrDefaultAsync(eos => eos.Id == id);

            if (existingOs != null)
            {
                existingOs.Name = updateOsDto.Name;
                computerContext.Osystems.Update(existingOs);
                await computerContext.SaveChangesAsync();
                return Ok(existingOs);
            }

            return NotFound(new { message = "Nincs ilyen találat." });
        }
    }
}