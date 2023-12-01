using UnityEngine;

namespace main.repository
{
    public interface IResourceLoader<out T> : IAssetRepository<T> where T : Object
    {
        string ResourcePath { get; }
    }
}