using SBSuite.Core.Diagram;
using SBSuite.Core.Domain;
using SBSuite.Presentation.Diagram;

namespace MoBi.Core.Domain.Model.Diagram
{

   public interface IMoBiBaseDiagramManager : IBaseDiagramManager
   {
      void RenameObjectBase(IObjectBase objectBase);
      void ApplyLayoutTemplate(IContainerBase containerBase, string diagramTemplateXmlFilePath, bool recursive);
      void SaveContainerToXml(IContainerBase containerBase, string diagramTemplateXmlFilePath);
      IDiagramModel LoadDiagramTemplate(string diagramTemplateXmlFilePath);
   }
}