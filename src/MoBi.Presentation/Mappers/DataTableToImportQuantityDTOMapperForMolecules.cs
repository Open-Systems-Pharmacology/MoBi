﻿using System.Data;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using static MoBi.Core.Mappers.ColumnIndexes;

namespace MoBi.Presentation.Mappers
{
   public interface IDataTableToImportQuantityDTOMapperForMolecules
   {
      QuantityImporterDTO MapFrom(DataTable table, PathAndValueEntityBuildingBlock<InitialCondition> buildingBlock);
   }

   public class DataTableToImportQuantityDTOMapperForMolecules : AbstractDataTableProviderToQuantityDTOMapper<PathAndValueEntityBuildingBlock<InitialCondition>>, IDataTableToImportQuantityDTOMapperForMolecules
   {
      private readonly IReactionDimensionRetriever _reactionDimensionRetriever;

      public DataTableToImportQuantityDTOMapperForMolecules(IMoBiDimensionFactory dimensionFactory, IReactionDimensionRetriever reactionDimensionRetriever) : base(dimensionFactory)
      {
         _reactionDimensionRetriever = reactionDimensionRetriever;
      }

      protected override bool ValidateDTOForUpdate(ImportedQuantityDTO dto)
      {
         return dto.IsQuantitySpecified || dto.IsScaleDivisorSpecified;
      }

      protected override bool IsUpdate(ImportedQuantityDTO dto, PathAndValueEntityBuildingBlock<InitialCondition> importTarget)
      {
         return !IsNewInsert(dto, importTarget);
      }

      protected override bool IsNewInsert(ImportedQuantityDTO dto, PathAndValueEntityBuildingBlock<InitialCondition> importTarget)
      {
         return importTarget[dto.Path] == null;
      }

      protected override bool ValidateDTOForInsert(ImportedQuantityDTO dto)
      {
         return !double.IsNaN(dto.QuantityInBaseUnit) && dto.IsQuantitySpecified;
      }

      protected override void ValidateInContext(ImportedQuantityDTO dto, QuantityImporterDTO quantityImporterDTO, PathAndValueEntityBuildingBlock<InitialCondition> buildingBlock, DataRow row, int rowIndex)
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

      protected override void AddDTOToImportList(QuantityImporterDTO quantityImporterDTO, PathAndValueEntityBuildingBlock<InitialCondition> importTarget, ImportedQuantityDTO dto)
      {
         quantityImporterDTO.QuantityDTOs.Add(dto);
         if (dto.HasWarning())
            quantityImporterDTO.AddToLog(dto.GetWarning());

         quantityImporterDTO.AddToLog(IsNewInsert(dto, importTarget)
            ? AppConstants.Captions.AddingInitialCondition(dto.Path, dto.ConvertToDisplayUnit(dto.QuantityInBaseUnit), dto.DisplayUnit, dto.IsPresent, dto.Name, dto.NegativeValuesAllowed)
            : AppConstants.Captions.UpdatingInitialCondition(dto.Path, dto.ConvertToDisplayUnit(dto.QuantityInBaseUnit), dto.DisplayUnit, dto.IsPresent, dto.Name, dto.NegativeValuesAllowed));
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
         if (row.ItemArray.Count() < InitialConditionsRowIndexes.COLUMNS)
            throw new ImportQuantityDTOsFromDataTablesMapperException(row, rowIndex, AppConstants.Exceptions.TableShouldBeNColumns(InitialConditionsRowIndexes.COLUMNS));

         var path = GetPath(row, InitialConditionsRowIndexes.PATH);

         var moleculeName = GetQuantityName(row, InitialConditionsRowIndexes.MOLECULE);

         var msv = new ImportedQuantityDTO
         {
            ContainerPath = new ObjectPath(path),
            Name = moleculeName,
            IsPresent = getIsPresent(row),
            IsScaleDivisorSpecified = getIsValueSpecified(row, InitialConditionsRowIndexes.SCALE_DIVISOR),
            IsQuantitySpecified = getIsValueSpecified(row, InitialConditionsRowIndexes.VALUE),
            NegativeValuesAllowed = getNegativeValuesAllowed(row)
         };
         msv.ScaleDivisor = msv.IsScaleDivisorSpecified ? getScaleFactor(table, rowIndex) : double.NaN;

         if (!msv.IsQuantitySpecified) return msv;

         var dimension = GetDimension(table, rowIndex, InitialConditionsRowIndexes.UNIT, InitialConditionsRowIndexes.DIMENSION);
         msv.Dimension = dimension;
         msv.DisplayUnit = dimension.Unit(row[InitialConditionsRowIndexes.UNIT].ToString());

         msv.QuantityInBaseUnit = msv.IsQuantitySpecified ? msv.ConvertToBaseUnit(GetQuantity(table, rowIndex, InitialConditionsRowIndexes.VALUE)) : double.NaN;

         return msv;
      }

      private double getScaleFactor(DataTable table, int rowIndex)
      {
         return GetDouble(table, rowIndex, InitialConditionsRowIndexes.SCALE_DIVISOR);
      }

      private bool getNegativeValuesAllowed(DataRow row)
      {
         return GetBool(row, InitialConditionsRowIndexes.NEGATIVE_VALUES_ALLOWED);
      }

      private bool getIsValueSpecified(DataRow row, int index)
      {
         return !string.IsNullOrEmpty(row[index].ToString());
      }

      private bool getIsPresent(DataRow row)
      {
         return GetBool(row, InitialConditionsRowIndexes.IS_PRESENT);
      }
   }
}