using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Cell
    {
        public Cell()
        {
            Prisoners = new List<Prisoner>();
        }

        [Key]
        public int Id { get; set; }

        //[Required]
        [Range(1, 1000)]
        public int CellNumber { get; set; }

        //[Required]
        public bool HasWindow { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public List<Prisoner> Prisoners { get; set; }
    }

    //• Id – integer, Primary Key
    //• CellNumber – integer in the range [1, 1000] (required)
    //• HasWindow – bool (required)
    //• DepartmentId - integer, foreign key
    //• Department – the cell's department (required)
    //• Prisoners - collection of type Prisoner
}