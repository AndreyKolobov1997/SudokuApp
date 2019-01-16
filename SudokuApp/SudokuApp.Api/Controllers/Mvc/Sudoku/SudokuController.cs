using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SudokuApp.Api.Controllers.Mvc.Sudoku.Board;
using SudokuApp.Api.Shared.Models;
using SudokuApp.Bl.ServiceInterfaces;
using SudokuApp.Common.Extensions;
using SudokuApp.Data.Model.Output;

namespace SudokuApp.Api.Controllers.Mvc.Sudoku
{
    public class SudokuController : Controller
    {
        private IBoardService _boardService;
        private IMapper _mapper;

        public SudokuController(IBoardService boardService, IMapper mapper)
        {
            _boardService = boardService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Board()
        {
            var model = new BoardViewModel()
            {
                IsAliveGame = false
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Board([FromForm] BoardViewModel model)
        {
            if (model.UserName.IsNullOrEmpty())
            {
                ModelState.AddModelError(nameof(model.UserName), "Имя пользователя не задано!");
                return View(model);
            }

            var boardOutput = await _boardService.GetBoardByUserName(model.UserName);
            
            var updatedModel =
                _mapper.Map(boardOutput, model, typeof(GetBoardOutput), typeof(BoardViewModel)) as BoardViewModel;
            updatedModel.IsAliveGame = true;

            return View(updatedModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveBoardFilled([FromForm]UpdateBoardRequestModel model)
        {
            await _boardService.SaveBoardForUser(model.BoardId, model.NewFilledBoard);
            return Json("Доска успешно сохранена!");
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateNewBoard([FromForm]CreateNewBoardRequestModel model)
        {
            await _boardService.CreateNewBoardForUser(model.BoardId);
            return Json("Доска успешно создана!");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}