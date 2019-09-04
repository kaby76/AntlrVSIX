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

using Mono.Collections.Generic;
using Mono.Cecil.Metadata;
using Mono.Cecil.PE;

using RVA = System.UInt32;

namespace Mono.Cecil {

	abstract class ModuleReader {
	}

	sealed class ImmediateModuleReader : ModuleReader {
	}

	sealed class DeferredModuleReader : ModuleReader {
	}

	sealed class MetadataReader : ByteBuffer {
	}

	sealed class SignatureReader : ByteBuffer {
	}
}
