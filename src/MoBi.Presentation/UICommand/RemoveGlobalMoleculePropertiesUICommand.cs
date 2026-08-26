using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class RemoveGlobalMoleculePropertiesUICommand : ObjectUICommand<IContainer>
   {
      private readonly IMoBiContext _context;
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;
      private readonly IDialogCreator _dialogCreator;

      public RemoveGlobalMoleculePropertiesUICommand(IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever, IDialogCreator dialogCreator)
      {
         _context = context;
         _activeSubjectRetriever = activeSubjectRetriever;
         _dialogCreator = dialogCreator;
      }

      protected override void PerformExecute()
      {
         var spatialStructure = _activeSubjectRetriever.Active<MoBiSpatialStructure>();
         if (!Equals(spatialStructure.GlobalMoleculeDependentProperties, Subject))
            return;

         if (_dialogCreator.MessageBoxYesNo(AppConstants.Dialog.Remove(ObjectTypes.Container, Subject.Name, spatialStructure.Name)) != ViewResult.Yes)
            return;

         _context.AddToHistory(new RemoveGlobalMoleculePropertiesCommand(spatialStructure, Subject).RunCommand(_context));
      }
   }
}