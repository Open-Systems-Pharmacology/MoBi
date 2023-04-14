using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using MoBi.Core;
using MoBi.Core.Serialization.Services;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ICreatePKSimMoleculePresenter : IDisposablePresenter
   {
      MoleculeBuilder CreateMolecule(MoleculeBuildingBlock moleculeBuildingBlock);
      void SetParameterUnit(ParameterDTO parameterDTO, Unit unit);
      void SetParameterValue(ParameterDTO parameterDTO, double newDisplayValue);
   }

   public class CreatePKSimMoleculePresenter : AbstractDisposablePresenter<ICreatePKSimMoleculeView, ICreatePKSimMoleculePresenter>, ICreatePKSimMoleculePresenter
   {
      private readonly IMoBiConfiguration _configuration;
      private readonly IParameterToParameterDTOMapper _parameterDTOMapper;
      private readonly IMoleculeBuilderToMoleculeBuilderDTOMapper _moleculeBuilderDTOMapper;
      private readonly ISerializationTask _serializationTask;
      private readonly IQuantityTask _quantityTask;
      private readonly IEditTaskFor<MoleculeBuilder> _editTask;
      private MoleculeBuilder _molecule;
      private MoleculeBuildingBlock _moleculeBuildingBlock;
      private MoleculeBuilderDTO _moleculeDTO;

      public CreatePKSimMoleculePresenter(ICreatePKSimMoleculeView view, IMoBiConfiguration configuration,
         IParameterToParameterDTOMapper parameterDTOMapper, IMoleculeBuilderToMoleculeBuilderDTOMapper moleculeBuilderDTOMapper,
         ISerializationTask serializationTask, IQuantityTask quantityTask, IEditTaskFor<MoleculeBuilder> editTask) : base(view)
      {
         _configuration = configuration;
         _parameterDTOMapper = parameterDTOMapper;
         _moleculeBuilderDTOMapper = moleculeBuilderDTOMapper;
         _serializationTask = serializationTask;
         _quantityTask = quantityTask;
         _editTask = editTask;
      }

      public MoleculeBuilder CreateMolecule(MoleculeBuildingBlock moleculeBuildingBlock)
      {
         _moleculeBuildingBlock = moleculeBuildingBlock;
         _molecule = loadMoleculeFromTemplate();
         _moleculeDTO = moleculeBuilderDTOFrom(_molecule);
         _view.BindTo(_moleculeDTO);
         _view.Display();

         if (_view.Canceled)
            return null;

         _molecule.Name = _moleculeDTO.Name;
         return _molecule;
      }

      private MoleculeBuilderDTO moleculeBuilderDTOFrom(MoleculeBuilder molecule)
      {
         var dto = _moleculeBuilderDTOMapper.MapFrom(molecule);
         dto.Parameters = allTemplateParametersFor(molecule);
         dto.AddUsedNames(_editTask.GetForbiddenNamesWithoutSelf(molecule, _moleculeBuildingBlock));
         return dto;
      }

      public void SetParameterUnit(ParameterDTO parameterDTO, Unit unit)
      {
         _quantityTask.SetQuantityDisplayUnit(parameterFrom(parameterDTO), unit, _moleculeBuildingBlock);
      }

      private IParameter parameterFrom(ParameterDTO parameterDTO)
      {
         return parameterDTO.Parameter;
      }

      public void SetParameterValue(ParameterDTO parameterDTO, double newDisplayValue)
      {
         _quantityTask.SetQuantityDisplayValue(parameterFrom(parameterDTO), newDisplayValue, _moleculeBuildingBlock);
      }

      private IEnumerable<ParameterDTO> allTemplateParametersFor(MoleculeBuilder molecule)
      {
         var templateParameter = molecule.Parameters.Where(isTemplateParameter).ToList();
         return templateParameter.MapAllUsing(_parameterDTOMapper).Cast<ParameterDTO>();
      }

      private bool isTemplateParameter(IParameter parameter)
      {
         return parameter.Visible && parameter.Formula.IsConstant() && double.IsNaN(parameter.Value);
      }

      private MoleculeBuilder loadMoleculeFromTemplate()
      {
         return _serializationTask.Load<MoleculeBuilder>(_configuration.StandardMoleculeTemplateFile, resetIds: true);
      }

      public override void ViewChanged()
      {
         base.ViewChanged();
         _view.OkEnabled = CanClose;
      }
   }
}