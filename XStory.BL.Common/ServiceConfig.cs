using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using XStory.BL.Common.Contracts;
using XStory.DTO.Config;
using XStory.Logger;

namespace XStory.BL.Common
{
	public class ServiceConfig : IServiceConfig
	{

		public void SetCurrentDataSource(DataSources dataSource)
		{
			StaticContext.DataSource = dataSource;
		}

		public Array GetDataSources()
		{
			return Enum.GetValues(typeof(DataSources));
		}

		public DataSources GetCurrentDataSource()
		{
			return StaticContext.DataSource;
		}
	}
}
