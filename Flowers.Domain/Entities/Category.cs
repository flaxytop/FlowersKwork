using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowers.Domain.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string NormalizedName { get; set; } // короткое название
    }
}
