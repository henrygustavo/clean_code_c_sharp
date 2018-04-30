namespace BusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CustomExceptions;
    using Enums;

    /// <summary>
    /// Represents a single speaker
    /// </summary>
    public class Speaker
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? YearsExperiencie { get; set; }
        public bool HasBlog { get; set; }
        public string BlogUrl { get; set; }
        public WebBrowser Browser { get; set; }
        public List<string> Certifications { get; set; }
        public string Employer { get; set; }
        public int RegistrationFee { get; set; }
        public List<Session> Sessions { get; set; }

        /// <summary>
        /// Register a speaker
        /// </summary>
        /// <returns>speakerID</returns>
        public int? Register(IRepository repository)
        {
            ValidateInformation();

            ValidateSkills();

            ValidateSessions();

            RegistrationFee = repository.GetRegistrationFee(YearsExperiencie);

            return repository.SaveSpeaker(this);
        }

        private void ValidateInformation()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                throw new ArgumentNullException("First Name is required");
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                throw new ArgumentNullException("Last name is required.");
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                throw new ArgumentNullException("Email is required.");
            }
        }

        private void ValidateSkills()
        {
            var isQualified = IsAGoodProfessional() || !HasRedFlags();

            if (!isQualified)
            {
                throw new SpeakerDoesntMeetRequirementsException(
                    "Speaker doesn't meet our abitrary and capricious standards.");
            }
        }

        private bool IsAGoodProfessional()
        {
            const int numberCertifications = 3;
            const int maxNumberYearsExperience = 10;

            List<string> employersList = new List<string> {"Microsoft", "Google", "Fog Creek Software", "37Signals"};

            return YearsExperiencie > maxNumberYearsExperience || HasBlog ||
                   Certifications.Count > numberCertifications ||
                   employersList.Contains(Employer);
        }

        private bool HasRedFlags()
        {
            const int validBrowserVersion = 9;

            List<string> domains = new List<string> {"aol.com", "hotmail.com", "prodigy.com", "CompuServe.com"};

            string emailDomain = Email.Split('@').Last();

            return domains.Contains(emailDomain) ||
                   (Browser.Name == BrowserName.InternetExplorer &&
                    Browser.MajorVersion < validBrowserVersion);

        }

        private void ValidateSessions()
        {
            if (!Sessions.Any())
                throw new ArgumentException("Can't register speaker with no sessions to present.");

            foreach (var session in Sessions)
            {
                session.Approved = !IsSessionAboutOldTechnologies(session);
            }

            if (!Sessions.Any(x => x.Approved))
            {
                throw new NoSessionsApprovedException("No sessions approved.");
            }
        }

        private bool IsSessionAboutOldTechnologies(Session session)
        {
            List<string> oldTechnologies = new List<string> {"Cobol", "Punch Cards", "Commodore", "VBScript"};

            return oldTechnologies.Any(x => session.Title.Contains(x) || session.Description.Contains(x));
        }
    }
}