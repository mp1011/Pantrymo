#nullable disable
using Pantrymo.Domain.Models;

namespace Pantrymo.Application.Models
{
    public partial class Author : IAuthor
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
