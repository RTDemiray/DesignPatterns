using System.Threading.Tasks;
using BaseProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BaseProject.UserCards
{
    //<user-card app-user="" />
    public class UserCardTagHelper : TagHelper
    {
        public UserCardTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        //Property'ler attribute dönüşüyor.

        public AppUser AppUser { get; set; }

        public IHttpContextAccessor _httpContextAccessor { get; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            UserCardTemplate userCardTemplate;

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                userCardTemplate = new PrimeUserCardTemplate();
            }
            else
            {
                userCardTemplate = new DefaultUserCardTemplate();
            }
            
            userCardTemplate.SetUser(AppUser);

            output.Content.SetHtmlContent(userCardTemplate.Build());
            return Task.CompletedTask;
        }
    }
}