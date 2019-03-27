using ProductShop.Data;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new ProductShopContext())
            {
                context.Database.EnsureCreated();
            }
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            return "";
        }
    }
}