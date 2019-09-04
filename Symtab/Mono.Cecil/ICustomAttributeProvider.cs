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
using Mono.Collections.Generic;

namespace Mono.Cecil {

	public interface ICustomAttributeProvider : IMetadataTokenProvider {

		Collection<CustomAttribute> CustomAttributes { get; }

		bool HasCustomAttributes { get; }
	}

	static partial class Mixin {

		public static bool GetHasCustomAttributes (
			this ICustomAttributeProvider self,
			ModuleDefinition module)
		{
            throw new Exception();
        }

		public static Collection<CustomAttribute> GetCustomAttributes (
			this ICustomAttributeProvider self,
			ref Collection<CustomAttribute> variable,
			ModuleDefinition module)
		{
            throw new Exception();
        }
	}
}
