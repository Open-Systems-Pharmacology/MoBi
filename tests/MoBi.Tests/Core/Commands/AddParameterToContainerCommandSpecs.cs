using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_AddParameterToContainerCommand : ContextSpecification<AddParameterToContainerCommand>
   {
      protected IBuildingBlock _buildingBlock;
      protected IParameter _parameterToAdd;
      protected IContainer _container;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _buildingBlock = A.Fake<IBuildingBlock>();
         _parameterToAdd = new Parameter();
         _container = GetContainer();
         _context = A.Fake<IMoBiContext>();

         sut = new AddParameterToContainerCommand(_container, _parameterToAdd, _buildingBlock);
      }

      protected abstract IContainer GetContainer();
   }

   public abstract class When_testing_for_appropriate_parameter_build_modes : concern_for_AddParameterToContainerCommand
   {
      protected abstract ParameterBuildMode GetBuildModeType();

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_parameter_build_mode_should_be_of_correct_type()
      {
         _parameterToAdd.BuildMode.ShouldBeEqualTo(GetBuildModeType());
      }

      [Observation]
      public void the_parameter_should_be_added_to_the_container()
      {
         _container.ShouldContain(_parameterToAdd);
      }
   }

   public class When_adding_a_new_parameter_to_transporter_molecule_builder : When_testing_for_appropriate_parameter_build_modes
   {
      protected override IContainer GetContainer()
      {
         return new TransporterMoleculeContainer();
      }

      protected override ParameterBuildMode GetBuildModeType()
      {
         return ParameterBuildMode.Global;
      }
   }

   public class When_adding_a_new_parameter_to_a_generic_container : When_testing_for_appropriate_parameter_build_modes
   {
      protected override IContainer GetContainer()
      {
         return new Container();
      }

      protected override ParameterBuildMode GetBuildModeType()
      {
         return ParameterBuildMode.Local;
      }
   }

   public class When_executing_inverse_of_add_a_new_paramter_to_container : concern_for_AddParameterToContainerCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.Get<IContainer>(_container.Id)).Returns(_container);
         A.CallTo(() => _context.Get<IParameter>(_parameterToAdd.Id)).Returns(_parameterToAdd);
         A.CallTo(() => _context.Get<IBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
      }

      protected override IContainer GetContainer()
      {
         return new Container();
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_parameter_should_be_removed_from_the_container()
      {
         _container.ShouldBeEmpty();
      }
   }

}
