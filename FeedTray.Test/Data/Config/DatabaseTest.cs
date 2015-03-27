using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FeedTray.Data.Config
{
	[TestFixture]
	class DatabaseTest
	{
		[Test]
		public void GetFeeds()
		{
			string dbFile = "FeedTray.Test.GetFeeds.db";
			//ExtractTestDb(dbFile);
			Database database = new Database(dbFile);
			Database.GetFeedsResult result = database.GetFeeds();
			Assert.AreEqual(3, result.Feeds.Count, "Feeds.Count");
			database.Dispose();
		}
		
	}
}
