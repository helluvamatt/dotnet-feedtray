using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using log4net;
using FeedTray.Data.Config.Async;
using System.Reflection;
using System.Data;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Platform.Win32;

namespace FeedTray.Data.Config
{
	/// <summary>
	/// Represents a connection to the configuration SQLite database
	/// </summary>
	public class Database : IDisposable
	{
		#region Public properties

		/// <summary>
		/// Logger interface
		/// </summary>
		public ILog Logger { get; private set; }

		#endregion

		private SQLiteConnection _Connection;

		/// <summary>
		/// Constructor
		/// </summary>
		public Database(string dbFile)
		{
			Logger = LogManager.GetLogger(GetType());
			_Connection = new SQLiteConnection(new SQLitePlatformWin32(), dbFile, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite);
			_Connection.CreateTable<Feed>();
			_Connection.CreateTable<FeedItem>();
		}

		#region Public methods

		// TODO anything else that needs to be saved in here

		#endregion

		#region Get Feeds

		/// <summary>
		/// Synchrounous load
		/// </summary>
		/// <param name="dbFile">File from which to load the database</param>
		public GetFeedsResult GetFeeds()
		{
			try
			{
				Logger.Info("Loading feeds from database...");
				Dictionary<int, Feed> feeds = _Connection.Table<Feed>().ToDictionary<Feed, int>(feed => feed.Id);
				Logger.InfoFormat("Load finished. {0} feed(s) loaded.", feeds.Count);
				return new GetFeedsResult { Exception = null, Success = true, Feeds = feeds };
			}
			catch (Exception ex)
			{
				return new GetFeedsResult { Exception = ex, Success = false, Feeds = null };
			}
		}

		/// <summary>
		/// Asynchronously load the database
		/// </summary>
		/// <param name="dbFile">File from which to load the database</param>
		public async void GetFeedsAsync(AsyncFeedsLoadedEvent callback)
		{
			GetFeedsResult result = await Task<GetFeedsResult>.Factory.StartNew(GetFeeds);
			if (callback != null) callback(result);
		}

		/// <summary>
		/// Result of an Asynchrounous Load
		/// </summary>
		public class GetFeedsResult : BasicResult
		{
			public Dictionary<int, Feed> Feeds { get; set; }
		}

		/// <summary>
		/// Event type for notifying when an asynchronous job is finished and its result
		/// </summary>
		/// <param name="result"></param>
		public delegate void AsyncFeedsLoadedEvent(GetFeedsResult result);

		#endregion

		#region Update feeds

		public BasicResult SaveFeed(Feed feed)
		{
			try
			{
				Logger.InfoFormat("Saving feed ({0})...", feed.Title);
				int updated = _Connection.InsertOrReplace(feed);
				Logger.InfoFormat("Save finished. Return = {0}", updated);
				return new BasicResult { Exception = null, Success = true };
			}
			catch (Exception ex)
			{
				return new BasicResult { Exception = ex, Success = false };
			}
		}

		public async void SaveFeedAsync(AsyncSaveFeedEvent callback, Feed feed)
		{
			BasicResult result = await Task<BasicResult>.Factory.StartNew(t => SaveFeed(t as Feed), feed);
			if (callback != null) callback(result);
		}

		/// <summary>
		/// Event type for notifying when an asynchronous job is finished and its result
		/// </summary>
		/// <param name="result"></param>
		public delegate void AsyncSaveFeedEvent(BasicResult result);

		#endregion

		#region Utility methods

		private DateTime? GetNullableDateTimeFromTimestampField(IDataReader reader, string field)
		{
			int ord = reader.GetOrdinal(field);
			return reader.IsDBNull(ord) ? (DateTime?)null : DateUtils.DateTimeFromUnixTimestampSeconds(reader.GetInt64(ord));
		}

		private DateTime GetDateTimeFromTimestampField(IDataReader reader, string field)
		{
			int ord = reader.GetOrdinal(field);
			return DateUtils.DateTimeFromUnixTimestampSeconds(reader.GetInt64(ord));
		}

		private string GetString(IDataReader reader, string field)
		{
			int ord = reader.GetOrdinal(field);
			return reader.IsDBNull(ord) ? null : reader.GetString(ord);
		}

		#endregion

		#region IDisposable interface

		public void Dispose()
		{
			// Close the SQLite database
			_Connection.Dispose();
		}

		#endregion
	}
}
