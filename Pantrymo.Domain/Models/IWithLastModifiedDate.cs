namespace Pantrymo.Domain.Models
{
    public interface IWithLastModifiedDate
    {
        DateTime LastModified { get; set; }
    }
}
