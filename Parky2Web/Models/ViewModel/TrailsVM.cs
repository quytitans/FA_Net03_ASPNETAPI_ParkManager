using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Parky2Web.Models.ViewModel
{
    public class TrailsVM
    {
        [ValidateNever]
        public IEnumerable<SelectListItem> NationalParkList { get; set; }
        public Trail Trail { get; set; }
    }
}
