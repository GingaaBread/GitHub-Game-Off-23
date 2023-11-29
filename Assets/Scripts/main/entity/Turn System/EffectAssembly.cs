using System.Collections.Generic;
using JetBrains.Annotations;
using main.entity.Card_Management.Card_Data;

namespace main.entity.Turn_System
{
    /// <summary>
    ///     This entity is used to assemble all end-of-turn effects that have been played in a turn to
    ///     execute them at the end of the next turn.
    /// </summary>
    public class EffectAssembly
    {
        /// <summary>
        ///     The non-null list of effects that should all be executed at the end of the next turn.
        ///     The service should also be responsible for clearing this list once all effects have been executed.
        /// </summary>
        [NotNull]
        public List<CardEffect> Effects { get; } = new();
    }
}