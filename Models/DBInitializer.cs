using Clase_1.Models;
using HomeBankingMindHub.Models;
using NuGet.Packaging;

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

            if(!context.Transactions.Any())
            {
                var account1 = context.Accounts.FirstOrDefault(c => c.Number == "VIN001");
                
                if(account1 != null)
                {
                    var transactions = new Transaction[] {
                        new Transaction { AccountId= account1.Id, Amount = 10000, Date= DateTime.Now.AddHours(-5), Description = "Recibo de Sueldo Enero", Type = TransactionType.CREDIT },
                        new Transaction { AccountId= account1.Id, Amount = 2000, Date= DateTime.Now.AddHours(-3), Description = "Venta de Insumos para el hogar", Type = TransactionType.CREDIT },
                        new Transaction { AccountId= account1.Id, Amount = 500, Date= DateTime.Now.AddHours(-1), Description = "Compra de almacen", Type = TransactionType.DEBIT }
                    };

                    context.Transactions.AddRange(transactions);
                    context.SaveChanges();
                }

            }

            if (!context.Loans.Any())
            {
                var loans = new Loan[]
                {
                    new Loan {Name="Hipotecario", MaxAmount = 500000, Payments="12,24,36,48,60"},
                    new Loan {Name="Personal", MaxAmount = 100000, Payments="6,12,24"},
                    new Loan {Name="Automotriz", MaxAmount = 300000, Payments="6,12,24,36"},
                };
                
                context.Loans.AddRange(loans);
                context.SaveChanges();

            }

            if (!context.ClientLoans.Any())
            {
                var client1 = context.Clients.FirstOrDefault(client => client.Email == "vcoronado@gmail.com");
                var loan1 = context.Loans.FirstOrDefault(loan => loan.Name == "Hipotecario");
                var loan2 = context.Loans.FirstOrDefault(loan => loan.Name == "Personal");
                var loan3 = context.Loans.FirstOrDefault(loan => loan.Name == "Automotriz");

                if (client1 != null && loan1 != null)
                {
                    var clientLoan1 = new ClientLoan { Amount= 400000, ClientId= client1.Id, LoanId= loan1.Id, Payments = "60" };
                    context.ClientLoans.Add(clientLoan1);
                }
                if (client1 != null && loan2 != null)
                {
                    var clientLoan2 = new ClientLoan { Amount= 50000, ClientId= client1.Id, LoanId= loan2.Id, Payments = "12" };
                    context.ClientLoans.Add(clientLoan2);
                }
                if (client1 != null && loan3 != null)
                {
                    var clientLoan3 = new ClientLoan { Amount= 100000, ClientId= client1.Id, LoanId= loan3.Id, Payments = "24" };
                    context.ClientLoans.Add(clientLoan3);
                }

                context.SaveChanges();   
            }

            if (!context.Cards.Any())
            {
                var client1 = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");

                if (client1 != null)
                {
                    var cards = new Card[]
                    {
                        new Card
                        {
                            ClientId = client1.Id,
                            CardHolder = client1.FirstName + " " + client1.LastName,
                            Type = CardType.DEBIT.ToString(),
                            Color = CardColor.GOLD.ToString(),
                            Number = "3325-6745-7876-4445",
                            Cvv = 990,
                            FromDate = DateTime.Now,
                            ThruDate = DateTime.Now.AddYears(4),
                        },
                        new Card
                        {
                            ClientId = client1.Id,
                            CardHolder = client1.FirstName + " " + client1.LastName,
                            Type = CardType.CREDIT.ToString(),
                            Color = CardColor.TITANIUM.ToString(),
                            Number = "2234-6745-552-7888",
                            Cvv = 750,
                            FromDate = DateTime.Now,
                            ThruDate = DateTime.Now.AddYears(5),
                        }
                    };

                    context.Cards.AddRange(cards);
                    context.SaveChanges();
                }
            }
        }
    }
}
