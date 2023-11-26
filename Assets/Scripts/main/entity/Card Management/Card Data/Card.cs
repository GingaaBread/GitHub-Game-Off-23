using System.Text;
using NaughtyAttributes;
using UnityEngine;

namespace main.entity.Card_Management.Card_Data
{
    /// <summary>
    ///     The Card SO entity defines the card data of the game.
    ///     It determines the attributes that each card should have and allows setting them in the Unity editor.
    /// </summary>
    public abstract class Card : ScriptableObject
    {
        /// <summary>
        ///     The name of the card as it will be rendered on the view component.
        ///     Assigned in the Editor on the ScriptableObject.
        /// </summary>
        [InfoBox("The name of the card as it will be rendered on the view component")] [SerializeField]
        private string _cardName;

        /// <summary>
        ///     The sprite of the card as it will be rendered on the view.
        ///     Assigned in the Editor on the ScriptableObject.
        /// </summary>
        [Space(20)] [InfoBox("The sprite of the card as it will be rendered on the view")] [SerializeField]
        private Sprite _cardSprite;

        /// <summary>
        ///     The class that the card belongs to.
        ///     Each card belongs to exactly one class.
        /// </summary>
        [Space(20)]
        [InfoBox("The class that the card belongs to. Each card belongs to exactly one class")]
        [SerializeField]
        private CardClass _cardClass;

        /// <summary>
        ///     The rarity of the card determines when it can be played.
        ///     Cards with rarity 2 can be played in round 2, cards with rarity 4 in round 4, etc.
        /// </summary>
        [Space(20)]
        [InfoBox(
            "The rarity of the card determines when it can be played. Cards with rarity 2 can be" +
            " played in round 2, cards with rarity 4 in round 4, etc.")]
        [SerializeField]
        private CardRarity _cardRarity;

        /// <summary>
        ///     The time required in order for the player to be able to play this card.
        ///     Right now, it is limited to 10, but feel free to change depending on the GDD.
        ///     Assigned in the Editor on the ScriptableObject.
        /// </summary>
        [Space(20)] [InfoBox("The time required in order for the player to be able to play this card")] [SerializeField]
        private UnitTime _unitTimeCost;

        /// <summary>
        ///     All card effects IN THE SAME ORDER as they should be executed in-game.
        ///     Index zero will always be executed before index one, and so on.
        ///     Must be created as a ScriptableObject first and then set as a reference on this card.
        /// </summary>
        [Space(20)]
        [InfoBox("All card effects IN THE SAME ORDER as they should be executed in-game")]
        [Expandable]
        [SerializeField]
        private CardEffect[] _cardEffects;

        /// <summary>
        ///     Yields the rarity of the card as an integer
        /// </summary>
        public int Rarity => _cardRarity.Rarity;

        /// <summary>
        ///     Yields the name of the card as a string
        /// </summary>
        public string Name => _cardName;

        /// <summary>
        ///     Yields the cost of the card as an integer
        /// </summary>
        public int TimeCost => _unitTimeCost.Time;

        /// <summary>
        ///     Yields the icon of the card as a Unity Sprite
        /// </summary>
        public Sprite IconSprite => _cardSprite;

        /// <summary>
        ///     Yields the classes of the card as a string
        ///     TODO Use localisation key
        /// </summary>
        public string Class => _cardClass.ToString();

        /// <summary>
        ///     Yields the description of the card as a string
        /// </summary>
        public string Description()
        {
            return "No effect";
            // TODO Use the card effect description instead of the "name" placeholder
            var bobTheBuilder = new StringBuilder();
            foreach (var cardEffect in _cardEffects) bobTheBuilder.Append(cardEffect.name + "\n\n");

            var description = bobTheBuilder.ToString();
            return description[..^2];
        }
    }
}