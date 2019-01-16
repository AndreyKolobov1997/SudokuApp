using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SudokuApp.DataAccess.Data.Entity;

namespace SudokuApp.Data.Entity
{
    /// <summary>
    /// �������� ������������.
    /// </summary>
    public class User : BaseEntity<int>
    {
        /// <summary>
        /// ���.
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// ��������� �����.
        /// </summary>
        public ICollection<Grid> Grids { get; set; }

        public User()
        {
            Grids = new List<Grid>();
        }
    }
}