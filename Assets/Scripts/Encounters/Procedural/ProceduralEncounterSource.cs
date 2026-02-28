using System.Collections.Generic;
using System.Linq;
using LoxQuest3D.Core;

namespace LoxQuest3D.Encounters.Procedural
{
    public sealed class ProceduralEncounterSource
    {
        private readonly List<EncounterDefinition> _templates;

        public ProceduralEncounterSource(IEnumerable<EncounterDefinition> templates)
        {
            _templates = templates?.Where(t => t != null).ToList() ?? new List<EncounterDefinition>();
        }

        public IEnumerable<EncounterDefinition> GetTemplates()
            => _templates;
    }
}

