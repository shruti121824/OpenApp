﻿using Abp.Application.Navigation;
using Abp.Localization;
using Satrabel.OpenApp;
using Satrabel.Starter.Web.Authorization;

namespace Satrabel.Starter.Web.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class NavigationProvider : Abp.Application.Navigation.NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                 .AddItem(
                    new MenuItemDefinition(
                        PageNames.Home,
                        L("Home"),
                        url: "",
                        icon: "home",
                        requiredPermissionName: PermissionNames.Pages_Home
                    )
                )
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Home,
                        L("About"),
                        url: "/About",
                        icon: "question",
                        requiredPermissionName: PermissionNames.Pages_About
                    )
                )
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Home,
                        L("ClientApp"),
                        url: "/App/Demo1",
                        icon: "question",
                        requiredPermissionName: PermissionNames.Pages_Home
                    )
                    


                );
            context.Manager.MainMenu.Items.MoveMenuItemToBottom("Admin");
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AppConsts.LocalizationSourceName);
        }
    }
}
