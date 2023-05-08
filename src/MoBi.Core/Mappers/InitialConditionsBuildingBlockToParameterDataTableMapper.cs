using System;
using System.Collections.Generic;
using System.Data;
using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;

namespace MoBi.Core.Mappers
{
   public interface IInitialConditionsBuildingBlockToParameterDataTableMapper
   {
      /// <summary>
      /// Maps all initial conditions in a building block to a data table.
      /// </summary>
      /// <param name="initialConditions">The initial conditions being mapped</param>
      /// <param name="moleculeBuilders">The original builder building block. This may be used to look up information not contained in the start value, but needed for the data table</param>
      /// <returns>The data table</returns>
      DataTable MapFrom(IEnumerable<InitialCondition> initialConditions, IEnumerable<MoleculeBuilder> moleculeBuilders);
   }

   public class InitialConditionsBuildingBlockToParameterDataTableMapper : IInitialConditionsBuildingBlockToParameterDataTableMapper
   {
      public DataTable MapFrom(IEnumerable<InitialCondition> initialConditions, IEnumerable<MoleculeBuilder> moleculeBuilders)
      {
         var dt = generateEmptyMoleculeParameterDataTable();
         initialConditions.Each(initialCondition => initialConditionToParametersDataTable(initialCondition, dt, moleculeBuilders.FindDescriptionForInitialConditionFromBuilder(initialCondition.Name)));
         return dt;
      }

      private static readonly string _name = AppConstants.Captions.Name;
      private static readonly string _path = AppConstants.Captions.Path;
      private static readonly string _initialValue = AppConstants.Captions.InitialValue;
      private static readonly string _formula = AppConstants.Captions.Formula;
      private static readonly string _unit = AppConstants.Captions.Unit;
      private static readonly string _scaleDivisor = AppConstants.Captions.ScaleDivisor;
      private const string _description = AppConstants.Captions.Description;

      private void initialConditionToParametersDataTable(InitialCondition initialCondition, DataTable dt, string description)
      {
         var row = dt.Rows.Add();
         row[_name] = initialCondition.Name;
         row[_path] = initialCondition.ContainerPath;
         row[_initialValue] = initialCondition.Value != null ? (object) initialCondition.ConvertToDisplayUnit(initialCondition.Value) : DBNull.Value; 
         row[_formula] = initialCondition.Formula ?? (object) String.Empty; 
         row[_unit] = initialCondition.DisplayUnit;
         row[_scaleDivisor] = initialCondition.ScaleDivisor;
         row[_description] = initialCondition.Description.IsNullOrEmpty() ? description : initialCondition.Description;
      }

      private DataTable generateEmptyMoleculeParameterDataTable()
      {
         var dt = new DataTable();
         dt.AddColumn<string>(_name);

         dt.AddColumn(_path);
         dt.AddColumn<double>(_initialValue);
         dt.AddColumn(_formula);
         dt.AddColumn(_unit);
         dt.AddColumn<double>(_scaleDivisor);
         dt.AddColumn(_description);
         dt.TableName = AppConstants.Captions.InitialConditions;
         return dt;
      }
   }
}