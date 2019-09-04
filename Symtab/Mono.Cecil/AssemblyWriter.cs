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
using System.IO.Compression;
using System.Text;
using System.Security.Cryptography;

using Mono;
using Mono.Collections.Generic;
using Mono.Cecil.Metadata;

using RVA = System.UInt32;
using RID = System.UInt32;
using CodedRID = System.UInt32;
using StringIndex = System.UInt32;
using BlobIndex = System.UInt32;
using GuidIndex = System.UInt32;

namespace Mono.Cecil {

	using ModuleRow      = Row<StringIndex, GuidIndex>;
	using TypeRefRow     = Row<CodedRID, StringIndex, StringIndex>;
	using TypeDefRow     = Row<TypeAttributes, StringIndex, StringIndex, CodedRID, RID, RID>;
	using FieldRow       = Row<FieldAttributes, StringIndex, BlobIndex>;
	using MethodRow      = Row<RVA, MethodImplAttributes, MethodAttributes, StringIndex, BlobIndex, RID>;
	using ParamRow       = Row<ParameterAttributes, ushort, StringIndex>;
	using InterfaceImplRow = Row<uint, CodedRID>;
	using MemberRefRow   = Row<CodedRID, StringIndex, BlobIndex>;
	using ConstantRow    = Row<ElementType, CodedRID, BlobIndex>;
	using CustomAttributeRow = Row<CodedRID, CodedRID, BlobIndex>;
	using FieldMarshalRow = Row<CodedRID, BlobIndex>;
	using DeclSecurityRow = Row<SecurityAction, CodedRID, BlobIndex>;
	using ClassLayoutRow = Row<ushort, uint, RID>;
	using FieldLayoutRow = Row<uint, RID>;
	using EventMapRow    = Row<RID, RID>;
	using EventRow       = Row<EventAttributes, StringIndex, CodedRID>;
	using PropertyMapRow = Row<RID, RID>;
	using PropertyRow    = Row<PropertyAttributes, StringIndex, BlobIndex>;
	using MethodSemanticsRow = Row<MethodSemanticsAttributes, RID, CodedRID>;
	using MethodImplRow  = Row<RID, CodedRID, CodedRID>;
	using ImplMapRow     = Row<PInvokeAttributes, CodedRID, StringIndex, RID>;
	using FieldRVARow    = Row<RVA, RID>;
	using AssemblyRow    = Row<AssemblyHashAlgorithm, ushort, ushort, ushort, ushort, AssemblyAttributes, uint, uint, uint>;
	using AssemblyRefRow = Row<ushort, ushort, ushort, ushort, AssemblyAttributes, uint, uint, uint, uint>;
	using FileRow        = Row<FileAttributes, StringIndex, BlobIndex>;
	using ExportedTypeRow = Row<TypeAttributes, uint, StringIndex, StringIndex, CodedRID>;
	using ManifestResourceRow = Row<uint, ManifestResourceAttributes, StringIndex, CodedRID>;
	using NestedClassRow = Row<RID, RID>;
	using GenericParamRow = Row<ushort, GenericParameterAttributes, CodedRID, StringIndex>;
	using MethodSpecRow = Row<CodedRID, BlobIndex>;
	using GenericParamConstraintRow = Row<RID, CodedRID>;
	using DocumentRow = Row<BlobIndex, GuidIndex, BlobIndex, GuidIndex>;
	using MethodDebugInformationRow = Row<RID, BlobIndex>;
	using LocalScopeRow = Row<RID, RID, RID, RID, uint, uint>;
	using LocalConstantRow = Row<StringIndex, BlobIndex>;
	using ImportScopeRow = Row<RID, BlobIndex>;
	using StateMachineMethodRow = Row<RID, RID>;
	using CustomDebugInformationRow = Row<CodedRID, GuidIndex, BlobIndex>;

	static class ModuleWriter {

	}

	abstract class MetadataTable {

	}

	abstract class OneRowTable<TRow> : MetadataTable where TRow : struct {

	}

	abstract class MetadataTable<TRow> : MetadataTable where TRow : struct {

	}

	sealed class ModuleTable : OneRowTable<ModuleRow> {
	}

	sealed class TypeRefTable : MetadataTable<TypeRefRow> {
	}

	sealed class TypeDefTable : MetadataTable<TypeDefRow> {
	}

	sealed class FieldTable : MetadataTable<FieldRow> {
	}

	sealed class MethodTable : MetadataTable<MethodRow> {
    }

	sealed class ParamTable : MetadataTable<ParamRow> {
    }

	sealed class InterfaceImplTable : MetadataTable<InterfaceImplRow> {
    }

	sealed class MemberRefTable : MetadataTable<MemberRefRow> {
    }


	sealed class StandAloneSigTable : MetadataTable<uint> {
	}

	sealed class EventMapTable : MetadataTable<EventMapRow> {
    }

	sealed class EventTable : MetadataTable<EventRow> {
	}

	sealed class PropertyMapTable : MetadataTable<PropertyMapRow> {
	}

	sealed class PropertyTable : MetadataTable<PropertyRow> {
	}

	sealed class MethodImplTable : MetadataTable<MethodImplRow> {
	}

	sealed class ModuleRefTable : MetadataTable<uint> {
	}

	sealed class TypeSpecTable : MetadataTable<uint> {
	}

	sealed class AssemblyTable : OneRowTable<AssemblyRow> {
	}

	sealed class AssemblyRefTable : MetadataTable<AssemblyRefRow> {
	}

	sealed class FileTable : MetadataTable<FileRow> {
	}

	sealed class ExportedTypeTable : MetadataTable<ExportedTypeRow> {
	}

	sealed class ManifestResourceTable : MetadataTable<ManifestResourceRow> {
	}

	sealed class GenericParamTable : MetadataTable<GenericParamRow> {
	}

	sealed class MethodSpecTable : MetadataTable<MethodSpecRow> {
	}

	sealed class GenericParamConstraintTable : MetadataTable<GenericParamConstraintRow> {
	}

	sealed class DocumentTable : MetadataTable<DocumentRow> {
	}

	sealed class MethodDebugInformationTable : MetadataTable<MethodDebugInformationRow> {
	}

	sealed class LocalScopeTable : MetadataTable<LocalScopeRow> {
	}

	sealed class LocalConstantTable : MetadataTable<LocalConstantRow> {
	}

	sealed class ImportScopeTable : MetadataTable<ImportScopeRow> {
	}

	sealed class StateMachineMethodTable : MetadataTable<StateMachineMethodRow> {
	}


	sealed class MetadataBuilder {

		readonly internal ModuleDefinition module;
		readonly internal string fq_name;
		readonly internal uint timestamp;

		readonly Dictionary<TypeRefRow, MetadataToken> type_ref_map;
		readonly Dictionary<uint, MetadataToken> type_spec_map;
		readonly Dictionary<MemberRefRow, MetadataToken> member_ref_map;
		readonly Dictionary<MethodSpecRow, MetadataToken> method_spec_map;
		readonly Collection<GenericParameter> generic_parameters;

		readonly internal DataBuffer data;
		readonly internal ResourceBuffer resources;
		readonly internal StringHeapBuffer string_heap;
		readonly internal GuidHeapBuffer guid_heap;
		readonly internal UserStringHeapBuffer user_string_heap;
		readonly internal BlobHeapBuffer blob_heap;
		readonly internal PdbHeapBuffer pdb_heap;

		internal MetadataToken entry_point;

		internal RID type_rid = 1;
		internal RID field_rid = 1;
		internal RID method_rid = 1;
		internal RID param_rid = 1;
		internal RID property_rid = 1;
		internal RID event_rid = 1;
		internal RID local_variable_rid = 1;
		internal RID local_constant_rid = 1;

		readonly TypeRefTable type_ref_table;
		readonly TypeDefTable type_def_table;
		readonly FieldTable field_table;
		readonly MethodTable method_table;
		readonly ParamTable param_table;
		readonly InterfaceImplTable iface_impl_table;
		readonly MemberRefTable member_ref_table;
		readonly StandAloneSigTable standalone_sig_table;
		readonly EventMapTable event_map_table;
		readonly EventTable event_table;
		readonly PropertyMapTable property_map_table;
		readonly PropertyTable property_table;
		readonly TypeSpecTable typespec_table;
		readonly MethodSpecTable method_spec_table;

		internal MetadataBuilder metadata_builder;

		readonly DocumentTable document_table;
		readonly MethodDebugInformationTable method_debug_information_table;
		readonly LocalScopeTable local_scope_table;
		readonly LocalConstantTable local_constant_table;
		readonly ImportScopeTable import_scope_table;
		readonly StateMachineMethodTable state_machine_method_table;

		readonly Dictionary<ImportScopeRow, MetadataToken> import_scope_map;
		readonly Dictionary<string, MetadataToken> document_map;


		uint GetStringIndex (string @string)
		{
			if (string.IsNullOrEmpty (@string))
				return 0;

			return string_heap.GetStringIndex (@string);
		}

		uint GetGuidIndex (Guid guid)
		{
			return guid_heap.GetGuidIndex (guid);
		}

#if !NET_CORE
		string GetModuleFileName (string name)
		{
			if (string.IsNullOrEmpty (name))
				throw new NotSupportedException ();

			var path = Path.GetDirectoryName (fq_name);
			return Path.Combine (path, name);
		}
#endif

		void AddAssemblyReferences ()
		{
		}

		void AddModuleReferences ()
		{
		}

		void AddResources ()
		{
		}

		uint AddLinkedResource (LinkedResource resource)
		{
            throw new Exception();
		}

		uint AddEmbeddedResource (EmbeddedResource resource)
		{
            throw new Exception();
        }

        void AddExportedTypes ()
		{
            throw new Exception();
        }

        MetadataToken GetExportedTypeScope (ExportedType exported_type)
		{
            throw new Exception();
        }

        void BuildTypes ()
		{
            throw new Exception();
        }

        void AttachTokens ()
		{
            throw new Exception();
        }

        void AttachTypeToken (TypeDefinition type)
		{
            throw new Exception();
        }

        void AttachNestedTypesToken (TypeDefinition type)
		{
            throw new Exception();
        }

        void AttachFieldsToken (TypeDefinition type)
		{
            throw new Exception();
        }

        void AttachMethodsToken (TypeDefinition type)
		{
            throw new Exception();
        }

        MetadataToken GetTypeToken (TypeReference type)
		{
            throw new Exception();
        }

        MetadataToken GetTypeSpecToken (TypeReference type)
		{
            throw new Exception();
        }

        MetadataToken AddTypeSpecification (TypeReference type, uint row)
		{
            throw new Exception();
        }

        MetadataToken GetTypeRefToken (TypeReference type)
		{
            throw new Exception();
        }

        TypeRefRow CreateTypeRefRow (TypeReference type)
		{
            throw new Exception();
        }

        MetadataToken GetScopeToken (TypeReference type)
		{
            throw new Exception();
        }

        static CodedRID MakeCodedRID (IMetadataTokenProvider provider, CodedIndex index)
		{
            throw new Exception();
        }

        static CodedRID MakeCodedRID (MetadataToken token, CodedIndex index)
		{
            throw new Exception();
        }

        MetadataToken AddTypeReference (TypeReference type, TypeRefRow row)
		{
            throw new Exception();
        }

        void AddTypes ()
		{
            throw new Exception();
        }

        void AddType (TypeDefinition type)
		{
            throw new Exception();
        }

        void AddGenericParameters (IGenericParameterProvider owner)
		{
            throw new Exception();
        }

        sealed class GenericParameterComparer : IComparer<GenericParameter> {

			public int Compare (GenericParameter a, GenericParameter b)
			{
				var a_owner = MakeCodedRID (a.Owner, CodedIndex.TypeOrMethodDef);
				var b_owner = MakeCodedRID (b.Owner, CodedIndex.TypeOrMethodDef);
				if (a_owner == b_owner) {
					var a_pos = a.Position;
					var b_pos = b.Position;
					return a_pos == b_pos ? 0 : a_pos > b_pos ? 1 : -1;
				}

				return a_owner > b_owner ? 1 : -1;
			}
		}

		void AddGenericParameters ()
		{
            throw new Exception();
        }

        void AddConstraints (GenericParameter generic_parameter, GenericParamConstraintTable table)
		{
            throw new Exception();
        }

        void AddInterfaces (TypeDefinition type)
		{
            throw new Exception();
        }

        void AddLayoutInfo (TypeDefinition type)
		{
            throw new Exception();
		}

		static bool HasNoInstanceField (TypeDefinition type)
		{
            throw new Exception();
        }

        void AddNestedTypes (TypeDefinition type)
		{
            throw new Exception();
        }

        void AddFields (TypeDefinition type)
		{
            throw new Exception();
        }

        void AddField (FieldDefinition field)
		{
            throw new Exception();
        }

        void AddFieldRVA (FieldDefinition field)
		{
            throw new Exception();
        }

        void AddFieldLayout (FieldDefinition field)
		{
            throw new Exception();
        }

        void AddMethods (TypeDefinition type)
		{
            throw new Exception();
        }

        void AddMethod (MethodDefinition method)
		{
            throw new Exception();
        }

        void AddParameters (MethodDefinition method)
		{
            throw new Exception();
        }

        void AddPInvokeInfo (MethodDefinition method)
		{
            throw new Exception();
        }

        void AddOverrides (MethodDefinition method)
		{
            throw new Exception();
        }

        static bool RequiresParameterRow (ParameterDefinition parameter)
		{
			return !string.IsNullOrEmpty (parameter.Name)
				|| parameter.Attributes != ParameterAttributes.None
				|| parameter.HasMarshalInfo
				|| parameter.HasConstant
				|| parameter.HasCustomAttributes;
		}

		void AddParameter (ushort sequence, ParameterDefinition parameter, ParamTable table)
		{
            throw new Exception();
        }

        void AddMarshalInfo (IMarshalInfoProvider owner)
		{
            throw new Exception();
        }

        void AddProperties (TypeDefinition type)
		{
            throw new Exception();
        }

        void AddProperty (PropertyDefinition property)
		{
            throw new Exception();
        }

        void AddOtherSemantic (IMetadataTokenProvider owner, Collection<MethodDefinition> others)
		{
            throw new Exception();
        }

        void AddEvents (TypeDefinition type)
		{
            throw new Exception();
        }

        void AddEvent (EventDefinition @event)
		{
            throw new Exception();
        }

        void AddSemantic (MethodSemanticsAttributes semantics, IMetadataTokenProvider provider, MethodDefinition method)
		{
            throw new Exception();
        }

        void AddConstant (IConstantProvider owner, TypeReference type)
		{
            throw new Exception();
        }

        static ElementType GetConstantType (TypeReference constant_type, object constant)
		{
            throw new Exception();
        }

        static ElementType GetConstantType (Type type)
		{
            throw new Exception();
        }

        void AddCustomAttributes (ICustomAttributeProvider owner)
		{
            throw new Exception();
        }

		void AddSecurityDeclarations (ISecurityDeclarationProvider owner)
		{
            throw new Exception();
        }

        MetadataToken GetMemberRefToken (MemberReference member)
		{
            throw new Exception();
        }

        MemberRefRow CreateMemberRefRow (MemberReference member)
		{
            throw new Exception();
        }

        MetadataToken AddMemberReference (MemberReference member, MemberRefRow row)
		{
            throw new Exception();
        }

        MetadataToken GetMethodSpecToken (MethodSpecification method_spec)
		{
            throw new Exception();
        }

        void AddMethodSpecification (MethodSpecification method_spec, MethodSpecRow row)
		{
            throw new Exception();
        }

        MethodSpecRow CreateMethodSpecRow (MethodSpecification method_spec)
		{
            throw new Exception();
        }


        public uint AddStandAloneSignature (uint signature)
		{
            throw new Exception();
        }

        public uint GetCallSiteBlobIndex (CallSite call_site)
		{
            throw new Exception();
        }

        public uint GetConstantTypeBlobIndex (TypeReference constant_type)
		{
            throw new Exception();
        }



        static Exception CreateForeignMemberException (MemberReference member)
		{
            throw new Exception();
        }

        public MetadataToken LookupToken (IMetadataTokenProvider provider)
		{
            throw new Exception();
        }
	}

	static partial class Mixin {
	}
}
