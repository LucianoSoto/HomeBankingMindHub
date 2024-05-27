using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace HomeBankingMindHub.Models
{
    public class DBInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            if (!context.Clients.Any())
            {
                var clients = new Client[]
                {
                    new Client { Email = "vcoronado@gmail.com", FirstName="Victor", LastName="Coronado", Password="123456"},
                    new Client { Email = "lusoto@gmail.com", FirstName="Luciano", LastName="Soto", Password="aaaaaaaa"}
                };

                context.Clients.AddRange(clients);

                context.SaveChanges();
            }
        }

        public static void Add(HomeBankingContext context, string Name, string LastName, string Email, string Password)
        {
            if (context.Clients.Count() >= 1)
            {
                var client = new Client[]
                {
                    new Client { Email = Email, FirstName= Name , LastName=LastName, Password=Password}
                };

                context.Clients.AddRange(client);

                context.SaveChanges();
            }
        }
    }
}
