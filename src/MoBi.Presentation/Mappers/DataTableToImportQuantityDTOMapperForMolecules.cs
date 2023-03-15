using System.Data;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Mappers
{
   public interface IDataTableToImportQuantityDTOMapperForMolecules
   {
      QuantityImporterDTO MapFrom(DataTable table, IStartValuesBuildingBlock<MoleculeStartValue> buildingBlock);
   }

   public class DataTableToImportQuantityDTOMapperForMolecules : AbstractDataTableProviderToQuantityDTOMapper<IStartValuesBuildingBlock<MoleculeStartValue>>, IDataTableToImportQuantityDTOMapperForMolecules
   {
      private readonly IReactionDimensionRetriever _reactionDimensionRetriever;

      private static class DataTableRowIndexes
      {
         public const int PATH = 0;
         public const int MOLECULE = 1;
         public const int IS_PRESENT = 2;
         public const int VALUE = 3;
         public const int UNIT = 4;
         public const int SCALE_DIVISOR = 5;
         public const int NEGATIVE_VALUES_ALLOWED = 6;
         //optional dimension column to use
         public const int DIMENSION = 7;
         public const int COLUMNS = 7;
      }

      public DataTableToImportQuantityDTOMapperForMolecules(IMoBiDimensionFactory dimensionFactory, IReactionDimensionRetriever reactionDimensionRetriever) : base(dimensionFactory)
      {
         _reactionDimensionRetriever = reactionDimensionRetriever;
      }

      protected override bool ValidateDTOForUpdate(ImportedQuantityDTO dto)
      {
         return dto.IsQuantitySpecified || dto.IsScaleDivisorSpecified;
      }

      protected override bool IsUpdate(ImportedQuantityDTO dto, IStartValuesBuildingBlock<MoleculeStartValue> importTarget)
      {
         return !IsNewInsert(dto, importTarget);
      }

      protected override bool IsNewInsert(ImportedQuantityDTO dto, IStartValuesBuildingBlock<MoleculeStartValue> importTarget)
      {
         return importTarget[dto.Path] == null;
      }

      protected override bool ValidateDTOForInsert(ImportedQuantityDTO dto)
      {
         return !double.IsNaN(dto.QuantityInBaseUnit) && dto.IsQuantitySpecified;
      }

      protected override void ValidateInContext(ImportedQuantityDTO dto, QuantityImporterDTO quantityImporterDTO, IStartValuesBuildingBlock<MoleculeStartValue> buildingBlock, DataRow row, int rowIndex)
      {
         base.ValidateInContext(dto, quantityImporterDTO, buildingBlock, row, rowIndex);
         if (isDimensionRequired(dto) && !isCorrectDimensionForDimensionMode(_reactionDimensionRetriever.SelectedDimensionMode, dto))
         {
            throw new ImportQuantityDTOsFromDataTablesMapperException(
               row, rowIndex,
               AppConstants.Exceptions.ImportedDimensionNotRecognized(getDimensionConstantForProject(_reactionDimensionRetriever.SelectedDimensionMode),
                  _dimensionFactory.Dimension(getDimensionConstantForProject(_reactionDimensionRetriever.SelectedDimensionMode)).GetUnitNames()));
         }
      }

      protected override void AddDTOToImportList(QuantityImporterDTO quantityImporterDTO, IStartValuesBuildingBlock<MoleculeStartValue> importTarget, ImportedQuantityDTO dto)
      {
         quantityImporterDTO.QuantityDTOs.Add(dto);
         if (dto.HasWarning())
            quantityImporterDTO.AddToLog(dto.GetWarning());

         quantityImporterDTO.AddToLog(IsNewInsert(dto, importTarget)
            ? AppConstants.Captions.AddingMoleculeStartValue(dto.Path, dto.ConvertToDisplayUnit(dto.QuantityInBaseUnit), dto.DisplayUnit, dto.IsPresent, dto.Name, dto.NegativeValuesAllowed)
            : AppConstants.Captions.UpdatingMoleculeStartValue(dto.Path, dto.ConvertToDisplayUnit(dto.QuantityInBaseUnit), dto.DisplayUnit, dto.IsPresent, dto.Name, dto.NegativeValuesAllowed));
      }

      private bool isDimensionRequired(ImportedQuantityDTO dto)
      {
         return dto.IsQuantitySpecified;
      }

      private static string getDimensionConstantForProject(ReactionDimensionMode dimensionMode)
      {
         return dimensionMode == ReactionDimensionMode.AmountBased ? Constants.Dimension.MOLAR_AMOUNT : Constants.Dimension.MOLAR_CONCENTRATION;
      }

      private bool isCorrectDimensionForDimensionMode(ReactionDimensionMode dimensionMode, ImportedQuantityDTO dto)
      {
         if (dto.Dimension == _dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION) && dimensionMode == ReactionDimensionMode.ConcentrationBased)
            return true;

         if (dto.Dimension == _dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT) && dimensionMode == ReactionDimensionMode.AmountBased)
            return true;

         return false;
      }

      protected override ImportedQuantityDTO MapQuantityFromRow(DataTable table, DataRow row, int rowIndex)
      {
         if (row.ItemArray.Count() < DataTableRowIndexes.COLUMNS)
            throw new ImportQuantityDTOsFromDataTablesMapperException(row, rowIndex, AppConstants.Exceptions.TableShouldBeNColumns(DataTableRowIndexes.COLUMNS));

         var path = GetPath(row, DataTableRowIndexes.PATH);

         var moleculeName = GetQuantityName(row, DataTableRowIndexes.MOLECULE);

         var msv = new ImportedQuantityDTO
         {
            ContainerPath = new ObjectPath(path),
            Name = moleculeName,
            IsPresent = getIsPresent(row),
            IsScaleDivisorSpecified = getIsValueSpecified(row, DataTableRowIndexes.SCALE_DIVISOR),
            IsQuantitySpecified = getIsValueSpecified(row, DataTableRowIndexes.VALUE),
            NegativeValuesAllowed = getNegativeValuesAllowed(row)
         };
         msv.ScaleDivisor = msv.IsScaleDivisorSpecified ? getScaleFactor(table, rowIndex) : double.NaN;

         if (!msv.IsQuantitySpecified) return msv;

         var dimension = GetDimension(table, rowIndex, DataTableRowIndexes.UNIT, DataTableRowIndexes.DIMENSION);
         msv.Dimension = dimension;
         msv.DisplayUnit = dimension.Unit(row[DataTableRowIndexes.UNIT].ToString());

         msv.QuantityInBaseUnit = msv.IsQuantitySpecified ? msv.ConvertToBaseUnit(GetQuantity(table, rowIndex, DataTableRowIndexes.VALUE)) : double.NaN;

         return msv;
      }

      private double getScaleFactor(DataTable table, int rowIndex)
      {
         return GetDouble(table, rowIndex, DataTableRowIndexes.SCALE_DIVISOR);
      }

      private bool getNegativeValuesAllowed(DataRow row)
      {
         return GetBool(row, DataTableRowIndexes.NEGATIVE_VALUES_ALLOWED);
      }

      private bool getIsValueSpecified(DataRow row, int index)
      {
         return !string.IsNullOrEmpty(row[index].ToString());
      }

      private bool getIsPresent(DataRow row)
      {
         return GetBool(row, DataTableRowIndexes.IS_PRESENT);
      }
   }
}