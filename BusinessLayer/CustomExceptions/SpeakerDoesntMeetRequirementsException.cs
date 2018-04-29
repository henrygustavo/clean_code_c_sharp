namespace BusinessLayer.CustomExceptions
{
    using System;

    public class SpeakerDoesntMeetRequirementsException : Exception
    {
        public SpeakerDoesntMeetRequirementsException(string message)
            : base(message)
        {
        }
    }

}
