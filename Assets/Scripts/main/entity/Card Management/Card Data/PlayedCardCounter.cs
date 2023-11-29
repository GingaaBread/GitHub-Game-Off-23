using System.Diagnostics;

namespace main.entity.Card_Management.Card_Data
{
    /// <summary>
    /// The PlayedCardCounter is a single tuple that is used to track how many times a played card
    /// can be counted. So if card X has been played 3 times, a PlayedCardCounter for X would have
    /// its amount go up to 3, then each time CountedSoFar is run the counted value would increase,
    /// allowing the game to see how many plays of a card have been counted for effects.
    /// </summary>
    
    public class PlayedCardCounter
    {
        private int amount;
        private int counted;
        private string cardName;

        public PlayedCardCounter(string cardName){
            this.cardName = cardName;
            amount = 1;
            counted = 1;
        }

        public int CurrentAmount(){return amount;}
        public void IncrementAmount(){amount++;}
        public int CountedSoFar(){
            if(counted < amount) return counted++;
            else return counted;
        }

        public string CardName(){return cardName;}
    }
}