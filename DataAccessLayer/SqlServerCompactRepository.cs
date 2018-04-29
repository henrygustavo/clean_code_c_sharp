namespace DataAccessLayer
{
    using System.Collections.Generic;
    using System.Linq;
    using BusinessLayer;

    public class SqlServerCompactRepository : IRepository
	{
		public int SaveSpeaker(Speaker speaker)
		{
			//TODO: Save speaker to DB for now. For demo, just assume success and return 1.
			return 1;
		}

	    public int GetRegistrationFee(int? yearsExperience)
	    {
            //TODO: Create a table Fee and populate it wih the data that is bellow

	        List<FeeTable> feeFakeData = new List<FeeTable>
	        {
	            new FeeTable {Fee = 500, MinYearExperience = 0, MaxYearExperience = 1},

	            new FeeTable {Fee = 250, MinYearExperience = 2, MaxYearExperience = 3},

	            new FeeTable {Fee = 100, MinYearExperience = 4, MaxYearExperience = 5},

	            new FeeTable {Fee = 50, MinYearExperience = 6, MaxYearExperience = 9}

	        };

	        var feedResult = feeFakeData.FirstOrDefault(x =>
	            x.MinYearExperience >= yearsExperience && x.MaxYearExperience <= yearsExperience);

	        return feedResult?.Fee ?? 0;
	    }
	}
}
