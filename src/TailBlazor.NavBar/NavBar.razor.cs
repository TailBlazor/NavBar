using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;

namespace TailBlazor.NavBar
{
    public partial class NavBar : ComponentBase
    {
        #region default values
        // default nav logo position
        private NavLogoPosition _logoPosition = NavLogoPosition.Left;
        #endregion

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
        /// The left items container class.
        /// Default class: flex-1 flex items-center justify-center sm:items-stretch sm:justify-start
        /// </summary>
        [Parameter]
        public string LeftItemsContainerClass { get; set; }

        /// <summary>
        /// The center items container class.
        /// Default class: hidden flex-1 items-center justify-center sm:flex sm:items-stretch sm:justify-center
        /// </summary>
        [Parameter]
        public string CenterItemsContainerClass { get; set; }

        /// <summary>
        /// The right items container class.
        /// Default class: hidden flex-1 items-center justify-center sm:flex sm:items-stretch sm:justify-end
        /// </summary>
        [Parameter]
        public string RightItemsContainerClass { get; set; }

        /// <summary>
        /// The mobile menu button class, button will only show on mobile views.
        /// Default class: inline-flex items-center justify-center p-2 rounded-md text-white hover:text-white hover:bg-blue-700 hover:bg-colorPrimaryHover focus:outline-none focus:ring-2 focus:ring-inset focus:ring-white
        /// Note: If colorPrimaryHover is not declared in your tailwind.config.js we default to blue-700.
        /// </summary>
        [Parameter]
        public string MobileMenuButtonClass { get; set; }

        /// <summary>
        /// A parameter to check if the logo is included, default is false.
        /// Default: False
        /// </summary>
        [Parameter]
        public bool HasLogo { get; set; }

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
        /// The position of the logo image.
        /// Default: NavLogoPosition.Left
        /// </summary>
        [Parameter]
        public NavLogoPosition LogoPosition
        {
            get => _logoPosition;
            set
            {
                if (LogoPosition != 0)
                {
                    _logoPosition = value;
                }
            }
        }

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
        [Parameter]
        public Dictionary<NavPosition, List<NavItem>> NavItems { get; set; }

        /// <summary>
        /// The items that will be placed on the left side of the nav.
        /// Note: This will override any of the [Parameter] NavItems on the left position.
        /// </summary>
        [Parameter]
        public RenderFragment LeftItems { get; set; }

        /// <summary>
        /// The items that will be placed on the center of the nav.
        /// Note: This will override any of the [Parameter] NavItems on the center position.
        /// </summary>
        [Parameter]
        public RenderFragment CenterItems { get; set; }

        /// <summary>
        /// The items that will be placed on the right side of the nav.
        /// Note: This will override any of the [Parameter] NavItems  on the right position.
        /// </summary>
        [Parameter]
        public RenderFragment RightItems { get; set; }

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
        protected RenderFragment MobileLeftItems { get; set; }
        protected RenderFragment MobileCenterItems { get; set; }
        protected RenderFragment MobileRightItems { get; set; }
        #endregion

        #region initialization
        protected override void OnInitialized()
        {
            OnLocationChanged(null, null);

            // hook to OnLocationChanged() so we can change the active class for the element
            _navManager.LocationChanged += OnLocationChanged;

            // if the nav has a logo, setup the logo for the navbar
            if (HasLogo)
            {
                RenderFragment navLogo = BuildNavLogoFragment();

                // depending on the position for the logo, add it to the correct position
                if (LogoPosition == NavLogoPosition.Left)
                {
                    LeftItems += navLogo;
                }
                else if (LogoPosition == NavLogoPosition.Center)
                {
                    CenterItems += navLogo;
                }
                else if (LogoPosition == NavLogoPosition.Right)
                {
                    RightItems += navLogo;
                }
            }

            // go through all the nav items and build the html for it
            // this is assuming that there is no custom work and the user has only passed a NavItems param
            foreach (var item in NavItems)
            {
                if (item.Key == NavPosition.Left &&
                    LeftItems == null)
                    BuildForLeft(item.Value);

                if (item.Key == NavPosition.Center &&
                    CenterItems == null)
                    BuildForCenter(item.Value);

                if (item.Key == NavPosition.Right &&
                    RightItems == null)
                    BuildForRight(item.Value);
            }
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
        /// Build the renderfragment for the left of the nav bar
        /// </summary>
        /// <param name="navItems">a list of nav items</param>
        private void BuildForLeft(List<NavItem> navItems)
        {
            RenderFragment item = i =>
            {
                i.OpenElement(0, "div");
                i.AddAttribute(1, "class", "hidden sm:block sm:ml-6");
                i.OpenElement(2, "div");
                i.AddAttribute(3, "class", "flex space-x-4");

                foreach (var navItem in navItems)
                {
                    var buildNavItem = BuildNavItemFragment(navItem);
                    i.AddContent(4, buildNavItem);

                    MobileLeftItems = buildNavItem;
                }

                i.CloseElement();
                i.CloseElement();
            };

            LeftItems += item;
        }

        /// <summary>
        /// Build the renderfragment for the center of the nav bar
        /// </summary>
        /// <param name="navItems">a list of nav items</param>
        private void BuildForCenter(List<NavItem> navItems)
        {
            RenderFragment item = i =>
            {
                i.OpenElement(0, "div");
                i.AddAttribute(1, "class", "hidden sm:block sm:ml-6");
                i.OpenElement(2, "div");
                i.AddAttribute(3, "class", "flex space-x-4");

                foreach (var navItem in navItems)
                {
                    var buildNavItem = BuildNavItemFragment(navItem);
                    i.AddContent(4, buildNavItem);

                    MobileCenterItems = buildNavItem;
                }

                i.CloseElement();
                i.CloseElement();
            };

            CenterItems += item;
        }

        /// <summary>
        /// Build the renderfragment for the right of the nav bar
        /// </summary>
        /// <param name="navItems">a list of nav items</param>
        private void BuildForRight(List<NavItem> navItems)
        {
            RenderFragment item = i =>
            {
                i.OpenElement(0, "div");
                i.AddAttribute(1, "class", "hidden sm:block sm:ml-6");
                i.OpenElement(2, "div");
                i.AddAttribute(3, "class", "flex space-x-4");

                foreach (var navItem in navItems)
                {
                    var buildNavItem = BuildNavItemFragment(navItem);
                    i.AddContent(4, buildNavItem);

                    MobileRightItems = buildNavItem;
                }

                i.CloseElement();
                i.CloseElement();
            };

            RightItems += item;
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

        /// <summary>
        /// Builds the navbar logo
        /// </summary>
        /// <returns>a renderfragment</returns>
        private RenderFragment BuildNavLogoFragment()
        {
            RenderFragment logo = l =>
            {
                l.OpenElement(0, "div");
                l.AddAttribute(1, "class", "flex-shrink-0 flex items-center");
                l.OpenElement(2, "img");
                l.AddAttribute(3, "class", string.IsNullOrEmpty(LogoInformation.Class) ? "block lg:hidden h-8 w-auto" : LogoInformation.Class);
                l.AddAttribute(4, "src", LogoInformation.MobileScreenUrl);
                l.AddAttribute(5, "alt", LogoInformation.Alt);
                l.CloseElement();
                l.OpenElement(2, "img");
                l.AddAttribute(3, "class", string.IsNullOrEmpty(LogoInformation.Class) ? "hidden lg:block h-8 w-auto" : LogoInformation.Class);
                l.AddAttribute(4, "src", LogoInformation.FullScreenUrl);
                l.AddAttribute(5, "alt", LogoInformation.Alt);
                l.CloseElement();
                l.CloseElement();
            };

            return logo;
        }
        #endregion
    }
}
