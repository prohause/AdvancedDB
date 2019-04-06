using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models
{
    public class User
    {
        public User()
        {
            Cards = new List<Card>();
        }

        public int Id { get; set; }

        [MaxLength(20), MinLength(3)]
        public string Username { get; set; }

        [RegularExpression(@"^[A-Z][a-z]*\s[A-Z][a-z]*$")]
        public string FullName { get; set; }

        public string Email { get; set; }

        [Range(3, 103)]
        public int Age { get; set; }

        public List<Card> Cards { get; set; }
    }

    //User
    //• Id – integer, Primary Key
    //• Username – text with length [3, 20] (required)
    //• FullName – text, which has two words, consisting of Latin letters. Both start with an upper letter and are separated by a single space (ex. "John Smith") (required)
    //• Email – text (required)
    //• Age – integer in the range [3, 103] (required)
    //• Cards – collection of type Card
}