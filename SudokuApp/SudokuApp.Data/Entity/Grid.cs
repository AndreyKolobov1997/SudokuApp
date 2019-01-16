using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SudokuApp.DataAccess.Data.Entity;

namespace SudokuApp.Data.Entity
{
    /// <summary>
    /// Сущность игральной доски. 
    /// </summary>
    public class Grid : BaseEntity<int>
    {
        /// <summary>
        /// Содержимое полной доски.
        /// </summary>
        [Required]
        public string FullBoardData { get; set; }
        
        /// <summary>
        /// Содержимое заполняемой доски.
        /// </summary>
        public string FilledBoardData { get; set; }

        /// <summary>
        /// Является ли данная доска текущей у пользователя.
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Id пользователя.
        /// </summary>
        [Required]
        public int UserId { get; set; }
        
        /// <summary>
        /// Пользователь.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}