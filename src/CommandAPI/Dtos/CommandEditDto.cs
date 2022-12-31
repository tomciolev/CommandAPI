using System.ComponentModel.DataAnnotations;

namespace CommandAPI.Dtos
{
    public class CommandEditDto
    {
        [Required]
        [MaxLength(250)]
        public string HowTo { get; set; }
        [Required]
        public string Platform { get; set; }
        [Required]
        public string CommandLine { get; set; }
    }
}
