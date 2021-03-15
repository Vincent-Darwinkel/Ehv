using System;

namespace File_Service.CustomExceptions
{
    [Serializable]
    internal class UnprocessableException : Exception
    {
        /// <summary>
        /// Can be thrown when the provided path is not valid
        /// </summary>
        internal UnprocessableException() { }
        internal UnprocessableException(string message) : base(message) { }
        internal UnprocessableException(string message, Exception inner) : base(message, inner) { }
        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected UnprocessableException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}