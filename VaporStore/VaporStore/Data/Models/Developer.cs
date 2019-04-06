using System.Collections.Generic;

namespace VaporStore.Data.Models
{
    public class Developer
    {
        public Developer()
        {
            Games = new List<Game>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public List<Game> Games { get; set; }
    }

    //• Id – integer, Primary Key
    //• Name – text (required)
    //• Games - collection of type Game
}