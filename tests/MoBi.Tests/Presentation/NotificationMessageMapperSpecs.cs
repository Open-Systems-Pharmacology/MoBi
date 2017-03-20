using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.Main;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;


namespace MoBi.Presentation
{
   public abstract class concern_for_NotificationMessageMapper : ContextSpecification<INotificationMessageMapper>
   {
      protected IObjectTypeResolver _objectTypeResolver;
      protected IBuildingBlockRetriever _buildingBlockRetriever;

      protected override void Context()
      {
         _objectTypeResolver = A.Fake<IObjectTypeResolver>();
         _buildingBlockRetriever = A.Fake<IBuildingBlockRetriever>();
         sut = new NotificationMessageMapper(_objectTypeResolver, _buildingBlockRetriever);
      }
   }

   public class When_mapping_a_validation_message_to_a_notification_message : concern_for_NotificationMessageMapper
   {
      private ValidationMessage _validationMessage;
      private IObjectBase _objectBase;
      private IBuildingBlock _buildingBlock;
      private NotificationMessageDTO _result;
      private string _buildingBlockType;
      private string _objectType;
      private IBuildingBlock _originBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _buildingBlockType = "BB";
         _objectType = "Builder";
         _objectBase = A.Fake<IObjectBase>();
         _originBuildingBlock = A.Fake<IBuildingBlock>();
         _buildingBlock = A.Fake<IBuildingBlock>();
         A.CallTo(() => _buildingBlockRetriever.GetBuildingBlockFor(_objectBase, _originBuildingBlock)).Returns(_buildingBlock);
         A.CallTo(() => _objectTypeResolver.TypeFor(_objectBase)).Returns(_objectType);
         A.CallTo(() => _objectTypeResolver.TypeFor(_buildingBlock)).Returns(_buildingBlockType);
         _validationMessage = new ValidationMessage(NotificationType.Error, "This is the message", _objectBase, _originBuildingBlock);
         _validationMessage.AddDetail("Details1");
         _validationMessage.AddDetail("Details2");
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_validationMessage);
      }
      
      [Observation]
      public void should_set_the_origin_to_simulation()
      {
         _result.MessageOrigin.ShouldBeEqualTo(MessageOrigin.Simulation);   
      }

      [Observation]
      public void should_have_added_all_the_details_from_the_validation_message_to_the_notification()
      {
         _result.Details.ShouldOnlyContain("Details1","Details2");
      }

      [Observation]
      public void should_have_set_the_building_block_in_the_message()
      {
         _result.BuildingBlock.ShouldBeEqualTo(_buildingBlock);
      }

      [Observation]
      public void should_have_retrieved_the_building_block_type()
      {
         _result.BuildingBlockType.ShouldBeEqualTo(_buildingBlockType);
      }

       [Observation]
      public void should_have_retrieved_the_object_type()
      {
         _result.ObjectType.ShouldBeEqualTo(_objectType);
      }
   }

   public class When_mapping_a_formula_to_a_notification_message : concern_for_NotificationMessageMapper
   {
      private IBuildingBlock _buildingBlock;
      private NotificationMessageDTO _result;
      private string _buildingBlockType;
      private string _objectType;
      private IFormula _formula;

      protected override void Context()
      {
         base.Context();
         _buildingBlockType = "BB";
         _objectType = "Formula";
         _formula = A.Fake<IFormula>();
         _buildingBlock = A.Fake<IBuildingBlock>();
         A.CallTo(() => _objectTypeResolver.TypeFor(_buildingBlock)).Returns(_buildingBlockType);
         A.CallTo(() => _objectTypeResolver.TypeFor<IObjectBase>(_formula)).Returns(_objectType);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_formula, _buildingBlock);
      }

      [Observation]
      public void should_set_the_origin_to_simulation()
      {
         _result.MessageOrigin.ShouldBeEqualTo(MessageOrigin.Formula);
      }

      [Observation]
      public void should_have_set_the_building_block_in_the_message()
      {
         _result.BuildingBlock.ShouldBeEqualTo(_buildingBlock);
      }

      [Observation]
      public void should_have_retrieved_the_building_block_type()
      {
         _result.BuildingBlockType.ShouldBeEqualTo(_buildingBlockType);
      }

      [Observation]
      public void should_have_retrieved_the_object_type()
      {
         _result.ObjectType.ShouldBeEqualTo(_objectType);
      }
   }
}	