using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mapper
{
   public abstract class concern_for_InteractionContainerToDTOInteractionContainerMapper : ContextSpecification<IInteractionContainerToInteractionContainerDTOMapper>
   {
      protected IParameterToParameterDTOMapper _parameterToParameterDTOMapper;

      protected override void Context()
      {
         _parameterToParameterDTOMapper = A.Fake<IParameterToParameterDTOMapper>();
         sut = new InteractionContainerToInteractionContainerDTOMapper(_parameterToParameterDTOMapper);
      }
   }

   class When_mapping_an_interaction_container_to_a_dto : concern_for_InteractionContainerToDTOInteractionContainerMapper
   {
      private InteractionContainer _interactionContainer;
      private InteractionContainerDTO _result;
      private IParameter _parameter;

      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
         _interactionContainer = A.Fake<InteractionContainer>();
         _interactionContainer.Icon = ApplicationIcons.Container.IconName;
         A.CallTo(() => _interactionContainer.GetChildren<IParameter>()).Returns(new[] {_parameter});
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_interactionContainer);
      }

      [Observation]
      public void should_return_a_right_dto()
      {
         _result.ShouldNotBeNull();
         _result.Name.ShouldBeEqualTo(_interactionContainer.Name);
         _result.Icon.ShouldBeEqualTo(ApplicationIcons.Container);
      }

      [Observation]
      public void should_also_map_all_child_parameter()
      {
         A.CallTo(() => _parameterToParameterDTOMapper.MapFrom(_parameter)).MustHaveHappened();
      }
   }
}