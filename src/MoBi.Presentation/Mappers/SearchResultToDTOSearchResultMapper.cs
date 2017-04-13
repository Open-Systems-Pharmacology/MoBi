using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Mappers
{
   public interface ISearchResultToDTOSearchResultMapper : IMapper<SearchResult, SearchResultDTO>
   {
   }

   internal class SearchResultToDTOSearchResultMapper : ISearchResultToDTOSearchResultMapper
   {
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IObjectTypeResolver _objectTypeResolver;

      public SearchResultToDTOSearchResultMapper(IObjectPathFactory objectPathFactory, IObjectTypeResolver objectTypeResolver)
      {
         _objectPathFactory = objectPathFactory;
         _objectTypeResolver = objectTypeResolver;
      }

      public SearchResultDTO MapFrom(SearchResult searchResult)
      {
         var dto = new SearchResultDTO();
         var foundObject = searchResult.FoundObject;
         dto.Object = foundObject;
         if (foundObject.IsAnImplementationOf<IEntity>())
         {
            dto.Path = _objectPathFactory.CreateAbsoluteObjectPath((IEntity) foundObject).ToString();
         }
         else
         {
            dto.Path = foundObject.Name;
         }
         dto.TypeName = _objectTypeResolver.TypeFor(foundObject);
         dto.ProjectItem = searchResult.ProjectItem;
         dto.ProjectItemName = $"{_objectTypeResolver.TypeFor(searchResult.ProjectItem)}: {searchResult.ProjectItem.Name}";
         return dto;
      }
   }
}