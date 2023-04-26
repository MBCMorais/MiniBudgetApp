using MiniBudgetApp.Models;

namespace MiniBudgetApp.Data
{
    public class AppDbInitializer
    {

        public static void Initialize(IApplicationBuilder applicationBuilder)
        {
            using var scope = applicationBuilder.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.EnsureCreated();

            if(!context.Transactions.Any()) 
            {
                context.Transactions.AddRange(new List<Transaction>()
                {
                    new Transaction() 
                    {
                        Amount = 20,
                        Note ="teste1 transactions",    

                    },
                    new Transaction()
                    {
                        Amount = 10,
                        Note ="teste2 transactions",
                        

                    }

                });
                context.SaveChanges();
            }

            if(!context.Categories.Any()) 
            {
                context.Categories.AddRange(new List<Category>()
                {
                    new Category()
                    {
                        Name = "Test1",
                        
                    },
                    new Category()
                    {
                        Name = "Test2",

                    }
                });
                context.SaveChanges();
            }
        }
    }
}
