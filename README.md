# Automated Registration and Configuration (of Classes) through Attributes (ARCA)


[Purpose](#purpose)<br/>
[How it works](#how-it-works)<br/>
[Attributes versus conventions](#attributes-versus-conventions)<br/>
[Attributes](#attributes)<br/>
[Extensions](#extensions)<br/>
[Processable classes](#processable-classes)<br/>
[Processing with complex input parameters](#processing-with-complex-input-parameters)<br/>
[Processing options](#processing-options)<br/>
[Inter-class dependencies](#inter-class-dependencies)<br/>
[Scopes](#scopes)<br/>
[Dependencies registered by default](#dependencies-registered-by-default)<br/>
[Middleware](#middleware)<br/>
[Performance considerations](#performance-considerations)<br/>
[Package descriptions](#package-descriptions)<br/>
[Examples](#examples)<br/>
[Demo](#demo)<br/>


## PURPOSE

The purpose of ARCA is to perform automated registration and configuration of classes.

The main use cases are the (parametrized) registration and configuration of classes for:
* Dependency injection
* CQRS: registration of events, commands, handlers
* Custom registries that are not meant for dependency injection

ARCA doesn't depend on a dependency injection container.


## HOW IT WORKS

Attributes are used to mark classes that have to be registered or configured, and to specify (complex) parameters for processing.

Attributes are processed by the ARCA manager, through extensions.

The combination between attributes and extensions allows you to easily perform any kind of automated registration and configuration.

Both attributes and extensions can be in the consumer code.

The manager ensures that all the attributes that are applied on classes have extensions that handle them.


### DEPENDENCY ISOLATION

ARCA doesn't depend on a dependency injection container. There is a single project which depends on and contains implementations for Microsoft's dependency injection container, which is passed through the application. You can easily reimplement this project for the desired container.

![Dependency isolation](DependencyIsolation.png?raw=true)


## ATTRIBUTES VERSUS CONVENTIONS

Conventions, and the associated scanning filters, must be manually maintained and enforced in order to prevent class changes from breaking conventions and scanning filters. On top of this, this maintenance is difficult because the registration code and the classes to register are in separate places in code, so the developer must dig in the code in order to create a fix.

Attributes eliminate the need for conventions. An attribute is always together with the class on which it's applied, so it's very easy to change the attribute if the behavior of the class changes.


## ATTRIBUTES

An attribute which marks a class for registration or configuration must derive from the `ProcessableAttribute` attribute; `ProcessableAttribute` may come from (anywhere in) the inheritance tree of the attribute.

An attribute must be applied on the class that it marks for registration or configuration, it's not enough to be applied on an ancestor of the class.

A class may have applied on it only one attribute (derived from the `ProcessableAttribute` attribute).

Some attributes allow you to specify an interface with which the (dependency injection) registration has to be made. They also offer you the option to not specify the interface, in which case the registration will be made with the default interface of the class on which the attribute is applied. The default interface of a class is considered to be the interface from which the class derives on the first ancestor level that has an interface; such an interface may be implemented either by the class itself, or by an ancestor class.


## EXTENSIONS

An extension is a class which derives from the `IExtensionForAttribute` interface; the interface may come from (anywhere in) the inheritance tree of the extension class.

The extension specifies which attribute it handles, in the `AttributeType` property, and provides methods for the registration and configuration of the class on which the attribute is applied; the attribute can't come from the inheritance tree of the class to process.


### REGISTER

The `Register` method registers a class (on which the attribute which is handled by the extension is applied on) in various registers, like a dependency injection register.


### CONFIGURE

The `Configure` method requests an instance (of a certain class) from the dependency injection provider, and calls a method on it, passing to it various parameters which may come from the attribute that the extension is handling.

If you create custom extensions, it's important to understand that this method is not called every time an instance of that class is created, but only once per dependency injection container. This means that configuring an instance (of that class) which is not instantiated per container (as a so called singleton) is a logical flaw; it's fine to configure the class itself because types are singletons.

You can achieve the same thing by using the implementation factory parameter during `Register`, and the factory would be called every time the class would be instantiated. But this means that you would have to manually create the instance in the extension, so you would create tight coupling between the extension of the class to instantiate.


### EXTENSION DEPENDENCIES

Extension dependencies allow ARCA to be completely independent from the internal business of the extensions, but at the same time it can still pass such dependencies from the application to the extensions.

Extension dependencies can be added to the manager with the `AddExtensionDependencyXXX` manager methods.

Examples of application dependencies which can be added as extension dependencies: `IConfiguration`, `IServiceCollection`, `IServiceProvider`.


## PROCESSABLE CLASSES

A class which can be marked for registration or configuration must:
* Be public and concrete (= not abstract).
* Have applied on it an attribute which derives from `ProcessableAttribute`; the attribute must be applied on the class, it's not enough to be applied on an ancestor of the class.
* Optionally derive from `IProcessable`; any ancestor of the class may derive from this interface. The manager can be instructed to ignore this interface.


## PROCESSING WITH COMPLEX INPUT PARAMETERS

### REGISTRATORS AND CONFIGURATORS

If the registration / configuration of a class requires complex input parameters, you can handle it in a registrator / configurator.

A registrator must derive from `IRegistrator`.

A configurator must derive from `IConfigurator`.

The classes which are registered and configured by registrators and configurators should not have applied on them attributes (derived from the `ProcessableAttribute` attribute) because the attributes would trigger a separate processing of the classes.


## PROCESSING OPTIONS

The ARCA manager must be instantiated with a set of options that can be created with the following fluent methods:

`UseLogger`: Specifies the logger which handles the processing messages of the manager. A logger significantly reduces the processing performance.

`AddAssemblyNamePrefix`: An assembly is processed only if its name starts with a prefix specified through this method. Each call of this method adds a prefix to the assembly name prefix list.

`ExcludeAssemblyName`: Excludes from processing all assemblies whose names are specified through this method. Each call of this method adds a name to a list.

`UseOnlyClassesDerivedFromIProcessable`: If called, the manager will process only the classes which are derived from the `IProcessable` interface, which significantly speeds up the processing.

`Exclude`: Specifies a type that the manager will not process. This is useful when you want to override the implementation of an interface which is registered in an assembly that you can't modify. By excluding such a type, you can add another implementation, in another assembly.

`Prioritize`: Specifies a type that the manager will process first. The types are processed in the specified order, and before types which are not specified. This is particularly useful for middleware.

Note: The attributes, extensions, registrators and configurators that you want to use must be defined in assemblies whose names start with a prefix from the prefix list. If the "Automated.Arca." prefix is added to the prefix list through the constructor of the options class, you can use the predefined attributes and extensions without specifying this prefix.


### SPECIFYING ASSEMBLIES

The ARCA manager should be instructed to load at least one assembly with the following fluent methods:

`AddAssembly`: Instructs the manager to process the specified assembly. The assembly name is automatically added to the assembly name prefix list.

`AddAssemblyFromFile`: Instructs the manager to process the assembly from the specified file name. The assembly name is automatically added to the assembly name prefix list.

`AddAssemblyContainingType`: Instructs the manager to process the assembly in which the specified type is defined. The assembly name is automatically added to the assembly name prefix list.

`AddEntryAssembly`: Instructs the manager to process the entry assembly (of the current process). The assembly name is automatically added to the assembly name prefix list.

`AddAssembliesLoadedInProcess`: Instructs the manager to process the asemblies which are already loaded (in the current process).

Note: An assembly is always processed only if its name starts with a prefix from the assembly name prefix list.

Note about recursiveness:
* Assembly processing is always recursive, so the assembly references of an assembly are also processed, and so on for references of references.
* The compiler removes the references to the assemblies which are not used in code, so it's not enough to see an assembly reference in your IDE. Something from the referenced assembly must be used in code all the way (reference by reference) to the root assembly, or you can add an assembly with any of the `AddXXX` manager methods.


## INTER-CLASS DEPENDENCIES

After you instantiate the manager, at application startup, and you add assemblies to it, you must call `Register` first, and only then call `Configure`. This is because the manager must register and configure classes based on their dependencies. This is what the manager does:
* Registers classes (based on extensions).
* Runs the registrators. The registrators can't depend on one another because the manager doesn't know the order in which to run the registrators.
* Configures the registered classes (based on extensions).
* Runs the configurators. The configurators can't depend on one another because the manager doesn't know the order in which to run the configurators.

Note: The `Register` and `Configure` manager methods may be called multiple times, but the manager checks the consistency of the state of each type that was loaded (by the manager) with an `AddXXX` manager method. So, for example if you call `AddXXX`, then `Register`, then `AddXXX` again, an exception is thrown because you didn't call `Configure` after `Register`. On top of this, Microsoft's dependency injection container stops registering components once the service provider is built, without throwing an exception, so trying to register new types after `Configure` was called is pointless.

Note: If the order of processing matters, use the `Prioritize` manager option.


## SCOPES

In applications which are not based on client requests, like test projects and desktop applications, the dependency injection container may throw exceptions if you try to instantiate dependencies which were registered to be instantiated per scope. This is because, in such a context, the instantiation per client request is meaningless, and those dependencies should be either:
* Retrieved from a scope, which is recommended.
* Registered to be instantiated per container. This can be done when you call `Register`, by passing `true` to the `instantiatePerContainerInsteadOfScope` parameter of the `InstantiationRegistry` constructor.

Scopes can be managed with an implementation of `ScopeManager`; you also have to implement either `IScopeNameProvider` (if the scope name is set from outside the provider) or `IScopeNameResolver` (if the scope name is set from inside the resolver). On the implementation, apply the `ScopeManagerAttribute` attribute so that ARCA can automatically add it to the dependency injection registry. Check the `TenantScopeUsage` test for a usage example.

Note: Every client request from a WebApi application gets its own scope; this is accessible (and even replaceable) through `IHttpContextAccessor.HttpContext.RequestServices`.


## DEPENDENCIES REGISTERED BY DEFAULT

When dependency injection is used, ARCA adds the following components to the instantiation registry (`IServiceCollection`), to be instantiated per container through the instance provider (`IServiceProvider`):
* `IGlobalInstanceProvider`
* `IInstanceProvider`

You can add any of these interfaces as parameters to the constructors of your classes, so they can be injected by the dependency injection container.


## MIDDLEWARE

Middleware support is provided by the `ChainMiddlewarePerContainerAttribute`, `ChainMiddlewarePerScopeAttribute` and `ChainMiddlewarePerInjectionAttribute` attributes from the `Automated.Arca.Attributes.Specialized` package. The attributes register the middleware, in the dependency injection container, for instantiation per container, scope or injection.

The middleware class must implement the `IMiddleware` interface, and must have applied on it one of the attributes above. Then, when it's time to call it in the request pipeline, ASP.NET will instantiate it through the dependency injection container.

Before you call the `Configure` manager method, call the `AddMiddlewareRegistry` extension method, on the manager. `Configure` must be called before calling the "IApplicationBuilder.UseEndpoints" extension method!

Note: If the order of the middleware in the pipeline matters, use the `Prioritize` manager option.


## PERFORMANCE CONSIDERATIONS

ARCA has to load all the assemblies (referenced by an application) and has to use reflection to scan all the classes at application startup, but this is done only once, no matter how many extensions are used. This means that the more extensions are used to perform all sorts of automated operations, the more effective ARCA becomes.

If manual registration and configuration were used instead of ARCA, the application startup would have to load (almost) the same number of assemblies in order to perform assembly-local operations, so there is little performance advantage in doing manual registration and configuration.

ARCA allows you to specify a prefix that each assembly must have in order to be loaded and scanned.

To improve performance, derive the classes (to register and configure) from the `IProcessable` interface. This works because checking if a class implements an interface is much faster (50 times) than calling `Type.GetCustomAttributes` for each class. By default, the manager ignores this interface.

To further improve performance:
* Do not pass a logger to the manager options.
* Group all classes to process in a single assembly, or as few assemblies as possible, because assembly loading takes most time from the processing. This will have the most significant effect on performance.

Unless you need to register and configure an absolutely enormous number of classes, the performance is fine without the `IProcessable` interface. An approximate performance can be viewed by executing the (release build of the) tests from the `ProcessingPerformanceTests` class. The number of assemblies involved and their loading time is not relevant in these tests because the assemblies are already loaded in the process. Some assembly loading is cached by .Net, but it's a small time, so it can be ignored here by repeating the tests. However, .Net caches some other things, so each test must be run separately. Because the number of classes that are registered and configured is very small compared to the number of processed types, and since most of the time spent there would also have to be spent during manual registration and configuration, it can be ignored.

The tests process about 10'000 types, and register and configure 30 classes. The relevant execution time for registration and configuration is about 33 ms (if the `IProcessable` interface is not used, it's 37 ms). This means that ARCA can process about 300'000 unprocessable types per second, and this matters because the time spent processing unprocessable types is not spent during manual registration and configuration.


## PACKAGE DESCRIPTIONS

* Automated.Arca.Abstractions.Core - Core abstractions. Contains `IProcessable`, so it's usually necessary. Use to create your own attributes and extensions.
* Automated.Arca.Abstractions.DependencyInjection - Dependency injection abstractions.
* Automated.Arca.Abstractions.Specialized - Specialized abstractions. Use for middleware and CQRS. Implement these interfaces in your CQRS implementation.
* Automated.Arca.Attributes.DependencyInjection - Dependency injection attributes to apply on classes to register / configure.
* Automated.Arca.Attributes.Specialized - Specialized attributes to apply on classes to register / configure. Use for middleware and CQRS.
* Automated.Arca.Extensions.DependencyInjection - Dependency injection extensions for the dependency injection attributes.
* Automated.Arca.Extensions.Specialized - Specialized extensions for the specialized attributes. Use for middleware and CQRS.
* Automated.Arca.Implementations.ForMicrosoft - Implementations for Microsoft's dependency injection.
* Automated.Arca.Libraries - Libraries for other packages.
* Automated.Arca.Manager - The ARCA manager. Use during the startup of an application.

The "ForMicrosoft" packages contain extension dependencies which are meant to be used in applications that use the Microsoft dependency injection container.


## EXAMPLES

For more examples look in the "Automated.Arca.Tests" project.


### STARTING THE ARCA MANAGER

Create an ASP.NET / WebApi application and install the following packages:
* Automated.Arca.Extensions.DependencyInjection
* Automated.Arca.Extensions.Specialized
* Automated.Arca.Implementations.ForMicrosoft
* Automated.Arca.Manager

```
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Extensions.DependencyInjection;
using Automated.Arca.Extensions.Specialized;
using Automated.Arca.Implementations.ForMicrosoft;
using Automated.Arca.Manager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FooCorp
{
	public class Startup
	{
		private readonly IConfiguration ApplicationOptionsProvider;
		private readonly IManager Manager;

		public Startup( IConfiguration options )
		{
			ApplicationOptionsProvider = options;

			var managerOptions = new ManagerOptions()
				.AddAssemblyNamePrefix( "FooCorp" )
				.UseOnlyClassesDerivedFromIProcessable();

			Manager = new Manager.Manager( managerOptions )
				.AddEntryAssembly()
				.AddAssemblyContainingType( typeof( ExtensionForInstantiatePerScopeAttribute ) )
				.AddAssemblyContainingType( typeof( ExtensionForBoundedContextAttribute ) )
				.AddKeyedOptionsProvider( ApplicationOptionsProvider );
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices( IServiceCollection services )
		{
			// ...

			Manager
				.AddInstantiationRegistry( services, false, true )
				.Register();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
		{
			// ...

			// When using middleware, "Manager.Configure" must be called before "app.UseEndpoints"!
			Manager
				.AddGlobalInstanceProvider( app.ApplicationServices )
				.AddMiddlewareRegistry( app )
				.Configure();

			app.UseEndpoints( endpoints => endpoints.MapControllers() );
		}
	}
}
```


### REGISTER A COMPONENT TO INSTANTIATE PER SCOPE

```
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Tests.Dummies
{
	[InstantiatePerScopeAttribute]
	public class SomeInstantiatePerScopeComponent : IProcessable
	{
	}
}
```


### REGISTER A COMPONENT WITH AN INTERFACE TO INSTANTIATE PER SCOPE

```
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Tests.Dummies
{
	public interface ISomeInstantiatePerScopeComponentWithInterface : IProcessable
	{
	}

	[InstantiatePerScopeWithInterfaceAttribute( typeof( ISomeInstantiatePerScopeComponentWithInterface ) )]
	public class SomeInstantiatePerScopeComponentWithInterface : ISomeInstantiatePerScopeComponentWithInterface
	{
	}
}
```


## DEMO

"Automated.Arca.Demo.WebApi" is a demo application which displays in the browser the logs generated by the ARCA manager.

Here is a sample output:

```
Created instance of 'CollectorLogger' at 2020-08-15T16:16:47
Using the assembly name prefix list: 'Automated.Arca.'
Assembly names to exclude: 
Excluded types: 
Priority types: 
Cached assembly 'Automated.Arca.Demo.WebApi'
Cached assembly 'Automated.Arca.Manager'
Cached assembly 'Automated.Arca.Abstractions.Core'
Cached assembly 'Automated.Arca.Libraries'
Cached assembly 'Automated.Arca.Attributes.Specialized'
Cached assembly 'Automated.Arca.Attributes.DependencyInjection'
Cached assembly 'Automated.Arca.Extensions.DependencyInjection'
Cached assembly 'Automated.Arca.Abstractions.DependencyInjection'
Cached assembly 'Automated.Arca.Extensions.Specialized'
Cached assembly 'Automated.Arca.Abstractions.Specialized'
Cached assembly 'Automated.Arca.Implementations.ForMicrosoft'
Cached extension 'ExtensionForInstantiatePerContainerAttribute' for attribute 'InstantiatePerContainerAttribute'
Cached extension 'ExtensionForInstantiatePerContainerWithInterfaceAttribute' for attribute 'InstantiatePerContainerWithInterfaceAttribute'
Cached extension 'ExtensionForInstantiatePerInjectionAttribute' for attribute 'InstantiatePerInjectionAttribute'
Cached extension 'ExtensionForInstantiatePerInjectionWithInterfaceAttribute' for attribute 'InstantiatePerInjectionWithInterfaceAttribute'
Cached extension 'ExtensionForInstantiatePerScopeAttribute' for attribute 'InstantiatePerScopeAttribute'
Cached extension 'ExtensionForInstantiatePerScopeWithInterfaceAttribute' for attribute 'InstantiatePerScopeWithInterfaceAttribute'
Cached extension 'ExtensionForScopeManagerAttribute' for attribute 'ScopeManagerAttribute'
Cached extension 'ExtensionForScopeNameProviderAttribute' for attribute 'ScopeNameProviderAttribute'
Cached extension 'ExtensionForScopeNameResolverAttribute' for attribute 'ScopeNameResolverAttribute'
Cached extension 'ExtensionForBoundedContextAttribute' for attribute 'BoundedContextAttribute'
Cached extension 'ExtensionForChainMiddlewarePerContainerAttribute' for attribute 'ChainMiddlewarePerContainerAttribute'
Cached extension 'ExtensionForChainMiddlewarePerInjectionAttribute' for attribute 'ChainMiddlewarePerInjectionAttribute'
Cached extension 'ExtensionForChainMiddlewarePerScopeAttribute' for attribute 'ChainMiddlewarePerScopeAttribute'
Cached extension 'ExtensionForCommandHandlerAttribute' for attribute 'CommandHandlerAttribute'
Cached extension 'ExtensionForCommandHandlerRegistryAttribute' for attribute 'CommandHandlerRegistryAttribute'
Cached extension 'ExtensionForDomainEventHandlerAttribute' for attribute 'DomainEventHandlerAttribute'
Cached extension 'ExtensionForDomainEventHandlerRegistryAttribute' for attribute 'DomainEventHandlerRegistryAttribute'
Cached extension 'ExtensionForDomainEventRegistryAttribute' for attribute 'DomainEventRegistryAttribute'
Cached extension 'ExtensionForExternalServiceAttribute' for attribute 'ExternalServiceAttribute'
Cached extension 'ExtensionForHostedServiceAttribute' for attribute 'HostedServiceAttribute'
Cached extension 'ExtensionForIntegrationEventHandlerAttribute' for attribute 'IntegrationEventHandlerAttribute'
Cached extension 'ExtensionForIntegrationEventHandlerRegistryAttribute' for attribute 'IntegrationEventHandlerRegistryAttribute'
Cached extension 'ExtensionForMessageBusSubscribeForExchangeCommandQueueTargetAttribute' for attribute 'MessageBusSubscribeForExchangeCommandQueueTargetAttribute'
Cached extension 'ExtensionForMessageBusSubscribeForExchangePublicationQueueBetweenAttribute' for attribute 'MessageBusSubscribeForExchangePublicationQueueBetweenAttribute'
Cached extension 'ExtensionForOutboxAttribute' for attribute 'OutboxAttribute'
Cached extension 'ExtensionForOutboxProcessorAttribute' for attribute 'OutboxProcessorAttribute'
Method 'AddAssembly' for assembly 'Automated.Arca.Demo.WebApi' executed in 13 ms.
Registered class 'LoggingMiddleware' with attribute 'ChainMiddlewarePerScopeAttribute'
Registered class 'LogsProvider' with attribute 'InstantiatePerScopeAttribute'
Method 'Register' executed in 2 ms. Registered 2 classes out of 120 cached types.
Configured class 'LoggingMiddleware' with attribute 'ChainMiddlewarePerScopeAttribute'
Configured class 'LogsProvider' with attribute 'InstantiatePerScopeAttribute'
Method 'Configure' executed in 1 ms.
Invoked middleware 'LoggingMiddleware':
	* URL: https://localhost:53712/logs
	* Endpoint: Automated.Arca.Demo.WebApi.Controllers.LogsController.Get (Automated.Arca.Demo.WebApi)
	* Request identifier: 8000000f-0001-ff00-583f-29710c7967de
```


Developed by https://github.com/georgehara/Arca
