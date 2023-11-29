using main.entity.Card_Management.Card_Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace main.entity.Card_Management.Card_Effects
{
    /// <summary>
    ///     A simple example of how classes can define card effects.
    ///     This simply defines a field "message" that will be printed upon execution.
    ///     NOTE: Can be removed once the actual card effects exist.
    /// </summary>
    [CreateAssetMenu(fileName = "Sample Card Effect", menuName = "Data/Effects/New Sample")]
    public class SampleEffect : CardEffect
    {
        [FormerlySerializedAs("message")] [SerializeField]
        private string _message;

        public override void Execute(int multiplier)
        {
            Debug.Log("My message is " + _message);
        }
    }
}