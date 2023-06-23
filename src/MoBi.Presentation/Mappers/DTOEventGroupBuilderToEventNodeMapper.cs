using System.Collections.Generic;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Nodes;
using OSPSuite.Assets;

namespace MoBi.Presentation.Mappers
{
   public interface IDTOEventGroupBuilderToEventNodeMapper : IMapper<EventGroupBuilderDTO, ITreeNode>
   {
   }

   internal class DTOEventGroupBuilderToEventNodeMapper : IDTOEventGroupBuilderToEventNodeMapper
   {
      public ITreeNode MapFrom(EventGroupBuilderDTO eventGroupBuilder)
      {
         var node = mapFrom(eventGroupBuilder);
         var children = eventGroupBuilder.EventGroups.MapAllUsing(this);
         children.Each(node.AddChild);

         eventGroupBuilder.Applications.Each(app => node.AddChild(MapFrom(app)));

         mapAll(eventGroupBuilder.Events).Each(node.AddChild);
         mapAll(eventGroupBuilder.ChildContainer).Each(node.AddChild);

         if (eventGroupBuilder.IsAnImplementationOf<ApplicationBuilderDTO>())
         {
            var inputApplication = eventGroupBuilder.DowncastTo<ApplicationBuilderDTO>();
            mapAll(inputApplication.Transports).Each(node.AddChild);
         }
         return node;
      }

      private IEnumerable<ITreeNode> mapAll<T>(IEnumerable<T> children) where T : ObjectBaseDTO
      {
         var nodes = new List<ITreeNode>();
         children.Each(c => nodes.Add(mapFrom(c)));
         return nodes;
      }

      private ITreeNode mapFrom(ObjectBaseDTO objectBaseDTO)
      {
         return new EventNode(objectBaseDTO).WithIcon(objectBaseDTO.Icon);
      }
   }
}