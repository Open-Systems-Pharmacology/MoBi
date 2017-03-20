using System.Data;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Mappers
{
   public abstract class DataTableToImportParameterQuantityDTOMapper<TImportTarget> : AbstractDataTableProviderToQuantityDTOMapper<TImportTarget> 
   {
      protected DataTableToImportParameterQuantityDTOMapper(IMoBiDimensionFactory dimensionFactory)
         : base(dimensionFactory)
      {
      }

      protected static class DataTableRowIndexes
      {
         public const int CONTAINER_PATH = 0;
         public const int NAME = 1;
         public const int VALUE = 2;
         public const int UNIT = 3;
         public const int COLUMNS = 4;
      }

      protected override ImportedQuantityDTO MapQuantityFromRow(DataTable table, DataRow row, int rowIndex)
      {
         if (row.ItemArray.Count() < DataTableRowIndexes.COLUMNS)
            throw new ImportQuantityDTOsFromDataTablesMapperException(row, rowIndex, AppConstants.Exceptions.TableShouldBeNColumns(DataTableRowIndexes.COLUMNS));

         var dimension = GetDimension(table, rowIndex, DataTableRowIndexes.UNIT);
         var containerPath = GetPath(row, DataTableRowIndexes.CONTAINER_PATH);
         var quantity = GetQuantity(table, rowIndex, DataTableRowIndexes.VALUE);
         var parameterName = GetQuantityName(row, DataTableRowIndexes.NAME);

         var dto = new ImportedQuantityDTO
         {
            Dimension = dimension,
            ContainerPath = new ObjectPath(containerPath),
            Name = parameterName,
            DisplayUnit = dimension.Unit(row[DataTableRowIndexes.UNIT].ToString()),
            IsQuantitySpecified = true,
            IsScaleDivisorSpecified = false
         };

         dto.QuantityInBaseUnit = dto.ConvertToBaseUnit(quantity);

         return dto;
      }

      protected override void AddDTOToImportList(QuantityImporterDTO quantityImporterDTO, TImportTarget importTarget, ImportedQuantityDTO dto)
      {
         if (!dto.SkipImport)
         {
            quantityImporterDTO.QuantitDTOs.Add(dto);
            quantityImporterDTO.AddToLog(LogMessageFor(dto, importTarget));
         }

         if (dto.HasWarning())
            quantityImporterDTO.AddToLog(dto.GetWarning());
      }

      protected abstract string LogMessageFor(ImportedQuantityDTO dto, TImportTarget importTarget);

      protected void ValidateDtoAgainstBuilderForUpdate(IWithDimension originalWithDimension, ImportedQuantityDTO dto, DataRow row, int rowIndex)
      {
         if (Equals(originalWithDimension.Dimension, dto.Dimension))
            return;

         //Different dimensions sharing a common unit. Make sure we import in the expected dimension
         if (haveCommonUnits(originalWithDimension.Dimension, dto.DisplayUnit))
         {
            updateImportedQuantityWithQuantityDimension(originalWithDimension, dto);
            return;
         }

         // +2 because there is a mandatory header row (+1) and this index is 0-based, while excel rows start at 1 (+1)
         dto.SetWarning(row, rowIndex + 2, AppConstants.Warnings.TheImportedDimensionDoesNotMatchTheExistingQuantity(dto.Path.ToString(), originalWithDimension.Dimension, dto.Dimension));
      }

      private static void updateImportedQuantityWithQuantityDimension(IWithDimension originalWithDimension, ImportedQuantityDTO dto)
      {
         var valueInDisplayUnit = dto.ConvertToDisplayUnit(dto.QuantityInBaseUnit);
         dto.Dimension = originalWithDimension.Dimension;
         dto.DisplayUnit = originalWithDimension.Dimension.Unit(dto.DisplayUnit.Name);
         dto.QuantityInBaseUnit = dto.ConvertToBaseUnit(valueInDisplayUnit);
      }

      private bool haveCommonUnits(IDimension sourceDimension, Unit importedUnit)
      {
         return sourceDimension.HasUnit(importedUnit.Name);
      }
   }
}