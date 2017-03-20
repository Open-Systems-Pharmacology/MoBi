using FakeItEasy;

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using NHibernate.AdoNet;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;


namespace MoBi.Core.Commands
{
   public abstract class concern_for_EditParameterTagCommandAtSumFormulaSpecs : ContextSpecification<EditTagCommand<SumFormula>>
   {
      protected SumFormula _sumFormula;
      protected string _newTag;
      protected string _oldTag;
      protected IBuildingBlock _buildingBlock;

      protected override void Context()
      {
         _sumFormula = new SumFormula();
         _newTag = "New";
         _oldTag = "Old";
         _buildingBlock = A.Fake<IBuildingBlock>();
         sut = new EditTagCommand<SumFormula>(_newTag, _oldTag,_sumFormula,  _buildingBlock,x=>x.Criteria);
      }
   }

   class When_executing_am_EditParameterTagCommandAtSumFormula : concern_for_EditParameterTagCommandAtSumFormulaSpecs
   {
      private IMoBiContext _context;
      private MatchTagCondition _tagCondition;

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
         _tagCondition = new MatchTagCondition(_oldTag);
         _sumFormula.Criteria.Add(_tagCondition);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_should_have_changed_Tag_at_condition()
      {
         _tagCondition.Tag.ShouldBeEqualTo(_newTag);
      }
   }
}	