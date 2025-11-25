using System;
using System.Collections.Generic;
using System.Text;
using XStory.DTO.Config;

namespace XStory.BL.Common.Contracts
{
	/// <summary>
	/// Xamarin.Forms specific configuration (DataSources, Colors, Theming...)
	/// </summary>
	public interface IServiceConfig
	{
		
		/// Gets Stories contexts data sources from a JSON file
		// List<DataSourceItem> GetDataSourcesFromJSON();

		/// <summary>
		/// Sets curret data source from Enum.
		/// </summary>
		/// <returns></returns>
		void SetCurrentDataSource(DataSources dataSource);
		Array GetDataSources();
		DataSources GetCurrentDataSource();
	}
}
