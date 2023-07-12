using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Data
{
    [Table ("TblCommand")]
    public class Command
    {
        [Key]
        [Required]
        public int CommandId { get; set; }

        public string CommandText { get; set; }

        public string CommandDescription { get; set; }

        [ForeignKey("Language")]
        public int LanguageId { get; set; }
    }
}
