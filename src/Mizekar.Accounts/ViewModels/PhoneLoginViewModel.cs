using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Mizekar.Accounts.ViewModels
{
    public class PhoneLoginViewModel
    {
        [Required]
        [DataType(DataType.PhoneNumber)]
        [JsonProperty("phone")]
        public string PhoneNumber { get; set; }
    }
}
