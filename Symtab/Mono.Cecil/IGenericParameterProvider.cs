//
// Author:
//   Jb Evain (jbevain@gmail.com)
//
// Copyright (c) 2008 - 2015 Jb Evain
// Copyright (c) 2008 - 2011 Novell, Inc.
//
// Licensed under the MIT/X11 license.
//


using Mono.Collections.Generic;

namespace Mono.Cecil {

	public interface IGenericParameterProvider : IMetadataTokenProvider {

		bool HasGenericParameters { get; }
		bool IsDefinition { get; }
		ModuleDefinition Module { get; }
		Collection<GenericParameter> GenericParameters { get; }
		GenericParameterType GenericParameterType { get; }
	}

	public enum GenericParameterType {
		Type,
		Method
	}

	interface IGenericContext {

		bool IsDefinition { get; }
		IGenericParameterProvider Type { get; }
		IGenericParameterProvider Method { get; }
	}

	static partial class Mixin {

		public static bool GetHasGenericParameters (
			this IGenericParameterProvider self,
			ModuleDefinition module)
		{
            return false;
        }

		public static Collection<GenericParameter> GetGenericParameters (
			this IGenericParameterProvider self,
			ref Collection<GenericParameter> collection,
			ModuleDefinition module)
		{
            return new Collection<GenericParameter>();
        }
	}
}
