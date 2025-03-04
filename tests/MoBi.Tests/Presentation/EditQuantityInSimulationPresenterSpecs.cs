using OSPSuite.Core.Commands.Core;
using FakeItEasy;
using OSPSuite.BDDHelper;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditQuantityInSimulationPresenter : ContextSpecification<EditQuantityInSimulationPresenter>
   {
      protected IEditQuantityInSimulationView _view;
      private IQuantityToQuantityDTOMapper _mapper;
      private IFormulaPresenterCache _presenterCache;
      protected IEditParametersInContainerPresenter _parametrPresenters;
      private IReactionDimensionRetriever _reactionDimensionRetriever;
      protected IQuantityTask _quantityTask;

      protected override void Context()
      {
         _view = A.Fake<IEditQuantityInSimulationView>();
         _mapper = A.Fake<IQuantityToQuantityDTOMapper>();
         _presenterCache = A.Fake<IFormulaPresenterCache>();
         _parametrPresenters = A.Fake<IEditParametersInContainerPresenter>();
         _quantityTask = A.Fake<IQuantityTask>();
         _reactionDimensionRetriever= A.Fake<IReactionDimensionRetriever>();

         sut = new EditQuantityInSimulationPresenter(_view,_mapper,_presenterCache,_parametrPresenters,_quantityTask,_reactionDimensionRetriever);
         sut.Simulation= A.Fake<IMoBiSimulation>();
         sut.InitializeWith(A.Fake<ICommandCollector>());
      }
   }

   class When_told_to_select_an_parameter : concern_for_EditQuantityInSimulationPresenter
   {
      private IParameter _selectedParameter;
      private MoleculeAmount _parentMolecule;

      protected override void Context()
      {
         base.Context();
         _parentMolecule = new MoleculeAmount().WithName("Drug");
         _selectedParameter = new Parameter().WithParentContainer(_parentMolecule).WithName("P").WithFormula(new ConstantFormula(2.1));
      }

      protected override void Because()
      {
         sut.SelectParameter(_selectedParameter);
      }

      [Observation]
      public void should_select_the_parameter_in_the_parameter_list()
      {
         A.CallTo(() => _parametrPresenters.Select(_selectedParameter)).MustHaveHappened();
      }


      [Observation]
      public void should_ensure_that_parameter_tab_is_shown()
      {
         A.CallTo(() => _view.ShowParameters()).MustHaveHappened();
      }
   }

   public class When_changing_the_value_of_the_start_value_concentration_of_a_molecule_amount    : concern_for_EditQuantityInSimulationPresenter
   {
      private MoleculeAmount _moleculeAmount;
      private Parameter _startValueParameter;
      private IFormula _explicitFormula;

      protected override void Context()
      {
         base.Context();
         _explicitFormula=new ExplicitFormula();
         _moleculeAmount=new MoleculeAmount();
         _startValueParameter = new Parameter().WithName(Constants.Parameters.START_VALUE).WithFormula(_explicitFormula);
         _startValueParameter.Dimension = Constants.Dimension.NO_DIMENSION;
         _moleculeAmount.Add(_startValueParameter);
         sut.Edit(_moleculeAmount);
      }
      protected override void Because()
      {
         sut.SetValue(15);
      }

      [Observation]
      public void should_update_the_value_of_the_start_value_parameter()
      {
         A.CallTo(() => _quantityTask.SetQuantityDisplayValue(_startValueParameter, 15,sut.Simulation)).MustHaveHappened();
      }
   }

   public class When_changing_the_display_unit_of_the_start_value_concentration_of_a_molecule_amount : concern_for_EditQuantityInSimulationPresenter
   {
      private MoleculeAmount _moleculeAmount;
      private Parameter _startValueParameter;
      private IFormula _explicitFormula;
      private Unit _displayUnit;

      protected override void Context()
      {
         base.Context();
         _explicitFormula = new ExplicitFormula();
         _moleculeAmount = new MoleculeAmount();
         _startValueParameter = new Parameter().WithName(Constants.Parameters.START_VALUE).WithFormula(_explicitFormula);
         _startValueParameter.Dimension = Constants.Dimension.NO_DIMENSION;
         _moleculeAmount.Add(_startValueParameter);
         _displayUnit= A.Fake<Unit>();
         sut.Edit(_moleculeAmount);
      }
      protected override void Because()
      {
         sut.SetDisplayUnit(_displayUnit);
      }

      [Observation]
      public void should_update_the_value_of_the_start_value_parameter()
      {
         A.CallTo(() => _quantityTask.SetQuantityDisplayUnit(_startValueParameter, _displayUnit, sut.Simulation)).MustHaveHappened();
      }
   }

   public class When_editing_a_quantity_that_is_a_container : concern_for_EditQuantityInSimulationPresenter
   {
      private MoleculeAmount _moleculeAmount;

      protected override void Context()
      {
         base.Context();
         _moleculeAmount = new MoleculeAmount();
      }
      protected override void Because()
      {
         sut.Edit(_moleculeAmount);
      }

      [Observation]
      public void the_parameters_tab_should_be_shown()
      {
         A.CallTo(() => _view.ShowParameters()).MustHaveHappened();
      }
   }

   public class When_reusing_the_presenter_for_new_quantity_that_is_a_container : concern_for_EditQuantityInSimulationPresenter
   {
      private MoleculeAmount _moleculeAmount;
      private MoleculeAmount _anotherMoleculeAmount;

      protected override void Context()
      {
         base.Context();
         _moleculeAmount = new MoleculeAmount();
         _anotherMoleculeAmount = new MoleculeAmount();
         sut.Edit(_moleculeAmount);
      }

      [Observation]
      public void the_parameters_tab_should_be_shown()
      {
         A.CallTo(() => _view.ShowParameters()).MustHaveHappened(1, Times.Exactly);
         sut.Edit(_anotherMoleculeAmount);
         A.CallTo(() => _view.ShowParameters()).MustHaveHappened(1, Times.Exactly);
      }
   }

   public class When_editing_a_quantity_that_is_not_a_container : concern_for_EditQuantityInSimulationPresenter
   {
      private Observer _observer;

      protected override void Context()
      {
         base.Context();
         _observer = new Observer();
      }
      protected override void Because()
      {
         sut.Edit(_observer);
      }

      [Observation]
      public void the_parameters_tab_should_be_hidden()
      {
         A.CallTo(() => _view.ShowParameters()).MustNotHaveHappened();
         A.CallTo(() => _view.HideParametersView()).MustHaveHappened();
      }
   }
}	