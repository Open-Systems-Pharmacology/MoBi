using System.Data;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Mappers
{
   public interface IDataTableToImportQuantityDTOMapperForSimulations
   {
      /// <summary>
      ///    Converts a specifically formatted data table into quantity DTO. The conversion is also validated against an object
      ///    which is the target of import
      /// </summary>
      /// <param name="table">The table being converted</param>
      /// <param name="simulation">The target of this import. This parameter supplies some context for the import algorithm</param>
      /// <returns>The DTO containing all the values that have validated and are ready to be inserted or updated</returns>
      QuantityImporterDTO MapFrom(DataTable table, IMoBiSimulation simulation);
   }

   public class DataTableToImportQuantityDTOMapperForSimulations : DataTableToImportParameterQuantityDTOMapper<IMoBiSimulation>, IDataTableToImportQuantityDTOMapperForSimulations
   {
      public DataTableToImportQuantityDTOMapperForSimulations(IMoBiDimensionFactory dimensionFactory) : base(dimensionFactory)
      {
      }

      protected override bool ValidateDTOForInsert(ImportedQuantityDTO dto)
      {
         // Cannot insert new values into simulations
         return false;
      }

      protected override bool ValidateDTOForUpdate(ImportedQuantityDTO dto)
      {
         return dto.IsQuantitySpecified;
      }

      protected override bool IsUpdate(ImportedQuantityDTO dto, IMoBiSimulation simulation)
      {
         return !IsNewInsert(dto, simulation);
      }

      protected override bool IsNewInsert(ImportedQuantityDTO dto, IMoBiSimulation simulation)
      {
         return dto.Path.TryResolve<IParameter>(simulation.Model.Root) == null;
      }

      protected override void ValidateInContext(ImportedQuantityDTO dto, QuantityImporterDTO quantityImporterDTO, IMoBiSimulation simulation, DataRow row, int rowIndex)
      {
         // Skipping base call because the default validation is to stop import when first error occurs
         if (IsUpdate(dto, simulation))
         {
            if (!ValidateDTOForUpdate(dto))
               throw new ImportQuantityDTOsFromDataTablesMapperException(row, rowIndex, AppConstants.Validation.ValueNotValidForUpdate(dto.Path.ToString()));

            ValidateDtoAgainstBuilderForUpdate(dto.Path.TryResolve<IParameter>(simulation.Model.Root), dto, row, rowIndex);
         }

         if (!IsNewInsert(dto, simulation)) return;

         // +2 because there is a mandatory header row (+1) and this index is 0-based, while excel rows start at 1 (+1)
         dto.SetWarning(row, rowIndex + 2, AppConstants.Warnings.CannotAddNewParameterFromImportToSimulation(simulation.Name, dto.Path.ToString()));
         dto.SkipImport = true;
      }

      protected override string LogMessageFor(ImportedQuantityDTO dto, IMoBiSimulation simulation)
      {
         return IsNewInsert(dto, simulation)
            ? AppConstants.Captions.AddingParameterValueToSimulation(dto.Path, dto.ConvertToDisplayUnit(dto.QuantityInBaseUnit), dto.DisplayUnit)
            : AppConstants.Captions.UpdatingParameterValueInSimulation(dto.Path, dto.ConvertToDisplayUnit(dto.QuantityInBaseUnit), dto.DisplayUnit);
      }
   }
}