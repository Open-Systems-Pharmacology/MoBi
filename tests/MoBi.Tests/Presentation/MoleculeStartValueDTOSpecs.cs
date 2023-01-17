using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Validation;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_MoleculeStartValueDTO : ContextSpecification<MoleculeStartValueDTO>
   {
      protected MoleculeStartValue _moleculeStartValue;
      protected MoleculeStartValuesBuildingBlock _moleculeStartValuesBuildingBlock;

      protected override void Context()
      {
         _moleculeStartValue = new MoleculeStartValue { Name = "MSV" };
         _moleculeStartValuesBuildingBlock = new MoleculeStartValuesBuildingBlock { _moleculeStartValue };
         sut = new MoleculeStartValueDTO(_moleculeStartValue, _moleculeStartValuesBuildingBlock) { ContainerPath = new ObjectPath("") };
      }
   }

   public class When_validating_molecule_start_value_with_path_elements_empty : concern_for_MoleculeStartValueDTO
   {
      protected override void Context()
      {
         base.Context();
         sut = new MoleculeStartValueDTO(_moleculeStartValue, _moleculeStartValuesBuildingBlock) { ContainerPath = new ObjectPath(string.Empty, "value") };
      }

      protected override void Because()
      {
         sut.Validate();
      }

      [Observation]
      public void validation_should_fail_with_message()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }

   public class When_validating_a_molecule_start_value_with_non_unique_path : concern_for_MoleculeStartValueDTO
   {
      protected override void Context()
      {
         base.Context();
         _moleculeStartValuesBuildingBlock.Add(new MoleculeStartValue { ContainerPath = new ObjectPath("ContainerPath"), Name = "MSV" });
         _moleculeStartValue.ContainerPath = new ObjectPath("ContainerPath");
      }

      protected override void Because()
      {
         sut.Validate();
      }

      [Observation]
      public void molecule_start_value_is_invalid()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }

   public class When_validating_a_molecule_start_value_with_a_scale_factor : concern_for_MoleculeStartValueDTO
   {
      [Observation]
      public void should_return_a_valid_state_it_the_scale_factor_is_bigger_than_0()
      {
         sut.ScaleDivisor = 3;
         sut.IsValid().ShouldBeTrue();
      }

      [Observation]
      public void should_return_an_invalid_state_it_the_scale_factor_is_smaller_than_or_equal_to_0()
      {
         sut.ScaleDivisor = 0;
         sut.IsValid().ShouldBeTrue();
      }
   }
}