using OSPSuite.Core.Services;
using MoBi.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Settings;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTaskContext
   {
      IMoBiContext Context { get; }
      IInteractionTask InteractionTask { get; }
      IMoBiApplicationController ApplicationController { get; }
      T Active<T>() where T : class;
      IUserSettings UserSettings { get; }
      IDialogCreator DialogCreator { get; }
      IMoBiFormulaTask MoBiFormulaTask { get; }
      ICheckNameVisitor CheckNamesVisitor { get; }
      Unit DisplayUnitFor(IWithDimension withDimension);
      Unit DisplayUnitFor(IDimension dimension);
      IDimension DimensionByName(string dimensionName);

      /// <summary>
      ///    Cancels the commands and returns an empty command
      /// </summary>
      /// <param name="command"></param>
      IMoBiCommand CancelCommand(IMoBiCommand command);

      /// <summary>
      ///    Gets the string describing Type T
      /// </summary>
      /// <typeparam name="T">The type being described</typeparam>
      /// <param name="objectToGetTypeFor">The object whose type is being described</param>
      string GetTypeFor<T>(T objectToGetTypeFor) where T : class;

      /// <summary>
      ///    Gets the string describing Type T
      /// </summary>
      /// <typeparam name="T">The type being described</typeparam>
      string GetTypeFor<T>();

      void UpdateTemplateDirectories();
   }

   public class InteractionTaskContext : IInteractionTaskContext
   {
      private readonly ICommandTask _commandTask;
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly IMoBiConfiguration _configuration;
      private readonly DirectoryMapSettings _directoryMapSettings;
      public IMoBiContext Context { get; }
      public IMoBiApplicationController ApplicationController { get; }
      public IInteractionTask InteractionTask { get; }
      public IActiveSubjectRetriever ActiveSubjectRetriever { get; }
      public IUserSettings UserSettings { get; }
      public IDialogCreator DialogCreator { get; }
      public IMoBiFormulaTask MoBiFormulaTask { get; }
      public ICheckNameVisitor CheckNamesVisitor { get; }
      public IDisplayUnitRetriever DisplayUnitRetriever { get; }

      public InteractionTaskContext(IMoBiContext context, IMoBiApplicationController applicationController,
         IInteractionTask interactionTask, IActiveSubjectRetriever activeSubjectRetriever, IUserSettings userSettings,
         IDisplayUnitRetriever displayUnitRetriever, IDialogCreator dialogCreator,
         ICommandTask commandTask, IObjectTypeResolver objectTypeResolver, IMoBiFormulaTask moBiFormulaTask,
         IMoBiConfiguration configuration, DirectoryMapSettings directoryMapSettings, ICheckNameVisitor checkNamesVisitor)
      {
         DialogCreator = dialogCreator;
         Context = context;
         ApplicationController = applicationController;
         InteractionTask = interactionTask;
         ActiveSubjectRetriever = activeSubjectRetriever;
         UserSettings = userSettings;
         DisplayUnitRetriever = displayUnitRetriever;
         _commandTask = commandTask;
         _objectTypeResolver = objectTypeResolver;
         _configuration = configuration;
         _directoryMapSettings = directoryMapSettings;
         MoBiFormulaTask = moBiFormulaTask;
         CheckNamesVisitor = checkNamesVisitor;
      }

      public Unit DisplayUnitFor(IWithDimension withDimension)
      {
         return DisplayUnitRetriever.PreferredUnitFor(withDimension);
      }

      public Unit DisplayUnitFor(IDimension dimension)
      {
         return DisplayUnitRetriever.PreferredUnitFor(dimension);
      }

      /// <summary>
      ///    Cancels the commands and returns an empty command
      /// </summary>
      /// <param name="command"></param>
      public IMoBiCommand CancelCommand(IMoBiCommand command)
      {
         _commandTask.ResetChanges(command, Context);
         return new MoBiEmptyCommand();
      }

      public string GetTypeFor<T>(T objectToGetTypeFor) where T : class
      {
         return _objectTypeResolver.TypeFor(objectToGetTypeFor);
      }

      public string GetTypeFor<T>()
      {
         return _objectTypeResolver.TypeFor<T>();
      }

      public void UpdateTemplateDirectories()
      {
         _directoryMapSettings.AddUsedDirectory(Constants.DirectoryKey.TEMPLATE, _configuration.TemplateFolder);
      }

      public IDimension DimensionByName(string dimensionName)
      {
         return Context.DimensionFactory.Dimension(dimensionName);
      }

      public T Active<T>() where T : class
      {
         return ActiveSubjectRetriever.Active<T>();
      }
   }
}