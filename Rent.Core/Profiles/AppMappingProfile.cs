using AutoMapper;
using Rent.Core.Managers.Data;
using Rent.DataAccess.Entity;

namespace Rent.Core.Managers.Profiles
{
	public class AppMappingProfile : Profile
	{
		public AppMappingProfile()
		{
			CreateMap<User, UserData>();
			CreateMap<UserData, User>();

			CreateMap<Chapter, ChapterData>();
			CreateMap<ChapterData, Chapter>();

			CreateMap<RentItem, RentItemData>();
			CreateMap<RentItemData, RentItem>();

			CreateMap<Properties, PropertiesData>();
			CreateMap<PropertiesData, Properties>();

			CreateMap<ChapterPropertiesConnection, ChapterPropertiesConnectionData>();
			CreateMap<ChapterPropertiesConnectionData, ChapterPropertiesConnection>();

			CreateMap<RentItemPropertiesConnection, RentItemPropertiesConnectionData>();
			CreateMap<RentItemPropertiesConnectionData, RentItemPropertiesConnection>();
		}
	}
}
