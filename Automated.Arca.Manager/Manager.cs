﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Libraries;

namespace Automated.Arca.Manager
{
	public class Manager : IManager
	{
		private readonly ManagerOptions Options;

		private readonly object Lock = new object();

		private readonly IDictionary<string, CachedAssembly> CachedAssemblies = new Dictionary<string, CachedAssembly>();
		private readonly IDictionary<Type, CachedType> CachedTypes = new Dictionary<Type, CachedType>();
		private readonly IDictionary<Type, IExtensionDependency> ExtensionDependencies =
			new Dictionary<Type, IExtensionDependency>();
		private readonly IDictionary<Type, IExtensionForProcessableAttribute> ExtensionInstancesByType =
			new Dictionary<Type, IExtensionForProcessableAttribute>();
		private readonly IDictionary<Type, Type> ExtensionTypesByAttributeType = new Dictionary<Type, Type>();

		public Manager( ManagerOptions options )
		{
			Options = options;

			LogOptions();
		}

		public IManager AddAssembly( Assembly assembly )
		{
			lock( Lock )
			{
				Options.AddAssemblyNamePrefix( assembly.GetName().Name! );

				CacheReferencedAssembliesAndTypesAndExtensions( assembly );

				return this;
			}
		}

		public IManager AddAssemblyFromFile( string assemblyFile )
		{
			lock( Lock )
			{
				var assembly = Assembly.LoadFrom( assemblyFile );

				Options.AddAssemblyNamePrefix( assembly.GetName().Name! );

				CacheReferencedAssembliesAndTypesAndExtensions( assembly );

				return this;
			}
		}

		public IManager AddAssemblyContainingType( Type type )
		{
			lock( Lock )
			{
				Options.AddAssemblyNamePrefix( type.Assembly.GetName().Name! );

				CacheReferencedAssembliesAndTypesAndExtensions( type.Assembly );

				return this;
			}
		}

		public IManager AddEntryAssembly()
		{
			lock( Lock )
			{
				var assembly = Assembly.GetEntryAssembly()!;
				var name = assembly.GetName().Name!;
				Options.AddAssemblyNamePrefix( name );

				CacheReferencedAssembliesAndTypesAndExtensions( assembly );

				return this;
			}
		}

		public IManager AddAssembliesLoadedInProcess()
		{
			lock( Lock )
			{
				var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

				foreach( var assembly in assemblies )
					CacheReferencedAssembliesAndTypesAndExtensions( assembly );

				return this;
			}
		}

		public IManager AddExtensionDependency( Type baseType, IExtensionDependency baseTypeImplementation )
		{
			lock( Lock )
			{
				if( !ExtensionDependencies.ContainsKey( baseType ) )
				{
					baseTypeImplementation.GetType().EnsureDerivesFrom( baseType );

					ExtensionDependencies[ baseType ] = baseTypeImplementation;
				}

				return this;
			}
		}

		public IManager AddExtensionDependency<BaseType>( IExtensionDependency baseTypeImplementation )
		{
			return AddExtensionDependency( typeof( BaseType ), baseTypeImplementation );
		}

		public object GetExtensionDependency( Type type )
		{
			lock( Lock )
			{
				if( !ExtensionDependencies.ContainsKey( type ) )
				{
					throw new InvalidOperationException( $"There is no extension dependency registered for the type" +
						$" '{type.Name}'. Register one with the '{nameof( AddExtensionDependency )}' method, before" +
						$" calling the '{nameof( Register )}' and '{nameof( Configure )}' methods." );
				}

				return ExtensionDependencies[ type ];
			}
		}

		public T GetExtensionDependency<T>()
		{
			return (T)GetExtensionDependency( typeof( T ) );
		}

		public IManager Register()
		{
			lock( Lock )
			{
				var context = new RegistrationContext( this );

				RegisterAssemblies( context );

				return this;
			}
		}

		public IManager Configure()
		{
			lock( Lock )
			{
				var context = new ConfigurationContext( this );

				ConfigureAssemblies( context );

				return this;
			}
		}

		private void LogOptions()
		{
			var logger = Options.Logger;

			if( logger == null )
				return;

			var prefixes = Options.ProcessOnlyAssemblyNamesPrefixedWith.JoinWithFormat( "'{0}'", ", " );
			logger.Log( $"Using the assembly name prefix list: {prefixes}" );

			if( !Options.ProcessOnlyClassesDerivedFromIProcessable )
			{
				logger.Log( $"Use the manager option '{nameof( ManagerOptions.UseOnlyClassesDerivedFromIProcessable )}' to" +
					$" significantly speed up the processing; if you do that, every class to register or configure must" +
					$" derive from '{nameof( IProcessable )}'." );
			}
		}

		private void CacheReferencedAssembliesAndTypesAndExtensions( Assembly assembly )
		{
			if( !MayAddAssemblyToCache( assembly ) )
				return;

			var watch = System.Diagnostics.Stopwatch.StartNew();

			CacheReferencedAssemblies( assembly );

			CacheTypes();
			CacheExtensions();

			TypesAndExtensionsAreCached();

			Options.Logger?.Log( $"Method '{nameof( AddAssembly )}' executed in {watch.ElapsedMilliseconds} ms." );
		}

		private void TypesAndExtensionsAreCached()
		{
			foreach( var kvp in CachedAssemblies )
				kvp.Value.TypesAndExtensionsAreCached = true;
		}

		private void CacheReferencedAssemblies( Assembly assembly )
		{
			if( !MayAddAssemblyToCache( assembly ) )
				return;

			AddAssemblyToCache( assembly );

			var assemblyNames = assembly.GetReferencedAssemblies();

			foreach( var assemblyName in assemblyNames )
			{
				if( MayAddAssemblyToCache( assemblyName ) )
					CacheReferencedAssemblies( Assembly.Load( assemblyName ) );
			}
		}

		private void AddAssemblyToCache( Assembly assembly )
		{
			var cachedAssembly = new CachedAssembly( assembly );
			CachedAssemblies.Add( cachedAssembly.FullName, cachedAssembly );

			Options.Logger?.Log( $"Cached assembly '{assembly.GetName().Name}'" );
		}

		private bool MayAddAssemblyToCache( Assembly assembly )
		{
			return MayAddAssemblyToCache( assembly.GetName() );
		}

		private bool MayAddAssemblyToCache( AssemblyName assemblyName )
		{
			return !CachedAssemblies.ContainsKey( assemblyName.FullName ) && AssemblyNameMatchesPrefix( assemblyName.Name! );
		}

		private bool AssemblyNameMatchesPrefix( string name )
		{
			return Options.ProcessOnlyAssemblyNamesPrefixedWith.Count > 0 &&
				Options.ProcessOnlyAssemblyNamesPrefixedWith.Any( x => name.StartsWith( x, StringComparison.OrdinalIgnoreCase ) );
		}

		private void CacheTypes()
		{
			foreach( var kvp in CachedAssemblies )
				CacheTypes( kvp.Value );
		}

		private void CacheTypes( CachedAssembly cachedAssembly )
		{
			if( cachedAssembly.TypesAndExtensionsAreCached )
				return;

			foreach( var type in cachedAssembly.Assembly.GetExportedTypes() )
			{
				var cachedType = new CachedType( type );
				CachedTypes.Add( cachedType.Type, cachedType );
			}
		}

		private void CacheExtensions()
		{
			foreach( var kvp in CachedTypes )
				CacheExtension( kvp.Value );
		}

		private void CacheExtension( CachedType cachedType )
		{
			if( !MayCacheExtensionType( cachedType ) )
			{
				if( cachedType.State == ProcessingState.None )
					cachedType.State = ProcessingState.CachedExtension;

				return;
			}

			if( ExtensionInstancesByType.ContainsKey( cachedType.Type ) )
				return;

			var extension = Activator.CreateInstance( cachedType.Type ) as IExtensionForProcessableAttribute;

			if( ExtensionTypesByAttributeType.ContainsKey( extension!.AttributeType ) )
			{
				throw new InvalidOperationException( $"The attribute '{extension.AttributeType.Name}' already" +
					$" has a cached extension '{ExtensionTypesByAttributeType[ extension.AttributeType ].Name}'." );
			}

			extension.AttributeType.EnsureDerivesFrom( typeof( ProcessableAttribute ) );

			ExtensionInstancesByType.Add( cachedType.Type, extension );
			ExtensionTypesByAttributeType.Add( extension.AttributeType, cachedType.Type );

			cachedType.State = ProcessingState.CachedExtension;

			Options.Logger?.Log( $"Cached extension '{cachedType.Type.Name}' for attribute '{extension.AttributeType.Name}'" );
		}

		private bool MayCacheExtensionType( CachedType cachedType )
		{
			if( cachedType.State >= ProcessingState.CachedExtension )
				return false;

			if( cachedType.State != ProcessingState.None )
			{
				throw new InvalidOperationException( $"The state of type '{cachedType.Type.Name}' must be" +
					$" '{ProcessingState.None}' but is '{cachedType.State}'." );
			}

			var type = cachedType.Type;

			return type.IsClass && !type.IsAbstract && typeof( IExtensionForProcessableAttribute ).IsAssignableFrom( type );
		}

		private IExtensionForProcessableAttribute GetExtensionForAttribute( Type attributeType )
		{
			if( !ExtensionTypesByAttributeType.ContainsKey( attributeType ) )
			{
				throw new InvalidOperationException( $"The attribute '{attributeType.Name}' doesn't have a cached" +
					" extension. Check the documentation how to implement one. If you already did, note that the" +
					" compiler removes the references to the assemblies which are not used in code, so it's not enough" +
					" to see an assembly reference in your IDE. Something from the referenced assembly must be used in code" +
					" all the way (reference by reference) to the root assembly, or you can add an assembly with any of the" +
					" 'AddXXX' manager methods." );
			}

			var extensionType = ExtensionTypesByAttributeType[ attributeType ];

			if( !ExtensionInstancesByType.ContainsKey( extensionType ) )
			{
				throw new InvalidOperationException( $"The attribute '{attributeType.Name}' doesn't have a cached" +
					$" instance for the extension '{extensionType.Name}'." );
			}

			return ExtensionInstancesByType[ extensionType ];
		}

		private void RegisterAssemblies( IRegistrationContext context )
		{
			var watch = System.Diagnostics.Stopwatch.StartNew();

			RegisterTypes( context );
			RunRegistrators( context );

			if( CachedTypes.Count <= 0 )
				Options.Logger?.Log( $"Calling '{nameof( Register )}' without first caching an assembly is pointless." );

			Options.Logger?.Log( $"Method '{nameof( RegisterAssemblies )}' executed in {watch.ElapsedMilliseconds} ms." );
		}

		private void ConfigureAssemblies( IConfigurationContext context )
		{
			var watch = System.Diagnostics.Stopwatch.StartNew();

			ConfigureTypes( context );
			RunConfigurators( context );

			Options.Logger?.Log( $"Method '{nameof( ConfigureAssemblies )}' executed in {watch.ElapsedMilliseconds} ms." );
		}

		private void RegisterTypes( IRegistrationContext context )
		{
			foreach( var kvp in CachedTypes )
				RegisterType( context, kvp.Value );
		}

		private void ConfigureTypes( IConfigurationContext context )
		{
			foreach( var kvp in CachedTypes )
				ConfigureType( context, kvp.Value );
		}

		private void RegisterType( IRegistrationContext context, CachedType cachedType )
		{
			if( !MayRegisterType( cachedType ) )
			{
				if( cachedType.State == ProcessingState.CachedExtension )
					cachedType.State = ProcessingState.Registered;

				return;
			}

			// This also returns the attributes derived from "ProcessableAttribute".
			var attribute = GetProcessableAttribute( cachedType.Type );
			if( attribute == null )
			{
				cachedType.State = ProcessingState.Registered;

				return;
			}

			RegisterType( context, cachedType.Type, attribute );

			cachedType.State = ProcessingState.Registered;
		}

		private bool MayRegisterType( CachedType cachedType )
		{
			if( cachedType.State >= ProcessingState.Registered )
				return false;

			if( cachedType.State != ProcessingState.CachedExtension )
			{
				throw new InvalidOperationException( $"The state of type '{cachedType.Type.Name}' must be" +
					$" '{ProcessingState.CachedExtension}' but is '{cachedType.State}'." );
			}

			var type = cachedType.Type;

			return type.IsClass && !type.IsAbstract &&
				(!Options.ProcessOnlyClassesDerivedFromIProcessable || typeof( IProcessable ).IsAssignableFrom( type ));
		}

		private ProcessableAttribute? GetProcessableAttribute( Type type )
		{
			var attributes = type.GetCustomAttributes<ProcessableAttribute>( false );
			if( attributes == null )
				return null;

			var count = attributes.Count();
			if( count <= 0 )
				return null;

			if( count > 1 )
			{
				throw new InvalidOperationException( $"Class '{type.Name}' may have applied on it only one attribute" +
					$" (derived from the '{nameof( ProcessableAttribute )}' attribute)." );
			}

			return attributes.SingleOrDefault();
		}

		private void RegisterType( IRegistrationContext context, Type type, ProcessableAttribute attribute )
		{
			var attributeType = attribute.GetType();
			var extension = GetExtensionForAttribute( attributeType );

			if( extension == null )
				throw new ArgumentOutOfRangeException( $"Unhandled attribute '{attributeType.Name}'" );

			extension.Register( context, attribute, type );

			Options.Logger?.Log( $"Registered class '{type.Name}' with attribute '{attributeType.Name}'" );
		}

		private void ConfigureType( IConfigurationContext context, CachedType cachedType )
		{
			if( !MayConfigureType( cachedType ) )
			{
				if( cachedType.State == ProcessingState.RegistratorRan )
					cachedType.State = ProcessingState.Configured;

				return;
			}

			var attribute = GetProcessableAttribute( cachedType.Type );
			if( attribute == null )
			{
				cachedType.State = ProcessingState.Configured;

				return;
			}

			ConfigureType( context, cachedType.Type, attribute );

			cachedType.State = ProcessingState.Configured;
		}

		private bool MayConfigureType( CachedType cachedType )
		{
			if( cachedType.State >= ProcessingState.Configured )
				return false;

			if( cachedType.State != ProcessingState.RegistratorRan )
			{
				throw new InvalidOperationException( $"The state of type '{cachedType.Type.Name}' must be" +
					$" '{ProcessingState.Registered}' but is '{cachedType.State}'." );
			}

			var type = cachedType.Type;

			return type.IsClass && !type.IsAbstract &&
				(!Options.ProcessOnlyClassesDerivedFromIProcessable || typeof( IProcessable ).IsAssignableFrom( type ));
		}

		private void ConfigureType( IConfigurationContext context, Type type, ProcessableAttribute attribute )
		{
			var attributeType = attribute.GetType();
			var extension = GetExtensionForAttribute( attributeType );

			if( extension == null )
				throw new ArgumentOutOfRangeException( $"Unhandled attribute '{attributeType.Name}'" );

			extension.Configure( context, attribute, type );

			Options.Logger?.Log( $"Configured class '{type.Name}' with attribute '{attributeType.Name}'" );
		}

		private void RunRegistrators( IRegistrationContext context )
		{
			foreach( var kvp in CachedTypes )
				RunRegistrator( context, kvp.Value );
		}

		private void RunConfigurators( IConfigurationContext context )
		{
			foreach( var kvp in CachedTypes )
				RunConfigurator( context, kvp.Value );
		}

		private void RunRegistrator( IRegistrationContext context, CachedType cachedType )
		{
			if( !MayRunRegistrator( cachedType ) )
			{
				if( cachedType.State == ProcessingState.Registered )
					cachedType.State = ProcessingState.RegistratorRan;

				return;
			}

			var registrator = Activator.CreateInstance( cachedType.Type ) as IRegistrator;

			registrator!.Register( context );

			cachedType.State = ProcessingState.RegistratorRan;

			Options.Logger?.Log( $"Ran registrator '{cachedType.Type.Name}'" );
		}

		private bool MayRunRegistrator( CachedType cachedType )
		{
			if( cachedType.State >= ProcessingState.RegistratorRan )
				return false;

			if( cachedType.State != ProcessingState.Registered )
			{
				throw new InvalidOperationException( $"The state of type '{cachedType.Type.Name}' must be" +
					$" '{ProcessingState.Registered}' but is '{cachedType.State}'." );
			}

			var type = cachedType.Type;

			return type.IsClass && !type.IsAbstract && typeof( IRegistrator ).IsAssignableFrom( type ) &&
				type != typeof( IRegistrator );
		}

		private void RunConfigurator( IConfigurationContext context, CachedType cachedType )
		{
			if( !MayRunConfigurator( cachedType ) )
			{
				if( cachedType.State == ProcessingState.Configured )
					cachedType.State = ProcessingState.ConfiguratorRan;

				return;
			}

			var registrator = Activator.CreateInstance( cachedType.Type ) as IConfigurator;

			registrator!.Configure( context );

			cachedType.State = ProcessingState.ConfiguratorRan;

			Options.Logger?.Log( $"Ran configurator '{cachedType.Type.Name}'" );
		}

		private bool MayRunConfigurator( CachedType cachedType )
		{
			if( cachedType.State >= ProcessingState.ConfiguratorRan )
				return false;

			if( cachedType.State != ProcessingState.Configured )
			{
				throw new InvalidOperationException( $"The state of type '{cachedType.Type.Name}' must be" +
					$" '{ProcessingState.Configured}' but is '{cachedType.State}'." );
			}

			var type = cachedType.Type;

			return type.IsClass && !type.IsAbstract && typeof( IConfigurator ).IsAssignableFrom( type ) &&
				type != typeof( IConfigurator );
		}
	}
}