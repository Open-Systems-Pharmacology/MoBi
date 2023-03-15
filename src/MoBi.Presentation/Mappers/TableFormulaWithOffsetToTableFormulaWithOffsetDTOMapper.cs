using System.Linq;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface ITableFormulaWithOffsetToTableFormulaWithOffsetDTOMapper : IMapper<TableFormulaWithOffset, TableFormulaWithOffsetDTO>
   {
   }

   public class TableFormulaWithOffsetToTableFormulaWithOffsetDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, ITableFormulaWithOffsetToTableFormulaWithOffsetDTOMapper
   {
      private readonly IFormulaUsablePathToFormulaUsablePathDTOMapper _mapper;

      public TableFormulaWithOffsetToTableFormulaWithOffsetDTOMapper(IFormulaUsablePathToFormulaUsablePathDTOMapper mapper)
      {
         _mapper = mapper;
      }

      public TableFormulaWithOffsetDTO MapFrom(TableFormulaWithOffset tableFormulaWithOffset)
      {
         var dto = Map(new TableFormulaWithOffsetDTO(tableFormulaWithOffset));

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

      private static FormulaUsablePath getPathByAlias(TableFormulaWithOffset input, string pathAlias)
      {
         return input.ObjectPaths.SingleOrDefault(x => x.Alias.Equals(pathAlias));
      }
   }
}