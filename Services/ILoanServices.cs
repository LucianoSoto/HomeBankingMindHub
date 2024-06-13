using Clase_1.DTOS;
using Clase_1.Models;
using System.Security.Claims;

namespace Clase_1.Services
{
    public interface ILoanServices
    {
        public IEnumerable<ViewLoanDTO> GetAllLoans();
        public void VerifyData(LoanApplicationDTO loanApplicationDTO);
        public void VerifyIfLoanExists(LoanApplicationDTO loanApplicationDTO, ClaimsPrincipal User);
        public void CheckMaxAmount(LoanApplicationDTO loanApplicationDTO);
        public void CheckPayments(LoanApplicationDTO loanApplicationDTO);
        public Account CheckAccount(LoanApplicationDTO loanApplicationDTO);
        public void CheckUserAccount(LoanApplicationDTO loanApplicationDTO, ClaimsPrincipal User);
        public void CreateLoan(LoanApplicationDTO loanApplicationDTO, ClaimsPrincipal User);
    }
}
