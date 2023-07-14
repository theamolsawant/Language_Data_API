namespace Services.Model
{
    public class CommandDTO
    {
        public int CommandId { get; set; }

        /// <summary>
        /// Command Text or command name
        /// </summary>
        public string CommandText { get; set; }

        /// <summary>
        /// Command description
        /// </summary>
        public string CommandDescription { get; set; }

        /// <summary>
        ///Map langauage Id from languyage model.
        /// </summary>
        public int LanguageId { get; set; }
    }
}
