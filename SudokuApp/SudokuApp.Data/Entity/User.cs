using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SudokuApp.DataAccess.Data.Entity;

namespace SudokuApp.Data.Entity
{
    /// <summary>
    /// Сущность пользователя.
    /// </summary>
    public class User : BaseEntity<int>
    {
        /// <summary>
        /// Имя.
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Игральные доски.
        /// </summary>
        public ICollection<Grid> Grids { get; set; }

        public User()
        {
            Grids = new List<Grid>();
        }
    }
}