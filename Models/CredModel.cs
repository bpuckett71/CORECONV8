using System.ComponentModel.DataAnnotations;

namespace Credential_Mvc_sample.Models
{
    public class CredModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ClientID { get; set; }
        [Required]
        public string SecretCode { get; set; }
        [Required]
        public string Scopes { get; set; }
        
    }
}