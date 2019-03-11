using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SwashBuckleExample.Models;

namespace SwashBuckleExample.Controllers
{
    /// <summary>
    /// Operations related to a person.
    /// </summary>
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class PersonController : ControllerBase
    {
        private List<PersonModel> _people;

        public PersonController()
        {
            _people = new List<PersonModel>
            { 
                new PersonModel() { Name = "homer", Age = 50 },
                new PersonModel() { Name = "marge", Age = 29 },
                new PersonModel() { Name = "bart", Age = 11 },
                new PersonModel() { Name = "lisa", Age = 7 },
                new PersonModel() { Name = "maggy", Age = 1 }
            };
        }

        /// <summary>
        /// Gets a single Person by their id (their name).
        /// </summary>
        [HttpGet]
        public ActionResult<PersonModel> Get(string id)
        {
            var result = _people.FirstOrDefault(x => x.Name.Equals(id, StringComparison.InvariantCultureIgnoreCase));
            if (result == null)
            {
                return NotFound(); // will give back a ProblemDetails. If you enter an error message, it won't.
            }

            return Ok(result);
        }

        /// <summary>
        /// Adds a single Person to the list of people.
        /// </summary>
        [HttpPost]
        public IActionResult Post(PersonModel person)
        {
            //_people.Add(person);
            
            return CreatedAtAction(nameof(Get), person.Name);
        }

        /// <summary>
        /// Updates a single Person in the list of people.
        /// </summary>
        [HttpPut]
        public IActionResult Put(PersonModel person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var existingPerson = _people.FirstOrDefault(x => x.Name.Equals(person.Name, StringComparison.InvariantCultureIgnoreCase));
            if (existingPerson == null)
            {
                return NotFound();
            }

            _people.Remove(existingPerson);
            _people.Add(person);

            return Ok();
        }

        /// <summary>
        /// Removes a single Person from the list of people.
        /// </summary>
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            var existingPerson = _people.FirstOrDefault(x => x.Name.Equals(id, StringComparison.InvariantCultureIgnoreCase));
            if (existingPerson == null)
            {
                return NotFound();
            }

            _people.Remove(existingPerson);

            return Ok();
        }
    }
}
