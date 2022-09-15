using System.ComponentModel.DataAnnotations;

namespace Credential_Mvc_sample.Models
{
    public class EquipmentTimeCard
    {
        [Required]
        public string Date { get; set; }
        [Required]
        public string EquipmentCode { get; set; }
        [Required]
        public string ProjectNumber { get; set; }
        [Required]
        public string PrimeContractNumber { get; set; }
        public string ChangeOrderNumber { get; set; }
        [Required]
        public string CostCode { get; set; }
        [Required]
        public decimal RTHours { get; set; }
        [Required]
        public decimal ITHours { get; set; }
        [Required]        
        public decimal DTHours { get; set; }
        [Required]
        public string BillableStatus { get; set; }
    }
}