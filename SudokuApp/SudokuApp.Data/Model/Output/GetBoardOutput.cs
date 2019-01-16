namespace SudokuApp.Data.Model.Output
{
    /// <summary>
    /// Модель доски, которую возвращает сервис.
    /// </summary>
    public class GetBoardOutput
    {
        /// <summary>
        /// Id доски.
        /// </summary>
        public int BoardId { get; set; }

        /// <summary>
        /// Поле в процессе заполнения.
        /// </summary>
        public string FilledBoard { get; set; }

        /// <summary>
        /// Заполненное поле.
        /// </summary>
        public string FullBoard { get; set; }
    }
}