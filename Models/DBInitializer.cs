using Clase_1.Models;
using HomeBankingMindHub.Models;

namespace HomeBankingMindHub.Models
{
    public class DBInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            //Verifica si la tabla esta vacia
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

            if (!context.Accounts.Any())
            {
                Client victorClient = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                Client luchoClient = context.Clients.FirstOrDefault(c => c.Email == "lusoto@gmail.com");
                {
                    if (victorClient != null && luchoClient != null) 
                    {
                        var vicAccounts = new Account[]
                        {
                            new Account{Number="VIN001", CreationDate=DateTime.Now, Balance=100, ClientId = victorClient.Id},
                            new Account{Number="VIN002", CreationDate=DateTime.Now, Balance=200, ClientId = luchoClient.Id}
                        };

                    context.Accounts.AddRange(vicAccounts);
                    context.SaveChanges();
                    }

                    
                }
            }
        }
    }
}
