using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditApplicationMoleculeBuilderPresenter : IPresenterWithFormulaCache, ICanEditPropertiesPresenter, IEditPresenter<IApplicationMoleculeBuilder>, ICreatePresenter<IApplicationMoleculeBuilder>
   {
      ObjectPath SetObjectPath();
   }

   public class EditApplicationMoleculeBuilderPresenter : AbstractEntityEditPresenter<IEditApplicationMoleculeBuilderView, IEditApplicationMoleculeBuilderPresenter, IApplicationMoleculeBuilder>, IEditApplicationMoleculeBuilderPresenter
   {
      private IApplicationMoleculeBuilder _applicationMoleculeBuilder;
      private readonly IEditTaskFor<IApplicationMoleculeBuilder> _editTask;
      private readonly IApplicationMoleculeBuilderToApplicationMoleculeBuilderDTOMapper _applicationMoleculeMapper;
      private readonly IEditFormulaPresenter _editFormulaPresenter;
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaToDTOFormulaMapper;
      private readonly IMoBiContext _context;
      private readonly ISelectReferencePresenterAtApplicationBuilder _selectItemPresenter;

      public EditApplicationMoleculeBuilderPresenter(IEditApplicationMoleculeBuilderView view, IEditTaskFor<IApplicationMoleculeBuilder> editTask,
         IApplicationMoleculeBuilderToApplicationMoleculeBuilderDTOMapper applicationMoleculeMapper, IFormulaToFormulaBuilderDTOMapper formulaToDTOFormulaMapper,
         IEditFormulaPresenter editFormulaPresenter, IMoBiContext context, ISelectReferencePresenterAtApplicationBuilder selectItemPresenter)
         : base(view)
      {
         _editTask = editTask;
         _context = context;
         _selectItemPresenter = selectItemPresenter;
         _editFormulaPresenter = editFormulaPresenter;
         _editFormulaPresenter.ReferencePresenter = _selectItemPresenter;
         _view.SetFormulaView(_editFormulaPresenter.BaseView);
         _formulaToDTOFormulaMapper = formulaToDTOFormulaMapper;
         _applicationMoleculeMapper = applicationMoleculeMapper;
         AddSubPresenters(_editFormulaPresenter, _selectItemPresenter);
      }

      private IApplicationBuilder getParent(IApplicationMoleculeBuilder applicationMoleculeBuilder, IBuildingBlock buildingBlock)
      {
         var eventGroupBuilding = getEventGroupBuilding(buildingBlock);

         return eventGroupBuilding.Select(x => x.GetAllContainersAndSelf<IApplicationBuilder>()
            .FirstOrDefault(ab => ab.Molecules.Contains(applicationMoleculeBuilder)))
            .FirstOrDefault(applicationBuilder => applicationBuilder != null);
      }

      private IEventGroupBuildingBlock getEventGroupBuilding(IBuildingBlock buildingBlockWithFormulaCache)
      {
         return buildingBlockWithFormulaCache.DowncastTo<IEventGroupBuildingBlock>();
      }

      public override object Subject => _applicationMoleculeBuilder;

      public override void Edit(IApplicationMoleculeBuilder applicationMoleculeBuilder, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         _applicationMoleculeBuilder = applicationMoleculeBuilder;
         _editFormulaPresenter.Init(_applicationMoleculeBuilder, BuildingBlock);
         _selectItemPresenter.Init(_applicationMoleculeBuilder.ParentContainer, getEventGroupBuilding(BuildingBlock), applicationMoleculeBuilder);
         var dto = _applicationMoleculeMapper.MapFrom(_applicationMoleculeBuilder);
         _view.Show(dto);
      }

      public void SetPropertyValueFromView<T>(string propertyName, T newValue, T oldValue)
      {
         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(propertyName, newValue, oldValue, _applicationMoleculeBuilder, BuildingBlock).Run(_context));
      }

      public void RenameSubject()
      {
         _editTask.Rename(_applicationMoleculeBuilder, BuildingBlock);
      }

      public IEnumerable<FormulaBuilderDTO> GetFormulas()
      {
         return FormulaCache.MapAllUsing(_formulaToDTOFormulaMapper);
      }

      public IBuildingBlock BuildingBlock { get; set; }

      public IFormulaCache FormulaCache
      {
         get { return BuildingBlock.FormulaCache; }
      }

      public ObjectPath SetObjectPath()
      {
         using (var modalPresenter = IoC.Resolve<IModalPresenter>())
         {
            modalPresenter.Encapsulate(_selectItemPresenter);
            _selectItemPresenter.Init(_applicationMoleculeBuilder.ParentContainer, new[] {_applicationMoleculeBuilder.RootContainer}, _applicationMoleculeBuilder);

            if (modalPresenter.Show())
            {
               var path = _selectItemPresenter.GetSelection();
               SetPropertyValueFromView(MoBiReflectionHelper.PropertyName<IApplicationMoleculeBuilder>(x => x.RelativeContainerPath), path, _applicationMoleculeBuilder.RelativeContainerPath);
               return path;
            }
         }
         return _applicationMoleculeBuilder.RelativeContainerPath ?? new ObjectPath();
      }
   }
}