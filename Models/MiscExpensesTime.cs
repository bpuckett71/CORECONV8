using System.ComponentModel.DataAnnotations;

namespace Credential_Mvc_sample.Models
{
    public class MiscExpensesTime
    {
        [Required]
        public string ProjectNumber { get; set; }
        [Required]
        public string PrimeContractNumber { get; set; }
        public string ChangeOrderNumber { get; set; }
        [Required]
        public string ExpenseDate { get; set; }
        [Required]
        public string ExpenseType { get; set; }       
        [Required]
        public string PaymentType { get; set; }
        [Required]
        public string EmployeeName { get; set; }
        [Required]
        public string PayeeType { get; set; }
        [Required]
        public string PayeeCompany { get; set; }
        [Required]
        public string ItemDescription { get; set; }
        [Required]
        public string ItemUnit { get; set; }
        [Required]
        public int ItemQuantity { get; set; }
        [Required]
        public decimal ItemUnitPrice { get; set; }
        [Required]
        public string ItemResource { get; set; }
        [Required]
        public string ItemCostCode { get; set; }        
        public string ItemTaxCode { get; set; }
        [Required]
        public string BillableStatus { get; set; }
    }
}