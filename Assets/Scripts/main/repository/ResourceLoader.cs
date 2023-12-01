using main.infrastructure;
using UnityEngine;

namespace main.repository
{
    public class ResourceLoader<T> : IResourceLoader<T> where T : Object
    {
        public string ResourcePath { get; }

        public ResourceLoader(ResourcePath resourcePath)
        {
            ResourcePath = $"{LocalizationSettingsWrapper.SelectedLocaleIdentifier}/{resourcePath.GetValue()}";
        }

        public T[] GetAll()
        {
            return Resources.LoadAll<T>(ResourcePath);
        }
    }
}