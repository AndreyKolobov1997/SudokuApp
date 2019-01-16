using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SudokuApp.DataAccess.Data.Entity;

namespace SudokuApp.Data.Entity
{
    /// <summary>
    /// �������� ��������� �����. 
    /// </summary>
    public class Grid : BaseEntity<int>
    {
        /// <summary>
        /// ���������� ������ �����.
        /// </summary>
        [Required]
        public string FullBoardData { get; set; }
        
        /// <summary>
        /// ���������� ����������� �����.
        /// </summary>
        public string FilledBoardData { get; set; }

        /// <summary>
        /// �������� �� ������ ����� ������� � ������������.
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Id ������������.
        /// </summary>
        [Required]
        public int UserId { get; set; }
        
        /// <summary>
        /// ������������.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}