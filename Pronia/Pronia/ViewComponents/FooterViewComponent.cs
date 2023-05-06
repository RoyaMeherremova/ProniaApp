using Microsoft.AspNetCore.Mvc;
using Pronia.Services.Interfaces;
using Pronia.ViewModels;

namespace Pronia.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        private readonly ISocialService _socialService;
        private readonly ILayoutService _layoutService;

        public FooterViewComponent(ISocialService socialService,ILayoutService layoutService)
        {
            _socialService = socialService;
            _layoutService = layoutService;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            FooterVM model = new FooterVM()
            {
                Socials = await _socialService.GetAllSocials(),
                Settings =  _layoutService.GetSettingDatas()
                
            };
            return await Task.FromResult(View(model));
        }
    }
}
