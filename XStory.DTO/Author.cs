using System;
using System.Collections.Generic;
using System.Text;

namespace XStory.DTO
{
	public class Author
	{
		/// <summary>
		/// Common
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Common
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Common
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// Common
		/// </summary>
		public string Avatar { get; set; }

		/// <summary>
		/// Common
		/// </summary>
		public List<DTO.Story> Stories { get; set; }

		// Author-page infos

		/// <summary>
		/// Status - XStory ONLY
		/// </summary>
		public string Status { get; set; }

		/// <summary>
		/// Status - XStory ONLY
		/// </summary>
		public bool IsCertified { get; set; }

		/// <summary>
		/// XStory ONLY
		/// </summary>
		public string Rank30Days { get; set; }

		/// <summary>
		/// XStory ONLY
		/// </summary>
		public string RankAllTime { get; set; }

		/// <summary>
		/// XStory ONLY
		/// </summary>
		public string FollowedBy { get; set; }

		/// <summary>
		/// XStory ONLY
		/// </summary>
		public string RegisterDate { get; set; }

		/// <summary>
		/// Common
		/// </summary>
		public string Gender { get; set; }

		/// <summary>
		/// XStory ONLY
		/// </summary>
		public string Age { get; set; }

		/// <summary>
		/// XStory ONLY
		/// </summary>
		public string Location { get; set; }


		/// <summary>
		/// HDS ONLY
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// HDS ONLY
		/// </summary>
		public string Slogan { get; set; }

		/// <summary>
		/// HDS ONLY
		/// </summary>
		public bool HasMorePages { get; set; }

		public Author()
		{
			Stories = new List<DTO.Story>();
		}
	}
}
