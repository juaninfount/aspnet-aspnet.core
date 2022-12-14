using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly DataContext  _context;

        public ValuesController(DataContext dc)
        {
            _context = dc;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult>  GetValues()
        {
            //return new string[] { "value1", "value2" };
            var result = await _context.Values.ToListAsync();
            return  Ok(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
           // throw new Exception("Test exception");
            return  Ok(await _context.Values.FirstOrDefaultAsync( x=>x.Id == id));
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
