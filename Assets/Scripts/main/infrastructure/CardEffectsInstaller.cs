using System.Collections.Generic;
using main.entity.Card_Management.Card_Data;
using UnityEngine;
using Zenject;

namespace main.infrastructure
{
    [CreateAssetMenu(fileName = "Card Effects Installer", menuName = "Installers/Card Effects Installer")]
    public class CardEffectsInstaller : ScriptableObjectInstaller<CardEffectsInstaller>
    {
        [SerializeField] private List<CardEffect> cardEffects;
        
        public override void InstallBindings()
        {
            cardEffects.ForEach(cardEffect =>
            {
                Container.Bind<CardEffect>()
                    .WithId(cardEffect)
                    .To(cardEffect.GetType())
                    .FromNewScriptableObject(cardEffect)
                    .AsTransient();
            });
        }
    }
}