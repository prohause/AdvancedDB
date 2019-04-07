using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Department
    {
        public Department()
        {
            Cells = new List<Cell>();
            Officers = new List<Officer>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string Name { get; set; }

        public List<Cell> Cells { get; set; }

        public List<Officer> Officers { get; set; }
    }

    //• Id – integer, Primary Key
    //• Name – text with min length 3 and max length 25 (required)
    //• Cells - collection of type Cell
}