# Introduction #

The Domain has some [AOP](http://en.wikipedia.org/wiki/Aspect-oriented_programming) basic features as it uses [Castle Dynamic Proxy](http://stw.castleproject.org/Default.aspx?Page=DynamicProxy&NS=Tools&AspxAutoDetectCookieSupport=1).

# How it works #

Before any call a of component's methods, all the configured dynamic proxies are executed. By default there's a proxy for:
  * Authorisation: it'll check whether the connected user can execute or not this feature and throw an exception if he/she isn't granted to.
  * Session checking: before any execution of a component's method, a proxy checks if the nHibernate session is configured and throws an exception is it's not configured.
  * Logging and historisation: So far, a log4net logger writes any call of component's methods.

# How to tweak it? #

## `[Removed]` Session checking ##

You can avoid the execution of the session check by decorating your method with `InspectionIgnored` attribute

## Automatic transaction ##

Every call of a component's method is wrapped in a session. That's, a session is opened before the call and closed just after.

If the developer doesn't use the `ExcludeFromTransaction` attribute, the call is wrapped into a transaction otherwise, it is only wrapped in a nHibernate's session

## Authorisation ##
You an decorate the methods of the DAL with `GrantedAttribute` that uses a string that defines the granted task. You can decorate a class or its members. When a class is decorated, it overrides all the member decoration.

Default behaviour without decoration is explained [here](CodingConventions#authorisation_management.md)