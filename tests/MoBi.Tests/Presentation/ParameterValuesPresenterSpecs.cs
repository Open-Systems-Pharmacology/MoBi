using System.Collections.Generic;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Mappers;
using MoBi.Core.Services;
using MoBi.HelpersForTests;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation
{
   internal class concern_for_ParameterValuesPresenter : ContextSpecification<ParameterValuesPresenter>
   {
      protected IParameterValuesView _parameterValuesView;
      protected IModalPresenter _modalPresenter;
      protected ParameterValuesBuildingBlock _parameterValuesBuildingBlock;
      private IParameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper _parameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper;
      protected IParameterValuesTask _parameterValuesTask;
      protected IDialogCreator _dialogCreator;
      private IPathAndValueEntityToDistributedParameterMapper _mapper;
      protected IParameterPathResolver _parameterPathResolver;
      private IParameterValuesCreator _parameterValuesCreator;

      protected override void Context()
      {
         _parameterValuesView = A.Fake<IParameterValuesView>();
         _modalPresenter = A.Fake<IModalPresenter>();
         _parameterValuesBuildingBlock = new ParameterValuesBuildingBlock();
         _mapper = A.Fake<IPathAndValueEntityToDistributedParameterMapper>();
         _parameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper = new ParameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper(new ParameterValueToParameterValueDTOMapper(A.Fake<IFormulaToValueFormulaDTOMapper>()), _mapper);
         _parameterValuesTask = A.Fake<IParameterValuesTask>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _parameterPathResolver = A.Fake<IParameterPathResolver>();
         _parameterValuesCreator = A.Fake<IParameterValuesCreator>();
         A.CallTo(() => _parameterValuesCreator.CreateEmptyStartValue(A<IDimension>._)).ReturnsLazily(() => new ParameterValue());
         sut = new ParameterValuesPresenter(
            _parameterValuesView,
            new ParameterValueToParameterValueDTOMapper(A.Fake<IFormulaToValueFormulaDTOMapper>()),
            _parameterValuesTask,
            _parameterValuesCreator,
            A.Fake<IMoBiContext>(),
            A.Fake<IDisplayUnitRetriever>(),
            A.Fake<IFormulaToValueFormulaDTOMapper>(),
            A.Fake<IParameterValueDistributedPathAndValueEntityPresenter>(),
            _parameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper,
            A.Fake<IDimensionFactory>(),
            A.Fake<IViewItemContextMenuFactory>(),
            _dialogCreator,
            _parameterPathResolver);

         sut.Edit(_parameterValuesBuildingBlock);
      }
   }

   internal class When_creating_new_parameter_values : concern_for_ParameterValuesPresenter
   {
      private IReadOnlyList<ObjectPath> _addedPaths;
      protected ObjectPath _path1;
      protected ObjectPath _path2;
      protected ObjectPath _path3;

      protected override void Context()
      {
         base.Context();
         _path1 = new ObjectPath("path", "1");
         _path2 = new ObjectPath("path", "2");
         _path3 = new ObjectPath("path", "3");
         _addedPaths = new List<ObjectPath>
         {
            _path1,
            _path2,
            _path3
         };

         A.CallTo(() => _modalPresenter.Show(null)).Returns(true);
         A.CallTo(() => _parameterValuesTask.GetNewPaths()).Returns(_addedPaths);
      }

      protected override void Because()
      {
         sut.AddNewParameterValues();
      }
   }

   internal class When_creating_parameter_values_and_none_are_in_the_building_block : When_creating_new_parameter_values
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _parameterValuesTask.SetFullPath(A<ParameterValue>._, _path1, _parameterValuesBuildingBlock)).Invokes(() => addPVToBuildingBlock(_path1));
      }

      private void addPVToBuildingBlock(ObjectPath path)
      {
         _parameterValuesBuildingBlock.Add(new ParameterValue(){Path = path});
      }

      [Observation]
      public void parameters_should_be_added_for_all_selected_paths()
      {
         A.CallTo(() => _parameterValuesTask.SetFullPath(A<ParameterValue>._, _path1, _parameterValuesBuildingBlock)).MustHaveHappened();
         A.CallTo(() => _parameterValuesTask.SetFullPath(A<ParameterValue>._, _path2, _parameterValuesBuildingBlock)).MustHaveHappened();
         A.CallTo(() => _parameterValuesTask.SetFullPath(A<ParameterValue>._, _path3, _parameterValuesBuildingBlock)).MustHaveHappened();
      }

      [Observation]
      public void the_view_should_have_columns_initialized()
      {
         A.CallTo(() => _parameterValuesView.InitializePathColumns()).MustHaveHappenedTwiceExactly();
      }
   }

   internal class When_creating_new_parameter_values_and_some_are_already_in_the_building_block : When_creating_new_parameter_values
   {
      protected override void Context()
      {
         base.Context();
         _parameterValuesBuildingBlock.Add(new ParameterValue
         {
            Path = _path1
         });
      }

      [Observation]
      public void the_dialog_creator_informs_the_user_of_the_paths_not_added()
      {
         A.CallTo(() => _dialogCreator.MessageBoxInfo(AppConstants.Captions.BuildingBlockAlreadyContains(new[] { _path1.PathAsString }))).MustHaveHappened();
      }

      [Observation]
      public void parameters_should_be_added_for_all_selected_paths()
      {
         A.CallTo(() => _parameterValuesTask.SetFullPath(A<ParameterValue>._, _path1, _parameterValuesBuildingBlock)).MustNotHaveHappened();
         A.CallTo(() => _parameterValuesTask.SetFullPath(A<ParameterValue>._, _path2, _parameterValuesBuildingBlock)).MustHaveHappened();
         A.CallTo(() => _parameterValuesTask.SetFullPath(A<ParameterValue>._, _path3, _parameterValuesBuildingBlock)).MustHaveHappened();
      }
   }

   internal class When_creating_new_parameter_values_and_some_paths_can_be_resolved : When_creating_new_parameter_values
   {
      private Parameter _resolvedParameter;
      private ParameterValue _parameterValueForPath1;

      protected override void Context()
      {
         base.Context();
         _resolvedParameter = new Parameter { Dimension = DimensionFactoryForSpecs.MassDimension };
         _resolvedParameter.DisplayUnit = _resolvedParameter.Dimension.Unit("mg");

         A.CallTo(() => _parameterPathResolver.Resolve(_path1)).Returns(_resolvedParameter);
         A.CallTo(() => _parameterPathResolver.Resolve(_path2)).Returns(null);
         A.CallTo(() => _parameterPathResolver.Resolve(_path3)).Returns(null);
         A.CallTo(() => _parameterValuesTask.SetFullPath(A<ParameterValue>._, _path1, _parameterValuesBuildingBlock)).Invokes(x => _parameterValueForPath1 = x.GetArgument<ParameterValue>(0));
      }

      [Observation]
      public void the_dimension_and_display_unit_of_the_resolved_parameter_should_be_used()
      {
         _parameterValueForPath1.Dimension.ShouldBeEqualTo(_resolvedParameter.Dimension);
         _parameterValueForPath1.DisplayUnit.ShouldBeEqualTo(_resolvedParameter.DisplayUnit);
      }

      [Observation]
      public void the_user_should_be_asked_to_check_the_dimension_of_the_parameters_that_could_not_be_resolved()
      {
         A.CallTo(() => _dialogCreator.MessageBoxInfo(AppConstants.Warnings.CheckDimensionOfAddedParameters(new[] { _path2.PathAsString, _path3.PathAsString }))).MustHaveHappened();
      }
   }

   internal class When_creating_new_parameter_values_and_all_paths_can_be_resolved : When_creating_new_parameter_values
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _parameterPathResolver.Resolve(A<ObjectPath>._)).Returns(new Parameter { Dimension = DimensionFactoryForSpecs.MassDimension });
      }

      [Observation]
      public void the_user_should_not_be_asked_to_check_the_dimensions()
      {
         A.CallTo(() => _dialogCreator.MessageBoxInfo(A<string>._)).MustNotHaveHappened();
      }
   }
}