namespace main.entity.Fish_Management
{
    /// <summary>
    ///     This entity is responsible for providing data about the fish that is currently being scaled.
    ///     At the moment, this just holds an amount of scales that should be decreased by the player,
    ///     but in the future it might be more complex.
    /// </summary>
    public class Fish
    {
        /// <summary>
        ///     The total amount of scales a fish has initially.
        /// </summary>
        public int TotalScales { get; }
        
        /// <summary>
        ///     The remaining fish scales that should be reduced to zero by the player.
        ///     Once it reaches zero or less, it is considered "scaled" and replaced by another fish.
        /// </summary>
        public int RemainingScales { get; set; }

        /// <summary>
        ///     Creates a new fish entity using the total scales as its base amount.
        /// </summary>
        /// <param name="totalScales">the amount of scales the fish should spawn with</param>
        public Fish(int totalScales)
        {
            TotalScales = totalScales;
            RemainingScales = TotalScales;
        }
    }
}