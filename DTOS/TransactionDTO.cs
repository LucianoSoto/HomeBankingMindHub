﻿using Clase_1.Models;
using System.Text.Json.Serialization;

namespace Clase_1.DTOS
{
    public class TransactionDTO
    {
        public long Id { get; set; }
        public TransactionType Type { get; set; }
        public double Amount { get; set; }

        public string Description { get; set; }
        public DateTime Date { get; set; }

        public TransactionDTO(Transaction transaction)
        {
            Id = transaction.Id;
            Type = transaction.Type;
            Amount = transaction.Amount;
            Description = transaction.Description;
            Date = transaction.Date;
        }
    }
}
