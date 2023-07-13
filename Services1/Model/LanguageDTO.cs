using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Services.Model
{
    public class LanguageDTO
    {

        public int LangaugeId { get; set; }

     
        public string LanguageName { get; set; }
    }
}
