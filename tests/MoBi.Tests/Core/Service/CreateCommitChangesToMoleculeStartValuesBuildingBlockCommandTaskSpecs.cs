using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Service
{
   public abstract class concern_for_CreateCommitChangesToMoleculeStartValuesBuildingBlockCommandTask : ContextSpecification<CreateCommitChangesToMoleculeStartValuesBuildingBlockCommandTask>
   {
      protected ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;

      protected override void Context()
      {
         _cloneManagerForBuildingBlock = A.Fake<ICloneManagerForBuildingBlock>();
         sut = new CreateCommitChangesToMoleculeStartValuesBuildingBlockCommandTask(_cloneManagerForBuildingBlock);
      }
   }

   public class When_commiting_the_changes_in_MSV_from_the_simulation_to_the_building_block : concern_for_CreateCommitChangesToMoleculeStartValuesBuildingBlockCommandTask
   {
      private IBuildingBlock _templateBuildingBlock;
      private IMoBiCommand _resultCommand;
      private IMoBiSimulation _simulation;
      private IMoleculeStartValuesBuildingBlock _cloneSimulationStartValueBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         _templateBuildingBlock = new MoleculeStartValuesBuildingBlock();
         _cloneSimulationStartValueBuildingBlock = new MoleculeStartValuesBuildingBlock();
         IMoleculeStartValuesBuildingBlock simulationStartValueBuildingBlock = new MoleculeStartValuesBuildingBlock();
         A.CallTo(() => _cloneManagerForBuildingBlock.Clone(simulationStartValueBuildingBlock)).Returns(_cloneSimulationStartValueBuildingBlock);
         A.CallTo(() => _simulation.BuildConfiguration.MoleculeStartValues).Returns(simulationStartValueBuildingBlock);
      }

      protected override void Because()
      {
         _resultCommand = sut.CreateCommitToBuildingBlockCommand(_simulation, _templateBuildingBlock);
      }

      [Observation]
      public void should_return_a_right_command_to_upate_moleule_start_values()
      {
         _resultCommand.IsEmpty().ShouldBeFalse();
      }
   }
}