using System.Collections.Generic;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class ObjectBaseNamingTask : IObjectBaseNamingTask
   {
      private readonly IMoBiApplicationController _applicationController;
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly IDialogCreator _dialogCreator;

      public ObjectBaseNamingTask(IMoBiApplicationController applicationController, IObjectTypeResolver objectTypeResolver, IDialogCreator dialogCreator)
      {
         _applicationController = applicationController;
         _objectTypeResolver = objectTypeResolver;
         _dialogCreator = dialogCreator;
      }

      public string RenameFor(IObjectBase objectToRename, IReadOnlyList<string> forbiddenNames)
      {
         using (var renameObjectPresenter = _applicationController.Start<IRenameObjectPresenter>())
         {
            return renameObjectPresenter.NewNameFrom(objectToRename, forbiddenNames, _objectTypeResolver.TypeFor(objectToRename));
         }
      }

      public string NewName(
         string prompt,
         string title,
         string defaultValue = null,
         IEnumerable<string> forbiddenValues = null,
         IEnumerable<string> predefinedValues = null,
         string iconName = null)
      {
         return _dialogCreator.AskForInput(prompt, title, defaultValue, forbiddenValues, predefinedValues, iconName);
      }
   }
}