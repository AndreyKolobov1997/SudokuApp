using AutoMapper;
using SudokuApp.Api.Controllers.Mvc.Sudoku.Board;
using SudokuApp.Data.Entity;
using SudokuApp.Data.Model.Output;

namespace SudokuApp.Api.Automapper
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Grid, GetBoardOutput>()
                .ForMember(dest => dest.BoardId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FilledBoard, opt => opt.MapFrom(src => src.FilledBoardData))
                .ForMember(dest => dest.FullBoard, opt => opt.MapFrom(src => src.FullBoardData));
            
            CreateMap<GetBoardOutput, BoardViewModel>()
                .ForMember(dest => dest.BoardId, opt => opt.MapFrom(src => src.BoardId))
                .ForMember(dest => dest.BoardFilled, opt => opt.MapFrom(src => src.FilledBoard))
                .ForMember(dest => dest.BoardFull, opt => opt.MapFrom(src => src.FullBoard));
        }
    }
}