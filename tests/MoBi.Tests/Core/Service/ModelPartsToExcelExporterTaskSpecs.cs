using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Mappers;
using MoBi.Core.Services;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Diagram;

namespace MoBi.Core.Service
{
   public abstract class concern_for_ModelPartsToExcelExporterTask : ContextSpecification<ModelPartsToExcelExporterTask>
   {
      protected IReactionBuildingBlockToReactionDataTableMapper _reactionBuildingBlockToReactionDataTableMapper;
      protected IParameterListToSimulationParameterDataTableMapper _parameterListToSimulationDataTableMapper;
      protected IMoleculeStartValuesBuildingBlockToParameterDataTableMapper _moleculeStartValuesBuildingBlockToParameterDataTableMapper;
      protected override void Context()
      {
         _reactionBuildingBlockToReactionDataTableMapper = A.Fake<IReactionBuildingBlockToReactionDataTableMapper>();
         _parameterListToSimulationDataTableMapper = A.Fake<IParameterListToSimulationParameterDataTableMapper>();
         _moleculeStartValuesBuildingBlockToParameterDataTableMapper = A.Fake<IMoleculeStartValuesBuildingBlockToParameterDataTableMapper>();
         sut = new ModelPartsToExcelExporterTask(
            _reactionBuildingBlockToReactionDataTableMapper,
            _parameterListToSimulationDataTableMapper,
            _moleculeStartValuesBuildingBlockToParameterDataTableMapper
            );

         A.CallTo(() => _reactionBuildingBlockToReactionDataTableMapper.MapFrom(A<IEnumerable<MoBiReactionBuildingBlock>>.Ignored)).Returns(new DataTable { TableName = "reactions" });
         A.CallTo(() => _parameterListToSimulationDataTableMapper.MapFrom(A<IReadOnlyList<IParameter>>.Ignored)).Returns(new DataTable { TableName = "parameters" });
         A.CallTo(() => _moleculeStartValuesBuildingBlockToParameterDataTableMapper.MapFrom(A<IEnumerable<MoleculeStartValue>>.Ignored, A<IEnumerable<MoleculeBuilder>>.Ignored)).Returns(new DataTable { TableName = "molecules" });
      }
   }

   public class When_mapping_model_parts : concern_for_ModelPartsToExcelExporterTask
   {
      private MoBiReactionBuildingBlock _moBiReactionBuildingBlock;
      private MoleculeStartValuesBuildingBlock _moleculeStartValuesBuildingBlock;
      private IMoBiSimulation _moBiSimulation;
      private IReadOnlyList<IParameter> _parameterList;

      protected override void Context()
      {
         base.Context();
         _moBiReactionBuildingBlock = new MoBiReactionBuildingBlock();
         _moleculeStartValuesBuildingBlock = new MoleculeStartValuesBuildingBlock();
         _moBiSimulation = new MoBiSimulation();

         _moBiSimulation.Configuration = new SimulationConfiguration();
         _moBiSimulation.Model = new Model();
         _moBiSimulation.Model.Root = new Container();

         var moduleConfiguration = new ModuleConfiguration(new Module
         {
            Reactions = _moBiReactionBuildingBlock
         });
         moduleConfiguration.Module.AddMoleculeStartValueBlock(_moleculeStartValuesBuildingBlock);
         _moBiSimulation.Configuration.AddModuleConfiguration(moduleConfiguration);
         moduleConfiguration.SelectedMoleculeStartValues = _moleculeStartValuesBuildingBlock;
         _parameterList = _moBiSimulation.Model.Root.GetAllChildren<IParameter>();
      }

      protected override void Because()
      {
         sut.ExportModelPartsToExcelFile("filename.xlsx", _moBiSimulation, false);
      }

      [Observation]
      public void calls_reaction_building_block_mapper()
      {
         A.CallTo(() => _reactionBuildingBlockToReactionDataTableMapper.MapFrom(A<IEnumerable<ReactionBuildingBlock>>.That.Matches(x => x.Contains(_moBiReactionBuildingBlock)))).MustHaveHappened();
      }

      [Observation]
      public void calls_simulation_to_parameter_mapper()
      {
         A.CallTo(() => _parameterListToSimulationDataTableMapper.MapFrom(A<IReadOnlyList<IParameter>>.That.Matches(x => x.ContainsAll(_parameterList)))).MustHaveHappened();
      }

      [Observation]
      public void calls_molecule_building_block_mapper()
      {
         A.CallTo(() => _moleculeStartValuesBuildingBlockToParameterDataTableMapper.MapFrom(A<IEnumerable<MoleculeStartValue>>._, A<IEnumerable<MoleculeBuilder>>._)).MustHaveHappened();
      }
   }
}
