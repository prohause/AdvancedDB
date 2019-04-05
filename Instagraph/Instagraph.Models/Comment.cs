namespace Instagraph.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }
    }

    //• Id – an integer, Primary Key
    //• Content – a string with max length 250
    //• UserId – an integer
    //• User – a User
    //• PostId – an integer
    //• Post – a Post
}