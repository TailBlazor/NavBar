using Microsoft.AspNetCore.Components;
using System;

namespace TailBlazor.NavBar
{
    /// <summary>
    /// The information for the navbar logo
    /// </summary>
    public class NavLogoInformation
    {
        public string FullScreenUrl { get; set; }
        public string MobileScreenUrl { get; set; }
        public string Alt { get; set; }
        public string Class { get; set; }
    }

    /// <summary>
    /// The nav item, contains basically everything that is in an <a></a> element
    /// </summary>
    public class NavItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Href { get; set; }
        public string ActiveItemClass { get; set; }
        public bool PreventDefaultClick { get; set; }
        public NavLinkTarget Target { get; set; }
        public Action Action { get; set; }
        public bool HasIcon { get; set; }
        public RenderFragment Icon { get; set; }

        /// <summary>
        /// set the id as soon as a user uses this class
        /// </summary>
        public NavItem()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
