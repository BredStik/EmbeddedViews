MvcCodeRouting
=============================================================================== 

MvcCodeRouting for ASP.NET MVC analyzes your controllers and actions and 
automatically creates the best possible routes for them.

Organize your controllers and views using namespaces (no more areas) that 
can go as deep as you want.

Includes support for hierarchical (a.k.a. RESTful) and also user-defined 
custom routes (using an attribute).

It also supports embedded views (as assembly resources) that can be 
individually overridden by file views.

MvcCodeRouting is a single unified solution that provides namespace-aware 
automatic route creation, URL generation and views location.

Licensing
=============================================================================== 

This software is licensed under the terms you may find in the file
named "LICENSE.txt" of this distribution.

Getting Started
=============================================================================== 

Import the MvcCodeRouting namespace and call the MapCodeRoutes extension 
method:

    void RegisterRoutes(RouteCollection routes) {

        routes.MapCodeRoutes(
            rootController: typeof(Controllers.HomeController),
            settings: new CodeRoutingSettings {
                UseImplicitIdToken = true
            }
        );
    }

To enable namespace-aware views location call the EnableCodeRouting
extension method:

    void RegisterViewEngines(ViewEngineCollection viewEngines) {
      
        // Call AFTER you are done making changes to viewEngines
        viewEngines.EnableCodeRouting();
    }

For Web API support you must install the MvcCodeRouting.Web.Http package.

Please refer to the website for more information
  
  http://mvccoderouting.codeplex.com/

Please help us make MvcCodeRouting better - we appreciate any feedback
you may have.

$ Donate
=============================================================================== 

If you would like to show your appreciation for MvcCodeRouting, please 
consider making a small donation

  http://mvccoderouting.codeplex.com/wikipage?title=Donate

Changes
=============================================================================== 
1.1.1 - Fixed #1155: FromRoute.BinderType ignored by BindRouteProperties

1.1.0 - Copied attributes to Web.Mvc subnamespace, deprecated old attributes 
        although they still work
      - Custom default actions using DefaultAction attribute
      - BinderType property on FromRoute attribute
      - Base route relative (aka absolute) custom route support for controllers
      - Highlight URL on routes.axd
      - Display assembly file version on routes.axd

1.0.2 - Support for loading views embedded in satellite assemblies
      - Fixed #1148: RouteDebugHandler fails if RouteTable has Route with 
        null DataTokens

1.0.1 - URL generation performance improvement
      - Web API support (CTP 1), must add reference to MvcCodeRouting.Web.Http.dll 
        or install MvcCodeRouting.Web.Http pre-release NuGet package

1.0.0 - .NET 3.5 support (compiled against MVC 2)
      - Fixed #1147: Generic methods should not be considered actions

      Special thanks to http://www.codeplex.com/site/users/view/tylerburd ,
      http://www.codeplex.com/site/users/view/Grenaderov and 
      http://www.codeplex.com/site/users/view/JoshuaGough
      for their invaluable feedback.

0.9.8 - Async controller support
      - MVC 4 Release Candidate support
      - Added Configuration setting
      - Fixed #950: Visual Studio build fails after using Extract-Views command
      - Absolute custom route support
      - CustomRoute attribute support for controllers

0.9.7 - Fixed #890: Embedded views don't work when assembly has multipart name
      - Added CodeRoutingSettings.Defaults property
      - Added CodeRoutingSettings.Reset() method
      - Added Extract-Views powershell command, use from Package Manager 
        Console

0.9.6 - Fixed #783: Default action with optional route parameters does not work.
      - Fixed #779: Allow multiple actions with same custom route if {action} 
        token is present.
      - Added RootOnly setting.
      - Removed limitation that required ViewEngineCollection.EnableCodeRouting() 
        to be called after RouteCollection.MapCodeRoutes(), when using embedded 
        views. Now both methods can be called in any order.

0.9.5 - Fixed issues 708, 744, 746 and 747.
      - Automatic constraints for enum parameters and properties, 
        using Enum.GetNames(Type).
      - Custom model binder support for parameters and properties 
        decorated with FromRouteAttribute.

0.9.4 - Added CodeRoutingSettings constructor that takes another 
        CodeRoutingSettings instance to copy the settings from.
      - Added TokenName property to FromRouteAttribute.
      - CodeRoutingConstraint parameter name renamed to __routecontext.
      - Ambiguous route check now done on actions just created and not 
        all registered actions. This allows you to override routes.
      - Added UseImplicitIdToken setting, useful for existing applications 
        that only use a generic {controller}/{action}/{id} route.
      - Case-only constraint on action segment formatting removed, now 
        SomeAction can be formatted as Some-Action, or some_action, etc.
      - BREAKING CHANGE: ~controller syntax now matches routes in the 
        same baseRoute.

0.9.3 - Fixed 2 bugs.
      - Changed RouteFormatter delegate.
      - Removed case-only constraint on namespace and controller segment 
        formatting (e.g. SomeLongNamespace/SomeLongController can be formatted 
        as some-long-namespace/some-long-controller).
      - CodeRoutingConstraint hidden from RouteDebugHandler.
      - Added CustomRouteAttribute for custom routes.
      - Support for embedded views (as assembly resources), enable with 
        CodeRoutingSettings.EnableEmbeddedViews .