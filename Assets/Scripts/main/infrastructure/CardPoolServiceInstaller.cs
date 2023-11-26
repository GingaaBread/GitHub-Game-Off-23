using main.entity.Card_Management;
using main.repository;
using main.repository.Card_Management.Deck_Definition;
using main.service.Card_Management;
using Zenject;

namespace main.infrastructure
{
    public class CardPoolServiceInstaller : MonoInstaller<CardPoolServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<CardPool>().AsSingle();
            
            Container.Bind<CardPoolService>()
                .FromMethod(CreateCardPoolService)
                .AsSingle();
        }
        
        private static CardPoolService CreateCardPoolService(InjectContext ctx)
        {
            var repositoryCardPool = ctx.Container.ResolveId<DeckDefinitionRepository>(ResourcePath.CardPool.GetValue());
            return new CardPoolService(ctx.Container.Resolve<CardPool>(), repositoryCardPool, ctx.Container.Resolve<StarterDeck>());
        }
    }
}