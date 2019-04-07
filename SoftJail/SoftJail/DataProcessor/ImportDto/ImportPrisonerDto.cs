using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportPrisonerDto
    {
        [Required]
        [MaxLength(20), MinLength(3)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^The\s[A-Z][A-Za-z]*$")]
        public string Nickname { get; set; }

        [Required]
        [Range(18, 65)]
        public int Age { get; set; }

        [Required]
        public DateTime IncarcerationDate { get; set; }

        public DateTime? ReleaseDate { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal? Bail { get; set; }

        [Required]
        public int CellId { get; set; }

        public List<ImportMailDto> Mails { get; set; }
    }
}