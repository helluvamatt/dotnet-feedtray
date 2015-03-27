using Common.TrayApplication;
using FeedTray.Data.Config;
using FeedTray.Data.Config.Async;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FeedTray
{
	public class FeedTrayApplicationContext : TrayApplicationContext
	{
		#region Public properties

		/// <summary>
		/// Cached feed list
		/// </summary>
		public Dictionary<int, Feed> Feeds { get; private set; }

		/// <summary>
		/// Config database
		/// </summary>
		public Database ConfigDatabase { get; private set; }

		#endregion

		public FeedTrayApplicationContext() : base()
		{
			Feeds = new Dictionary<int, Feed>();

			// Initialize the AppDataFolder and make sure it is created: %APPDATA%/FeedTray
			AppDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create), "FeedTray");
			Directory.CreateDirectory(AppDataFolder);

			// Initialize the database under the AppDataFolder: %APPDATA%/FeedTray/FeedTray.db
			string dbFile = Path.Combine(AppDataFolder, "FeedTray.db");
			ConfigDatabase = new Database(dbFile);
		}

		#region TrayApplicationContext implementation

		protected override OptionsForm BuildOptionsForm()
		{
			FeedTrayOptionsForm form = new FeedTrayOptionsForm();
			form.FeedUpdated += optionsForm_FeedUpdated;
			return form;
		}

		protected override void OnInitializeContext()
		{
			ConfigDatabase.GetFeedsAsync(ConfigDatabase_GetFeeds);
		}

		protected override void BuildContextMenu()
		{
			// Build the context menu: showOptionsMenuItem
			ToolStripMenuItem showOptionsMenuItem = new ToolStripMenuItem("&Manage");
			showOptionsMenuItem.Click += showOptionsMenuItem_Click;

			// Build the context menu: feedsMenuItem
			ToolStripMenuItem feedsMenuItem = new ToolStripMenuItem("Feeds");
			if (Feeds.Count > 0)
			{
				foreach (Feed item in Feeds.Values)
				{
					ToolStripMenuItem feedMenuItem = new ToolStripMenuItem(item.Title);
					feedMenuItem.Tag = item.Id;
					feedMenuItem.CheckOnClick = true;
					feedMenuItem.Checked = item.Enabled;
					feedMenuItem.DoubleClickEnabled = true;
					feedMenuItem.DoubleClick += feedMenuItem_DoubleClick;
					feedMenuItem.CheckedChanged += feedMenuItem_CheckedChanged;
					feedMenuItem.DropDownItems.Add(feedMenuItem);
				}
			}
			else
			{
				ToolStripMenuItem noFeedsMenuItem = new ToolStripMenuItem("No Feeds");
				noFeedsMenuItem.Enabled = false;
				feedsMenuItem.DropDownItems.Add(noFeedsMenuItem);
			}
			
			// Build the context menu: exitMenuItem
			ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("E&xit");
			exitMenuItem.Click += exitMenuItem_Click;

			// Build contextMenu
			notifyIcon.ContextMenuStrip.Items.Add(showOptionsMenuItem);
			notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			notifyIcon.ContextMenuStrip.Items.Add(feedsMenuItem);
			notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			notifyIcon.ContextMenuStrip.Items.Add(exitMenuItem);
		}

		#endregion

		#region Event handlers

		private void feedMenuItem_CheckedChanged(object sender, EventArgs e)
		{
			ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
			int feedId = (int)menuItem.Tag;
			Feed feed = Feeds[feedId];
			feed.Enabled = menuItem.Checked;
			Logger.DebugFormat("CheckedChanged on Feed: {0} ({1}) to {2}", feedId, feed.Title, feed.Enabled);
			OnFeedUpdated(feed);
		}

		private void feedMenuItem_DoubleClick(object sender, EventArgs e)
		{
			ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
			int feedId = (int)menuItem.Tag;
			Feed feed = Feeds[feedId];
			Logger.DebugFormat("DoubleClick on Feed: {0} ({1})", feedId, feed.Title);
			// TODO Actually do something
		}

		// attach to context menu items
		private void showOptionsMenuItem_Click(object sender, EventArgs e)
		{
			CreateOptionsForm();
		}

		// exit the application
		private void exitMenuItem_Click(object sender, EventArgs e)
		{
			ExitThread();
		}

		private void optionsForm_FeedUpdated(object sender, FeedTrayOptionsForm.FeedUpdatedEventArgs e)
		{
			OnFeedUpdated(e.Feed);
		}

		private void ConfigDatabase_GetFeeds(Database.GetFeedsResult result)
		{
			if (result.Success)
			{
				foreach (KeyValuePair<int, Feed> entry in result.Feeds)
				{
					Feeds.Add(entry.Key, entry.Value);
				}
				Logger.Info("Get feeds successful.");
			}
			else
			{
				Logger.Error("Get feeds failed:", result.Exception);
			}
		}

		private void ConfigDatabase_SaveFeed(BasicResult result)
		{
			if (result.Success)
			{
				Logger.Info("Save feed successful.");
			}
			else
			{
				Logger.Error("Save feed failed:", result.Exception);
			}
		}

		#endregion

		#region Utility methods

		private void OnFeedUpdated(Feed feed)
		{
			// Save changes
			ConfigDatabase.SaveFeedAsync(ConfigDatabase_SaveFeed, feed);

			// Make sure the local cache reflects the change
			Feeds[feed.Id] = feed;

			// Send the update back to the optionsForm
			if (optionsForm != null) ((FeedTrayOptionsForm)optionsForm).BindFeeds(Feeds);

			// TODO Make sure the tray menu is updating
		}

		#endregion
	}
}
