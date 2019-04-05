using Instagraph.Data;
using Instagraph.Data.Dto.Import;
using Instagraph.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Instagraph.DataProcessor
{
    public class Deserializer
    {
        public static string ImportPictures(InstagraphContext context, string jsonString)
        {
            var result = new StringBuilder();

            var importedPictures = JsonConvert.DeserializeObject<List<Picture>>(jsonString);

            var pictures = new List<Picture>();

            var uniquePaths = new HashSet<string>();

            foreach (var picture in importedPictures)
            {
                var oldCount = uniquePaths.Count;
                uniquePaths.Add(picture.Path);
                var newCount = uniquePaths.Count;

                if (string.IsNullOrEmpty(picture.Path) || string.IsNullOrWhiteSpace(picture.Path) || picture.Size <= 0 || oldCount == newCount)
                {
                    result.AppendLine("Error: Invalid data.");
                    continue;
                }

                pictures.Add(picture);
                result.AppendLine($"Successfully imported Picture {picture.Path}.");
            }

            context.AddRange(pictures);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportUsers(InstagraphContext context, string jsonString)
        {
            var result = new StringBuilder();
            var pictures = context.Pictures.ToList();
            var users = new List<User>();
            var uniqueUsernames = new HashSet<string>();

            var importedUsers = JsonConvert.DeserializeObject<List<ImportUserJsonDto>>(jsonString);

            foreach (var user in importedUsers)
            {
                var profilePicture = pictures.FirstOrDefault(p => p.Path == user.ProfilePicture);
                var oldCount = uniqueUsernames.Count;
                uniqueUsernames.Add(user.Username);
                var newCount = uniqueUsernames.Count;

                if (string.IsNullOrEmpty(user.Username) || string.IsNullOrWhiteSpace(user.Username)
                    || string.IsNullOrEmpty(user.Password) || string.IsNullOrWhiteSpace(user.Password)
                    || user.Username.Length > 30 || user.Password.Length > 20
                    || oldCount == newCount || profilePicture == null)
                {
                    result.AppendLine("Error: Invalid data.");
                    continue;
                }

                users.Add(new User
                {
                    Username = user.Username,
                    Password = user.Password,
                    ProfilePicture = profilePicture
                });

                result.AppendLine($"Successfully imported User {user.Username}.");
            }

            context.AddRange(users);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportFollowers(InstagraphContext context, string jsonString)
        {
            var result = new StringBuilder();

            var allUsers = context.Users.ToList();

            var importedUserFollowers = JsonConvert.DeserializeObject<List<ImportUserFollowerDto>>(jsonString);

            var usersFollowers = new List<UserFollower>();

            foreach (var userFollower in importedUserFollowers)
            {
                var user = allUsers.FirstOrDefault(u => u.Username == userFollower.User);
                var follower = allUsers.FirstOrDefault(uf => uf.Username == userFollower.Follower);

                if (user == null || follower == null || usersFollowers.Any(uf => uf.User.Username == user.Username && uf.Follower.Username == follower.Username))
                {
                    result.AppendLine("Error: Invalid data.");
                    continue;
                }

                usersFollowers.Add(new UserFollower
                {
                    User = user,
                    Follower = follower
                });

                result.AppendLine($"Successfully imported Follower {follower.Username} to User {user.Username}.");
            }

            context.AddRange(usersFollowers.Distinct());
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportPosts(InstagraphContext context, string xmlString)
        {
            var result = new StringBuilder();

            var users = context.Users.ToList();

            var pictures = context.Pictures.ToList();

            var serializer = new XmlSerializer(typeof(List<ImportPostDto>), new XmlRootAttribute("posts"));

            var importedPosts = (List<ImportPostDto>)serializer.Deserialize(new StringReader(xmlString));

            var posts = new List<Post>();

            foreach (var importPostDto in importedPosts)
            {
                var user = users.FirstOrDefault(u => u.Username == importPostDto.Username);

                var picture = pictures.FirstOrDefault(p => p.Path == importPostDto.PicturePath);

                if (user == null || picture == null)
                {
                    result.AppendLine("Error: Invalid data.");
                    continue;
                }

                posts.Add(new Post
                {
                    Caption = importPostDto.Caption,
                    User = user,
                    Picture = picture
                });

                result.AppendLine($"Successfully imported Post {importPostDto.Caption}.");
            }

            context.AddRange(posts);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportComments(InstagraphContext context, string xmlString)
        {
            var result = new StringBuilder();

            var serializer = new XmlSerializer(typeof(List<ImportCommentDto>), new XmlRootAttribute("comments"));

            var importedComments = (List<ImportCommentDto>)serializer.Deserialize(new StringReader(xmlString));

            var users = context.Users.ToList();

            var posts = context.Posts.ToList();

            var comments = new List<Comment>();

            foreach (var importCommentDto in importedComments)
            {
                if (string.IsNullOrEmpty(importCommentDto.Username) || importCommentDto.ImportSinglePostDto == null)
                {
                    result.AppendLine("Error: Invalid data.");
                    continue;
                }
                var user = users.FirstOrDefault(u => u.Username == importCommentDto.Username);

                var post = posts.FirstOrDefault(p => p.Id == importCommentDto.ImportSinglePostDto.Id);

                if (user == null || post == null)
                {
                    result.AppendLine("Error: Invalid data.");
                    continue;
                }

                comments.Add(new Comment
                {
                    User = user,
                    Post = post,
                    Content = importCommentDto.Content
                });

                result.AppendLine($"Successfully imported Comment {importCommentDto.Content}.");
            }

            context.AddRange(comments);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }
    }
}