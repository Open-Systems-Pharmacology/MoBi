using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Tasks
{
   public interface IEntityTask
   {
      IMoBiCommand AddNewTagTo(IEntity entity, ITaggedEntityDTO dto, IBuildingBlock buildingBlock);
      IMoBiCommand RemoveTagFrom(TagDTO tagDTO, IEntity entity, IBuildingBlock buildingBlock);
   }

   public class EntityTask : IEntityTask
   {
      private readonly IMoBiContext _context;
      private readonly ITagVisitor _tagVisitor;
      private readonly IDialogCreator _dialogCreator;

      public EntityTask(IMoBiContext context, ITagVisitor tagVisitor, IDialogCreator dialogCreator)
      {
         _context = context;
         _tagVisitor = tagVisitor;
         _dialogCreator = dialogCreator;
      }

      public IMoBiCommand AddNewTagTo(IEntity entity, ITaggedEntityDTO dto, IBuildingBlock buildingBlock)
      {
         var tag = _dialogCreator.AskForInput("New Tag for Container", "New Tag", string.Empty, Enumerable.Empty<string>(), getUsedTags());
         if (string.IsNullOrEmpty(tag))
            return new MoBiEmptyCommand();

         dto.Tags.Add(new TagDTO(tag));
         return new AddTagCommand(tag, entity, buildingBlock).Run(_context);
      }

      private IEnumerable<string> getUsedTags()
      {
         return _tagVisitor.AllTags();
      }

      public IMoBiCommand RemoveTagFrom(TagDTO tagDTO, IEntity entity, IBuildingBlock buildingBlock)
      {
         return new RemoveTagCommand(tagDTO.Value, entity, buildingBlock).Run(_context);
      }
   }
}