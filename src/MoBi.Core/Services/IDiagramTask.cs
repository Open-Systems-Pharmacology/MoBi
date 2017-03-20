using System;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Services
{
   public interface IDiagramTask
   {
      /// <summary>
      ///    Moves nodes in <paramref name="targetBuildingBlock" /> to locations specified by like-named nodes in
      ///    <paramref name="sourceBuildingBlock" />
      /// </summary>
      /// <returns>The command used to move the nodes</returns>
      ICommand<IMoBiContext> MoveDiagramNodes(IMoBiReactionBuildingBlock sourceBuildingBlock, IMoBiReactionBuildingBlock targetBuildingBlock, IReactionBuilder builder, string builderOriginalName);

      void RenameObjectBase(IObjectBase objectBase, IDiagramModel model, Predicate<string> mustHandleExisting);
      void ApplyLayoutTemplate(IContainerBase containerBase, string diagramTemplateXmlFilePath, IDiagramModel model, Action refreshFromDiagramOptions, bool recursive);
      void SaveContainerToXml(IContainerBase containerBase, string diagramTemplateXmlFilePath);
      IDiagramModel LoadDiagramTemplate(string diagramTemplateXmlFilePath);
   }
}