using DemoSvelte.Models;
using MongoDB.Driver;

namespace DemoSvelte.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IMongoCollection<Property> _properties;
         
        public PropertyService(IPropertyListDatabaseSettings settings,IMongoClient mongoClient)
        {
           var database=  mongoClient.GetDatabase(settings.DatabaseName);
           _properties = database.GetCollection<Property>(settings.PropertyCollectionName);
        }
        public  Property  Create(Property property)
        {
            _properties.InsertOne(property);
            return property;
        }
        public List<Property> Get()
        { 
            return _properties.Find(Property => true).ToList();
        }

        public Property Get(string id)
        {
            return _properties.Find(property => property.Id == id).FirstOrDefault();
        }

        public void Delete(string id)
        {
            _properties.DeleteOne(properties => properties.Id == id);
        }

        public void Update(string id, Property property)
        {
            _properties.ReplaceOne(property => property.Id == id, property);
        }
    }
}
