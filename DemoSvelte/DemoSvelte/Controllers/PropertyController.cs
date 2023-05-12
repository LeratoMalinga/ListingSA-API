using DemoSvelte.Models;
using DemoSvelte.Models.ViewModels;
using DemoSvelte.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DemoSvelte.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService propertyService;
        private readonly UserManager<AppUser> _userManager;
        public PropertyController(IPropertyService propertyService, UserManager<AppUser> userManager)
        {
            this.propertyService = propertyService;
            _userManager = userManager;
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

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);
                return BadRequest(errors);
            }

            var userId = vm.UserId; // Get the userId from the view model

            // Get the AppUser object from the database using the userId
            var appUser = _userManager.FindByIdAsync(userId).Result;

            if (appUser == null)
            {
                return BadRequest("User not found");
            }

            var property = new Property
            {
                Name = vm.Name,
                Price = vm.Price,
                Province = vm.Province,
                Description= "Property",
                City = vm.City,
                Suburb = vm.Suburb,
                Type = vm.Type,
                Address = vm.Address,
                IsAvaliable = true,
                ImageBase64 = vm.ImageBase64,
                AppUser = appUser
            };

            try
            {
                propertyService.Create(property);
                return CreatedAtAction(nameof(GetProperties), new { id = property.Id }, property);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateProperty/{id}")]
        public ActionResult Put(string id, [FromBody] UpdatePropertyVM vm)
        {
            var existingProperty = propertyService.Get(id);

            if (existingProperty == null)
            {
                return NotFound($"Property with Id = {id} not found");
            }

            // Map properties from VM to existing Property object
            existingProperty.Name = vm.Name;
            existingProperty.Description = vm.Description;
            existingProperty.Province = vm.Province;
            existingProperty.City = vm.City;
            existingProperty.Suburb = vm.Suburb;
            existingProperty.Price = vm.Price;
            existingProperty.Address = vm.Address;
            existingProperty.ImageBase64 = vm.ImageBase64;
            existingProperty.Type = vm.Type;

            propertyService.Update(id, existingProperty);

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
    