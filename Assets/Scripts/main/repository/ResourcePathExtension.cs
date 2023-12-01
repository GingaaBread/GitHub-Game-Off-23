using System;

namespace main.repository
{
    public static class ResourcePathExtension
    {
        private const string StarterDeckPath = "Starter Deck";
        private const string CardPoolPath = "Card Pool";
        
        public static string GetValue(this ResourcePath resourcePath)
        {
            return resourcePath switch
            {
                ResourcePath.StarterDeck => StarterDeckPath,
                ResourcePath.CardPool => CardPoolPath,
                _ => throw new ArgumentOutOfRangeException(nameof(resourcePath), resourcePath, null)
            };
        }
    }
}