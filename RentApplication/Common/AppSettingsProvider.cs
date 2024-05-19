namespace RentApplication.Common
{
	public class AppSettingsProvider
	{
		public AppSettingsProvider(string environmentName)
		{
			_appSettingsFileName = _fileName + "." + environmentName + "." + _fileExtension;
		}

		public TSettings GetSettings<TSettings>(string sectionName = null)
		{
			if (string.IsNullOrWhiteSpace(sectionName))
			{
				sectionName = typeof(TSettings).Name;
			}

			var configurationRoot = new ConfigurationBuilder().AddJsonFile(_appSettingsFileName, true, true).Build();

			var section = configurationRoot.GetSection(sectionName);

			return section.Get<TSettings>();
		}

		private const string _fileName = "appsettings";
		private const string _fileExtension = "json";
		private readonly string _appSettingsFileName;
	}
}
