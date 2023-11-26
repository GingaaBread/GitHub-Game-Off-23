using main.entity.Card_Management;
using main.service.Card_Management;
using Zenject;

namespace main.infrastructure
{
    public class DiscardPileServiceInstaller : MonoInstaller<DiscardPileServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<CardPile>()
                .WithId("DiscardPile")
                .AsTransient();
            
            Container.Bind<DiscardPileService>()
                .FromMethod(CreateDiscardPileService)
                .AsSingle();
        }
        
        private static DiscardPileService CreateDiscardPileService(InjectContext ctx)
        {
            var discardPile = ctx.Container.ResolveId<CardPile>("DiscardPile");
            var deckService = ctx.Container.Resolve<DeckService>();
            return new DiscardPileService(discardPile, deckService);
        }
    }
}