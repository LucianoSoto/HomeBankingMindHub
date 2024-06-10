using Clase_1.DTOS;
using System.Security.Claims;

namespace Clase_1.Services
{
    public interface ITransferServices
    {
        IEnumerable<TransactionDTO> GetAllTransactions();
        TransactionDTO GetTransactionById(long id);
        void CheckIfEmpty(TransferDTO transfer);
        void CheckIfEqual(TransferDTO transfer);
        void CheckIfExists(TransferDTO transfer);
        AccountDTO CheckClientAccount(ClaimsPrincipal User, TransferDTO transferDTO);
        void CheckBalance(ClaimsPrincipal User, TransferDTO transfer);
        void CreateTransaction(ClaimsPrincipal User, TransferDTO transfer);
    }
}
