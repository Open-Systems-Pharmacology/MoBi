﻿using System.Linq;
using FakeItEasy;
using MoBi.IntegrationTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Container;

namespace MoBi.OSPSuiteCore.Tests
{
   public class concern_for_CoreExpressionProfile : ContextForIntegration<CoreExpressionProfile>
   {
      protected override void Context()
      {
         sut = new CoreExpressionProfile();
      }
   }

   public class when_updating_properties_of_building_block : concern_for_CoreExpressionProfile
   {
      private CoreExpressionProfile _coreExpressionProfile;
      private ICloneManager _cloneManager;

      protected override void Context()
      {
         base.Context();
         _coreExpressionProfile = new CoreExpressionProfile();
         _cloneManager = IoC.Resolve<ICloneManager>();
         _coreExpressionProfile.Name = "Molecule|Species|Name";
         _coreExpressionProfile.Type = ExpressionType.MetabolizingEnzyme;
         _coreExpressionProfile.PKSimVersion = 11;
         _coreExpressionProfile.Add(new ExpressionParameter().WithName("name1"));
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_coreExpressionProfile, _cloneManager);
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

   public class when_setting_the_name_of_the_building_block : concern_for_CoreExpressionProfile
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
