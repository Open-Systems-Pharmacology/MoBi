using System.Linq;
using MoBi.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Assets;

namespace MoBi.Presentation.Mapper
{
   public abstract class concern_for_PassiveTransportBuilderToObjectBaseSummaryDTOMapper : ContextSpecification<PassiveTransportBuilderToObjectBaseSummaryDTOMapper>
   {
      protected TransportBuilder _builder;

      protected override void Context()
      {
         _builder = new TransportBuilder();
         sut = new PassiveTransportBuilderToObjectBaseSummaryDTOMapper(new ObjectTypeResolver());
      }
   }

   public class when_mapping_transport_builder_for_all : concern_for_PassiveTransportBuilderToObjectBaseSummaryDTOMapper
   {
      private ObjectBaseSummaryDTO _result;

      protected override void Context()
      {
         base.Context();
         _builder.Name = "Name";
         _builder.ForAll = true;

         _builder.SourceCriteria.Add(new MatchTagCondition("tag1"));
         _builder.SourceCriteria.Add(new NotMatchTagCondition("tag2"));

         _builder.TargetCriteria.Add(new MatchTagCondition("tag3"));
         _builder.TargetCriteria.Add(new MatchTagCondition("tag4"));

         _builder.MoleculeList.AddMoleculeNameToExclude("M1");
         _builder.MoleculeList.AddMoleculeNameToExclude("M2");
         _builder.MoleculeList.AddMoleculeName("M3");
         _builder.MoleculeList.AddMoleculeName("M4");
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_builder);
      }

      [Observation]
      public void when_mapping_passive_transport_dto_has_correct_values()
      {
         _result.EntityName.ShouldBeEqualTo("Name");
         _result.ApplicationIcon.ShouldBeEqualTo(ApplicationIcons.PassiveTransport);
      }

      [Observation]
      public void when_mapping_passive_transport_dictionary_has_correct_values()
      {
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.SourceDescriptor)).ShouldBeEqualTo(1);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.TargetDescriptor)).ShouldBeEqualTo(1);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.Type) && x.Value.Equals("Passive Transport")).ShouldBeEqualTo(1);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.ForAll) && x.Value.Equals("True")).ShouldBeEqualTo(1);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.MoleculesToInclude)).ShouldBeEqualTo(0);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.MoleculesToExclude)).ShouldBeEqualTo(0);
      }
   }

   public class when_mapping_transport_builder_not_for_all : concern_for_PassiveTransportBuilderToObjectBaseSummaryDTOMapper
   {
      private ObjectBaseSummaryDTO _result;

      protected override void Context()
      {
         base.Context();
         _builder.Name = "Name";
         _builder.ForAll = false;

         _builder.SourceCriteria.Add(new MatchTagCondition("tag1"));
         _builder.SourceCriteria.Add(new NotMatchTagCondition("tag2"));

         _builder.TargetCriteria.Add(new MatchTagCondition("tag3"));
         _builder.TargetCriteria.Add(new MatchTagCondition("tag4"));

         _builder.MoleculeList.AddMoleculeNameToExclude("M1");
         _builder.MoleculeList.AddMoleculeNameToExclude("M2");
         _builder.MoleculeList.AddMoleculeName("M3");
         _builder.MoleculeList.AddMoleculeName("M4");
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_builder);
      }

      [Observation]
      public void when_mapping_passive_transport_dto_has_correct_values()
      {
         _result.EntityName.ShouldBeEqualTo("Name");
         _result.ApplicationIcon.ShouldBeEqualTo(ApplicationIcons.PassiveTransport);
      }

      [Observation]
      public void when_mapping_passive_transport_dictionary_has_correct_values()
      {
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.SourceDescriptor)).ShouldBeEqualTo(1);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.TargetDescriptor)).ShouldBeEqualTo(1);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.Type) && x.Value.Equals("Passive Transport")).ShouldBeEqualTo(1);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.ForAll) && x.Value.Equals("False")).ShouldBeEqualTo(1);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.MoleculesToInclude) && x.Value.Equals("M3, M4")).ShouldBeEqualTo(1);
         _result.Dictionary.Count(x => x.Key.Equals(AppConstants.Captions.MoleculesToExclude) && x.Value.Equals("M1, M2")).ShouldBeEqualTo(1);
      }
   }
}
