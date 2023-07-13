using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer
{
    [Table("TblLangauge")]
    public class Language
    {
        [Key]
        [Required]
        public int LangaugeId { get; set; }

        public string LanguageName { get; set; }

    }
}
