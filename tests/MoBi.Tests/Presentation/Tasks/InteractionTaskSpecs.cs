using System.Collections.Generic;
using System.IO;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Repositories;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

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

         sut = new InteractionTask(_serializationTask, _dialogCreator, _iconRepository, _nameCorrector, _cloneManagerForBuildingBlock, _adjustFormulasVisitor, _objectTypeResolver, _forbiddenNamesRetriever);
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
         _originalFormula1 = new ExplicitFormula().WithName("F1").WithId("X1");
         _originalFormula2 = new ExplicitFormula().WithName("F2").WithId("X2");
         _buildingBlockToClone = new MoleculeBuildingBlock();
         var clonedBuildingBlock = new MoleculeBuildingBlock();

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
         _result = sut.Clone(_buildingBlockToClone);
      }

      [Observation]
      public void should_also_add_the_formula_that_were_not_referenced_in_the_original_block()
      {
         _result.FormulaCache.Contains(_cloneFormula1).ShouldBeTrue();
         _result.FormulaCache.Contains(_cloneFormula2).ShouldBeTrue();
      }
   }

   public class When_saving_multiple_modules : concern_for_InteractionTask
   {
      private readonly Module _module1 = new Module().WithName("Module 1");
      private readonly Module _module2 = new Module().WithName("Module 2");
      private readonly string _path = "C:\\Path\\To\\Save\\";
      private readonly List<Module> _modulesToSave = new List<Module>();

      protected override void Context()
      {
         base.Context();
         _modulesToSave.Add(_module1);
         _modulesToSave.Add(_module2);
         A.CallTo(() => _dialogCreator.AskForFolder(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).Returns(_path);
      }

      protected override void Because()
      {
         sut.SaveMultiple(_modulesToSave);
      }

      [Observation]
      public void should_call_save_as_many_times_as_modules_with_correct_filePath()
      {
         A.CallTo(() => _serializationTask.SaveModelPart(_module1, Path.Combine(_path, buildFileNameFromModule(_module1)))).MustHaveHappenedOnceExactly();
         A.CallTo(() => _serializationTask.SaveModelPart(_module2, Path.Combine(_path, buildFileNameFromModule(_module2)))).MustHaveHappenedOnceExactly();
      }

      private string buildFileNameFromModule(Module module) =>
         $"{module.Name}{Constants.Filter.PKML_EXTENSION}";
   }

   public class When_saving_multiple_modules_and_cancel : concern_for_InteractionTask
   {
      private readonly Module _module1 = new Module().WithName("Module 1");
      private readonly Module _module2 = new Module().WithName("Module 2");
      private readonly List<Module> _modulesToSave = new List<Module>();

      protected override void Context()
      {
         base.Context();
         _modulesToSave.Add(_module1);
         _modulesToSave.Add(_module2);
         A.CallTo(() => _dialogCreator.AskForFolder(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).Returns(string.Empty);
      }

      protected override void Because()
      {
         sut.SaveMultiple(_modulesToSave);
      }

      [Observation]
      public void should_call_save_as_many_times_as_modules_with_correct_filePath()
      {
         A.CallTo(() => _serializationTask.SaveModelPart(_module1, A<string>.Ignored)).MustNotHaveHappened();
      }
   }
}