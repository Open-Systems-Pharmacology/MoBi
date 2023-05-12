using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation
{
   public class concern_for_AddBuildingBlocksToModuleDTO : ContextSpecification<AddBuildingBlocksToModuleDTO>
   {
      protected override void Context()
      {
         var module = new Module
         {
            new MoBiSpatialStructure(),
            new EventGroupBuildingBlock()
         }.WithName("module name");

         module.Add(new ParameterValuesBuildingBlock());
         module.Add(new InitialConditionsBuildingBlock());

         sut = new AddBuildingBlocksToModuleDTO(module);
      }
   }

   public class When_changing_parameter_values_add : concern_for_AddBuildingBlocksToModuleDTO
   {
      private bool _eventFired;

      protected override void Context()
      {
         base.Context();
         sut.PropertyChanged += (sender, args) =>
         {
            if (args.PropertyName.Equals(MoBiReflectionHelper.PropertyName<AddBuildingBlocksToModuleDTO>(x => x.ParameterValuesName)))
               _eventFired = true;
         };
      }

      protected override void Because()
      {
         sut.WithParameterValues = !sut.WithParameterValues;
      }

      [Observation]
      public void the_name_property_change_event_should_be_raised()
      {
         _eventFired.ShouldBeTrue();
      }
   }

   public class When_changing_initial_condition_add : concern_for_AddBuildingBlocksToModuleDTO
   {
      private bool _eventFired;

      protected override void Context()
      {
         base.Context();
         sut.PropertyChanged += (sender, args) =>
         {
            if (args.PropertyName.Equals(MoBiReflectionHelper.PropertyName<AddBuildingBlocksToModuleDTO>(x => x.InitialConditionsName)))
               _eventFired = true;
         };
      }

      protected override void Because()
      {
         sut.WithInitialConditions = !sut.WithInitialConditions;
      }

      [Observation]
      public void the_name_property_change_event_should_be_raised()
      {
         _eventFired.ShouldBeTrue();
      }
   }

   public class When_not_adding_a_initial_conditions_when_name_already_exists : concern_for_AddBuildingBlocksToModuleDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.InitialConditionsName = "Unoriginal Name";
         sut.AddUsedInitialConditionsNames(new[] { "Unoriginal Name" });
         sut.WithInitialConditions = false;
      }

      [Observation]
      public void the_dto_should_be_valid()
      {
         sut.IsValid().ShouldBeTrue();
      }
   }

   public class When_adding_a_initial_conditions_when_name_already_exists : concern_for_AddBuildingBlocksToModuleDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.InitialConditionsName = "Unoriginal Name";
         sut.AddUsedInitialConditionsNames(new[] { "Unoriginal Name" });
         sut.WithInitialConditions = true;
      }

      [Observation]
      public void the_dto_should_be_invalid()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }

   public class When_not_adding_a_parameter_values_when_name_already_exists : concern_for_AddBuildingBlocksToModuleDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.ParameterValuesName = "Unoriginal Name";
         sut.AddUsedParameterValuesNames(new[] { "Unoriginal Name" });
         sut.WithParameterValues = false;
      }

      [Observation]
      public void the_dto_should_be_valid()
      {
         sut.IsValid().ShouldBeTrue();
      }
   }

   public class When_adding_a_parameter_values_when_name_already_exists : concern_for_AddBuildingBlocksToModuleDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.ParameterValuesName = "Unoriginal Name";
         sut.AddUsedParameterValuesNames(new[] { "Unoriginal Name" });
         sut.WithParameterValues = true;
      }

      [Observation]
      public void the_dto_should_be_invalid()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }

   public class When_adding_a_building_block_through_the_UI : concern_for_AddBuildingBlocksToModuleDTO
   {
      protected override void Because()
      {
         sut.WithObserver = true;
      }

      [Observation]
      public void the_dto_should_allow_adding_building_blocks_not_already_present()
      {
         sut.CanSelectEventGroup.ShouldBeFalse();
         sut.CanSelectMolecule.ShouldBeTrue();
         sut.CanSelectInitialConditions.ShouldBeTrue();
         sut.CanSelectObserver.ShouldBeTrue();
         sut.CanSelectParameterValues.ShouldBeTrue();
         sut.CanSelectPassiveTransport.ShouldBeTrue();
         sut.CanSelectSpatialStructure.ShouldBeFalse();
         sut.CanSelectReaction.ShouldBeTrue();
      }

      [Observation]
      public void should_indicate_which_building_blocks_are_present()
      {
         sut.WithEventGroup.ShouldBeTrue();
         sut.WithMolecule.ShouldBeFalse();
         sut.WithInitialConditions.ShouldBeFalse();
         sut.WithObserver.ShouldBeTrue();
         sut.WithParameterValues.ShouldBeFalse();
         sut.WithPassiveTransport.ShouldBeFalse();
         sut.WithSpatialStructure.ShouldBeTrue();
         sut.WithReaction.ShouldBeFalse();
      }

      [Observation]
      public void should_indicate_that_the_newly_selected_building_block_only_should_be_created()
      {
         sut.CreateEventGroup.ShouldBeFalse();
         sut.CreateMolecule.ShouldBeFalse();
         sut.CreateObserver.ShouldBeTrue();
         sut.CreatePassiveTransport.ShouldBeFalse();
         sut.CreateSpatialStructure.ShouldBeFalse();
         sut.CreateReaction.ShouldBeFalse();
      }
   }
}