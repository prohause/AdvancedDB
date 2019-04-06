﻿using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Dto.Import
{
    public class ImportUserDto
    {
        [Required]
        [MaxLength(20), MinLength(3)]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z][a-z]*\s[A-Z][a-z]*$")]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Range(3, 103)]
        public int Age { get; set; }

        [Required]
        [MinLength(1)]
        public ImportCardDto[] Cards { get; set; }
    }
}