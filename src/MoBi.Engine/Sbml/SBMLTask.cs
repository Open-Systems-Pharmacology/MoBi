using System;
using System.Collections.ObjectModel;
using System.Linq;
using libsbmlcs;
using MoBi.Assets;
using MoBi.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Core.Events;
using MoBi.Core.Exceptions;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using Model = libsbmlcs.Model;

namespace MoBi.Engine.Sbml
{
   public class SbmlTask : ISbmlTask
   {
      public SBMLInformation SBMLInformation { get; private set; }

      private readonly IMoBiContext _mobiContext;
      private readonly SBMLImporterRepository _importerRepository;
      private IMoBiDimensionFactory _moBiDimensionFactory;

      public SbmlTask(
         IMoBiContext moBiContext,
         SBMLImporterRepository importerRepository,
         IMoBiDimensionFactory moBiDimensionFactory)
      {
         _mobiContext = moBiContext;
         _importerRepository = importerRepository;
         _moBiDimensionFactory = moBiDimensionFactory;
      }

      public IMoBiCommand ImportModelFromSbml(string filename, IMoBiProject project)
      {
         var command = new MoBiMacroCommand()
         {
            CommandType = AppConstants.Commands.AddCommand,
            ObjectType = SBMLConstants.Model,
            Comment = AppConstants.Commands.AddToProjectDescription(SBMLConstants.Model, filename)
         };
         var model = GetModel(filename);
         if (model == null) return new MoBiEmptyCommand();

         initialiseDimensionFactory();
         project.Name = getProjectName(model);

         reportConstraints(project, model);
         _importerRepository.AllFor(model).OfType<IStartable>().Each(impoter => impoter.Start());

         foreach (var importer in _importerRepository.AllFor(model))
         {
            importer.DoImport(model, project, SBMLInformation, command);
         }

         ShowNotificationsMessages();
         return command;
      }

      private void reportConstraints(IMoBiProject project, Model model)
      {
         if (model.getNumConstraints() != 0)
         {
            var msg = new NotificationMessage(project, MessageOrigin.All, null, NotificationType.Warning)
            {
               Message = ("SBML Constraints are not supported.")
            };
            SBMLInformation.NotificationMessages.Add(msg);
         }
      }

      private void initialiseDimensionFactory()
      {
         _moBiDimensionFactory = new MoBiDimensionFactory();
         _mobiContext.DimensionFactory.Dimensions.Each(_moBiDimensionFactory.AddDimension);
      }

      private string getProjectName(Model model)
      {
         var name = String.Empty;
         if (model.isSetId()) name += model.getId() + SBMLConstants.SPACE;
         if (model.isSetName()) name += model.getName();
         return name != string.Empty ? name : SBMLConstants.DEFAULT_PROJECT_NAME;
      }

      /// <summary>
      ///    Displays all the Notification messsage of the SBML Import.
      /// </summary>
      public void ShowNotificationsMessages()
      {
         _mobiContext.PublishEvent(
            new ShowNotificationsEvent(new ReadOnlyCollection<NotificationMessage>(SBMLInformation.NotificationMessages)));
      }

      /// <summary>
      ///    Gets the SBML Model from a SBML file. If it's not possible to read the SBML file
      ///    a error message is displayed.
      /// </summary>
      public Model GetModel(string filename)
      {
         var sbmlDoc = libsbml.readSBML(filename);
         if (sbmlDoc.getNumErrors() > 0)
         {
            throw new MoBiException(SBMLConstants.ModelNotRead(sbmlDoc.getErrorLog().ToString()));
         }
         convertSBML(sbmlDoc);
         var model = sbmlDoc.getModel();
         SaveSBMLInformation(model, sbmlDoc);
         return model;
      }

      /// <summary>
      ///    Saves additional Information and important Information that is necessary for the Import.
      /// </summary>
      public SBMLInformation SaveSBMLInformation(Model sbmlModel, SBMLDocument sbmlDoc)
      {
         SBMLInformation = new SBMLInformation();
         SBMLInformation.Initialize(sbmlModel, sbmlDoc);
         return SBMLInformation;
      }

      /// <summary>
      ///    Converts a SBML document to the latest version (3.1) that is supported.
      /// </summary>
      private void convertSBML(SBMLDocument sbmlDoc)
      {
         var latestLevel = SBMLDocument.getDefaultLevel();
         var latestVersion = SBMLDocument.getDefaultVersion();
         if (sbmlDoc.getLevel() == latestLevel || sbmlDoc.getVersion() == latestVersion) return;

         var success = sbmlDoc.setLevelAndVersion(latestLevel, latestVersion);

         if (!success)
         {
            throw new MoBiException(SBMLConstants.CouldNotConvertToActualLevel(latestLevel, latestVersion,
               sbmlDoc.getLevel(), sbmlDoc.getVersion()));
         }
      }
   }
}