using System.Linq;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface ITableFormulaWithXArgumentToTableFormulaWithXArgumentDTOMapper : IMapper<TableFormulaWithXArgument, TableFormulaWithXArgumentDTO>
   {
   }

   public class TableFormulaWithXArgumentToTableFormulaWithXArgumentDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, ITableFormulaWithXArgumentToTableFormulaWithXArgumentDTOMapper
   {
      private readonly IFormulaUsablePathToFormulaUsablePathDTOMapper _mapper;

      public TableFormulaWithXArgumentToTableFormulaWithXArgumentDTOMapper(IFormulaUsablePathToFormulaUsablePathDTOMapper mapper)
      {
         _mapper = mapper;
      }

      public TableFormulaWithXArgumentDTO MapFrom(TableFormulaWithXArgument tableFormulaWithXArgument)
      {
         var dto = Map<TableFormulaWithXArgumentDTO>(tableFormulaWithXArgument);

         var xArgumentObjectPath = getPathByAlias(tableFormulaWithXArgument, tableFormulaWithXArgument.XArgumentAlias);
         if (xArgumentObjectPath != null)
         {
            dto.XArgumentObjectPath = _mapper.MapFrom(xArgumentObjectPath, tableFormulaWithXArgument);
         }

         var tableObjectPath = getPathByAlias(tableFormulaWithXArgument, tableFormulaWithXArgument.TableObjectAlias);
         if (tableObjectPath != null)
         {
            dto.TableObjectPath = _mapper.MapFrom(tableObjectPath, tableFormulaWithXArgument);
         }

         return dto;
      }

      private static FormulaUsablePath getPathByAlias(TableFormulaWithXArgument tableFormulaWithXArgument, string pathAlias)
      {
         return tableFormulaWithXArgument.ObjectPaths.SingleOrDefault(x => x.Alias.Equals(pathAlias));
      }
   }
}