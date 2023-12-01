using UnityEngine;

namespace main.service
{
    /// <summary>
    ///     The base class of all service classes equips them with the ability to log simple info messages.
    /// </summary>
    public abstract class Service
    {
        /// <summary>
        ///     Defines whether the service is currently debugged or not.
        ///     If it is, messages will be logged. If it is not, they will be ignored.
        /// </summary>
        public bool debugMode = true;

        /// <summary>
        ///     If the debug mode is set to true, logs the message to the Unity console
        /// </summary>
        /// <param name="message">the message that should be logged</param>
        protected void LogInfo(string message)
        {
            if (debugMode) Debug.Log($"({this}): {message}");
        }
    }
}