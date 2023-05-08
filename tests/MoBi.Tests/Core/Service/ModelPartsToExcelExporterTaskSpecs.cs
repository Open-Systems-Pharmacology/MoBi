using System.Collections.Generic;
using System.Data;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Mappers;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;

namespace MoBi.Core.Service
{
   public abstract class concern_for_ModelPartsToExcelExporterTask : ContextSpecification<ModelPartsToExcelExporterTask>
   {
      protected IReactionBuildingBlockToReactionDataTableMapper _reactionBuildingBlockToReactionDataTableMapper;
      protected IParameterListToSimulationParameterDataTableMapper _parameterListToSimulationDataTableMapper;
      protected IInitialConditionsBuildingBlockToParameterDataTableMapper _initialConditionsBuildingBlockToParameterDataTableMapper;

      protected override void Context()
      {
         _reactionBuildingBlockToReactionDataTableMapper = A.Fake<IReactionBuildingBlockToReactionDataTableMapper>();
         _parameterListToSimulationDataTableMapper = A.Fake<IParameterListToSimulationParameterDataTableMapper>();
         _initialConditionsBuildingBlockToParameterDataTableMapper = A.Fake<IInitialConditionsBuildingBlockToParameterDataTableMapper>();
         sut = new ModelPartsToExcelExporterTask(
            _reactionBuildingBlockToReactionDataTableMapper,
            _parameterListToSimulationDataTableMapper,
            _initialConditionsBuildingBlockToParameterDataTableMapper
         );

         A.CallTo(() => _reactionBuildingBlockToReactionDataTableMapper.MapFrom(A<IEnumerable<MoBiReactionBuildingBlock>>.Ignored)).Returns(new DataTable {TableName = "reactions"});
         A.CallTo(() => _parameterListToSimulationDataTableMapper.MapFrom(A<IReadOnlyList<IParameter>>.Ignored)).Returns(new DataTable {TableName = "parameters"});
         A.CallTo(() => _initialConditionsBuildingBlockToParameterDataTableMapper.MapFrom(A<IEnumerable<InitialCondition>>.Ignored, A<IEnumerable<MoleculeBuilder>>.Ignored)).Returns(new DataTable {TableName = "molecules"});
      }
   }

   public class When_mapping_model_parts : concern_for_ModelPartsToExcelExporterTask
   {
      private MoBiReactionBuildingBlock _moBiReactionBuildingBlock;
      private InitialConditionsBuildingBlock _initialConditionsBuildingBlock;
      private IMoBiSimulation _moBiSimulation;
      private IReadOnlyList<IParameter> _parameterList;

      protected override void Context()
      {
         base.Context();
         _moBiReactionBuildingBlock = new MoBiReactionBuildingBlock();
         _initialConditionsBuildingBlock = new InitialConditionsBuildingBlock();
         _moBiSimulation = new MoBiSimulation();

         _moBiSimulation.Configuration = new SimulationConfiguration();
         _moBiSimulation.Model = new Model();
         _moBiSimulation.Model.Root = new Container();

         var moduleConfiguration = new ModuleConfiguration(new Module
         {
            _moBiReactionBuildingBlock
         });
         moduleConfiguration.Module.Add(_initialConditionsBuildingBlock);
         _moBiSimulation.Configuration.AddModuleConfiguration(moduleConfiguration);
         moduleConfiguration.SelectedInitialConditions = _initialConditionsBuildingBlock;
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
         A.CallTo(() => _initialConditionsBuildingBlockToParameterDataTableMapper.MapFrom(A<IEnumerable<InitialCondition>>._, A<IEnumerable<MoleculeBuilder>>._)).MustHaveHappened();
      }
   }
}