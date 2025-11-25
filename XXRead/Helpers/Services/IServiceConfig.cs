namespace XXRead.Helpers.Services
{
	/// <summary>
	/// Xamarin.Forms specific configuration (DataSources, Colors, Theming...)
	/// </summary>
	public interface IServiceConfig
	{
		/// <summary>
		/// Gets Stories contexts data sources from a JSON file
		/// </summary>
		/// <returns></returns>
		List<DataSourceItem> GetDataSourcesFromJSON();


	}
}
