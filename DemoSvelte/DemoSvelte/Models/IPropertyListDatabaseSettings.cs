namespace DemoSvelte.Models
{
    public interface IPropertyListDatabaseSettings
    {
         string PropertyCollectionName { get; set; } 
         string AppUserCollectionName { get; set; }
         string NewsletterSubcriber { get; set; } 
         string ChatMessage { get; set; }
         string ConnectionString { get; set; } 
         string DatabaseName { get; set; } 
    }
}
