using System.Threading.Tasks;
using SudokuApp.Data.Model.Output;

namespace SudokuApp.Bl.ServiceInterfaces
{
    /// <summary>
    /// Сервис работы с досками.
    /// </summary>
    public interface IBoardService
    {
        /// <summary>
        /// Получить доску для пользователя по его имени.
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <returns>Доска для пользователя</returns>
        Task<GetBoardOutput> GetBoardByUserName(string userName);

        /// <summary>
        /// Сохранить игру для пользователя по Id доски.
        /// </summary>
        /// <param name="gridId">Id доски</param>
        /// <param name="updateFilledBoard">Обновленная заполняемая доска</param>
        Task SaveBoardForUser(int gridId, string updateFilledBoard);

        /// <summary>
        /// Создать новую доску для пользователя по Id доски.
        /// </summary>
        /// <param name="gridId">Id доски</param>
        Task CreateNewBoardForUser(int gridId);
    }
}