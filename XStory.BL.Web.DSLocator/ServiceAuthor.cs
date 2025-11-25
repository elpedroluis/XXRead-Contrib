using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XStory.BL.Web.DSLocator.Contracts;
using XStory.DTO;

namespace XStory.BL.Web.DSLocator
{
	public class ServiceAuthor : IServiceAuthor
	{
		private BL.Web.XStory.Contracts.IServiceAuthor _serviceAuthorXStory;
		private BL.Web.HDS.Contracts.IServiceAuthor _serviceAuthorHDS;
		private BL.Web.Demo.Contracts.IServiceAuthor _serviceAuthorDemo;

		public ServiceAuthor(BL.Web.XStory.Contracts.IServiceAuthor serviceAuthorXStory,
			BL.Web.HDS.Contracts.IServiceAuthor serviceAuthorHDS,
			BL.Web.Demo.Contracts.IServiceAuthor serviceAuthorDemo)
		{
			_serviceAuthorXStory = serviceAuthorXStory;
			_serviceAuthorHDS = serviceAuthorHDS;
			_serviceAuthorDemo = serviceAuthorDemo;
		}

		public async Task<Author> GetAuthorPage(string dataSource, Author author, int pageNumber = 1)
		{
			try
			{
				switch (dataSource)
				{
					case "XStory":
						return await _serviceAuthorXStory.GetAuthorPage(author);
					case "HDS":
						return await _serviceAuthorHDS.GetAuthorPage(author, pageNumber);
					case "Demo":
						return await _serviceAuthorDemo.GetAuthorPage(author);
					default /* "All" */:
						// TO BE IMPLEMENTED
						return null;
				}
			}
			catch (Exception ex)
			{
				Logger.ServiceLog.Error(ex);
				return null;
			}
		}
	}
}
