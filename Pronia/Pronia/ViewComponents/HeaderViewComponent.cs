using Microsoft.AspNetCore.Mvc;
using Pronia.Services.Interfaces;
using Pronia.ViewModels;

namespace Pronia.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private readonly ILayoutService _layoutService; 

        public HeaderViewComponent(ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            LayoutVM model = new()
            {
                Settings = _layoutService.GetSettingDatas(),

            };
            return await Task.FromResult(View(model)); 
        }
    }
}


