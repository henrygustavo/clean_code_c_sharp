namespace BusinessLayer.CustomExceptions
{
    using System;
    public class NoSessionsApprovedException : Exception
    {
        public NoSessionsApprovedException(string message)
            : base(message)
        {
        }
    }
}
