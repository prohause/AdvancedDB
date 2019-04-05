using AutoMapper;
using Instagraph.Data;
using Instagraph.DataProcessor;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Formatting = Newtonsoft.Json.Formatting;

namespace Instagraph.App
{
    public class StartUp
    {
        public static DefaultContractResolver ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        public static XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

        public static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = ContractResolver,
            NullValueHandling = NullValueHandling.Ignore
        };

        public static void Main(string[] args)
        {
            Mapper.Initialize(options => options.AddProfile<InstagraphProfile>());

            //Console.WriteLine(ResetDatabase());

            //Console.WriteLine(ImportData());

            ExportData();
        }

        private static string ImportData()
        {
            StringBuilder sb = new StringBuilder();

            using (var context = new InstagraphContext())
            {
                string picturesJson = File.ReadAllText(@"C:\Users\proha\source\repos\AdvancedDB\Instagraph\Instagraph.App\files\input\pictures.json");

                sb.AppendLine(Deserializer.ImportPictures(context, picturesJson));

                string usersJson = File.ReadAllText(@"C:\Users\proha\source\repos\AdvancedDB\Instagraph\Instagraph.App\files\input\users.json");

                sb.AppendLine(Deserializer.ImportUsers(context, usersJson));

                string followersJson = File.ReadAllText(@"C:\Users\proha\source\repos\AdvancedDB\Instagraph\Instagraph.App\files\input\users_followers.json");

                sb.AppendLine(Deserializer.ImportFollowers(context, followersJson));

                string postsXml = File.ReadAllText(@"C:\Users\proha\source\repos\AdvancedDB\Instagraph\Instagraph.App\files\input\posts.xml");

                sb.AppendLine(Deserializer.ImportPosts(context, postsXml));

                string commentsXml = File.ReadAllText(@"C:\Users\proha\source\repos\AdvancedDB\Instagraph\Instagraph.App\files\input\comments.xml");

                sb.AppendLine(Deserializer.ImportComments(context, commentsXml));
            }

            string result = sb.ToString().Trim();
            return result;
        }

        private static void ExportData()
        {
            using (var context = new InstagraphContext())
            {
                string uncommentedPostsOutput = Serializer.ExportUncommentedPosts(context);

                File.WriteAllText(@"C:\Users\proha\source\repos\AdvancedDB\Instagraph\Instagraph.App\files\output\UncommentedPosts.json", uncommentedPostsOutput);

                string usersOutput = Serializer.ExportPopularUsers(context);

                File.WriteAllText(@"C:\Users\proha\source\repos\AdvancedDB\Instagraph\Instagraph.App\files\output\PopularUsers.json", usersOutput);

                string commentsOutput = Serializer.ExportCommentsOnPosts(context);

                File.WriteAllText(@"C:\Users\proha\source\repos\AdvancedDB\Instagraph\Instagraph.App\files\output\CommentsOnPosts.xml", commentsOutput);
            }
        }

        private static string ResetDatabase()
        {
            using (var context = new InstagraphContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return $"Database reset succsessfully.";
        }
    }
}