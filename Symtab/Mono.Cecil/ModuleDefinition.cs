//
// Author:
//   Jb Evain (jbevain@gmail.com)
//
// Copyright (c) 2008 - 2015 Jb Evain
// Copyright (c) 2008 - 2011 Novell, Inc.
//
// Licensed under the MIT/X11 license.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using SR = System.Reflection;

using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;
using Mono.Cecil.PE;
using Mono.Collections.Generic;

namespace Mono.Cecil {

	public enum ReadingMode {
		Immediate = 1,
		Deferred = 2,
	}

	public sealed class ReaderParameters {

		ReadingMode reading_mode;
		internal IAssemblyResolver assembly_resolver;
		internal IMetadataResolver metadata_resolver;
		internal IMetadataImporterProvider metadata_importer_provider;
		internal IReflectionImporterProvider reflection_importer_provider;
		Stream symbol_stream;
		ISymbolReaderProvider symbol_reader_provider;
		bool read_symbols;
		bool throw_symbols_mismatch;
		bool projections;
		bool in_memory;
		bool read_write;

		public ReadingMode ReadingMode {
			get { return reading_mode; }
			set { reading_mode = value; }
		}

		public bool InMemory {
			get { return in_memory; }
			set { in_memory = value; }
		}

		public IAssemblyResolver AssemblyResolver {
			get { return assembly_resolver; }
			set { assembly_resolver = value; }
		}

		public IMetadataResolver MetadataResolver {
			get { return metadata_resolver; }
			set { metadata_resolver = value; }
		}

		public IMetadataImporterProvider MetadataImporterProvider {
			get { return metadata_importer_provider; }
			set { metadata_importer_provider = value; }
		}

		public IReflectionImporterProvider ReflectionImporterProvider {
			get { return reflection_importer_provider; }
			set { reflection_importer_provider = value; }
		}

		public Stream SymbolStream {
			get { return symbol_stream; }
			set { symbol_stream = value; }
		}

		public ISymbolReaderProvider SymbolReaderProvider {
			get { return symbol_reader_provider; }
			set { symbol_reader_provider = value; }
		}

		public bool ReadSymbols {
			get { return read_symbols; }
			set { read_symbols = value; }
		}

		public bool ThrowIfSymbolsAreNotMatching {
			get { return throw_symbols_mismatch; }
			set { throw_symbols_mismatch = value; }
		}

		public bool ReadWrite {
			get { return read_write; }
			set { read_write = value; }
		}

		public bool ApplyWindowsRuntimeProjections {
			get { return projections; }
			set { projections = value; }
		}

		public ReaderParameters ()
			: this (ReadingMode.Deferred)
		{
		}

		public ReaderParameters (ReadingMode readingMode)
		{
			this.reading_mode = readingMode;
			this.throw_symbols_mismatch = true;
		}
	}

	public sealed class ModuleParameters {

		ModuleKind kind;
		TargetRuntime runtime;
		uint? timestamp;
		TargetArchitecture architecture;
		IAssemblyResolver assembly_resolver;
		IMetadataResolver metadata_resolver;
		IMetadataImporterProvider metadata_importer_provider;
		IReflectionImporterProvider reflection_importer_provider;

		public ModuleKind Kind {
			get { return kind; }
			set { kind = value; }
		}

		public TargetRuntime Runtime {
			get { return runtime; }
			set { runtime = value; }
		}

		public uint? Timestamp {
			get { return timestamp; }
			set { timestamp = value; }
		}

		public TargetArchitecture Architecture {
			get { return architecture; }
			set { architecture = value; }
		}

		public IAssemblyResolver AssemblyResolver {
			get { return assembly_resolver; }
			set { assembly_resolver = value; }
		}

		public IMetadataResolver MetadataResolver {
			get { return metadata_resolver; }
			set { metadata_resolver = value; }
		}

		public IMetadataImporterProvider MetadataImporterProvider {
			get { return metadata_importer_provider; }
			set { metadata_importer_provider = value; }
		}

		public IReflectionImporterProvider ReflectionImporterProvider {
			get { return reflection_importer_provider; }
			set { reflection_importer_provider = value; }
		}

		public ModuleParameters ()
		{
			this.kind = ModuleKind.Dll;
			this.Runtime = GetCurrentRuntime ();
			this.architecture = TargetArchitecture.I386;
		}

		static TargetRuntime GetCurrentRuntime ()
		{
			return typeof (object).Assembly.ImageRuntimeVersion.ParseRuntime ();
		}
	}

	public sealed class WriterParameters {

		uint? timestamp;
		Stream symbol_stream;
		ISymbolWriterProvider symbol_writer_provider;
		bool write_symbols;
		byte [] key_blob;
		string key_container;
		SR.StrongNameKeyPair key_pair;

		public uint? Timestamp {
			get { return timestamp; }
			set { timestamp = value; }
		}

		public Stream SymbolStream {
			get { return symbol_stream; }
			set { symbol_stream = value; }
		}

		public ISymbolWriterProvider SymbolWriterProvider {
			get { return symbol_writer_provider; }
			set { symbol_writer_provider = value; }
		}

		public bool WriteSymbols {
			get { return write_symbols; }
			set { write_symbols = value; }
		}

		public bool HasStrongNameKey {
			get { return key_pair != null || key_blob != null || key_container != null; }
		}

		public byte [] StrongNameKeyBlob {
			get { return key_blob; }
			set { key_blob = value; }
		}

		public string StrongNameKeyContainer {
			get { return key_container; }
			set { key_container = value; }
		}

		public SR.StrongNameKeyPair StrongNameKeyPair {
			get { return key_pair; }
			set { key_pair = value; }
		}

		public bool DeterministicMvid { get; set; }
	}

	public sealed class ModuleDefinition : ModuleReference, ICustomAttributeProvider, ICustomDebugInformationProvider, IDisposable {

		internal Image Image;
		internal MetadataSystem MetadataSystem;
		internal ReadingMode ReadingMode;
		internal ISymbolReaderProvider SymbolReaderProvider;

		internal ISymbolReader symbol_reader;
		internal Disposable<IAssemblyResolver> assembly_resolver;
		internal IMetadataResolver metadata_resolver;
		internal TypeSystem type_system;
		internal readonly MetadataReader reader;
		readonly string file_name;

		internal string runtime_version;
		internal ModuleKind kind;
		WindowsRuntimeProjections projections;
		MetadataKind metadata_kind;
		TargetRuntime runtime;
		TargetArchitecture architecture;
		ModuleAttributes attributes;
		ModuleCharacteristics characteristics;
		Guid mvid;

		internal ushort linker_version = 8;
		internal ushort subsystem_major = 4;
		internal ushort subsystem_minor = 0;
		internal uint timestamp;

		internal AssemblyDefinition assembly;
		MethodDefinition entry_point;

		internal IReflectionImporter reflection_importer;
		internal IMetadataImporter metadata_importer;

		Collection<CustomAttribute> custom_attributes;
		Collection<AssemblyNameReference> references;
		Collection<ModuleReference> modules;
		Collection<Resource> resources;
		Collection<ExportedType> exported_types;
		TypeDefinitionCollection types;

		internal Collection<CustomDebugInformation> custom_infos;

		internal MetadataBuilder metadata_builder;

		public bool IsMain {
			get { return kind != ModuleKind.NetModule; }
		}

		public ModuleKind Kind {
			get { return kind; }
			set { kind = value; }
		}

		public MetadataKind MetadataKind {
			get { return metadata_kind; }
			set { metadata_kind = value; }
		}

		internal WindowsRuntimeProjections Projections {
			get {
				if (projections == null)
					Interlocked.CompareExchange (ref projections, new WindowsRuntimeProjections (this), null);

				return projections;
			}
		}

		public TargetRuntime Runtime {
			get { return runtime; }
			set {
				runtime = value;
				runtime_version = runtime.RuntimeVersionString ();
			}
		}

		public string RuntimeVersion {
			get { return runtime_version; }
			set {
				runtime_version = value;
				runtime = runtime_version.ParseRuntime ();
			}
		}

		public TargetArchitecture Architecture {
			get { return architecture; }
			set { architecture = value; }
		}

		public ModuleAttributes Attributes {
			get { return attributes; }
			set { attributes = value; }
		}

		public ModuleCharacteristics Characteristics {
			get { return characteristics; }
			set { characteristics = value; }
		}

		[Obsolete ("Use FileName")]
		public string FullyQualifiedName {
			get { return file_name; }
		}

		public string FileName {
			get { return file_name; }
		}

		public Guid Mvid {
			get { return mvid; }
			set { mvid = value; }
		}

		internal bool HasImage {
			get { return Image != null; }
		}

		public bool HasSymbols {
			get { return symbol_reader != null; }
		}

		public ISymbolReader SymbolReader {
			get { return symbol_reader; }
		}

		public override MetadataScopeType MetadataScopeType {
			get { return MetadataScopeType.ModuleDefinition; }
		}

		public AssemblyDefinition Assembly {
			get { return assembly; }
		}

		internal IReflectionImporter ReflectionImporter {
			get {
				if (reflection_importer == null)
					Interlocked.CompareExchange (ref reflection_importer, new DefaultReflectionImporter (this), null);

				return reflection_importer;
			}
		}

		internal IMetadataImporter MetadataImporter {
			get {
				if (metadata_importer == null)
					Interlocked.CompareExchange (ref metadata_importer, new DefaultMetadataImporter (this), null);

				return metadata_importer;
			}
		}

		public IAssemblyResolver AssemblyResolver {
			get {
				if (assembly_resolver.value == null) {
					lock (module_lock) {
						assembly_resolver = Disposable.Owned (new DefaultAssemblyResolver () as IAssemblyResolver);
					}
				}

				return assembly_resolver.value;
			}
		}

		public IMetadataResolver MetadataResolver {
			get {
				if (metadata_resolver == null)
					Interlocked.CompareExchange (ref metadata_resolver, new MetadataResolver (this.AssemblyResolver), null);

				return metadata_resolver;
			}
		}

		public TypeSystem TypeSystem {
			get {
				if (type_system == null)
					Interlocked.CompareExchange (ref type_system, TypeSystem.CreateTypeSystem (this), null);

				return type_system;
			}
		}

		public bool HasAssemblyReferences {
			get {
				if (references != null)
					return references.Count > 0;

				return HasImage && Image.HasTable (Table.AssemblyRef);
			}
		}

		public Collection<AssemblyNameReference> AssemblyReferences {
			get {
				if (references != null)
					return references;

				return references = new Collection<AssemblyNameReference> ();
			}
		}

		public bool HasModuleReferences {
			get {
				if (modules != null)
					return modules.Count > 0;

				return HasImage && Image.HasTable (Table.ModuleRef);
			}
		}

		public Collection<ModuleReference> ModuleReferences {
			get {
				if (modules != null)
					return modules;

				return modules = new Collection<ModuleReference> ();
			}
		}

		public bool HasResources {
			get {
				if (resources != null)
					return resources.Count > 0;

				return false;
			}
		}

		public Collection<Resource> Resources {
			get {
				if (resources != null)
					return resources;

				return resources = new Collection<Resource> ();
			}
		}

		public bool HasCustomAttributes {
			get {
				if (custom_attributes != null)
					return custom_attributes.Count > 0;

				return this.GetHasCustomAttributes (this);
			}
		}

		public Collection<CustomAttribute> CustomAttributes {
			get { return custom_attributes ?? (this.GetCustomAttributes (ref custom_attributes, this)); }
		}

		public bool HasTypes {
			get {
				if (types != null)
					return types.Count > 0;

                return false;
            }
		}

		public Collection<TypeDefinition> Types {
			get {
				if (types != null)
					return types;

				return types = new TypeDefinitionCollection (this);
			}
		}

		public bool HasExportedTypes {
			get {
				if (exported_types != null)
					return exported_types.Count > 0;

                return false;
            }
		}

		public Collection<ExportedType> ExportedTypes {
			get {
				if (exported_types != null)
					return exported_types;

				return exported_types = new Collection<ExportedType> ();
			}
		}

		public MethodDefinition EntryPoint {
			get {
				if (entry_point != null)
					return entry_point;
				return entry_point = null;
			}
			set { entry_point = value; }
		}

		public bool HasCustomDebugInformations {
			get {
				return custom_infos != null && custom_infos.Count > 0;
			}
		}

		public Collection<CustomDebugInformation> CustomDebugInformations {
			get {
				return custom_infos ?? (custom_infos = new Collection<CustomDebugInformation> ());
			}
		}

		internal ModuleDefinition ()
		{
			this.MetadataSystem = new MetadataSystem ();
			this.token = new MetadataToken (TokenType.Module, 1);
		}

		internal ModuleDefinition (Image image)
			: this ()
		{
			this.Image = image;
			this.kind = image.Kind;
			this.RuntimeVersion = image.RuntimeVersion;
			this.architecture = image.Architecture;
			this.attributes = image.Attributes;
			this.characteristics = image.Characteristics;
			this.linker_version = image.LinkerVersion;
			this.subsystem_major = image.SubSystemMajor;
			this.subsystem_minor = image.SubSystemMinor;
			this.file_name = image.FileName;
			this.timestamp = image.Timestamp;
		}

		public void Dispose ()
		{
			if (Image != null)
				Image.Dispose ();

			if (symbol_reader != null)
				symbol_reader.Dispose ();

			if (assembly_resolver.value != null)
				assembly_resolver.Dispose ();
		}

		public bool HasTypeReference (string fullName)
		{
			return HasTypeReference (string.Empty, fullName);
		}

		public bool HasTypeReference (string scope, string fullName)
		{
			Mixin.CheckFullName (fullName);

			if (!HasImage)
				return false;

			return GetTypeReference (scope, fullName) != null;
		}

		public bool TryGetTypeReference (string fullName, out TypeReference type)
		{
            throw new Exception();
        }

        public bool TryGetTypeReference (string scope, string fullName, out TypeReference type)
		{
            throw new Exception();
        }

        TypeReference GetTypeReference (string scope, string fullname)
		{
            throw new Exception();
        }

        public IEnumerable<TypeReference> GetTypeReferences ()
		{
			if (!HasImage)
				return Empty<TypeReference>.Array;

            throw new Exception();
        }

        public IEnumerable<MemberReference> GetMemberReferences ()
		{
			if (!HasImage)
				return Empty<MemberReference>.Array;

            throw new Exception();
        }

        public IEnumerable<CustomAttribute> GetCustomAttributes ()
		{
			if (!HasImage)
				return Empty<CustomAttribute>.Array;

            throw new Exception();
        }

		public TypeReference GetType (string fullName, bool runtimeName)
		{
			return runtimeName
				? TypeParser.ParseType (this, fullName, typeDefinitionOnly: true)
				: GetType (fullName);
		}

		public TypeDefinition GetType (string fullName)
		{
			Mixin.CheckFullName (fullName);

			var position = fullName.IndexOf ('/');
			if (position > 0)
				return GetNestedType (fullName);

			return ((TypeDefinitionCollection) this.Types).GetType (fullName);
		}

		public TypeDefinition GetType (string @namespace, string name)
		{
			Mixin.CheckName (name);

			return ((TypeDefinitionCollection) this.Types).GetType (@namespace ?? string.Empty, name);
		}

		public IEnumerable<TypeDefinition> GetTypes ()
		{
			return GetTypes (Types);
		}

		static IEnumerable<TypeDefinition> GetTypes (Collection<TypeDefinition> types)
		{
			for (int i = 0; i < types.Count; i++) {
				var type = types [i];

				yield return type;

				if (!type.HasNestedTypes)
					continue;

				foreach (var nested in GetTypes (type.NestedTypes))
					yield return nested;
			}
		}

		TypeDefinition GetNestedType (string fullname)
		{
			var names = fullname.Split ('/');
			var type = GetType (names [0]);

			if (type == null)
				return null;

			for (int i = 1; i < names.Length; i++) {
				var nested_type = type.GetNestedType (names [i]);
				if (nested_type == null)
					return null;

				type = nested_type;
			}

			return type;
		}

		internal FieldDefinition Resolve (FieldReference field)
		{
			return MetadataResolver.Resolve (field);
		}

		internal MethodDefinition Resolve (MethodReference method)
		{
			return MetadataResolver.Resolve (method);
		}

		internal TypeDefinition Resolve (TypeReference type)
		{
			return MetadataResolver.Resolve (type);
		}

		static void CheckContext (IGenericParameterProvider context, ModuleDefinition module)
		{
			if (context == null)
				return;

			if (context.Module != module)
				throw new ArgumentException ();
		}

		[Obsolete ("Use ImportReference", error: false)]
		public TypeReference Import (Type type)
		{
			return ImportReference (type, null);
		}

		public TypeReference ImportReference (Type type)
		{
			return ImportReference (type, null);
		}

		[Obsolete ("Use ImportReference", error: false)]
		public TypeReference Import (Type type, IGenericParameterProvider context)
		{
			return ImportReference (type, context);
		}

		public TypeReference ImportReference (Type type, IGenericParameterProvider context)
		{
			Mixin.CheckType (type);
			CheckContext (context, this);

			return ReflectionImporter.ImportReference (type, context);
		}

		[Obsolete ("Use ImportReference", error: false)]
		public FieldReference Import (SR.FieldInfo field)
		{
			return ImportReference (field, null);
		}

		[Obsolete ("Use ImportReference", error: false)]
		public FieldReference Import (SR.FieldInfo field, IGenericParameterProvider context)
		{
			return ImportReference (field, context);
		}

		public FieldReference ImportReference (SR.FieldInfo field)
		{
			return ImportReference (field, null);
		}

		public FieldReference ImportReference (SR.FieldInfo field, IGenericParameterProvider context)
		{
			Mixin.CheckField (field);
			CheckContext (context, this);

			return ReflectionImporter.ImportReference (field, context);
		}

		[Obsolete ("Use ImportReference", error: false)]
		public MethodReference Import (SR.MethodBase method)
		{
			return ImportReference (method, null);
		}

		[Obsolete ("Use ImportReference", error: false)]
		public MethodReference Import (SR.MethodBase method, IGenericParameterProvider context)
		{
			return ImportReference (method, context);
		}

		public MethodReference ImportReference (SR.MethodBase method)
		{
			return ImportReference (method, null);
		}

		public MethodReference ImportReference (SR.MethodBase method, IGenericParameterProvider context)
		{
            throw new Exception();
        }

        [Obsolete ("Use ImportReference", error: false)]
		public TypeReference Import (TypeReference type)
		{
            throw new Exception();
        }

        [Obsolete ("Use ImportReference", error: false)]
		public TypeReference Import (TypeReference type, IGenericParameterProvider context)
		{
            throw new Exception();
        }

        public TypeReference ImportReference (TypeReference type)
		{
            throw new Exception();
        }

        public TypeReference ImportReference (TypeReference type, IGenericParameterProvider context)
		{
            throw new Exception();
        }

        [Obsolete ("Use ImportReference", error: false)]
		public FieldReference Import (FieldReference field)
		{
            throw new Exception();
        }

        [Obsolete ("Use ImportReference", error: false)]
		public FieldReference Import (FieldReference field, IGenericParameterProvider context)
		{
            throw new Exception();
        }

        public FieldReference ImportReference (FieldReference field)
		{
            throw new Exception();
        }

        public FieldReference ImportReference (FieldReference field, IGenericParameterProvider context)
		{
            throw new Exception();
        }

        [Obsolete ("Use ImportReference", error: false)]
		public MethodReference Import (MethodReference method)
		{
            throw new Exception();
        }

        [Obsolete ("Use ImportReference", error: false)]
		public MethodReference Import (MethodReference method, IGenericParameterProvider context)
		{
            throw new Exception();
        }

        public MethodReference ImportReference (MethodReference method)
		{
            throw new Exception();
        }

        public MethodReference ImportReference (MethodReference method, IGenericParameterProvider context)
		{
            throw new Exception();
        }

        public IMetadataTokenProvider LookupToken (int token)
		{
            throw new Exception();
        }

        public IMetadataTokenProvider LookupToken (MetadataToken token)
		{
            throw new Exception();
        }

        readonly object module_lock = new object();

		internal object SyncRoot {
			get { return module_lock; }
		}

		internal void Read<TItem> (TItem item, Action<TItem, MetadataReader> read)
		{
            throw new Exception();
        }

        internal TRet Read<TItem, TRet> (TItem item, Func<TItem, MetadataReader, TRet> read)
		{
            throw new Exception();
        }

        internal TRet Read<TItem, TRet> (ref TRet variable, TItem item, Func<TItem, MetadataReader, TRet> read) where TRet : class
		{
            throw new Exception();
        }

        public bool HasDebugHeader {
			get { return Image != null && Image.DebugHeader != null; }
		}

		public ImageDebugHeader GetDebugHeader ()
		{
            throw new Exception();
        }

        public static ModuleDefinition CreateModule (string name, ModuleKind kind)
		{
			return CreateModule (name, new ModuleParameters { Kind = kind });
		}

		public static ModuleDefinition CreateModule (string name, ModuleParameters parameters)
		{
            throw new Exception();
        }

        static AssemblyNameDefinition CreateAssemblyName (string name)
		{
            throw new Exception();
        }

        public void ReadSymbols ()
		{
            throw new Exception();
        }

        public void ReadSymbols (ISymbolReader reader)
		{
            throw new Exception();
        }

        public void ReadSymbols (ISymbolReader reader, bool throwIfSymbolsAreNotMaching)
		{
		}

		public static ModuleDefinition ReadModule (string fileName)
		{
            throw new Exception();
        }

        public static ModuleDefinition ReadModule (string fileName, ReaderParameters parameters)
		{
            throw new Exception();
		}

		static Stream GetFileStream (string fileName, FileMode mode, FileAccess access, FileShare share)
		{
            throw new Exception();
        }

        public static ModuleDefinition ReadModule (Stream stream)
		{
            throw new Exception();
        }

        public static ModuleDefinition ReadModule (Stream stream, ReaderParameters parameters)
		{
            throw new Exception();
        }

        static ModuleDefinition ReadModule (Disposable<Stream> stream, string fileName, ReaderParameters parameters)
		{
            throw new Exception();
		}

		public void Write (string fileName)
		{
            throw new Exception();
        }

        public void Write (string fileName, WriterParameters parameters)
		{
		}

		public void Write ()
		{
		}

		public void Write (WriterParameters parameters)
		{
		}

		public void Write (Stream stream)
		{
		}

		public void Write (Stream stream, WriterParameters parameters)
		{
		}
	}

	static partial class Mixin {

		public enum Argument {
			name,
			fileName,
			fullName,
			stream,
			type,
			method,
			field,
			parameters,
			module,
			modifierType,
			eventType,
			fieldType,
			declaringType,
			returnType,
			propertyType,
			interfaceType,
			constraintType,
		}

		public static void CheckName (object name)
		{
			if (name == null)
				throw new ArgumentNullException (Argument.name.ToString ());
		}

		public static void CheckName (string name)
		{
			if (string.IsNullOrEmpty (name))
				throw new ArgumentNullOrEmptyException (Argument.name.ToString ());
		}

		public static void CheckFileName (string fileName)
		{
			if (string.IsNullOrEmpty (fileName))
				throw new ArgumentNullOrEmptyException (Argument.fileName.ToString ());
		}

		public static void CheckFullName (string fullName)
		{
			if (string.IsNullOrEmpty (fullName))
				throw new ArgumentNullOrEmptyException (Argument.fullName.ToString ());
		}

		public static void CheckStream (object stream)
		{
			if (stream == null)
				throw new ArgumentNullException (Argument.stream.ToString ());
		}

		public static void CheckWriteSeek (Stream stream)
		{
			if (!stream.CanWrite || !stream.CanSeek)
				throw new ArgumentException ("Stream must be writable and seekable.");
		}

		public static void CheckReadSeek (Stream stream)
		{
			if (!stream.CanRead || !stream.CanSeek)
				throw new ArgumentException ("Stream must be readable and seekable.");
		}

		public static void CheckType (object type)
		{
			if (type == null)
				throw new ArgumentNullException (Argument.type.ToString ());
		}

		public static void CheckType (object type, Argument argument)
		{
			if (type == null)
				throw new ArgumentNullException (argument.ToString ());
		}

		public static void CheckField (object field)
		{
			if (field == null)
				throw new ArgumentNullException (Argument.field.ToString ());
		}

		public static void CheckMethod (object method)
		{
			if (method == null)
				throw new ArgumentNullException (Argument.method.ToString ());
		}

		public static void CheckParameters (object parameters)
		{
			if (parameters == null)
				throw new ArgumentNullException (Argument.parameters.ToString ());
		}

		public static uint GetTimestamp ()
		{
			return (uint) DateTime.UtcNow.Subtract (new DateTime (1970, 1, 1)).TotalSeconds;
		}

		public static bool HasImage (this ModuleDefinition self)
		{
			return self != null && self.HasImage;
		}

		public static string GetFileName (this Stream self)
		{
			var file_stream = self as FileStream;
			if (file_stream == null)
				return string.Empty;

			return Path.GetFullPath (file_stream.Name);
		}

		public static TargetRuntime ParseRuntime (this string self)
		{
			if (string.IsNullOrEmpty (self))
				return TargetRuntime.Net_4_0;

			switch (self [1]) {
			case '1':
				return self [3] == '0'
					? TargetRuntime.Net_1_0
					: TargetRuntime.Net_1_1;
			case '2':
				return TargetRuntime.Net_2_0;
			case '4':
			default:
				return TargetRuntime.Net_4_0;
			}
		}

		public static string RuntimeVersionString (this TargetRuntime runtime)
		{
			switch (runtime) {
			case TargetRuntime.Net_1_0:
				return "v1.0.3705";
			case TargetRuntime.Net_1_1:
				return "v1.1.4322";
			case TargetRuntime.Net_2_0:
				return "v2.0.50727";
			case TargetRuntime.Net_4_0:
			default:
				return "v4.0.30319";
			}
		}

		public static bool IsWindowsMetadata (this ModuleDefinition module)
		{
			return module.MetadataKind != MetadataKind.Ecma335;
		}

		public static byte [] ReadAll (this Stream self)
		{
			int read;
			var memory = new MemoryStream ((int) self.Length);
			var buffer = new byte [1024];

			while ((read = self.Read (buffer, 0, buffer.Length)) != 0)
				memory.Write (buffer, 0, read);

			return memory.ToArray ();
		}

		public static void Read (object o)
		{
		}
	}
}
