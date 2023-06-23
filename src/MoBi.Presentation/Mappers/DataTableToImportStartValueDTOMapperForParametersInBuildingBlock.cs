using System.Data;
using MoBi.Assets;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface IDataTableToImportQuantityDTOMapperForParameters
   {
      /// <summary>
      ///    Maps quantity DTO's from a data table. Some validation against the import target is also done while convertfing
      /// </summary>
      /// <param name="table">The table being converted</param>
      /// <param name="buildingBlock">The context building block. This will be the target of the import</param>
      /// <returns>The DTO containing all the quantities to be updated or inserted</returns>
      QuantityImporterDTO MapFrom(DataTable table, PathAndValueEntityBuildingBlock<ParameterValue> buildingBlock);
   }

   public class DataTableToImportQuantityDTOMapperForParameters : DataTableToImportParameterQuantityDTOMapper<PathAndValueEntityBuildingBlock<ParameterValue>>, IDataTableToImportQuantityDTOMapperForParameters
   {
      public DataTableToImportQuantityDTOMapperForParameters(IMoBiDimensionFactory dimensionFactory)
         : base(dimensionFactory)
      {
      }

      protected override void ValidateInContext(ImportedQuantityDTO dto, QuantityImporterDTO quantityImporterDTO, PathAndValueEntityBuildingBlock<ParameterValue> buildingBlock, DataRow row, int rowIndex)
      {
         base.ValidateInContext(dto, quantityImporterDTO, buildingBlock, row, rowIndex);
         var builder = buildingBlock[dto.Path];
         if (builder != null)
            ValidateDtoAgainstBuilderForUpdate(builder, dto, row, rowIndex);
      }

      protected override bool ValidateDTOForUpdate(ImportedQuantityDTO dto)
      {
         return !double.IsNaN(dto.QuantityInBaseUnit);
      }

      protected override bool IsUpdate(ImportedQuantityDTO dto, PathAndValueEntityBuildingBlock<ParameterValue> importTarget)
      {
         return !IsNewInsert(dto, importTarget);
      }

      protected override bool IsNewInsert(ImportedQuantityDTO dto, PathAndValueEntityBuildingBlock<ParameterValue> importTarget)
      {
         return importTarget[dto.Path] == null;
      }

      protected override bool ValidateDTOForInsert(ImportedQuantityDTO dto)
      {
         return !double.IsNaN(dto.QuantityInBaseUnit);
      }

      protected override string LogMessageFor(ImportedQuantityDTO dto, PathAndValueEntityBuildingBlock<ParameterValue> importTarget)
      {
         return IsNewInsert(dto, importTarget)
            ? AppConstants.Captions.AddingParameterValue(dto.Path, dto.ConvertToDisplayUnit(dto.QuantityInBaseUnit), dto.DisplayUnit)
            : AppConstants.Captions.UpdatingParameterValue(dto.Path, dto.ConvertToDisplayUnit(dto.QuantityInBaseUnit), dto.DisplayUnit);
      }
   }
}