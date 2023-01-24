using DemoSvelte.Models;

namespace DemoSvelte.Services
{
    public interface IPropertyService
    {
        List<Property> Get();
        Property Get(string Id);
        Property Create(Property property);
        void Update(string id, Property property);
        void Delete(string id);
    }
}
