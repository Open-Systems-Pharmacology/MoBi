using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Extensions;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IMoleculeDependentBuilderPresenter : IPresenter<IMoleculeDependentBuilderView>, IEditPresenter<IMoleculeDependentBuilder>
   {
      void RemoveFromIncludeList(string molecule);
      void RemoveFromExcludeList(string molecule);
      void AddToIncludeList();
      void AddToExcludeList();
      void SetForAll(bool forAll);
      IBuildingBlock BuildingBlock { set; get; }
   }

   public class MoleculeDependentBuilderPresenter : AbstractEditPresenter<IMoleculeDependentBuilderView, IMoleculeDependentBuilderPresenter, IMoleculeDependentBuilder>, IMoleculeDependentBuilderPresenter
   {
      private readonly IMoBiContext _context;
      private IMoleculeDependentBuilder _moleculeDependentBuilder;
      private readonly IObjectBaseNamingTask _namingTask;
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      public IBuildingBlock BuildingBlock { get; set; }

      public MoleculeDependentBuilderPresenter(IMoleculeDependentBuilderView view, IMoBiContext context, IObjectBaseNamingTask namingTask, IBuildingBlockRepository buildingBlockRepository) : base(view)
      {
         _context = context;
         _namingTask = namingTask;
         _buildingBlockRepository = buildingBlockRepository;
      }

      public override void Edit(IMoleculeDependentBuilder objectToEdit)
      {
         _moleculeDependentBuilder = objectToEdit;
         _view.BuilderType = new ObjectTypeResolver().TypeFor(objectToEdit);
         refreshView();
      }

      private void refreshView()
      {
         _view.BindTo(_moleculeDependentBuilder.MoleculeList);
      }

      public override object Subject => _moleculeDependentBuilder;

      public void RemoveFromIncludeList(string molecule)
      {
         removeMolecule(_moleculeDependentBuilder.MoleculeNames(), molecule, () => new RemoveMoleculeNameFromIncludeCommand(_moleculeDependentBuilder, molecule, BuildingBlock));
      }

      public void RemoveFromExcludeList(string molecule)
      {
         removeMolecule(_moleculeDependentBuilder.MoleculeNamesToExclude(), molecule, () => new RemoveMoleculeNameFromExcludeCommand(_moleculeDependentBuilder, molecule, BuildingBlock));
      }

      private void removeMolecule(IEnumerable<string> availableMolecules, string molecule, Func<RemoveMoleculeNameCommand> removeItemCommand)
      {
         if (!availableMolecules.ContainsItem(molecule)) return;
         AddCommand(removeItemCommand().RunCommand(_context));
      }

      public void AddToIncludeList()
      {
         addMolecule(_moleculeDependentBuilder.MoleculeNames(), moleculeName => new AddMoleculeNameToIncludeCommand(_moleculeDependentBuilder, moleculeName, BuildingBlock));
      }

      public void AddToExcludeList()
      {
         addMolecule(_moleculeDependentBuilder.MoleculeNamesToExclude(), moleculeName => new AddMoleculeNameToExcludeCommand(_moleculeDependentBuilder, moleculeName, BuildingBlock));
      }

      private string newMoleculeName()
      {
         return _namingTask.NewName(AppConstants.Dialog.GetReactionMoleculeName,
            AppConstants.Captions.AddReactionMolecule,
            string.Empty, Enumerable.Empty<string>(), getMoleculeNames());
      }

      private void addMolecule(IEnumerable<string> availableMolecules, Func<string, AddMoleculeNameCommand> addItemCommand)
      {
         var moleculeName = newMoleculeName();
         if (string.IsNullOrEmpty(moleculeName)) return;
         if (availableMolecules.ContainsItem(moleculeName)) return;
         AddCommand(addItemCommand(moleculeName).RunCommand(_context));
      }

      public void SetForAll(bool forAll)
      {
         AddCommand(new SetForAllCommand(_moleculeDependentBuilder, forAll, BuildingBlock).RunCommand(_context));
      }

      public override void AddCommand(ICommand command)
      {
         base.AddCommand(command);
         refreshView();
      }

      private IEnumerable<string> getMoleculeNames()
      {
         var moleculeBB = _buildingBlockRepository.MoleculeBlockCollection;
         var moleculeNames = new HashSet<string>();
         moleculeBB.SelectMany(x => x).Each(molecule => moleculeNames.Add(molecule.Name));
         return moleculeNames.OrderBy(x => x);
      }
   }
}