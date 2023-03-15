using System.Collections.Generic;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.Presenter
{
   public interface IEditTransporterMoleculeContainerPresenter : IEditPresenterWithParameters<TransporterMoleculeContainer>, ICanEditPropertiesPresenter, IPresenterWithFormulaCache
   {
      void ChangeTransportName();
   }

   public class EditTransporterMoleculeContainerPresenter : AbstractEntityEditPresenter<IEditActiveTransportBuilderContainerView,
                                                               IEditTransporterMoleculeContainerPresenter, TransporterMoleculeContainer>, IEditTransporterMoleculeContainerPresenter
   {
      private TransporterMoleculeContainer _transporterMoleculeContainer;
      private readonly IEditTasksForTransporterMoleculeContainer _editTasks;
      private readonly IEditParametersInContainerPresenter _parameterBuilderPresenter;
      private readonly ITransporterMoleculeContainerToTransporterMoleculeContainerDTOMapper _transporterMoleculeContainerMapper;
      private readonly IMoBiContext _context;

      public EditTransporterMoleculeContainerPresenter(IEditActiveTransportBuilderContainerView view,
         IEditTasksForTransporterMoleculeContainer editTasks,
         IEditParametersInContainerPresenter parameterBuilderPresenter, 
         ITransporterMoleculeContainerToTransporterMoleculeContainerDTOMapper transporterMoleculeContainerMapper, IMoBiContext context)
         : base(view)
      {
         _editTasks = editTasks;
         _context = context;
         _parameterBuilderPresenter = parameterBuilderPresenter;
         AddSubPresenters(_parameterBuilderPresenter);
         _view.SetParameterView(parameterBuilderPresenter.BaseView);
         _transporterMoleculeContainerMapper = transporterMoleculeContainerMapper;
      }

      public override void Edit(TransporterMoleculeContainer transporterMoleculeContainer, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         _transporterMoleculeContainer = transporterMoleculeContainer;
         _parameterBuilderPresenter.Edit(transporterMoleculeContainer);

         var dto = _transporterMoleculeContainerMapper.MapFrom(_transporterMoleculeContainer);
         dto.AddUsedNames(_editTasks.GetForbiddenNamesWithoutSelf(transporterMoleculeContainer, existingObjectsInParent));
         _view.Show(dto);
      }

      public override object Subject
      {
         get { return _transporterMoleculeContainer; }
      }

      public void SelectParameter(IParameter parameter)
      {
         _view.ShowParameters();
         _parameterBuilderPresenter.Select(parameter);
      }

      public void SetPropertyValueFromView<T>(string propertyName, T newValue, T oldValue)
      {
         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(propertyName, newValue, oldValue,_transporterMoleculeContainer, BuildingBlock).Run(_context));
      }

      public void RenameSubject()
      {
         _editTasks.Rename(_transporterMoleculeContainer, BuildingBlock);
      }

      public IEnumerable<FormulaBuilderDTO> GetFormulas()
      {
         return _parameterBuilderPresenter.GetFormulas();
      }

      public IBuildingBlock BuildingBlock
      {
         get { return _parameterBuilderPresenter.BuildingBlock; }
         set { _parameterBuilderPresenter.BuildingBlock = value; }
      }

      public IFormulaCache FormulaCache
      {
         get { return BuildingBlock.FormulaCache; }
      }

      public void ChangeTransportName()
      {
         _editTasks.ChangeTransportName(_transporterMoleculeContainer, BuildingBlock);
         Edit(_transporterMoleculeContainer);
      }

     
   }
}