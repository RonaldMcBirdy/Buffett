using System.ComponentModel.DataAnnotations;

namespace Buffett.Endpoint.Settings
{
    public class BuffettSettings
    {
        [Required]
        public string ApiKey { get; set; }
    }
}
