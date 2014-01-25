﻿using System;
using System.Collections.Generic;

namespace Lardite.RefAssistant.Algorithms.Contracts
{
    public interface IAssembly : ICustomAttributeProvider
    {
        string Name { get; }

        Version Version { get; }

        string Culture { get; }

        IEnumerable<byte> PublicKeyToken { get; }

        IEnumerable<IAssembly> References { get; }

        IEnumerable<ITypeDefinition> TypeDefinitions { get; }

        IEnumerable<ITypeReference> TypeReferences { get; }
    }
}
