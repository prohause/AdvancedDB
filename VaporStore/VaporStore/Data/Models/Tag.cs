using System.Collections.Generic;

namespace VaporStore.Data.Models
{
    public class Tag

    {
        public Tag()
        {
            GameTags = new List<GameTag>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public List<GameTag> GameTags { get; set; }
    }

    //• Id – integer, Primary Key
    //• Name – text (required)
    //• GameTags - collection of type GameTag
}