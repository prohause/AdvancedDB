using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportDepartmentDto
    {
        [Required]
        [MaxLength(25), MinLength(3)]
        public string Name { get; set; }

        public List<ImportCellsDto> Cells { get; set; }
    }
}