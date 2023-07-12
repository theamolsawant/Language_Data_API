using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Model
{
    public class Command
    {
        public int CommandId { get; set; }

        public string CommandText { get; set; }

        public string CommandDescription { get; set; }

        public int LangaugeId { get; set; }
    }
}
