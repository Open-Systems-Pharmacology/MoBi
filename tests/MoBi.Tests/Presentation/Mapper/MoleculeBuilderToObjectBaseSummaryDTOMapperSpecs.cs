using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Presentation.Mapper
{
   public abstract class concern_for_MoleculeBuilderToObjectBaseSummaryDTOMapper : ContextSpecification<MoleculeBuilderToObjectBaseSummaryDTOMapper>
   {
      protected IMoleculeBuilder _builder;

      protected override void Context()
      {
         _builder = new MoleculeBuilder();
         sut = new MoleculeBuilderToObjectBaseSummaryDTOMapper(new ObjectTypeResolver());
      }
   }

   public class When_mapping_builder_to_dto : concern_for_MoleculeBuilderToObjectBaseSummaryDTOMapper
   {
      private ObjectBaseSummaryDTO _result;

      protected override void Context()
      {
         base.Context();
         _builder.Name = "Name";
         _builder.AddTransporterMoleculeContainer(A.Fake<TransporterMoleculeContainer>());
         _builder.AddParameter(new Parameter{Name="name"});
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_builder);
      }

      [Observation]
      public void mapped_fields_should_have_correct_values()
      {
         _result.EntityName.ShouldBeEqualTo("Name");
         _result.ApplicationIcon.ShouldBeEqualTo(ApplicationIcons.Molecule);
      }

      [Observation]
      public void mapped_dictionary_should_have_correct_values()
      {
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.Type) && x.Value.Equals("Molecule")).ShouldBeEqualTo(1);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.NumberOfParameters) && x.Value.Equals("1")).ShouldBeEqualTo(1);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.TransporterMolecules) && x.Value.Equals("1")).ShouldBeEqualTo(1);
      }
   }
}
