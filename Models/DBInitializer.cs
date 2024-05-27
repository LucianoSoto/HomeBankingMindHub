using HomeBankingMindHub.Models;

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
    }
}
