using main.entity.Card_Management.Deck_Definition;
using main.repository;
using Zenject;

namespace main.infrastructure
{
    public class ResourceLoaderInstaller : MonoInstaller<ResourceLoaderInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IResourceLoader<DeckDefinition>>()
                .WithId(ResourcePath.StarterDeck.GetValue())
                .To<ResourceLoader<DeckDefinition>>()
                .AsTransient()
                .WithArguments(ResourcePath.StarterDeck);

            Container.Bind<IResourceLoader<DeckDefinition>>()
                .WithId(ResourcePath.CardPool.GetValue())
                .To<ResourceLoader<DeckDefinition>>()
                .AsTransient()
                .WithArguments(ResourcePath.CardPool);
        }
    }
}