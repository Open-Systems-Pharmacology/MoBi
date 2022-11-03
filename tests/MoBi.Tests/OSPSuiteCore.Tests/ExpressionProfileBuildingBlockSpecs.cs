using System.Linq;
using MoBi.IntegrationTests;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Container;

namespace MoBi.OSPSuiteCore.Tests
{
   public class concern_for_ExpressionProfileBuildingBlock : ContextForIntegration<ExpressionProfileBuildingBlock>
   {
      protected override void Context()
      {
         sut = new ExpressionProfileBuildingBlock();
      }
   }

   public class when_updating_properties_of_building_block : concern_for_ExpressionProfileBuildingBlock
   {
      private ExpressionProfileBuildingBlock _expressionProfileBuildingBlock;
      private ICloneManager _cloneManager;

      protected override void Context()
      {
         base.Context();
         _expressionProfileBuildingBlock = new ExpressionProfileBuildingBlock();
         _cloneManager = IoC.Resolve<ICloneManager>();
         _expressionProfileBuildingBlock.Name = "Molecule|Species|Name";
         _expressionProfileBuildingBlock.Type = ExpressionType.MetabolizingEnzyme;
         _expressionProfileBuildingBlock.PKSimVersion = 11;
         _expressionProfileBuildingBlock.Add(new ExpressionParameter().WithName("name1"));
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_expressionProfileBuildingBlock, _cloneManager);
      }

      [Observation]
      public void the_updated_expression_profile_should_have_properties_set()
      {
         sut.Name.ShouldBeEqualTo("Molecule|Species|Name");
         sut.Type.ShouldBeEqualTo(ExpressionType.MetabolizingEnzyme);
         sut.PKSimVersion.ShouldBeEqualTo(11);
         sut.Count().ShouldBeEqualTo(1);
      }
   }

   public class when_reading_the_icon_name_for_the_building_block : concern_for_ExpressionProfileBuildingBlock
   {
      [Observation]
      public void icon_name_translated_for_each_expression_type()
      {
         sut.Type = ExpressionType.MetabolizingEnzyme;
         sut.Icon.ShouldBeEqualTo("Enzyme");

         sut.Type = ExpressionType.TransportProtein;
         sut.Icon.ShouldBeEqualTo("Transporter");

         sut.Type = ExpressionType.ProteinBindingPartner;
         sut.Icon.ShouldBeEqualTo("Protein");
      }
   }

   public class when_setting_the_name_of_the_building_block : concern_for_ExpressionProfileBuildingBlock
   {
      protected override void Because()
      {
         sut.Name = "Molecule|Species|Phenotype";
      }

      [Observation] 
      public void the_name_should_set_the_category_of_the_building_block()
      {
         sut.Category.ShouldBeEqualTo("Phenotype");
         sut.MoleculeName.ShouldBeEqualTo("Molecule");
         sut.Species.ShouldBeEqualTo("Species");
      }
   }
}
