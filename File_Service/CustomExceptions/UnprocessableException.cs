using System;

namespace File_Service.CustomExceptions
{
    [Serializable]
    public class UnprocessableException : Exception
    {
        /// <summary>
        /// Can be thrown when the provided path is not valid
        /// </summary>
        public UnprocessableException() { }
        public UnprocessableException(string message) : base(message) { }
        public UnprocessableException(string message, Exception inner) : base(message, inner) { }
        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected UnprocessableException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}