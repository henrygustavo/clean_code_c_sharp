namespace BusinessLayer.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using CustomExceptions;
    using DataAccessLayer;

    [TestClass]
	public class SpeakerTests
	{
		private readonly SqlServerCompactRepository _repository = new SqlServerCompactRepository();

		[TestMethod]
		public void Register_EmptyFirstName_ThrowsArgumentNullException()
		{
			//arrange
			var speaker = GetSpeakerThatWouldBeApproved();
			speaker.FirstName = "";
			
			//act
			var exception = ExceptionAssert.Throws<ArgumentNullException>( () => speaker.Register(_repository));

			//assert
			Assert.AreEqual(exception.GetType(), typeof(ArgumentNullException));
		}

		[TestMethod]
		public void Register_EmptyLastName_ThrowsArgumentNullException()
		{
			//arrange
			var speaker = GetSpeakerThatWouldBeApproved();
			speaker.LastName = "";

			//act
			var exception = ExceptionAssert.Throws<ArgumentNullException>(() => speaker.Register(_repository));

			//assert
			Assert.AreEqual(exception.GetType(), typeof(ArgumentNullException));
		}

		[TestMethod]
		public void Register_EmptyEmail_ThrowsArgumentNullException()
		{
			//arrange
			var speaker = GetSpeakerThatWouldBeApproved();
			speaker.Email = "";

			//act
			var exception = ExceptionAssert.Throws<ArgumentNullException>(() => speaker.Register(_repository));

			//assert
			Assert.AreEqual(exception.GetType(), typeof(ArgumentNullException));
		}

		[TestMethod]
		public void Register_WorksForPrestigiousEmployerButHasRedFlags_ReturnsSpeakerId()
		{
			//arrange
			var speaker = GetSpeakerWithRedFlags();
			speaker.Employer = "Microsoft";

			//act
			int? speakerId = speaker.Register(new SqlServerCompactRepository());

			//assert
			Assert.IsFalse(speakerId == null);
		}

		[TestMethod]
		public void Register_HasBlogButHasRedFlags_ReturnsSpeakerId()
		{
			//arrange
			var speaker = GetSpeakerWithRedFlags();

			//act
			int? speakerId = speaker.Register(new SqlServerCompactRepository());

			//assert
			Assert.IsFalse(speakerId == null);
		}

		[TestMethod]
		public void Register_HasCertificationsButHasRedFlags_ReturnsSpeakerId()
		{
			//arrange
			var speaker = GetSpeakerWithRedFlags();
			speaker.Certifications = new List<string>()
			{
				"cert1",
				"cert2",
				"cert3",
				"cert4"
			};

			//act
			int? speakerId = speaker.Register(new SqlServerCompactRepository());

			//assert
			Assert.IsFalse(speakerId == null);
		}

		[TestMethod]
		public void Register_SingleSessionThatsOnOldTech_ThrowsNoSessionsApprovedException()
		{
			//arrange
			var speaker = GetSpeakerThatWouldBeApproved();
			speaker.Sessions = new List<Session>() {
				new Session("Cobol for dummies", "Intro to Cobol")
			};

			//act
			var exception = ExceptionAssert.Throws<NoSessionsApprovedException>(() => speaker.Register(_repository));

			//assert
			Assert.AreEqual(exception.GetType(), typeof(NoSessionsApprovedException));
		}

		[TestMethod]
		public void Register_NoSessionsPassed_ThrowsArgumentException()
		{
			//arrange
			var speaker = GetSpeakerThatWouldBeApproved();
			speaker.Sessions = new List<Session>();

			//act
			var exception = ExceptionAssert.Throws<ArgumentException>(() => speaker.Register(_repository));

			//assert
			Assert.AreEqual(exception.GetType(), typeof(ArgumentException));
		}

		[TestMethod]
		public void Register_DoesntAppearExceptionalAndUsingOldBrowser_ThrowsNoSessionsApprovedException()
		{
			//arrange
			var speakerThatDoesntAppearExceptional = GetSpeakerThatWouldBeApproved();
			speakerThatDoesntAppearExceptional.HasBlog = false;
			speakerThatDoesntAppearExceptional.Browser = new WebBrowser("IE", 6);

			//act
			var exception = ExceptionAssert.Throws<SpeakerDoesntMeetRequirementsException>(() => speakerThatDoesntAppearExceptional.Register(_repository));

			//assert
			Assert.AreEqual(exception.GetType(), typeof(SpeakerDoesntMeetRequirementsException));
		}

		[TestMethod]
		public void Register_DoesntAppearExceptionalAndHasAncientEmail_ThrowsNoSessionsApprovedException()
		{
			//arrange
			var speakerThatDoesntAppearExceptional = GetSpeakerThatWouldBeApproved();
			speakerThatDoesntAppearExceptional.HasBlog = false;
			speakerThatDoesntAppearExceptional.Email = "name@aol.com";

			//act
			var exception = ExceptionAssert.Throws<SpeakerDoesntMeetRequirementsException>(() => speakerThatDoesntAppearExceptional.Register(_repository));

			//assert
			Assert.AreEqual(exception.GetType(), typeof(SpeakerDoesntMeetRequirementsException));
		}

		#region Helpers
		private Speaker GetSpeakerThatWouldBeApproved()
		{
			return new Speaker()
			{
				FirstName = "First",
				LastName = "Last",
				Email = "example@domain.com",
				Employer = "Example Employer",
				HasBlog = true,
				Browser = new WebBrowser("test", 1),
				YearsExperiencie = 1,
				Certifications = new List<string>(),
				BlogUrl = "",
				Sessions = new List<Session>
				{
					new Session("test title", "test description")
				}
			};
		}

		private Speaker GetSpeakerWithRedFlags()
		{
			var speaker = GetSpeakerThatWouldBeApproved();
			speaker.Email = "tom@aol.com";
			speaker.Browser = new WebBrowser("IE", 6);
			return speaker;
		}
		#endregion
	}
}
