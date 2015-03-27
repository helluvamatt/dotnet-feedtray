using Newtonsoft.Json;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedTray.Data.Config
{
	public class Feed
    {
		[PrimaryKey, AutoIncrement]
		public int Id { get;set;}

		public string Title { get; set; }
		public string Url { get; set; }
		public string IconImageUrl { get; set; }
		public DateTime? DateAdded { get; set; }
		public bool Enabled { get; set; }

		[OneToMany(CascadeOperations=CascadeOperation.None)]
		public List<FeedItem> Items { get; set; }
	}
}
