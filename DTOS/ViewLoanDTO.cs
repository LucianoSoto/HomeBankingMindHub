using Clase_1.Models;

namespace Clase_1.DTOS
{
    public class ViewLoanDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public double MaxAmount { get; set; }

        public string Payments { get; set; }

        public ViewLoanDTO(Loan loan)
        {
            Id = loan.Id;
            Name = loan.Name;
            MaxAmount = loan.MaxAmount;
            Payments = loan.Payments;
        }
    }
}
