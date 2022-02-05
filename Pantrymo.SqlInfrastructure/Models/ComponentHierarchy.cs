namespace Pantrymo.Application.Models
{
    public partial class ComponentHierarchy : IComponentHierarchy
    {
        public int ComponentId { get; set; }
        public int Id { get; set; }
        public DateTime LastModified { get; set; }
    }
}
