using UnityEngine;

namespace main.entity.Card_Management.Card_Data
{
    /// <summary>
    ///     The CardEffect ScriptableObject is used to provide data that determines what happens when a
    ///     card effect is executed. Card effects are defined as scriptable objects that inherit from
    ///     this base class. It requires the use of a method "Execute", which defines the card effect
    ///     behaviour.
    /// </summary>
    public abstract class CardEffect : ScriptableObject
    {
        [SerializeField] private bool isEndTurnEffect;

        public bool IsEndTurnEffect => isEndTurnEffect;

        /// <summary>
        ///     Determines the behaviour of the card effect.
        ///     An example execution could be a card effect that prints a message to the console.
        ///     When the card is played, all its effects will be executed in order using this method.
        ///     The card effect that extends this class implements the actual execute method, using
        ///     Debug.Log, etc.
        /// </summary>
        public abstract void Execute();
    }
}