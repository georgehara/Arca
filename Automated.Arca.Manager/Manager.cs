using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Libraries;

namespace Automated.Arca.Manager
{
	public class Manager : IManager
	{
		private readonly IManagerOptions Options;
		private readonly bool SimulateOnlyUnprocessableTypes;

		private readonly IDictionary<string, CachedAssembly> CachedAssemblies = new Dictionary<string, CachedAssembly>();
		private readonly CachedTypes CachedTypes;
		private readonly IDictionary<Type, IExtensionDependency> ExtensionDependencies =
			new Dictionary<Type, IExtensionDependency>();
		private readonly IDictionary<Type, IExtensionForProcessableAttribute> ExtensionInstancesByType =
			new Dictionary<Type, IExtensionForProcessableAttribute>();
		private readonly IDictionary<Type, Type> ExtensionTypesByAttributeType = new Dictionary<Type, Type>();

		private readonly object Lock = new object();

		public IManagerStatistics Statistics { get; } = new ManagerStatistics();

		public Manager( IManagerOptions options, bool simulateOnlyUnprocessableTypes = false )
		{
			Options = options;
			SimulateOnlyUnprocessableTypes = simulateOnlyUnprocessableTypes;

			CachedTypes = new CachedTypes( Options.PriorityTypes );

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
				var assembly = LoadAssemblyFromFile( assemblyFile );

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

		public IManager AddAssemblyContainingType<T>()
		{
			return AddAssemblyContainingType( typeof( T ) );
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

				if( Options.Logger != null )
					Options.Logger.Log( $"Processing {assemblies.Count} assemblies loaded into the current process." );

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
					baseTypeImplementation.GetType().EnsureDerivesFromNotEqual( baseType );

					ExtensionDependencies[ baseType ] = baseTypeImplementation;
				}

				return this;
			}
		}

		public IManager AddExtensionDependency<BaseType>( IExtensionDependency baseTypeImplementation )
		{
			return AddExtensionDependency( typeof( BaseType ), baseTypeImplementation );
		}

		public bool ContainsExtensionDependency( Type type )
		{
			lock( Lock )
			{
				return ExtensionDependencies.ContainsKey( type );
			}
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

				Register( context );

				return this;
			}
		}

		public IManager Configure()
		{
			lock( Lock )
			{
				var context = new ConfigurationContext( this );

				Configure( context );

				return this;
			}
		}

		public IEnumerable<Type> GetPriorityTypes()
		{
			lock( Lock )
			{
				return CachedTypes.GetPriorityTypes().Select( x => x.Type ).ToList();
			}
		}

		private void LogOptions()
		{
			var logger = Options.Logger;

			if( logger == null )
				return;

			var prefixes = Options.ProcessOnlyAssemblyNamesPrefixedWith.JoinWithFormat( "'{0}'", ", " );
			logger.Log( $"Using the assembly name prefix list: {prefixes}" );

			var excludedNames = Options.ExcludedAssemblyNames.JoinWithFormat( "'{0}'", ", " );
			logger.Log( $"Assembly names to exclude: {excludedNames}" );

			if( !Options.ProcessOnlyClassesDerivedFromIProcessable )
			{
				logger.Log( $"Use the manager option '{nameof( ManagerOptions.UseOnlyClassesDerivedFromIProcessable )}' to" +
					$" significantly speed up the processing; if you do that, every class to register or configure must" +
					$" derive from '{nameof( IProcessable )}'." );
			}

			var excludeTypes = Options.ExcludedTypes.Select( x => x.Name ).JoinWithFormat( "'{0}'", ", " );
			logger.Log( $"Excluded types: {excludeTypes}" );

			var priorityTypes = Options.PriorityTypes.GetOrderedTypes().Select( x => x.Name ).JoinWithFormat( "'{0}'", ", " );
			logger.Log( $"Priority types: {priorityTypes}" );
		}

		private Assembly LoadAssemblyFromFile( string assemblyFile )
		{
			var watch = System.Diagnostics.Stopwatch.StartNew();

			var assembly = Assembly.LoadFrom( assemblyFile );

			long elapsedMilliseconds = watch.ElapsedMilliseconds;

			Statistics.LoadedAssemblies++;
			Statistics.AssemblyLoadingTime += elapsedMilliseconds;

			Options.Logger?.Log( $"Method '{nameof( LoadAssemblyFromFile )}' for assembly file '{assemblyFile}' executed in" +
				$" {elapsedMilliseconds} ms." );

			return assembly;
		}

		private Assembly LoadAssemblyWithName( AssemblyName assemblyName )
		{
			var watch = System.Diagnostics.Stopwatch.StartNew();

			var assembly = Assembly.Load( assemblyName );

			long elapsedMilliseconds = watch.ElapsedMilliseconds;

			Statistics.LoadedAssemblies++;
			Statistics.AssemblyLoadingTime += elapsedMilliseconds;

			Options.Logger?.Log( $"Method '{nameof( LoadAssemblyWithName )}' for assembly '{assemblyName.Name}' executed in" +
				$" {elapsedMilliseconds} ms." );

			return assembly;
		}

		private void CacheReferencedAssembliesAndTypesAndExtensions( Assembly assembly )
		{
			var watch = System.Diagnostics.Stopwatch.StartNew();

			if( !MayAddAssemblyToCache( assembly ) )
				return;

			CacheReferencedAssemblies( assembly );

			CacheTypes();
			CacheExtensions();

			TypesAndExtensionsAreCached();

			long elapsedMilliseconds = watch.ElapsedMilliseconds;

			Statistics.CachingTime += elapsedMilliseconds;

			Options.Logger?.Log( $"Method '{nameof( CacheReferencedAssembliesAndTypesAndExtensions )}' for assembly" +
				$" '{assembly.GetName().Name}' executed in {elapsedMilliseconds} ms." );
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
				// Avoid loading the assembly before checking if it's processable.
				if( MayAddAssemblyToCache( assemblyName ) )
					CacheReferencedAssemblies( LoadAssemblyWithName( assemblyName ) );
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
			return !assembly.IsDynamic && MayAddAssemblyToCache( assembly.GetName() );
		}

		private bool MayAddAssemblyToCache( AssemblyName assemblyName )
		{
			return !CachedAssemblies.ContainsKey( assemblyName.FullName ) && AssemblyNameMatchesPrefix( assemblyName.Name! ) &&
				!Options.ExcludedAssemblyNames.Contains( assemblyName.Name! );
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
				if( Options.ExcludedTypes.Contains( type ) )
				{
					Options.Logger?.Log( $"Type '{type.Name}' is excluded from processing." );
				}
				else
				{
					var cachedType = new CachedType( type );
					CachedTypes.Add( cachedType.Type, cachedType );

					Statistics.CachedTypes++;
				}
			}
		}

		private void CacheExtensions()
		{
			CachedTypes.ForAll( x => CacheExtension( x ) );
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

			var extension = (IExtensionForProcessableAttribute)Activator.CreateInstance( cachedType.Type )!;

			if( ExtensionTypesByAttributeType.ContainsKey( extension.AttributeType ) )
			{
				throw new InvalidOperationException( $"The attribute '{extension.AttributeType.Name}' already" +
					$" has a cached extension '{ExtensionTypesByAttributeType[ extension.AttributeType ].Name}'." );
			}

			extension.AttributeType.EnsureDerivesFromNotEqual( typeof( ProcessableAttribute ) );

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

		private void Register( IRegistrationContext context )
		{
			var watch = System.Diagnostics.Stopwatch.StartNew();

			if( !CachedTypes.HasItems )
			{
				Options.Logger?.Log( $"Calling '{nameof( Register )}' without first caching an assembly is pointless." +
					$" If you already did that, do the classes to register implement the '{nameof( IProcessable )}' interface?" );
			}

			RegisterTypes( context );
			RunRegistrators( context );

			long elapsedMilliseconds = watch.ElapsedMilliseconds;

			Statistics.ClassRegistrationTime += elapsedMilliseconds;

			Options.Logger?.Log( $"Method '{nameof( Register )}' executed in {elapsedMilliseconds} ms." +
				$" Registered {Statistics.RegisteredClasses} classes out of {Statistics.CachedTypes} cached types." );
		}

		private void Configure( IConfigurationContext context )
		{
			var watch = System.Diagnostics.Stopwatch.StartNew();

			ConfigureTypes( context );
			RunConfigurators( context );

			long elapsedMilliseconds = watch.ElapsedMilliseconds;

			Statistics.ClassConfigurationTime += elapsedMilliseconds;

			Options.Logger?.Log( $"Method '{nameof( Configure )}' executed in {elapsedMilliseconds} ms." );
		}

		private void RegisterTypes( IRegistrationContext context )
		{
			CachedTypes.ForAll( x => RegisterType( context, x ) );
		}

		private void ConfigureTypes( IConfigurationContext context )
		{
			CachedTypes.ForAll( x => ConfigureType( context, x ) );
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
			if( attribute == null || SimulateOnlyUnprocessableTypes )
			{
				cachedType.ProcessableAttribute = null;
				cachedType.State = ProcessingState.Registered;

				return;
			}
			else
			{
				cachedType.ProcessableAttribute = attribute;
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

			if( extension.BaseInterfaceOfTypeWithAttribute != null )
				type.EnsureDerivesFromInterfaceOrGenericInterfaceWithUntypedParameter( extension.BaseInterfaceOfTypeWithAttribute );

			extension.Register( context, attribute, type );

			Statistics.RegisteredClasses++;

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

			if( !cachedType.HasProcessableAttribute || SimulateOnlyUnprocessableTypes )
			{
				cachedType.State = ProcessingState.Configured;

				return;
			}

			ConfigureType( context, cachedType.Type, cachedType.ProcessableAttribute! );

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

			// The conditions were already checked during registration.
			return true;
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
			CachedTypes.ForAll( x => RunRegistrator( context, x ) );
		}

		private void RunConfigurators( IConfigurationContext context )
		{
			CachedTypes.ForAll( x => RunConfigurator( context, x ) );
		}

		private void RunRegistrator( IRegistrationContext context, CachedType cachedType )
		{
			if( !MayRunRegistrator( cachedType ) || SimulateOnlyUnprocessableTypes )
			{
				if( cachedType.State == ProcessingState.Registered )
					cachedType.State = ProcessingState.RegistratorRan;

				return;
			}

			var registrator = (IRegistrator)Activator.CreateInstance( cachedType.Type )!;

			registrator.Register( context );

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
			if( !MayRunConfigurator( cachedType ) || SimulateOnlyUnprocessableTypes )
			{
				if( cachedType.State == ProcessingState.Configured )
					cachedType.State = ProcessingState.ConfiguratorRan;

				return;
			}

			var registrator = (IConfigurator)Activator.CreateInstance( cachedType.Type )!;

			registrator.Configure( context );

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
