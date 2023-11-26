using main.entity.Card_Management.Deck_Definition;
using main.repository;
using main.repository.Card_Management.Deck_Definition;
using Zenject;

namespace main.infrastructure
{
    public class DeckDefinitionRepositoryInstaller : MonoInstaller<DeckDefinitionRepositoryInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<DeckDefinitionRepository>()
                .WithId(ResourcePath.StarterDeck.GetValue())
                .FromMethod(ctx => CreateDeckDefinitionRepository(ctx, ResourcePath.StarterDeck))
                .AsTransient();

            Container.Bind<DeckDefinitionRepository>()
                .WithId(ResourcePath.CardPool.GetValue())
                .FromMethod(ctx => CreateDeckDefinitionRepository(ctx, ResourcePath.CardPool))
                .AsTransient();
        }
        
        private static DeckDefinitionRepository CreateDeckDefinitionRepository(InjectContext ctx, ResourcePath resourcePath)
        {
            var loader = ctx.Container.ResolveId<IResourceLoader<DeckDefinition>>(resourcePath.GetValue());
            return new DeckDefinitionRepository(loader);
        }
    }
}