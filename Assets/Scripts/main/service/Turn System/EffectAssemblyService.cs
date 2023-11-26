using JetBrains.Annotations;
using main.entity.Card_Management.Card_Data;
using main.entity.Turn_System;

namespace main.service.Turn_System
{
    /// <summary>
    ///     This service provides the business logic for the <see cref="EffectAssembly" /> entity, providing a way for
    ///     card classes to add effects that should all be executed at the end of the turn. Then, once all card effects
    ///     have been executed, the list is cleared for the next turn.
    /// </summary>
    public class EffectAssemblyService : Service
    {
        /// <summary>
        ///     The non-null <see cref="EffectAssembly" /> entity.
        ///     The instance is created automatically once the service is created.
        /// </summary>
        [NotNull] private readonly EffectAssembly effectAssembly;

        public EffectAssemblyService([NotNull] EffectAssembly effectAssembly)
        {
            this.effectAssembly = effectAssembly;
        }

        /// <summary>
        ///     Adds a non-null end-of-turn card effect to the <see cref="EffectAssembly" /> list.
        ///     Note that it will be deleted from the list at the end of the next turn!
        /// </summary>
        /// <param name="effect">The non-null <see cref="CardEffectInPlay" /> that should be executed at the end</param>
        public void AddEffect([NotNull] CardEffectInPlay effect)
        {
            LogInfo($"Adding a new card effect to the end-of-turn effects: '{effect}'");
            effectAssembly.Effects.Add(effect);
        }
        
        /// <summary>
        ///     Executes each end-of-turn effect assembled in the <see cref="EffectAssembly" /> and then clears the
        ///     list, making it ready for the next turn.
        /// </summary>
        public void ExecuteAll()
        {
            LogInfo("Now executing all end of turn effects");
            effectAssembly.Effects.ForEach(effectInPlay => effectInPlay.Execute());
            Clear();
            LogInfo("Successfully executed all end-of-turn effects");
        }
        
        /// <summary>
        ///     Removes all end-of-turn effects from the <see cref="EffectAssembly" /> once they have been executed.
        /// </summary>
        private void Clear()
        {
            effectAssembly.Effects.Clear();
        }
    }
}