using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   internal class concern_for_PathAndValueEntitySelectionPresenter : ContextSpecification<PathAndValueEntitySelectionPresenter>
   {
      protected IPathAndValueEntityToSelectableDTOMapper _mapper;
      protected IPathAndValueEntitySelectionView _view;
      protected ParameterValuesBuildingBlock _buildingBlock;

      protected override void Context()
      {
         _mapper = new PathAndValueEntityToSelectableDTOMapper();
         _view = A.Fake<IPathAndValueEntitySelectionView>();
         sut = new PathAndValueEntitySelectionPresenter(_view, _mapper);
         _buildingBlock = new ParameterValuesBuildingBlock();
      }
   }

   internal class When_selecting_replacement_entities_without_collision : concern_for_PathAndValueEntitySelectionPresenter
   {
      private ParameterValue _newEntity;
      private IReadOnlyList<ParameterValue> _entitiesToAdd;

      protected override void Context()
      {
         base.Context();
         var objectPath = new ObjectPath("the", "path");
         _newEntity = new ParameterValue
         {
            Value = 1.0,
            Path = objectPath,
         };
      }

      protected override void Because()
      {
         _entitiesToAdd = sut.SelectReplacementEntities(new[] { _newEntity }, _buildingBlock);
      }

      [Observation]
      public void should_not_display_the_view()
      {
         A.CallTo(() => _view.Display()).MustNotHaveHappened();
      }

      [Observation]
      public void should_return_the_entire_list_of_new_entities()
      {
         _entitiesToAdd.ShouldOnlyContain(_newEntity);
      }
   }

   internal class When_selecting_replacement_entities_with_collision_and_the_colliding_entity_is_not_selected : concern_for_PathAndValueEntitySelectionPresenter
   {
      private ParameterValue _newEntity;
      private IReadOnlyList<ParameterValue> _entitiesToAdd;
      private ParameterValue _existingEntity;
      private ParameterValue _anotherEntity;

      protected override void Context()
      {
         base.Context();
         var objectPath = new ObjectPath("the", "path");
         _newEntity = new ParameterValue
         {
            Value = 1.0,
            Path = objectPath,
         };

         _existingEntity = new ParameterValue
         {
            Value = 2.0,
            Path = objectPath,
         };
         _buildingBlock.Add(_existingEntity);

         _anotherEntity = new ParameterValue
         {
            Value = 2.0,
            Path = new ObjectPath("different"),
         };
      }

      protected override void Because()
      {
         _entitiesToAdd = sut.SelectReplacementEntities(new[] { _newEntity, _anotherEntity }, _buildingBlock);
      }

      [Observation]
      public void should_display_the_view()
      {
         A.CallTo(() => _view.Display()).MustHaveHappened();
      }

      [Observation]
      public void should_return_only_the_non_colliding_entries()
      {
         _entitiesToAdd.ShouldOnlyContain(_anotherEntity);
      }
   }

   internal class When_selecting_replacement_entities_with_collision_and_the_colliding_entity_is_selected : concern_for_PathAndValueEntitySelectionPresenter
   {
      private ParameterValue _newEntity;
      private IReadOnlyList<ParameterValue> _entitiesToAdd;
      private ParameterValue _existingEntity;
      private ParameterValue _anotherEntity;

      protected override void Context()
      {
         base.Context();
         var objectPath = new ObjectPath("the", "path");
         _newEntity = new ParameterValue
         {
            Value = 1.0,
            Path = objectPath,
         };

         _existingEntity = new ParameterValue
         {
            Value = 2.0,
            Path = objectPath,
         };
         _buildingBlock.Add(_existingEntity);

         _anotherEntity = new ParameterValue
         {
            Value = 2.0,
            Path = new ObjectPath("different"),
         };

         A.CallTo(() => _view.AddSelectableEntities(A<IReadOnlyList<SelectableReplacePathAndValueDTO>>._)).Invokes(x => x.GetArgument<IReadOnlyList<SelectableReplacePathAndValueDTO>>(0).First().Selected = true);
      }

      protected override void Because()
      {
         _entitiesToAdd = sut.SelectReplacementEntities(new[] { _newEntity, _anotherEntity }, _buildingBlock);
      }

      [Observation]
      public void should_display_the_view()
      {
         A.CallTo(() => _view.Display()).MustHaveHappened();
      }

      [Observation]
      public void should_return_the_non_colliding_and_selected_entries()
      {
         _entitiesToAdd.ShouldOnlyContain(_anotherEntity, _newEntity);
      }
   }
}
