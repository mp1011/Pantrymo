namespace Pantrymo.Application.Models
{
    public interface IWithLastModifiedDate
    {
        DateTime LastModified { get; set; }
    }
}
