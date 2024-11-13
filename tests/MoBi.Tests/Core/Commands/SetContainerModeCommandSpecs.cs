using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_SetContainerModeCommand : ContextSpecification<SetContainerModeCommand>
   {
      private IBuildingBlock _buildingBlock;
      protected IContainer _container;
      protected ContainerMode _newContainerMode;
      protected ContainerMode _oldContainerMode;
      protected IMoBiContext _context;
      protected IParameter _volumeParameter;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _buildingBlock = A.Fake<IBuildingBlock>();
         _container = new Container {Mode = _oldContainerMode,Id = "Container"};
         A.CallTo(() => _context.Get<IContainer>(_container.Id)).Returns(_container);
         _volumeParameter = new Parameter().WithName(Constants.Parameters.VOLUME);
         var parameterFactory = A.Fake<IParameterFactory>();
         A.CallTo(() => parameterFactory.CreateVolumeParameter()).Returns(_volumeParameter);
         A.CallTo(() => _context.Resolve<IParameterFactory>()).Returns(parameterFactory);
         sut = new SetContainerModeCommand(_buildingBlock, _container, _newContainerMode);
      }
   }

   public class When_setting_the_container_mode_from_physical_to_logical_in_a_container_that_had_a_volume_parameter : concern_for_SetContainerModeCommand
   {
      private IParameter _existingVolumeParameter;

      protected override void Context()
      {
         _newContainerMode = ContainerMode.Logical;
         _oldContainerMode = ContainerMode.Physical;
         base.Context();
         _existingVolumeParameter = new Parameter().WithName(Constants.Parameters.VOLUME);
         _container.Add(_existingVolumeParameter);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_not_remove_the_volume_parameter()
      {
         _container.EntityAt<IParameter>(Constants.Parameters.VOLUME).ShouldBeEqualTo(_existingVolumeParameter);
      }

      [Observation]
      public void The_inverse_command_should_have_no_problem_if_the_volume_is_already_there()
      {
         sut.InvokeInverse(_context);
         _container.EntityAt<IParameter>(Constants.Parameters.VOLUME).ShouldBeEqualTo(_existingVolumeParameter);
      }
   }
}