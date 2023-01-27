using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Services;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Repositories;
using MoBi.Core.Serialization.Services;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_InteractionTask : ContextSpecification<IInteractionTask>
   {
      protected ISerializationTask _serializationTask;
      protected IDialogCreator _dialogCreator;
      protected IIconRepository _iconRepository;
      protected INameCorrector _nameCorrector;
      protected ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;
      protected IAdjustFormulasVisitor _adjustFormulasVisitor;
      protected IObjectTypeResolver _objectTypeResolver;
      protected IForbiddenNamesRetriever _forbiddenNamesRetriever;
      protected IMoBiApplicationController _moBiApplicationController;
      private ICheckNameVisitor _checkNamesVisitor;
      private IMoBiContext _moBiContext;

      protected override void Context()
      {
         _serializationTask = A.Fake<ISerializationTask>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _iconRepository = A.Fake<IIconRepository>();
         _nameCorrector = A.Fake<INameCorrector>();
         _cloneManagerForBuildingBlock = A.Fake<ICloneManagerForBuildingBlock>();
         _adjustFormulasVisitor = A.Fake<IAdjustFormulasVisitor>();
         _objectTypeResolver = A.Fake<IObjectTypeResolver>();
         _forbiddenNamesRetriever = A.Fake<IForbiddenNamesRetriever>();
         _moBiApplicationController = A.Fake<IMoBiApplicationController>();
         _moBiContext = A.Fake<IMoBiContext>();
         _checkNamesVisitor = A.Fake<ICheckNameVisitor>();

         sut = new InteractionTask(_serializationTask,_dialogCreator, _iconRepository, _nameCorrector, _cloneManagerForBuildingBlock, _adjustFormulasVisitor, _objectTypeResolver, _forbiddenNamesRetriever);
      }
   }

   public class When_cloning_a_building_block : concern_for_InteractionTask
   {
      private IBuildingBlock _buildingBlockToClone;
      
      private IFormula _originalFormula1;
      private IFormula _originalFormula2;
      private IFormula _cloneFormula1;
      private IFormula _cloneFormula2;
      private IBuildingBlock _result;

      protected override void Context()
      {
         base.Context();
         _originalFormula1 =new ExplicitFormula().WithName("F1").WithId("X1");
         _originalFormula2 = new ExplicitFormula().WithName("F2").WithId("X2");
         _buildingBlockToClone = new MoleculeBuildingBlock();
         var clonedBuildingBlock= new MoleculeBuildingBlock();

         _buildingBlockToClone.AddFormula(_originalFormula1);
         _buildingBlockToClone.AddFormula(_originalFormula2);

         _cloneFormula1 = new ExplicitFormula().WithName("F1").WithId("X11");
         _cloneFormula2 = new ExplicitFormula().WithName("F2").WithId("X22");

         A.CallTo(() => _cloneManagerForBuildingBlock.Clone(_originalFormula2)).Returns(_cloneFormula2);
         A.CallTo(() => _cloneManagerForBuildingBlock.Clone(_buildingBlockToClone, A<IFormulaCache>._))
            .Invokes(x =>
            {
               var formulaCache = x.GetArgument<IFormulaCache>(1);
               formulaCache.Add(_cloneFormula1);
            })
            .Returns(clonedBuildingBlock);
      }

      protected override void Because()
      {
         _result= sut.Clone(_buildingBlockToClone);
      }

      [Observation]
      public void should_also_add_the_formula_that_were_not_referenced_in_the_original_block()
      {
         _result.FormulaCache.Contains(_cloneFormula1).ShouldBeTrue();  
         _result.FormulaCache.Contains(_cloneFormula2).ShouldBeTrue();  
      }
   }
}	