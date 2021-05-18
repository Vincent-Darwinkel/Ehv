using System;

namespace Authentication_Service.CustomExceptions
{
    [Serializable]
    public class DisabledUserException : Exception
    {
        /// <summary>
        /// Can be thrown when the provided path is not valid
        /// </summary>
        public DisabledUserException() { }
        public DisabledUserException(string message) : base(message) { }
        public DisabledUserException(string message, Exception inner) : base(message, inner) { }
        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected DisabledUserException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}