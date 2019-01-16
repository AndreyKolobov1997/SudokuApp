using SudokuApp.Common.Extensions;

namespace SudokuApp.Api.Controllers.Mvc.Sudoku.Board
{
    /// <summary>
    /// Модель доски для отображения.
    /// </summary>
    public class BoardViewModel
    {
        /// <summary>
        /// Id доски.
        /// </summary>
        public int BoardId { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Доска в процессе заполнения.
        /// </summary>
        public string BoardFilled { get; set; }
        
        /// <summary>
        /// Заполненная доска.
        /// </summary>
        public string BoardFull { get; set; }

        /// <summary>
        /// Проверка на активную игру.
        /// </summary>
        public bool IsAliveGame { get; set; }
        
        public BoardViewModel()
        {
            IsAliveGame = !UserName.IsNullOrEmpty();
        }
    }
}