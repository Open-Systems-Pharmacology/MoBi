using System.Collections.Generic;
using System.Data;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure.Export;

namespace MoBi.Core.Services
{
   public interface IModelPartsToExcelExporterTask
   {
      /// <summary>
      ///    Export parts of a simulation model to an Excel file
      /// </summary>
      /// <param name="excelFileName">The name of the excel file</param>
      /// <param name="simulation">The simulation that is being exported</param>
      /// <param name="openExcel">true if an an attempt to open excel should be made after export</param>
      void ExportModelPartsToExcelFile(string excelFileName, IMoBiSimulation simulation, bool openExcel);
   }

   public class ModelPartsToExcelExporterTask : IModelPartsToExcelExporterTask
   {
      private readonly IReactionBuildingBlockToReactionDataTableMapper _reactionBuildingBlockToReactionDataTableMapper;
      private readonly IParameterListToSimulationParameterDataTableMapper _parameterListToSimulationParameterDataTableMapper;
      private readonly IMoleculeStartValuesBuildingBlockToParameterDataTableMapper _moleculeStartValuesBuildingBlockToParameterDataTableMapper;

      public ModelPartsToExcelExporterTask(IReactionBuildingBlockToReactionDataTableMapper reactionBuildingBlockToReactionDataTableMapper,
         IParameterListToSimulationParameterDataTableMapper parameterListToSimulationParameterDataTableMapper,
         IMoleculeStartValuesBuildingBlockToParameterDataTableMapper moleculeStartValuesBuildingBlockToParameterDataTableMapper)
      {
         _reactionBuildingBlockToReactionDataTableMapper = reactionBuildingBlockToReactionDataTableMapper;
         _parameterListToSimulationParameterDataTableMapper = parameterListToSimulationParameterDataTableMapper;
         _moleculeStartValuesBuildingBlockToParameterDataTableMapper = moleculeStartValuesBuildingBlockToParameterDataTableMapper;
      }

      public void ExportModelPartsToExcelFile(string excelFileName, IMoBiSimulation simulation, bool openExcel)
      {
         var reactionDataTable = _reactionBuildingBlockToReactionDataTableMapper.MapFrom(simulation.Configuration.All<ReactionBuildingBlock>());
         var simulationParameterDataTable = _parameterListToSimulationParameterDataTableMapper.MapFrom(simulation.Model.Root.GetAllChildren<IParameter>());

         var moleculeParameterDataTable = _moleculeStartValuesBuildingBlockToParameterDataTableMapper.MapFrom(simulation.Configuration.All<MoleculeStartValuesBuildingBlock>().SelectMany(x => x).Where(msv => msv.IsPresent), 
            simulation.Configuration.All<MoleculeBuildingBlock>().SelectMany(x => x));

         var dataTables = new List<DataTable> {reactionDataTable, simulationParameterDataTable, moleculeParameterDataTable};
         ExportToExcelTask.ExportDataTablesToExcel(dataTables, excelFileName, openExcel: openExcel);
      }
   }
}