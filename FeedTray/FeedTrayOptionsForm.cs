using Common.TrayApplication;
using FeedTray.Data.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FeedTray
{
	public partial class FeedTrayOptionsForm : OptionsForm
	{
		private Dictionary<int, Feed> _BoundFeeds;

		public FeedTrayOptionsForm()
		{
			InitializeComponent();

			// setup listview
			feedsListBox.DisplayMember = "Title";

			// on creation, we bind to an empty list
			BindFeeds(new Dictionary<int, Feed>());
		}

		public void BindFeeds(Dictionary<int, Feed> feeds)
		{
			_BoundFeeds = feeds;
			feedsListBox.Items.Clear();
			foreach (Feed feed in _BoundFeeds.Values)
			{
				feedsListBox.Items.Add(feed, feed.Enabled);
			}
		}

		public override void PopulateSettings()
		{
			// TODO
		}

		#region Event handlers

		private void addButton_Click(object sender, EventArgs e)
		{
			int newId = _BoundFeeds.Values.Max(f => f.Id) + 1;
			OnFeedUpdated(new Feed { Id = newId, Title = "New Feed" });
		}

		private void feedsListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			feedPropertyGrid.SelectedObject = _BoundFeeds[feedsListBox.SelectedIndex];
		}

		private void feedPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			// TODO This may not be needed
			OnFeedUpdated((Feed)feedPropertyGrid.SelectedObject);
		}

		#endregion

		#region Feed updated event

		public class FeedUpdatedEventArgs : EventArgs
		{
			public Feed Feed { get; set; }
		}

		public delegate void FeedUpdatedEvent(object sender, FeedUpdatedEventArgs e);

		public event FeedUpdatedEvent FeedUpdated;

		protected void OnFeedUpdated(Feed feed)
		{
			if (FeedUpdated != null)
			{
				FeedUpdated(this, new FeedUpdatedEventArgs { Feed = feed });
			}
		}

		#endregion
	}
}
