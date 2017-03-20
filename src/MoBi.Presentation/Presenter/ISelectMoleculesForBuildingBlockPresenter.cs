using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectMoleculesForBuildingBlockPresenter : IDisposablePresenter
   {
      IEnumerable<IMoleculeBuildingBlock> MoleculeBuildinBlocks { set; }
      NewMoleculeBuildingBlockDescription Selected { get; }
      bool AskForCreation();
      string CheckSelection();
   }

   internal class SelectMoleculesForBuildingBlockPresenter : AbstractDisposablePresenter<ISelectMoleculesForBuildingBlockView, ISelectMoleculesForBuildingBlockPresenter>, ISelectMoleculesForBuildingBlockPresenter
   {
      private IEnumerable<IMoleculeBuildingBlock> _moleculeBuidingBlocks;
      private NewMoleculeBuildingBlockDescription _selected;
      private IList<DTOMoleculeSelection> _dtoMolecules;

      public SelectMoleculesForBuildingBlockPresenter(ISelectMoleculesForBuildingBlockView view) : base(view)
      {
      }

      public IEnumerable<IMoleculeBuildingBlock> MoleculeBuildinBlocks
      {
         set { _moleculeBuidingBlocks = value; }
      }

      public NewMoleculeBuildingBlockDescription Selected
      {
         get { return _selected; }
      }

      public bool AskForCreation()
      {
         _dtoMolecules = new List<DTOMoleculeSelection>();
         _moleculeBuidingBlocks.Each(bb => addMolecules(bb, _dtoMolecules));
         var selectDTO = new SelectMoleculesDTO
            {
               Molecules = _dtoMolecules
            };
         selectDTO.AddUsedNames(_moleculeBuidingBlocks.Select(x => x.Name));
         _view.Show(selectDTO);
         _view.Display();

         var canceled = _view.Canceled;
         if (canceled)
         {
            _selected = null;
         }
         else
         {
            _selected = new NewMoleculeBuildingBlockDescription
               {
                  Name = selectDTO.Name,
                  Molecules = getSelected(_dtoMolecules)
               };
         }
         return !canceled;
      }

      public string CheckSelection()
      {
         if (selectionOK(_dtoMolecules))
            return String.Empty;

         return AppConstants.Exceptions.SelectUniqueMolecules;
      }

      private bool selectionOK(IEnumerable<DTOMoleculeSelection> dtoMolecules)
      {
         var selected = dtoMolecules.Where(dto => dto.Selected);
         var uniqueName = selected.Select(x => x.Molecule).Distinct();
         return selected.Count().Equals(uniqueName.Count());
      }

      private IEnumerable<IMoleculeBuilder> getSelected(IEnumerable<DTOMoleculeSelection> dtoMoleculeSelections)
      {
         return dtoMoleculeSelections.Where(dto => dto.Selected).Select(dto => dto.MoleculeBuilder).ToList();
      }

      private void addMolecules(IMoleculeBuildingBlock moleculeBuilders, IList<DTOMoleculeSelection> dtos)
      {
         moleculeBuilders.Each(x => dtos.Add(new DTOMoleculeSelection
            {
               BuildingBlock = moleculeBuilders.Name,
               Molecule = x.Name,
               MoleculeBuilder = x
            }));
      }
   }

   public class SelectMoleculesDTO : ObjectBaseDTO
   {
      public IList<DTOMoleculeSelection> Molecules { get; set; }
   }

   public class DTOMoleculeSelection
   {
      public string BuildingBlock { get; set; }

      public string Molecule { get; set; }

      public IMoleculeBuilder MoleculeBuilder { get; set; }

      public bool Selected { get; set; }
   }
}