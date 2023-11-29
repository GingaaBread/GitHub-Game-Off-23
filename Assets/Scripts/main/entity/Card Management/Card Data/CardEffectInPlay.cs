namespace main.entity.Card_Management.Card_Data
{
    /// <summary>
    /// The CardEffectInPlay is a single instance of a CardEffect, to be included in the EffectAssembly
    /// effects list. It stores relevant data for the instance of the card played, such as multipliers.
    /// </summary>
    
    public class CardEffectInPlay
    {
        private int multiplier;
        private CardEffect effectBase;

        public void Execute(){
            effectBase.Execute();
        }
    }
}