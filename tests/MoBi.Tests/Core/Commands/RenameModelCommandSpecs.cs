using FakeItEasy;

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;


namespace MoBi.Core.Commands
{
   public abstract class concern_for_RenameModelCommandSpecs : ContextSpecification<RenameModelCommand>
   {
      protected IModel _model ;
      protected string _newName= "new";
      protected string _oldName ="old";
      protected FormulaUsablePath _changedObjectPath;
      protected FormulaUsablePath _unchangedPath;
      protected FormulaUsablePath _rhsPath;
      protected FormulaUsablePath _neighborhoodPath;

      protected override void Context()
      {
         _model = new Model().WithName(_oldName);
         _model.Root = new Container().WithName(_oldName);
         var explicitFormula = new ExplicitFormula("A+B");
         _unchangedPath = new FormulaUsablePath(new[]{"A","B"});
         _changedObjectPath = new FormulaUsablePath(new[]{_oldName,"A"});
         var rhsFormula = new ExplicitFormula("-C");
         _rhsPath = new FormulaUsablePath(new []{_oldName,"C"});
         rhsFormula.AddObjectPath(_rhsPath);
         explicitFormula.AddObjectPath(_changedObjectPath);
         explicitFormula.AddObjectPath(_unchangedPath);
         var parameter = new Parameter().WithName("P1").WithFormula(explicitFormula).WithRHS(rhsFormula);
         _model.Root.Add(parameter);
         _model.Neighborhoods = new Container().WithName(Constants.NEIGHBORHOODS);
         var neighborhood = new Neighborhood().WithName("BLA");
         var neighborhoodFormula = new ExplicitFormula("u");
         _neighborhoodPath = new FormulaUsablePath(new[]{_oldName,_oldName,"u"});
         neighborhoodFormula.AddObjectPath(_neighborhoodPath);
         var neighborhoodParameter = new Parameter().WithFormula(neighborhoodFormula);
         neighborhood.Add(neighborhoodParameter);
         _model.Neighborhoods.Add(neighborhood);
         sut = new RenameModelCommand(_model,_newName);
      }
   }

   class When_executing_rename_model_command : concern_for_RenameModelCommandSpecs
   {
      private IMoBiContext _context;

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_change_Model_Name()
      {
         _model.Name.ShouldBeEqualTo(_newName);
      }

      [Observation]
      public void should_change_root_name()
      {
         _model.Root.Name.ShouldBeEqualTo(_newName);
      }

      [Observation]
      public void should_change_changed_Path()
      {
         _changedObjectPath.ShouldOnlyContainInOrder(_newName,"A");
      }

      [Observation]
      public void should_leave_unchanged_path_unchanged()
      {
         _unchangedPath.ShouldOnlyContainInOrder("A","B");
      }

      [Observation]
      public void should_change_rhs_path()
      {
         _rhsPath.ShouldOnlyContainInOrder(_newName,"C");
      }

      [Observation]
      public void should_only_change_first_element_of_neighborhood_path()
      {
         _neighborhoodPath.ShouldOnlyContainInOrder(_newName,_oldName,"u");
      }
   }

}	