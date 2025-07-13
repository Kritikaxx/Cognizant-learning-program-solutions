using Microsoft.AspNetCore.Mvc;

namespace MyFirstWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private static readonly List<string> Values = new List<string>
        {
            "Value1", "Value2", "Value3"
        };

        // GET: api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok(Values);
        }

        // GET: api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            if (id < 0 || id >= Values.Count)
            {
                return NotFound($"Value with ID {id} not found");
            }
            return Ok(Values[id]);
        }

        // POST: api/values
        [HttpPost]
        public ActionResult<string> Post([FromBody] string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return BadRequest("Value cannot be empty");
            }
            
            Values.Add(value);
            return CreatedAtAction(nameof(Get), new { id = Values.Count - 1 }, value);
        }

        // PUT: api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            if (id < 0 || id >= Values.Count)
            {
                return NotFound($"Value with ID {id} not found");
            }
            
            if (string.IsNullOrEmpty(value))
            {
                return BadRequest("Value cannot be empty");
            }
            
            Values[id] = value;
            return Ok(Values[id]);
        }

        // DELETE: api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id < 0 || id >= Values.Count)
            {
                return NotFound($"Value with ID {id} not found");
            }
            
            Values.RemoveAt(id);
            return NoContent();
        }
    }
}