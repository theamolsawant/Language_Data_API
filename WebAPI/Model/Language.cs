
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WebAPI.Model
{
    public class Language
    {
    
        public int LangaugeId { get; set; }

        [Required(ErrorMessage = "LanguageName is required.")]
        public string Name { get; set; }
    }
}
