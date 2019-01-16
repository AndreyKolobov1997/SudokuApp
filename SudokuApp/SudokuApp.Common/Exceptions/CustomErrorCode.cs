using System.ComponentModel.DataAnnotations;

namespace SudokuApp.Common.Exceptions
{
    public enum CustomErrorCode
    {
        UnknownError = 0,

        [Display(Description = "Success")]
        Success = 999,

        [Display(Description = "Internal server error")]
        InternalServerErrorDefault = 1000,
        
        [Display(Description = "Bad user input")]
        BadUserInputDefault = 1010,
        
        [Display(Description = "Not found")]
        NotFoundDefault = 1020,
        
        [Display(Description = "Source IP address not found")]
        GeoLocationForbiddenDefault = 1030,
        
        [Display(Description = "Forbidden")]
        ForbiddenDefault = 1040,
        
        [Display(Description = "Item already exists")]
        ItemAlreadyExistsDefault = 1050,
    }
}
