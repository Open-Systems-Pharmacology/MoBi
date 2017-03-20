using System.Linq;
using OSPSuite.Utility;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.Mappers
{
   public interface ITableFormulaWithOffsetToDTOTableFormulaWithOffsetMapper : IMapper<TableFormulaWithOffset, TableFormulaWithOffsetDTO>
   {
   }

   internal class TableFormulaWithOffsetToDTOTableFormulaWithOffsetMapper : ObjectBaseToObjectBaseDTOMapperBase, ITableFormulaWithOffsetToDTOTableFormulaWithOffsetMapper
   {
      private readonly IFormulaUsablePathToFormulaUsablePathDTOMapper _mapper;

      public TableFormulaWithOffsetToDTOTableFormulaWithOffsetMapper(IFormulaUsablePathToFormulaUsablePathDTOMapper mapper)
      {
         _mapper = mapper;
      }

      public TableFormulaWithOffsetDTO MapFrom(TableFormulaWithOffset tableFormulaWithOffset)
      {
         var dto = Map<TableFormulaWithOffsetDTO>(tableFormulaWithOffset);

         var offsetPath = getPathByAlias(tableFormulaWithOffset, tableFormulaWithOffset.OffsetObjectAlias);
         if (offsetPath != null)
         {
            dto.OffsetObjectPath = _mapper.MapFrom(offsetPath, tableFormulaWithOffset);
         }

         var tableObjectPath = getPathByAlias(tableFormulaWithOffset, tableFormulaWithOffset.TableObjectAlias);
         if (tableObjectPath != null)
         {
            dto.TableObjectPath = _mapper.MapFrom(tableObjectPath, tableFormulaWithOffset);
         }
         return dto;
      }

      private static IFormulaUsablePath getPathByAlias(TableFormulaWithOffset input, string pathAlias)
      {
         return input.ObjectPaths.SingleOrDefault(x => x.Alias.Equals(pathAlias));
      }
   }
}