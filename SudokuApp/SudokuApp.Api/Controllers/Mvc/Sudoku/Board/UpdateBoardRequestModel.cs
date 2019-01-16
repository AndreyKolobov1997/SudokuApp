namespace SudokuApp.Api.Controllers.Mvc.Sudoku.Board
{
    /// <summary>
    /// Модель обновления доски которая приходит на контроллер.
    /// </summary>
    public class UpdateBoardRequestModel
    {
        /// <summary>
        /// Id доски.
        /// </summary>
        public int BoardId { get; set; }

        /// <summary>
        /// Новая заполняемая доска.
        /// </summary>
        public string NewFilledBoard { get; set; }
    }
}