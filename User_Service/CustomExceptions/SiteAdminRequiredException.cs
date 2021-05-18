using System;

namespace User_Service.CustomExceptions
{
    [Serializable]
    public class SiteAdminRequiredException : Exception
    {
        /// <summary>
        /// Can be thrown when the provided path is not valid
        /// </summary>
        public SiteAdminRequiredException() { }
        public SiteAdminRequiredException(string message) : base(message) { }
        public SiteAdminRequiredException(string message, Exception inner) : base(message, inner) { }
        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected SiteAdminRequiredException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}