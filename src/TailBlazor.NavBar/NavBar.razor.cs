using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;

namespace TailBlazor.NavBar
{
    public partial class NavBar : ComponentBase
    {
        #region parameters
        /// <summary>
        /// The container class for the nav.
        /// Default class: bg-blue-500 color-primary
        /// Note: If color-primary is not defined, it will default to blue-500.
        /// </summary>
        [Parameter]
        public string NavContainerClass { get; set; }

        /// <summary>
        /// The main div containter class. This is where you can set width, padding etc...
        /// Default class: {(!MaxWidthFull ? "max-w-7xl" : "")} mx-auto px-2 sm:px-6
        /// Note; If MaxWidthFull is true we do not set a max width here. Default is false.
        /// </summary>
        [Parameter]
        public string MainDivContainerClass { get; set; }

        /// <summary>
        /// The mobile menu button class, button will only show on mobile views.
        /// Default class: inline-flex items-center justify-center p-2 rounded-md text-white hover:text-white hover:bg-blue-700 hover:bg-colorPrimaryHover focus:outline-none focus:ring-2 focus:ring-inset focus:ring-white
        /// Note: If colorPrimaryHover is not declared in your tailwind.config.js we default to blue-700.
        /// </summary>
        [Parameter]
        public string MobileMenuButtonClass { get; set; }

        /// <summary>
        /// A flag to check if the navbar will be full with or max-width of 80rem.
        /// Default: False
        /// </summary>
        [Parameter]
        public bool MaxWidthFull { get; set; }

        /// <summary>
        /// The mobile menu button position, either left or right. 
        /// Default: MobileMenuButtonPosition.Left
        /// </summary>
        [Parameter]
        public MobileMenuButtonPosition MobileMenuButtonPosition { get; set; }

        /// <summary>
        /// The logo information, such as fullwidthsizeurl, mobilesizeurl, alt, class.
        /// </summary>
        [Parameter]
        public NavLogoInformation LogoInformation { get; set; }

        /// <summary>
        /// A dictionary with the position of the nav items, and a list of nav items.
        /// This will automatically place a list of items in the navbar without having to do any custom work.
        /// Note: If you end up doing custom work, it will override this.
        /// </summary>
        //[Parameter]
        //public Dictionary<NavPosition, List<NavItem>> NavItems { get; set; }

        /// <summary>
        /// The items that will be placed on the left side of the nav.
        /// Note: This will override any of the [Parameter] NavItems on the left position.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// The mobile content, this will show up when in mobile view and you have clicked the hamburger menu to open.
        /// Note: If you have supplied a [Parameter] NavItems, this will override all the content that was created for the mobile view.
        /// </summary>
        [Parameter]
        public RenderFragment MobileContent { get; set; }

        /// <summary>
        /// The mobile menu button content, in case you would like to change the buttons that are used for the hamburger menu.
        /// Default: Hamburger on closed, X on open
        /// </summary>
        [Parameter]
        public RenderFragment MobileMenuButtonContent { get; set; }
        #endregion

        #region injection services
        [Inject]
        protected NavigationManager _navManager { get; set; }
        #endregion

        #region variables
        // Boolean Variables
        protected bool mobileMenuOpen = false;

        // String Variables
        protected string relativePath = "";

        // RenderFragment Variables
        protected RenderFragment MobileLeftContent { get; set; }
        protected RenderFragment MobileCenterContent { get; set; }
        protected RenderFragment MobileRightContent { get; set; }
        #endregion

        #region initialization
        protected override void OnInitialized()
        {
            OnLocationChanged(null, null);

            // hook to OnLocationChanged() so we can change the active class for the element
            _navManager.LocationChanged += OnLocationChanged;

            // if the nav has a logo, setup the logo for the navbar
            //if (LogoInformation != null)
            //{
            //    RenderFragment navLogo = BuildNavLogoFragment();

            //    // depending on the position for the logo, add it to the correct position
            //    if (LogoInformation.Position == NavLogoPosition.Left)
            //    {
            //        ChildContent = navLogo + LeftChildContent;
            //    }
            //    else if (LogoInformation.Position == NavLogoPosition.Center)
            //    {
            //        CenterChildContent = navLogo + CenterChildContent;
            //    }
            //    else if (LogoInformation.Position == NavLogoPosition.Right)
            //    {
            //        RightChildContent = navLogo + RightChildContent;
            //    }
            //}

            // go through all the nav items and build the html for it
            // this is assuming that there is no custom work and the user has only passed a NavItems param
            //foreach (var item in NavItems)
            //{
            //    if (item.Key == NavPosition.Left &&
            //        ChildContent == null)
            //        BuildForLeft(item.Value);

            //    if (item.Key == NavPosition.Center &&
            //        ChildContent == null)
            //        BuildForCenter(item.Value);

            //    if (item.Key == NavPosition.Right &&
            //        ChildContent == null)
            //        BuildForRight(item.Value);
            //}
        }
        #endregion

        #region protected functions
        /// <summary>
        /// Toggle the mobile menu when the user clicks the hamburger icon
        /// </summary>
        protected void HamburgerClicked()
        {
            mobileMenuOpen = !mobileMenuOpen;
        }
        #endregion

        #region private functions
        /// <summary>
        /// A listener when the location has changed,
        /// sets the relative path state variable to change the active class for the element on the nav bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLocationChanged(object sender, LocationChangedEventArgs e)
        {
            relativePath = _navManager.ToBaseRelativePath(_navManager.Uri);
            StateHasChanged();
        }

        /// <summary>
        /// Build the nav items, which are a tags
        /// Based on the NavItem return it will add the appropriate attributes/content
        /// </summary>
        /// <param name="navItem">a list of nav items</param>
        /// <returns>a renderfragment</returns>
        private RenderFragment BuildNavItemFragment(NavItem navItem)
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
        #endregion
    }
}
