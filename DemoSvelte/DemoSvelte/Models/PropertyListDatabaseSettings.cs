namespace DemoSvelte.Models
{
    public class PropertyListDatabaseSettings :IPropertyListDatabaseSettings
    {
        public string PropertyCollectionName { get; set; } = String.Empty;
        public string AppUserCollectionName { get; set; } = String.Empty;
        public string NewsletterSubcriber { get; set; } = String.Empty;
        public string ConnectionString { get; set; } = String.Empty;
        public string DatabaseName { get; set; } = String.Empty;
    }
}
