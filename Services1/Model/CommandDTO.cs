using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Services.Model
{
    public class CommandDTO
    {
        public int CommandId { get; set; }

        public string CommandText { get; set; }

        public string CommandDescription { get; set; }

        public int LanguageId { get; set; }
    }
}
