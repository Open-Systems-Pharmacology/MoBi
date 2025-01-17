using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   internal class concern_for_RemovePathAndValueEntityToBuildingBlockInSimulationCommand : ContextSpecification<RemovePathAndValueEntityFromBuildingBlockInSimulationCommand<ParameterValue>>
   {
      protected PathAndValueEntityBuildingBlock<ParameterValue> _buildingBlock;
      protected ParameterValue _parameterValue;
      private Module _module;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _parameterValue = new ParameterValue();
         _buildingBlock = new ParameterValuesBuildingBlock();
         _module = new Module
         {
            _buildingBlock
         };
         _buildingBlock.Add(_parameterValue);
         _context = A.Fake<IMoBiContext>();

         _module.IsPKSimModule = true;

         sut = new RemovePathAndValueEntityFromBuildingBlockInSimulationCommand<ParameterValue>(_parameterValue.Path, _buildingBlock);
      }
   }

   internal class When_removing_the_parameter : concern_for_RemovePathAndValueEntityToBuildingBlockInSimulationCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_parameter_is_removed_from_the_building_block()
      {
         _buildingBlock.ShouldNotContain(_parameterValue);
      }
   }

   internal class When_reversing_removing_the_parameter : concern_for_RemovePathAndValueEntityToBuildingBlockInSimulationCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.Get<PathAndValueEntityBuildingBlock<ParameterValue>>(_buildingBlock.Id)).Returns(_buildingBlock);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_parameter_is_added_to_the_building_block()
      {
         _buildingBlock.ShouldContain(_parameterValue);
      }
   }
}