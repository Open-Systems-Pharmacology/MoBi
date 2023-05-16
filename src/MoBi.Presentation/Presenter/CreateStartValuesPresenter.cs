using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Assets;

namespace MoBi.Presentation.Presenter
{
   public interface ICreateStartValuesPresenter : IDisposablePresenter
   {
      IEnumerable<MoleculeBuildingBlock> GetMolecules();
      IEnumerable<MoBiSpatialStructure> GetSpatialStructures();
   }

   public interface ICreateStartValuesPresenter<T> : ICreateStartValuesPresenter
   {
      T Create();
   }

   public abstract class CreateStartValuesPresenter<T> : AbstractDisposablePresenter<ICreateStartValuesView, ICreateStartValuesPresenter>, ICreateStartValuesPresenter<T> where T : class
   {
      protected readonly IBuildingBlockRepository _buildingBlockRepository;
      protected readonly List<string> _unallowedNames;

      protected CreateStartValuesPresenter(ICreateStartValuesView view, IBuildingBlockRepository buildingBlockRepository) : base(view)
      {
         _buildingBlockRepository = buildingBlockRepository;
         _unallowedNames = new List<string>();
      }

      public T Create()
      {
         var dto = createDto(String.Empty, GetMolecules().First(), GetSpatialStructures().First());
         _view.Show(dto);
         _view.Display();
         if (_view.Canceled) return null;
         return CreateStartValuesFromDTO(dto);
      }

      protected abstract T CreateStartValuesFromDTO(StartValuesDTO dto);

      private StartValuesDTO createDto(string name, MoleculeBuildingBlock moleculeBuildingBlock, MoBiSpatialStructure spatialStructure)
      {
         var dto = new StartValuesDTO {Name = name, Molecules = moleculeBuildingBlock, SpatialStructure = spatialStructure};
         dto.AddUsedNames(AppConstants.UnallowedNames);
         dto.AddUsedNames(_unallowedNames);
         return dto;
      }

      public IEnumerable<MoleculeBuildingBlock> GetMolecules()
      {
         return _buildingBlockRepository.MoleculeBlockCollection;
      }

      public IEnumerable<MoBiSpatialStructure> GetSpatialStructures()
      {
         return _buildingBlockRepository.SpatialStructureCollection;
      }
   }

   public class CreateInitialConditionsPresenter : CreateStartValuesPresenter<InitialConditionsBuildingBlock>
   {
      private readonly IInitialConditionsCreator _initialConditionsCreator;

      public CreateInitialConditionsPresenter(ICreateStartValuesView view, IBuildingBlockRepository buildingBlockRepository, IInitialConditionsCreator initialConditionsCreator) : base(view, buildingBlockRepository)
      {
         _initialConditionsCreator = initialConditionsCreator;
         _unallowedNames.AddRange(_buildingBlockRepository.InitialConditionBlockCollection.Select(x => x.Name));
         view.ApplicationIcon = ApplicationIcons.InitialConditions;
         view.Caption = AppConstants.Captions.NewInitialConditions;
      }

      protected override InitialConditionsBuildingBlock CreateStartValuesFromDTO(StartValuesDTO dto)
      {
         return _initialConditionsCreator.CreateFrom(dto.SpatialStructure, dto.Molecules).WithName(dto.Name);
      }
   }

   public class CreateParameterValuesPresenter : CreateStartValuesPresenter<ParameterValuesBuildingBlock>
   {
      private readonly IParameterValuesCreator _parameterValuesCreator;

      public CreateParameterValuesPresenter(ICreateStartValuesView view, IBuildingBlockRepository buildingBlockRepository, IParameterValuesCreator parameterValuesCreator)
         : base(view, buildingBlockRepository)
      {
         _parameterValuesCreator = parameterValuesCreator;
         _unallowedNames.AddRange(_buildingBlockRepository.ParametersValueBlockCollection.Select(x => x.Name));
         view.ApplicationIcon = ApplicationIcons.ParameterValues;
         view.Caption = AppConstants.Captions.NewParameterValues;
      }

      protected override ParameterValuesBuildingBlock CreateStartValuesFromDTO(StartValuesDTO dto)
      {
         // return _startValuesCreator.CreateFrom(dto.SpatialStructure, dto.Molecules).WithName(dto.Name);
         return new ParameterValuesBuildingBlock();
      }
   }
}