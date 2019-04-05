using System.Collections.Generic;

namespace Instagraph.Models
{
    public class Picture
    {
        public int Id { get; set; }

        public string Path { get; set; }

        public decimal Size { get; set; }

        public List<User> Users { get; set; }

        public List<Post> Posts { get; set; }
    }

    //• Id – an integer, Primary Key
    //• Path – a string
    //• Size – a decimal
    //• Users – a Collection of type User
    //• Posts – a Collection of type Post
}