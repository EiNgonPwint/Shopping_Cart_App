using System;
namespace CA_Team5.Models
{
    public class PurchaseHistory
    {
        public Guid PurchaseId { get; set; }
        public int UserId { get; set; }

        public Guid ProductId { get; set; }

        public Guid ActivationCode { get; set; }

        public String PurchaseDate { get; set; }
    }
}

