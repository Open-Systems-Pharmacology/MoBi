using System.Collections.Generic;
using System.Data;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Mappers;
using OSPSuite.Core.Domain;
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
         var reactionDataTable = _reactionBuildingBlockToReactionDataTableMapper.MapFrom(simulation.MoBiBuildConfiguration.MoBiReactions);
         var simulationParameterDataTable = _parameterListToSimulationParameterDataTableMapper.MapFrom(simulation.Model.Root.GetAllChildren<IParameter>());

         var moleculeParameterDataTable = _moleculeStartValuesBuildingBlockToParameterDataTableMapper.MapFrom(simulation.MoBiBuildConfiguration.MoleculeStartValues.Where(msv => msv.IsPresent), simulation.MoBiBuildConfiguration.Molecules);

         var dataTables = new List<DataTable> {reactionDataTable, simulationParameterDataTable, moleculeParameterDataTable};
         //TODO: have to fix the export here
         /*
         ExportToExcelTask.ExportDataTablesToExcel(dataTables, excelFileName, openExcel: openExcel, workbookConfiguration: (wb, dt) =>
         {
            wb.setSelection(0, 0, 0, dt.Columns.Count);
            var rangeStyle = wb.getRangeStyle();
            rangeStyle.FontBold = true;
            wb.setRangeStyle(rangeStyle);
         });*/
      }
   }
}