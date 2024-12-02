using ComputerApiHetfo.Models;
using ComputerAPInet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Cmp;

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

        [HttpGet]
        public async Task<ActionResult<Comp>> GetNumOfComp()
        {
            var num = await computerContext.Comps.ToListAsync();

            if (num != null)
            {
                return Ok(new { message = $"Az adatbázisban {num}db computer van." });
            }
            return BadRequest();
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

        [HttpGet("allcompwithos")]
        public async Task<ActionResult<Comp>> getallcompwithos()
        {
            var allcomp = await computerContext.Comps.Select(cmp => new
            {
                cmp.Brand,
                cmp.Type,
                cmp.Os.Name
            }).ToListAsync();
            return Ok(allcomp);
        }

        public async Task<ActionResult<Comp>> getallmicrosoft(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest("Keyword cannot be empty.");
            }
            else
            {
                var allwithsearchos = await computerContext.Comps.Where(cmp =>
                cmp.Os.Name.Contains(keyword)).Select(cmp => new { cmp.Brand, cmp.Type, cmp.Os.Name }).ToListAsync();

                if (allwithsearchos != null)
                {
                    return Ok(allwithsearchos);
                }
            }
            return NotFound();
        }

        [HttpGet("biggestDisplay")]
        public async Task<ActionResult<Comp>> getbiggestdisplay()
        {
            var biggestdisplay = await computerContext.Comps.MaxAsync(cmp => cmp.Display);
            var cmpmaxdisplay = await computerContext.Comps.Where(cmp => cmp.Display == biggestdisplay)
                .Select(cmp => new { cmp.Brand, cmp.Type, cmp.Display, cmp.Os.Name }).ToListAsync();
            return Ok(cmpmaxdisplay);
        }

        [HttpGet("most recent")]
        public async Task<ActionResult<Comp>> getrecent()
        {
            var mostrecent = await computerContext.Comps.MaxAsync(cmp => cmp.CreatedTime);
            var cmprecent = await computerContext.Comps.Where(cmp => cmp.CreatedTime == mostrecent)
                .Select(cmp => new {cmp}).ToListAsync();
            return Ok(cmprecent);
        }
    }
}