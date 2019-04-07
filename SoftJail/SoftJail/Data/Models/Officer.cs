using SoftJail.Data.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Officer
    {
        public Officer()
        {
            OfficerPrisoners = new List<OfficerPrisoner>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string FullName { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Salary { get; set; }

        [Required]
        public PositionType Position { get; set; }

        [Required]
        public WeaponType Weapon { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public List<OfficerPrisoner> OfficerPrisoners { get; set; }
    }

    //• Id – integer, Primary Key
    //• FullName – text with min length 3 and max length 30 (required)
    //• Salary – decimal (non-negative, minimum value: 0) (required)
    //• Position - Position enumeration with possible values: “Overseer, Guard, Watcher, Labour” (required)
    //• Weapon - Weapon enumeration with possible values: “Knife, FlashPulse, ChainRifle, Pistol, Sniper” (required)
    //• DepartmentId - integer, foreign key
    //• Department – the officer's department (required)
    //• OfficerPrisoners - collection of type OfficerPrisoner
}