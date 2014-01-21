﻿using System.Collections.Generic;
using Lardite.RefAssistant.Algorithms;
using Lardite.RefAssistant.Algorithms.Data;
using Lardite.RefAssistant.Model.Processing.Data.Loaders;
using Lardite.RefAssistant.Model.Projects;
using Lardite.RefAssistant.ReflectionServices;

namespace Lardite.RefAssistant.Model.Processing.ProjectAgents
{
    [ProjectKind(VsProjectKinds.CSharp)]
    internal sealed class CSharpProjectAgent : ProjectAgentBase
    {
        public CSharpProjectAgent(IProject project, IServiceConfigurator serviceConfigurator)
            : base(project, serviceConfigurator)
        {
        }

        protected override IEnumerable<IAlgorithmLauncher> Algorithms
        {
            get 
            {
                yield return new AlgorithmLauncher<AssemblyManifestAlgorithm, IProject>(new ProjectInputLoader());
            }
        }
    }
}