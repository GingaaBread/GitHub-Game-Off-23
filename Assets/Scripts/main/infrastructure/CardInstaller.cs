using System.Collections.Generic;
using main.entity.Card_Management.Card_Data;
using UnityEngine;
using Zenject;

namespace main.infrastructure
{
    [CreateAssetMenu(fileName = "Card Installer", menuName = "Installers/Card Installer")]
    public class CardInstaller : ScriptableObjectInstaller<CardInstaller>
    {
        [SerializeField] private List<Card> cards;
        
        public override void InstallBindings()
        {
            cards.ForEach(card =>
                    Container.Bind<Card>()
                    .WithId(card.Name)
                    .To(card.GetType())
                    .FromNewScriptableObject(card)
                    .AsTransient()
            );
        }
    }
}