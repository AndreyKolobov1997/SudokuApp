namespace SudokuApp.Api.Controllers.Mvc.Sudoku.Board
{
    /// <summary>
    /// Модель создания новой доски которая приходит на контроллер.
    /// </summary>
    public class CreateNewBoardRequestModel
    {
        /// <summary>
        /// Id доски.
        /// </summary>
        public int BoardId { get; set; }
    }
}