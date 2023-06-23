using System;
using System.IO;
using System.Linq;
using System.Xml;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Exceptions;
using MoBi.Core.Services;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization.Diagram;
using OSPSuite.Presentation.Diagram.Elements;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class DiagramTask : IDiagramTask
   {
      public void RenameObjectBase(IObjectBase objectBase, IDiagramModel model, Predicate<string> mustHandleExisting)
      {
         try
         {
            if (objectBase == null) return;
            if (!mustHandleExisting(objectBase.Id)) return;

            model.RenameNode(objectBase.Id, objectBase.Name);
            model.ClearUndoStack(); //because cannot undo this action, reset undo stack
         }
         catch (Exception ex)
         {
            IoC.Resolve<IExceptionManager>().LogException(ex);
         }
      }

      public void ApplyLayoutTemplate(IContainerBase containerBase, string diagramTemplateXmlFilePath, IDiagramModel model, Action refreshFromDiagramOptions, bool recursive)
      {
         if (string.IsNullOrEmpty(diagramTemplateXmlFilePath)) throw new MoBiException(AppConstants.Exceptions.MissingName);
         var diagramTemplateModel = LoadDiagramTemplate(diagramTemplateXmlFilePath);
         if (diagramTemplateModel == null) throw new MoBiException(AppConstants.Exceptions.DeserializationFailed);

         try
         {
            model.BeginUpdate();

            var copier = new LayoutCopyService();
            if (recursive)
               copier.CopyRecursive(diagramTemplateModel, containerBase);
            else
               copier.Copy(diagramTemplateModel, containerBase);

            foreach (var neighborhoodNode in containerBase.GetAllChildren<INeighborhoodNode>())
               neighborhoodNode.AdjustPosition();

            refreshFromDiagramOptions();
            model.IsLayouted = true;
         }
         finally { model.EndUpdate(); }
      }

      public void SaveContainerToXml(IContainerBase containerBase, string diagramTemplateXmlFilePath)
      {
         if (string.IsNullOrEmpty(diagramTemplateXmlFilePath)) throw new MoBiException(AppConstants.Exceptions.MissingName);
         var serializer = IoC.Resolve<IDiagramModelToXmlMapper>();

         var containerSerializer = serializer as IContainerBaseXmlSerializer;
         if (containerSerializer == null) return;
         var xmlDoc = containerSerializer.ContainerToXmlDocument(containerBase);

         var containerBaseNode = containerBase as IContainerNode;
         if (containerBaseNode != null)
            xmlDoc.InnerXml = xmlDoc.InnerXml.Replace(containerBaseNode.Name, LayoutCopyService.ContainerNamePlaceHolder);

         xmlDoc.Save(diagramTemplateXmlFilePath);
      }

      public IDiagramModel LoadDiagramTemplate(string diagramTemplateXmlFilePath)
      {
         if (!File.Exists(diagramTemplateXmlFilePath))
         {
            throw new FileNotFoundException("File not found", diagramTemplateXmlFilePath);
         }
         var diagramTemplateXmlDocument = new XmlDocument();
         diagramTemplateXmlDocument.Load(diagramTemplateXmlFilePath);

         if (diagramTemplateXmlDocument.ChildNodes.Count == 0)
         {
            throw new MoBiException("Template is empty");
         }

         var serializer = IoC.Resolve<IDiagramModelToXmlMapper>();
         if (serializer == null)
         {
            throw new MoBiException("Serializer not found");
         }

         var diagramTemplateModel = serializer.XmlDocumentToDiagramModel(diagramTemplateXmlDocument);
         if (diagramTemplateModel == null)
         {
            throw new MoBiException("Deserialization failed");
         }

         return diagramTemplateModel;
      }

      public ICommand<IMoBiContext> MoveDiagramNodes(MoBiReactionBuildingBlock sourceBuildingBlock, MoBiReactionBuildingBlock targetBuildingBlock, ReactionBuilder builder, string builderOriginalName)
      {
         if (sourceBuildingBlock.DiagramModel == null || targetBuildingBlock.DiagramModel == null)
            return new MoBiEmptyCommand();

         return getMoveCommands(sourceBuildingBlock, targetBuildingBlock, builder, builderOriginalName);
      }

      private MoBiMacroCommand getMoveCommands(MoBiReactionBuildingBlock sourceBuildingBlock, MoBiReactionBuildingBlock targetBuildingBlock, ReactionBuilder builder, string builderOriginalName)
      {
         var macroCommand = new MoBiMacroCommand();

         macroCommand.Add(getMoveCommand(sourceBuildingBlock, targetBuildingBlock, builder.Name, builderOriginalName));
         
         
         builder.Educts.Each(educt => macroCommand.Add(movePartnersCommand(sourceBuildingBlock, targetBuildingBlock, educt)));
         builder.Products.Each(product => macroCommand.Add(movePartnersCommand(sourceBuildingBlock, targetBuildingBlock, product)));
         builder.ModifierNames.Each(modifier => macroCommand.Add(movePartnerNamed(sourceBuildingBlock, targetBuildingBlock, modifier)));
         return macroCommand;
      }

      private ICommand<IMoBiContext> movePartnersCommand(MoBiReactionBuildingBlock sourceBuildingBlock, MoBiReactionBuildingBlock targetBuildingBlock, ReactionPartnerBuilder partner)
      {
         return movePartnerNamed(sourceBuildingBlock, targetBuildingBlock, partner.MoleculeName);
      }

      private ICommand<IMoBiContext> movePartnerNamed(MoBiReactionBuildingBlock sourceBuildingBlock, MoBiReactionBuildingBlock targetBuildingBlock, string partnerName)
      {
         if(targetBuildingBlock.DiagramManager.PkModel == null)
            return new MoBiEmptyCommand();

         return targetBuildingBlock.AllMolecules.Any(x => string.Equals(x, partnerName)) ? new MoBiEmptyCommand() : getMoveCommand(sourceBuildingBlock, targetBuildingBlock, partnerName, partnerName);
      }

      private ICommand<IMoBiContext> getMoveCommand(MoBiReactionBuildingBlock sourceBuildingBlock, MoBiReactionBuildingBlock targetBuildingBlock, string builderName, string builderOriginalName)
      {
         var sourceBuilderNode = sourceBuildingBlock.DiagramModel.FindByName(builderOriginalName);

         if (sourceBuilderNode == null) return new MoBiEmptyCommand();
         return new MoveDiagramNodeCommand(targetBuildingBlock, builderName, sourceBuilderNode);
      }
   }
}