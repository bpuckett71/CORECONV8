using System.ComponentModel.DataAnnotations;

namespace Credential_Mvc_sample.Models
{
    public class LaborTimeCard
    {
        [Required]
        public string Date { get; set; }

        [Required]
        public string EmployeeName  { get; set; }
        [Required]
        public string ProjectNumber { get; set; }
        [Required]
        public string PrimeContractNumber { get; set; }
        public string ChangeOrderNumber { get; set; }
        public string LaborCode { get; set; }
        [Required]
        public string PayrollCode { get; set; }
        [Required]
        public string CostCode { get; set; }
        public string WCCode { get; set; }
        [Required]
        public decimal Hours { get; set; }
    }
}