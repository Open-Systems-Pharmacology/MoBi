using FakeItEasy;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditSumFormulaPresenter : ContextSpecification<IEditSumFormulaPresenter>
   {
      protected IEditSumFormulaView _view;
      protected ISumFormulaToDTOSumFormulaMapper _sumFormulaDTOMapper;
      protected IDescriptorConditionListPresenter<SumFormula> _descriptorConditionListPresenter;
      protected IMoBiFormulaTask _moBiFormulaTask;
      protected IDisplayUnitRetriever _displayUnitRetriever;
      protected IEditFormulaPathListPresenter _editFormulaPathListPresenter;
      protected SumFormula _sumFormula;

      protected override void Context()
      {
         _view = A.Fake<IEditSumFormulaView>();
         _sumFormulaDTOMapper = A.Fake<ISumFormulaToDTOSumFormulaMapper>();
         _descriptorConditionListPresenter = A.Fake<IDescriptorConditionListPresenter<SumFormula>>();
         _moBiFormulaTask = A.Fake<IMoBiFormulaTask>();
         _displayUnitRetriever = A.Fake<IDisplayUnitRetriever>();
         _editFormulaPathListPresenter = A.Fake<IEditFormulaPathListPresenter>();
         _sumFormula = new SumFormula();
         A.CallTo(() => _sumFormulaDTOMapper.MapFrom(_sumFormula)).Returns(new SumFormulaDTO(_sumFormula));
         A.CallTo(() => _moBiFormulaTask.Validate(A<string>._, A<SumFormula>._, A<OSPSuite.Core.Domain.Builder.IBuildingBlock>._))
            .Returns((true, string.Empty));
         sut = new EditSumFormulaPresenter(
            _view, _sumFormulaDTOMapper, _descriptorConditionListPresenter, _moBiFormulaTask, _displayUnitRetriever, _editFormulaPathListPresenter);
         sut.Edit(_sumFormula);
         Fake.ClearRecordedCalls(_view);
      }
   }

   public class When_handling_a_TagChangedEvent_for_the_edited_sum_formula : concern_for_EditSumFormulaPresenter
   {
      protected override void Because()
      {
         sut.Handle(new TagChangedEvent(_sumFormula));
      }

      [Observation]
      public void should_refresh_the_view()
      {
         A.CallTo(() => _view.Show(A<SumFormulaDTO>._)).MustHaveHappened();
      }
   }

   public class When_handling_a_TagChangedEvent_for_a_different_tagged_object : concern_for_EditSumFormulaPresenter
   {
      private SumFormula _otherFormula;

      protected override void Context()
      {
         base.Context();
         _otherFormula = new SumFormula();
      }

      protected override void Because()
      {
         sut.Handle(new TagChangedEvent(_otherFormula));
      }

      [Observation]
      public void should_not_refresh_the_view()
      {
         A.CallTo(() => _view.Show(A<SumFormulaDTO>._)).MustNotHaveHappened();
      }
   }
}
