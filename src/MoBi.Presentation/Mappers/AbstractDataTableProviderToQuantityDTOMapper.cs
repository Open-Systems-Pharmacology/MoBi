using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Core.Extensions;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;

namespace MoBi.Presentation.Mappers
{
   public abstract class AbstractDataTableProviderToQuantityDTOMapper<TImportTarget>
   {
      protected readonly IMoBiDimensionFactory _dimensionFactory;

      protected abstract bool ValidateDTOForInsert(ImportedQuantityDTO dto);

      protected abstract bool ValidateDTOForUpdate(ImportedQuantityDTO dto);

      protected abstract bool IsUpdate(ImportedQuantityDTO dto, TImportTarget importTarget);

      protected abstract bool IsNewInsert(ImportedQuantityDTO dto, TImportTarget importTarget);

      protected abstract ImportedQuantityDTO MapQuantityFromRow(DataTable table, DataRow row, int rowIndex);

      protected abstract void AddDTOToImportList(QuantityImporterDTO quantityImporterDTO, TImportTarget importTarget, ImportedQuantityDTO dto);

      protected AbstractDataTableProviderToQuantityDTOMapper(IMoBiDimensionFactory dimensionFactory)
      {
         _dimensionFactory = dimensionFactory;
      }

      /// <summary>
      ///    Gets a dimension unit string in a data row. Indicate which row contains the unit
      /// </summary>
      /// <param name="rowIndex">The index of the row being converted</param>
      /// <param name="columnIndex">The index of the column containing the unit</param>
      /// <param name="table">The table containing the row</param>
      /// <returns>A dimension corresponding to the unit given</returns>
      protected IDimension GetDimension(DataTable table, int rowIndex, int columnIndex)
      {
         var row = table.Rows[rowIndex];
         var dimension = _dimensionFactory.DimensionForUnit(row[columnIndex].ToString());
         if (dimension == null)
            throw new ImportQuantityDTOsFromDataTablesMapperException(row, rowIndex, AppConstants.Exceptions.CouldNotFindDimensionFromUnits(row[columnIndex].ToString()));
         return dimension;
      }

      /// <summary>
      ///    Gets the object path from a string in a datarow
      /// </summary>
      /// <param name="row">The row to convert</param>
      /// <param name="index">The index of the column containing the path</param>
      /// <returns>The an enumeration of strings that can be used to create an ObjectPath</returns>
      protected static IEnumerable<string> GetPath(DataRow row, int index)
      {
         return row[index].ToString().ToPathArray();
      }

      /// <summary>
      ///    Gets the quantity from a datarow
      /// </summary>
      /// <param name="table"></param>
      /// <param name="rowIndex">The row being converted</param>
      /// <param name="index">The index of the column containing the quantity</param>
      /// <returns>The quantity</returns>
      protected static double GetQuantity(DataTable table, int rowIndex, int index)
      {
         return GetDouble(table, rowIndex, index);
      }

      protected static double GetDouble(DataTable table, int rowIndex, int columnIndex)
      {
         var row = table.Rows[rowIndex];
         double quantity;
         if (!double.TryParse(row[columnIndex].ToString(), out quantity))
            throw new ImportQuantityDTOsFromDataTablesMapperException(row, rowIndex, AppConstants.Exceptions.ColumnNMustBeNumeric(row[columnIndex].ToString(), columnIndex + 1));
         return quantity;
      }

      protected static bool GetBool(DataRow row, int columnIndex)
      {
         var booleanValue = row[columnIndex].ToString().ToLower();
         return booleanValue.IsOneOf("true", "1");
      }

      /// <summary>
      ///    Gets the quantity name from a datarow
      /// </summary>
      /// <param name="row">The row being converted</param>
      /// <param name="index">The index of the column containing the quantity name</param>
      /// <returns>The name of the parameter as a string</returns>
      protected static string GetQuantityName(DataRow row, int index)
      {
         return row[index].ToString();
      }

      protected virtual void ValidateInContext(ImportedQuantityDTO dto, QuantityImporterDTO quantityImporterDTO, TImportTarget buildingBlock, DataRow row, int rowIndex)
      {
         if (IsUpdate(dto, buildingBlock) && !ValidateDTOForUpdate(dto))
            throw new ImportQuantityDTOsFromDataTablesMapperException(row, rowIndex, AppConstants.Validation.ValueNotValidForUpdate(dto.Path.ToString()));

         if (IsNewInsert(dto, buildingBlock) && !ValidateDTOForInsert(dto))
            throw new ImportQuantityDTOsFromDataTablesMapperException(row, rowIndex, AppConstants.Validation.ValueNotValidForInsert(dto.Path.ToString()));

         if (string.IsNullOrEmpty(dto.Name))
            throw new ImportQuantityDTOsFromDataTablesMapperException(row, rowIndex, AppConstants.Exceptions.ImportedStartValueMustHaveName);

         if (string.IsNullOrEmpty(dto.ContainerPath.PathAsString))
            throw new ImportQuantityDTOsFromDataTablesMapperException(row, rowIndex, AppConstants.Exceptions.ImportedStartValueMustHaveContainerPath);

         if (quantityImporterDTO.QuantitDTOs.Any(x => Equals(x.Path, dto.Path)))
            throw new ImportQuantityDTOsFromDataTablesMapperException(row, rowIndex, AppConstants.Exceptions.DuplicatedImportedStartValue(dto.Path.PathAsString));
      }

      public QuantityImporterDTO MapFrom(DataTable table, TImportTarget target)
      {
         var importerDTO = convertTableToQuantityDTOs(table, target);

         if (!importerDTO.QuantitDTOs.Any())
            importerDTO.AddToLog(AppConstants.Captions.NoValuesFound);

         return importerDTO;
      }

      private bool isRowEmpty(DataTable table, int rowIndex)
      {
         var row = table.Rows[rowIndex];
         return row.ItemArray.All(item => string.IsNullOrEmpty(item.ToString()));
      }

      private QuantityImporterDTO convertTableToQuantityDTOs(DataTable table, TImportTarget buildingBlock)
      {
         var importerDTO = new QuantityImporterDTO();
         importerDTO.AddToLog(AppConstants.Captions.ConvertingExcelSheetToQuantities(table.TableName));
         var rowIndex = 0;

         foreach (DataRow row in table.Rows)
         {
            if (isRowEmpty(table, rowIndex))
               continue;
            try
            {
               var dto = MapQuantityFromRow(table, table.Rows[rowIndex], rowIndex);

               ValidateInContext(dto, importerDTO, buildingBlock, row, rowIndex);

               AddDTOToImportList(importerDTO, buildingBlock, dto);
            }
            catch (ImportQuantityDTOsFromDataTablesMapperException e)
            {
               importerDTO.AddToLog(e.Message);
               importerDTO.QuantitDTOs.Clear();
               break;
            }
            catch (Exception e)
            {
               importerDTO.AddToLog($"{AppConstants.Exceptions.FrameworkExceptionOccurred}, {e.Message}, {row.ToNiceString()}");
               importerDTO.QuantitDTOs.Clear();
               break;
            }
            rowIndex ++;
         }

         return importerDTO;
      }
   }
}