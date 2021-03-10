# TailBlazor.NavBar

Blazor NavBar is a basic yet customizable NavBar component for Tailwindcss

Without passing anything to it you will get a default styling, with a background of `bg-blue-500`. However you still have full capability of styling and customization on your end at any point in time.

It has the ability to be a single section or multiple for easy customization.

![Nuget](https://img.shields.io/nuget/v/TailBlazor.HeroIcons.svg)

![Demo](screenshot.png)

## Getting Setup

You can install the package via the NuGet package manager just search for TailBlazor.NavBar. You can also install via powershell using the following command.

`Install-Package TailBlazor.NavBar`

Or via the dotnet CLI.

`dotnet add package TailBlazor.NavBar`

### 1. Add Imports

Add line to your \_Imports.razor

```
@using TailBlazor.NavBar
```

### 2. Create NavBar Component

Simply open up a component and add your content.

```
<NavBar NavItems="new List<NavItem>()"></NavBar>
```

If you would like to add custom content instead of using the default list of NavItems, then all you have to do is add one or all of the children to the navbar content.
```
<NavBar>
    <LeftItems>
        ...
    </LeftItems>
    <CenterItems>
        ...
    </CenterItems>
    <LeftItems>
        ...
    </LeftItems>
</NavBar>
```

### 3. Customization

By default you don't have to include anything but the NavItems and the NavLogoInformation (if you would like to add a logo to the navbar)

Parameter | Default Value
--- | ---
`Width` | `64`
`Height` | `64`
`StrokeWidth` | `2`
`Class` | `text-black`
`IconStyle` | `IconStyle.Outline`
`EnableComments` | `false`

<br/><br/>
Width, Height, and StrokeWidth all take an int.

class takes any tailwind text colour you've added to your project: `text-blue-500`. The fill is erased when building the SVG and this is put at a class instead. Because this is set as a class inside the `class` variable you can add additional classes to further customize the icon.

`<TailBlazorHeroIcon class="text-purple-300 transform rotate-45" Icon="HeroIcon.Share" />`

By adding the transform rotate-45 to the class the icon will also be rotated 45 degrees. You can also add animation classes.


IconStyle accepts `.Outline` and `.Solid`

when comments are enabled they can allow it easier for you to remember what you've used when inspecting an element for debugging.
Enabling it shows the following comment above the icon.


`<!-- HeroIcon: annotation (style: outlined, size: 64x64, stroke (colour): text-grey-500, stroke-width: 2) -->`




SPECIAL NOTE TO PURGING TAILWINDCSS.

Sometimes we don't use all the styles and tailwind will purge them. If you find your icon's not styling, make sure stroke-current is added to your website tailwindcss purged code.
