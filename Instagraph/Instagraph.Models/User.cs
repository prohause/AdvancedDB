using System.Collections.Generic;

namespace Instagraph.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int ProfilePictureId { get; set; }

        public Picture ProfilePicture { get; set; }

        public List<UserFollower> Followers { get; set; }

        public List<UserFollower> UsersFollowing { get; set; }

        public List<Post> Posts { get; set; }

        public List<Comment> Comments { get; set; }
    }

    //• Id – an integer, Primary Key
    //• Username – a string with max length 30, Unique
    //• Password – a string with max length 20
    //• ProfilePictureId – an integer
    //• ProfilePicture – a Picture
    //• Followers – a Collection of type UserFollower
    //• UsersFollowing – a Collection of type UserFollower
    //• Posts – a Collection of type Post
    //• Comments – a Collection of type Comment
}