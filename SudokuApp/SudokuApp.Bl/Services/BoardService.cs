using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SudokuApp.Bl.ServiceInterfaces;
using SudokuApp.Common.Exceptions;
using SudokuApp.Data.Entity;
using SudokuApp.Data.Model.Output;
using SudokuApp.DataAccess.RepositoryInterface;
using SudokuSharp;

namespace SudokuApp.Bl.Services
{
    public class BoardService : IBoardService
    {
        private static readonly IList<string> IncludePropertiesUser;

        private readonly IRepository _repository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public BoardService(ILoggerFactory loggerFactory,
            IRepository repository,
            IReadOnlyRepository readOnlyRepository,
            IMapper mapper)
        {
            _logger = loggerFactory.CreateLogger<BoardService>();
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
        }

        static BoardService()
        {
            IncludePropertiesUser = new List<string>()
            {
                $"{nameof(User.Grids)}"
            };
        }

        public async Task<GetBoardOutput> GetBoardByUserName(string userName)
        {
            _logger.LogTrace($"Сервис: {nameof(BoardService)}. Метод: {nameof(GetBoardByUserName)}." +
                             $"Аргументы: {nameof(userName)}: {userName}.");

            var user = await _readOnlyRepository.GetOne<User>(filter: x => x.UserName == userName,
                includeProperties: string.Join(",", IncludePropertiesUser));

            if (user is null)
            {
                user = new User()
                {
                    UserName = userName
                };
                _repository.Add(user);

                var newUserBoard = await CreateAndGetNewGrid(user.Id);
                return _mapper.Map<Grid, GetBoardOutput>(newUserBoard);
            }

            if (user.Grids.Count == 0)
            {
                var newUserBoard = await CreateAndGetNewGrid(user.Id);
                return _mapper.Map<Grid, GetBoardOutput>(newUserBoard);
            }

            var userBoard = user.Grids.SingleOrDefault(x => x.IsActive);

            if (userBoard is null)
            {
                throw new NotFoundException($"Не найдено активных досок у пользователя: {userName}!");
            }

            return _mapper.Map<Grid, GetBoardOutput>(userBoard);
        }

        public async Task SaveBoardForUser(int gridId, string updateFilledBoard)
        {
            _logger.LogTrace($"Сервис: {nameof(BoardService)}. Метод: {nameof(SaveBoardForUser)}." +
                             $"Аргументы: {nameof(gridId)}: {gridId}.");

            var grid = await _readOnlyRepository.GetOne<Grid>(filter: x => x.Id == gridId);

            if (grid is null)
            {
                throw new NotFoundException($"Доска с таким Id не найдена: {gridId}!");
            }

            grid.FilledBoardData = updateFilledBoard;
            _repository.Update(grid);
            await _repository.Save();
        }

        public async Task CreateNewBoardForUser(int gridId)
        {
            _logger.LogTrace($"Сервис: {nameof(BoardService)}. Метод: {nameof(CreateNewBoardForUser)}." +
                             $"Аргументы: {nameof(gridId)}: {gridId}.");

            var grid = await _readOnlyRepository.GetOne<Grid>(filter: x => x.Id == gridId);

            if (grid is null)
            {
                throw new NotFoundException($"Доска с таким Id не найдена: {gridId}!");
            }

            var filledBoard = BoardGeneration(out var fullBoard);
            grid.FilledBoardData = filledBoard.ToString().Replace(" ", "").Replace("\n", "");
            grid.FullBoardData = fullBoard.ToString().Replace(" ", "").Replace("\n", "");
            _repository.Update(grid);
            await _repository.Save();
        }

        private Board BoardGeneration(out Board fullBoard)
        {
            int RandomFunction()
            {
                var random = new Random();
                return random.Next(1, 20);
            }

            var filledBoard = Factory.Puzzle(RandomFunction(), RandomFunction(), RandomFunction(), RandomFunction());
            fullBoard = filledBoard.Fill.Sequential();
            return filledBoard;
        }

        private async Task<Grid> CreateAndGetNewGrid(int userId)
        {
            _logger.LogTrace($"Сервис: {nameof(BoardService)}. Метод: {nameof(CreateAndGetNewGrid)}." +
                             $"Аргументы: {nameof(userId)}: {userId}.");
            
            var filledBoard = BoardGeneration(out var fullBoard);

            var newUserBoard = new Grid()
            {
                UserId = userId,
                FullBoardData = fullBoard.ToString().Replace(" ", "").Replace("\n", ""),
                FilledBoardData = filledBoard.ToString().Replace(" ", "").Replace("\n", ""),
                IsActive = true
            };
            _repository.Add(newUserBoard);

            await _repository.Save();

            return newUserBoard;
        }
    }
}