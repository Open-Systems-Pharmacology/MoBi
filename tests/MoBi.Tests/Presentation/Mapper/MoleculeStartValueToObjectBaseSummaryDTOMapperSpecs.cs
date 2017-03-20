using System.Linq;
using System.Runtime.Remoting.Contexts;
using MoBi.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Presentation.Mapper
{
   public abstract class concern_for_MoleculeStartValueToObjectBaseSummaryDTOMapper : ContextSpecification<MoleculeStartValueToObjectBaseSummaryDTOMapper>
   {
      protected MoleculeStartValue _builder;

      protected override void Context()
      {
         _builder = new MoleculeStartValue();
         sut = new MoleculeStartValueToObjectBaseSummaryDTOMapper();
      }
   }

   public class when_mapping_molecule_start_value_to_dto : concern_for_MoleculeStartValueToObjectBaseSummaryDTOMapper
   {
      private ObjectBaseSummaryDTO _result;

      protected override void Context()
      {
         base.Context();
         _builder.IsPresent = true;
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_builder);
      }

      [Observation]
      public void dto_should_contain_correct_fields()
      {
         _result.ApplicationIcon.ShouldBeEqualTo(ApplicationIcons.MoleculeStartValues);
      }

      [Observation]
      public void dictionary_should_contain_correct_values()
      {
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.IsPresent) && x.Value.Equals("True")).ShouldBeEqualTo(1);
      }
   }
}
