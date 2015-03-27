using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace FeedTray.Data.Config
{
	public class FeedItem
	{
		[PrimaryKey]
		public string Guid { get; set; }
		public string Title { get; set; }
		public string Link { get; set; }
		public string Category { get; set; }
		public DateTime PublishedDate { get; set; }
		public string Description { get; set; }
		public string Author { get; set; }
		public DateTime? Read { get; set; }

		[ForeignKey(typeof(Feed))]
		public int FeedId { get; set; }

		[ManyToOne]
		public Feed Feed { get; set; }
	}
}
