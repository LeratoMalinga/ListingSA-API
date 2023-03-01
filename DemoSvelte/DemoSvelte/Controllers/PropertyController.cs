using DemoSvelte.Models;
using DemoSvelte.Models.ViewModels;
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
        public ActionResult<Property> Post([FromBody] AddPropertyVM vm)
        {
            //var user = db.Users.Include(u => u.Patient).Where(u => u.Id == userId).FirstOrDefault();

            var property = new Property { Name = vm.Name, Price = vm.Price, Province = vm.Province, City = vm.City, Suburb = vm.Suburb, Type = vm.Type, Address = vm.Address, IsAvaliable =true,Picture="pic"};
            try
            {
                propertyService.Create(property);

                return CreatedAtAction(nameof(GetProperties), new { id = property.Id }, property);
            }
            catch (Exception)
            {

                return BadRequest("Property no created");
            }

            
           
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
    