using DemoSvelte.Models;
using DemoSvelte.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoSvelte.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService propertyService;
        public PropertyController(IPropertyService propertyService) 
        {
            this.propertyService = propertyService;
        }

        [HttpGet ("GetProperties")]   
        public ActionResult<List<Property>> GetProperties() 
        {
            return propertyService.Get();
        }

        [HttpGet("GetPropertyById/{id}")]
        public ActionResult<Property> GetPropertybyId(string id)
        {
            var property = propertyService.Get(id);

            if (property == null)
            {
                return NotFound($"Property with Id = {id} not found");
            }

            return property;
        }

        [HttpPost("AddProperty")]
        public ActionResult<Property> Post([FromBody] Property property)
        {
            propertyService.Create(property);
            
            return CreatedAtAction(nameof(GetProperties), new { id = property.Id }, property);
        }

        
        [HttpPut("UpdateProperty/{id}")]
        public ActionResult Put(string id, [FromBody] Property property)
        {
            var existingProperty = propertyService.Get(id);

            if (existingProperty == null)
            {
                return NotFound($"Property with Id = {id} not found");
            }

            propertyService.Update(id, property);

            return NoContent();
        }

        
        [HttpDelete("DeleteProperty/{id}")]
        public ActionResult DeleteProperty(string id)
        {
            var property = propertyService.Get(id);

            if (property == null)
            {
                return NotFound($"Property with Id = {id} not found");
            }

            propertyService.Delete(property.Id);

            return Ok($"Property with Id = {id} deleted");
        }

    }
}
    