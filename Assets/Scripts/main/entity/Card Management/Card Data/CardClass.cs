namespace main.entity.Card_Management.Card_Data
{
    /// <summary>
    ///     This entity defines the class of a card. Each card has exactly one class it belongs to.
    ///     This enum provides the constants the Card SO entity uses, which is translated into a proper name by the
    ///     localisation service, using the constant in UPPER_CASE as the key.
    ///     For example, "KNIFE" in English would be "Knife".
    /// </summary>
    public enum CardClass
    {
        /// <summary>
        ///     Defines the card to be part of the knife class.
        ///     Knife cards are predominantly used to scale fish.
        /// </summary>
        KNIFE,

        /// <summary>
        ///     Defines the card to be part of the help class.
        ///     Help cards usually buff other card effects.
        /// </summary>
        HELP,

        /// <summary>
        ///     Defines the card to be part of the prep class.
        ///     Prep cards usually prepare other effects.
        /// </summary>
        PREP,

        /// <summary>
        ///     Defines the card to be part of the water class.
        ///     Water cards are usually powerful attacks or effects.
        /// </summary>
        WATER
    }
}