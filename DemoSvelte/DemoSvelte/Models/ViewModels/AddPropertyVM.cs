using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DemoSvelte.Models.ViewModels
{
    public class AddPropertyVM
    {

       
            public string Id { set; get; } = String.Empty;
           
            public string Name { set; get; } = String.Empty;
          
            public string Description { set; get; } = String.Empty;

            public string Province { set; get; } = String.Empty;
           
            public string City { set; get; } = String.Empty;

           
            public string Suburb { set; get; } = String.Empty;

           
            public string? Price { set; get; }
          
            public string Address { set; get; } = String.Empty;

           
            public string Type { set; get; } = String.Empty;

            public string UserID { set; get; } = String.Empty;

    }

}
