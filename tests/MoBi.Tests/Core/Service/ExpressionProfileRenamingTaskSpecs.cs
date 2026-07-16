using System.Linq;
using MoBi.Core.Domain.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Service
{
   public abstract class concern_for_ExpressionProfileRenamingTask : ContextSpecification<IExpressionProfileRenamingTask>
   {
      protected ExpressionProfileBuildingBlock _expressionProfile;

      protected override void Context()
      {
         sut = new ExpressionProfileRenamingTask();

         _expressionProfile = new ExpressionProfileBuildingBlock().WithName("OldMolecule|Species|Category");
         _expressionProfile.Add(new ExpressionParameter { Path = new ObjectPath("Organism", "Liver", "OldMolecule", "RelExp") });
         _expressionProfile.Add(new ExpressionParameter { Path = new ObjectPath("Organism", "Kidney", "OldMolecule", "RelExp") });
         _expressionProfile.AddInitialCondition(new InitialCondition { Path = new ObjectPath("Organism", "OldMolecule", "IC") });
      }
   }

   public class When_renaming_an_expression_profile_to_a_new_molecule : concern_for_ExpressionProfileRenamingTask
   {
      protected override void Because()
      {
         sut.Rename(_expressionProfile, "NewMolecule|Species|Category");
      }

      [Observation]
      public void should_set_the_building_block_name()
      {
         _expressionProfile.Name.ShouldBeEqualTo("NewMolecule|Species|Category");
      }

      [Observation]
      public void should_replace_the_molecule_name_in_all_expression_parameter_paths()
      {
         _expressionProfile.Any<ExpressionParameter>(x => x.Path.Contains("OldMolecule")).ShouldBeFalse();
         _expressionProfile.All<ExpressionParameter>(x => x.Path.Contains("NewMolecule")).ShouldBeTrue();
      }

      [Observation]
      public void should_replace_the_molecule_name_in_all_initial_condition_paths()
      {
         _expressionProfile.Any<InitialCondition>(x => x.Path.Contains("OldMolecule")).ShouldBeFalse();
         _expressionProfile.All<InitialCondition>(x => x.Path.Contains("NewMolecule")).ShouldBeTrue();
      }
   }

   public class When_renaming_an_expression_profile_without_changing_the_molecule : concern_for_ExpressionProfileRenamingTask
   {
      protected override void Because()
      {
         sut.Rename(_expressionProfile, "OldMolecule|Species|Category - Clone");
      }

      [Observation]
      public void should_set_the_building_block_name()
      {
         _expressionProfile.Name.ShouldBeEqualTo("OldMolecule|Species|Category - Clone");
      }

      [Observation]
      public void should_leave_all_paths_unchanged()
      {
         _expressionProfile.All<ExpressionParameter>(x => x.Path.Contains("OldMolecule")).ShouldBeTrue();
         _expressionProfile.All<InitialCondition>(x => x.Path.Contains("OldMolecule")).ShouldBeTrue();
      }
   }
}
