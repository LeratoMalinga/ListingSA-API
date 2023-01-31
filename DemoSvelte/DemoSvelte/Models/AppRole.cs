using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System.Runtime.Serialization;

namespace DemoSvelte.Models
{
    [CollectionName("ApplicationRoles")]
    public class AppRole : MongoIdentityRole<Guid>
    {

    }
}
