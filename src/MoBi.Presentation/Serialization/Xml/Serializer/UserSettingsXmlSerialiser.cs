using System.Xml.Linq;
using MoBi.Presentation.Settings;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Serializer.Xml;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.Serialization.Xml.Serializer
{
   internal class UserSettingsXmlSerialiser : XmlSerializer<IUserSettings, SerializationContext>, IOSPSuiteXmlSerializer
   {
      public override void PerformMapping()
      {
         Map(x => x.ChartEditorLayout);
         Map(x => x.CheckDimensions);
         Map(x => x.ActiveSkin);
         Map(x => x.IconSizeGeneral);
         Map(x => x.MRUListItemCount);
         Map(x => x.RenameDependentObjectsDefault);
         Map(x => x.DiagramOptions);
         Map(x => x.ForceLayoutConfigutation);
         Map(x => x.ChartOptions);
         Map(x => x.MainViewLayout);
         Map(x => x.RibbonLayout);
         Map(x => x.ParameterDefaultDimension);
         Map(x => x.LayoutVersion);
         Map(x => x.ShowAdvancedParameters);
         Map(x => x.VisibleNotification);
         Map(x => x.DecimalPlace);
         Map(x => x.ShowCannotCalcErrors);
         Map(x => x.ShowPKSimDimensionProblemWarnings);
         Map(x => x.ShowPKSimObserverMessages);
         Map(x => x.ShowUnresolvedEndosomeMessagesForInitialConditions);
         Map(x => x.CheckRules);
         Map(x => x.CheckCircularReference);
         Map(x => x.GroupParameters);
         Map(x => x.DisplayUnits);
         Map(x => x.ComparerSettings);
         Map(x => x.JournalPageEditorSettings);
         Map(x => x.ParameterIdentificationFeedbackEditorSettings);
         Map(x => x.MergeConflictViewSettings);
         Map(x => x.ColorGroupObservedDataFromSameFolder);
         MapEnumerable(x => x.ProjectFiles, x => x.ProjectFiles.Add);
         MapEnumerable(x => x.UsedDirectories, x => x.DirectoryMapSettings.AddUsedDirectory);
      }

      public override IUserSettings CreateObject(XElement element, SerializationContext serializationContext)
      {
         return IoC.Resolve<IUserSettings>();
      }
   }
}