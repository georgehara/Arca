# Automated Registration and Configuration (of Classes) through Attributes (ARCA)


## PURPOSE

The purpose of ARCA is to perform automated registration and configuration of classes.

The main use cases are the (parametrized) registration and configuration of classes for:
* Dependency injection
* CQRS: registration of events, commands, handlers
* Custom registries that are not meant for dependency injection

ARCA doesn't depend on a dependency injection container. It doesn't even know what dependency injection is; that's known only to a few dedicated packages.


### MIXING MANUAL AND AUTOMATED REGISTRATION

ARCA can be introduced progressively into an existing project because it's possible to mix manual and automated dependency injection registration. You should not register the same class both manually and automatically.


## HOW IT WORKS

Attributes are used to mark classes that have to be registered or configured, and to specify (complex) parameters for processing.

Attributes are processed by the ARCA manager, through extensions.

The combination between attributes and extensions allows you to easily perform any kind of automated registration and configuration.

Both attributes and extensions can be in the consumer code.


## PACKAGE DESCRIPTIONS

* `Automated.Arca.Abstractions.Core` - Core abstractions. Contains `IProcessable`. Use it to create your own attributes and extensions. It's usually necessary.
* `Automated.Arca.Abstractions.DependencyInjection` - Dependency injection abstractions.
* `Automated.Arca.Abstractions.Specialized` - Specialized abstractions. Use for middleware and CQRS. Implement these interfaces in your CQRS implementation.
* `Automated.Arca.Attributes.DependencyInjection` - Dependency injection attributes to apply on classes to register / configure.
* `Automated.Arca.Attributes.Specialized` - Specialized attributes to apply on classes to register / configure. Use for middleware and CQRS.
* `Automated.Arca.Extensions.DependencyInjection` - Dependency injection extensions for the dependency injection attributes.
* `Automated.Arca.Extensions.Specialized` - Specialized extensions for the specialized attributes. Use for middleware and CQRS.
* `Automated.Arca.Implementations.ForMicrosoft` - Implementations for Microsoft's dependency injection.
* `Automated.Arca.Libraries` - Libraries for other packages.
* `Automated.Arca.Manager` - The ARCA manager. Use during the startup of an application.
* `Automated.Arca.Single` - All other packages merged together. Use instead of the individual packages.

The `ForMicrosoft` packages contain dependencies which are meant to be used in applications that use the Microsoft dependency injection container.


Developed by https://github.com/georgehara/Arca . The full documentation is included there.
