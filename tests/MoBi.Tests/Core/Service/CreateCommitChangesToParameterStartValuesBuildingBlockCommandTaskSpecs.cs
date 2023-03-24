using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Service
{
   public abstract class concern_for_CreateCommitChangesToParameterStartValuesBuildingBlockCommandTaskSpecs : ContextSpecification<CreateCommitChangesToParameterStartValuesBuildingBlockCommandTask>
   {
      protected ICloneManagerForBuildingBlock _cloneManager;
      protected IObjectPathFactory _objectPathFactory;

      protected override void Context()
      {
         _cloneManager = A.Fake<ICloneManagerForBuildingBlock>();
         _objectPathFactory = new ObjectPathFactory(new AliasCreator());
         sut = new CreateCommitChangesToParameterStartValuesBuildingBlockCommandTask(_cloneManager);
      }
   }

   internal class When_told_to_create_CommitChangesToParameterStartValuesCommand : concern_for_CreateCommitChangesToParameterStartValuesBuildingBlockCommandTaskSpecs
   {
      private IMoBiSimulation _simulation;
      private ParameterStartValuesBuildingBlock _buildingBlock;
      private IMoBiCommand _resultCommand;
      private SimulationConfiguration _buildConfiguration;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new ParameterStartValuesBuildingBlock();
         _simulation = A.Fake<IMoBiSimulation>();
         _buildConfiguration = new SimulationConfiguration() { Module = new Module() };
         _buildConfiguration.Module.AddParameterStartValueBlock(A.Fake<ParameterStartValuesBuildingBlock>());
         A.CallTo(() => _simulation.Configuration).Returns(_buildConfiguration);
      }

      protected override void Because()
      {
         _resultCommand = sut.CreateCommitToBuildingBlockCommand(_simulation, _buildingBlock);
      }

      [Observation]
      public void should_return_right_configured_ChangeParameterStartValueValuePropertyCommand()
      {
         _resultCommand.IsEmpty().ShouldBeFalse();
      }

      [Observation]
      public void should_clone_the_simulation_building_block()
      {
         A.CallTo(() => _cloneManager.CloneBuildingBlock(_buildConfiguration.ParameterStartValues)).MustHaveHappened();
      }
   }
}