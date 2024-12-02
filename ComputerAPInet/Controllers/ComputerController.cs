using ComputerApiHetfo.Models;
using ComputerAPInet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerAPInet.Controllers
{
    [Route("comp")]
    [ApiController]


    public class CompController : ControllerBase
    {
        private readonly ComputerContext computerContext;
        public CompController(ComputerContext computerContext)
        {
            this.computerContext = computerContext;
        }

        [HttpPost]
        public async Task<ActionResult<Comp>> Post(CreateCompDto createCompDto)
        {
            var comp = new Comp
            {
                Id = Guid.NewGuid(),
                Brand = createCompDto.Brand,
                Type = createCompDto.Type,
                Display = createCompDto.Display,
                Memory = createCompDto.Memory,
                CreatedTime = DateTime.Now,
                OsId = createCompDto.OsId,

            };

            if (comp != null)
            {
                await computerContext.Comps.AddAsync(comp);
                await computerContext.SaveChangesAsync();
                return StatusCode(201, comp);
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult<Comp>> Get()
        {
            return Ok(await computerContext.Comps.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comp>> GetById(Guid id)
        {
            var comp = await computerContext.Comps.FirstOrDefaultAsync(comp => comp.Id == id);
            return comp != null ? Ok(comp) : NotFound(new { message = "Nincs ilyen találat." });
        }        

        [HttpPut("{id}")]
        public async Task<ActionResult<Comp>> Put(Guid id, UpdateCompDto updateCompDto)
        {
            var existingComp = await computerContext.Comps.FirstOrDefaultAsync(ecomp => ecomp.Id == id);

            if (existingComp != null)
            {
                existingComp.Brand = updateCompDto.Brand;
                existingComp.Type = updateCompDto.Type;
                existingComp.Display = updateCompDto.Display;
                existingComp.Memory = updateCompDto.Memory;
                computerContext.Comps.Update(existingComp);
                await computerContext.SaveChangesAsync();
                return Ok(existingComp);
            }

            return NotFound(new { message = "Nincs ilyen találat." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var comp = await computerContext.Comps.FirstOrDefaultAsync(c => c.Id == id);

            if (comp == null)
            {
                return NotFound(new { message = "Nincs ilyen találat." });
            }
            computerContext.Comps.Remove(comp);
            await computerContext.SaveChangesAsync();

            return NoContent();
        }
    }
}