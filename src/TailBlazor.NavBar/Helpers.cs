using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace TailBlazor.NavBar
{
    public class Helpers
    {
        /// <summary>
        /// Build the nav items, which are a tags
        /// Based on the NavItem return it will add the appropriate attributes/content
        /// </summary>
        /// <param name="navItem">a list of nav items</param>
        /// <returns>a renderfragment</returns>
        public static RenderFragment BuildNavItemFragment(NavItem navItem, string relativePath)
        {
            //default class
            string htmlClass = $"{(navItem.HasIcon ? "flex items-center gap-x-2" : "")} text-white hover:bg-blue-700 hover:bg-colorPrimaryHover hover:text-white px-3 py-2 rounded-md text-sm font-medium";

            if (!string.IsNullOrEmpty(navItem.Class))
                htmlClass = navItem.Class;

            if (string.IsNullOrEmpty(relativePath) && string.IsNullOrEmpty(navItem.Href.TrimStart('/')) ||
                !string.IsNullOrEmpty(navItem.Href.TrimStart('/')) && relativePath.Contains(navItem.Href.TrimStart('/')))
            {
                if (string.IsNullOrEmpty(navItem.ActiveItemClass))
                {
                    htmlClass = $"{(navItem.HasIcon ? "flex items-center gap-x-2" : "")} bg-blue-700 color-primary-active text-white px-3 py-2 rounded-md text-sm font-medium";
                }
                else
                {
                    htmlClass = navItem.ActiveItemClass;
                }
            }

            RenderFragment item = i =>
            {
                i.OpenElement(0, "a");
                i.AddAttribute(1, "id", navItem.Id);
                i.AddAttribute(2, "class", htmlClass);

                if (!string.IsNullOrEmpty(navItem.Href))
                    i.AddAttribute(3, "href", navItem.Href);

                if (navItem.Action != null)
                    i.AddAttribute(4, "onclick", navItem.Action);

                if (navItem.PreventDefaultClick)
                    i.AddEventPreventDefaultAttribute(5, "onclick", true);

                if (navItem.Target != NavLinkTarget.Undefined)
                    i.AddAttribute(6, "target", $"_{navItem.Target.ToString().ToLower()}");

                if (navItem.HasIcon)
                    i.AddContent(7, navItem.Icon);

                i.AddContent(8, navItem.Name);

                i.CloseElement();
            };

            return item;
        }

        /// <summary>
        /// Builds the navbar logo
        /// </summary>
        /// <returns>a renderfragment</returns>
        public static RenderFragment BuildNavLogoFragment(NavLogoInformation logoInfomation)
        {
            RenderFragment logo = l =>
            {
                l.OpenElement(0, "div");
                l.AddAttribute(1, "class", "flex-shrink-0 flex items-center");
                
                l.OpenElement(2, "img");
                l.AddAttribute(3, "class", string.IsNullOrEmpty(logoInfomation.Class) ? "block lg:hidden h-8 w-auto" : logoInfomation.Class);
                l.AddAttribute(4, "src", logoInfomation.MobileScreenUrl);
                l.AddAttribute(5, "alt", logoInfomation.Alt);
                l.CloseElement();

                l.OpenElement(2, "img");
                l.AddAttribute(3, "class", string.IsNullOrEmpty(logoInfomation.Class) ? "hidden lg:block h-8 w-auto" : logoInfomation.Class);
                l.AddAttribute(4, "src", logoInfomation.FullScreenUrl);
                l.AddAttribute(5, "alt", logoInfomation.Alt);
                l.CloseElement();
                
                l.CloseElement();
            };

            return logo;
        }
    }
}
