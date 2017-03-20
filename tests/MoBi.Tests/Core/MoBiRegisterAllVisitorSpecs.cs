using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Core.Domain.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core
{
   public abstract class concern_for_MoBiRegisterAllVisitor : ContextSpecification<RegisterAllVisitor>
   {
      protected IWithIdRepository _repository;

      protected override void Context()
      {
         _repository = A.Fake<IWithIdRepository>();
         sut = new RegisterAllVisitor(_repository);
      }
   }

   internal class When_visiting_an_formula_useable_with_a_not_constant_formula : concern_for_MoBiRegisterAllVisitor
   {
      private IUsingFormula _usingFormula;

      protected override void Context()
      {
         base.Context();
         _usingFormula = A.Fake<IUsingFormula>();
         _usingFormula.Id = "UF";
         _usingFormula.Formula = A.Fake<ExplicitFormula>();
      }

      protected override void Because()
      {
         sut.Visit(_usingFormula);
      }

      [Observation]
      public void should_register_the_formula()
      {
         A.CallTo(() => _repository.Register(_usingFormula.Formula)).MustHaveHappened();
      }
   }

   internal class When_visiting_an_formula_useable_with_a_constant_formula : concern_for_MoBiRegisterAllVisitor
   {
      private IUsingFormula _usingFormula;

      protected override void Context()
      {
         base.Context();
         _usingFormula = A.Fake<IUsingFormula>();
         _usingFormula.Id = "UF";
         _usingFormula.Formula = A.Fake<ConstantFormula>();
      }

      protected override void Because()
      {
         sut.Visit(_usingFormula);
      }

      [Observation]
      public void should_register_the_formula()
      {
         A.CallTo(() => _repository.Register(_usingFormula.Formula)).MustHaveHappened();
      }
   }
}