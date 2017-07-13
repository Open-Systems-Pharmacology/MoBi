using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Core.Domain.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core
{
   public abstract class concern_for_MoBIUnregisterAllVisitor : ContextSpecification<IUnregisterTask>
   {
      protected IWithIdRepository _objectbaseRepository;

      protected override void Context()
      {
         _objectbaseRepository = A.Fake<IWithIdRepository>();
         sut = new UnregisterTask(_objectbaseRepository);
      }
   }

   internal class When_visiting_for_unregister_a_formula_useable_with_a_not_constant_formula : concern_for_MoBIUnregisterAllVisitor
   {
      private Container _container;
      private Parameter _p1;
      private Parameter _p2;

      protected override void Context()
      {
         base.Context();
         _container = new Container().WithName("C1");
         _p1 = new Parameter {Id = "P1", Name = "P1", Formula = A.Fake<ExplicitFormula>().WithId("F1")};
         _container.Add(_p1);
         _p2 = new Parameter {Id = "P2", Name = "P2", Formula = new ConstantFormula().WithId("F2")};
         _container.Add(_p2);
      }

      protected override void Because()
      {
         sut.UnregisterAllIn(_container);
      }

      [Observation]
      public void should_unregister_any_constant_formula_formula()
      {
         A.CallTo(() => _objectbaseRepository.Unregister(_p2.Formula.Id)).MustHaveHappened();
      }

      [Observation]
      public void should_not_unregister_any_cachable_formula()
      {
         A.CallTo(() => _objectbaseRepository.Unregister(_p1.Formula.Id)).MustNotHaveHappened();
      }
   }

   internal class When_visiting_for_unregister_a_formula_useable_with_a_constant_formula : concern_for_MoBIUnregisterAllVisitor
   {
      private IUsingFormula _usingFormula;

      protected override void Context()
      {
         base.Context();
         _usingFormula = new Parameter {Id = "UF", Formula = A.Fake<ConstantFormula>().WithId("F")};
      }

      protected override void Because()
      {
         sut.UnregisterAllIn(_usingFormula);
      }

      [Observation]
      public void should_unregister_the_formula()
      {
         A.CallTo(() => _objectbaseRepository.Unregister(_usingFormula.Formula.Id)).MustHaveHappened();
      }
   }

   internal class When_visiting_for_unregister_a_model : concern_for_MoBIUnregisterAllVisitor
   {
      private IParameter _parameter;
      private IModel _model;

      protected override void Context()
      {
         base.Context();
         _model = new Model();
         _model.Root = new Container().WithName("Root");
         _parameter = new Parameter{Id = "UF",Formula = A.Fake<ConstantFormula>().WithId("F")};
         _model.Root.Add(_parameter);
      }

      protected override void Because()
      {
         sut.UnregisterAllIn(_model);
      }

      [Observation]
      public void should_unregister_the_formula()
      {
         A.CallTo(() => _objectbaseRepository.Unregister(_parameter.Formula.Id)).MustHaveHappened();
      }
   }
}