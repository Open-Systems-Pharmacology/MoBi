using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Tasks
{
   public interface IEntityTask
   {
      IMoBiCommand AddNewTagTo(IEntity entity, IBuildingBlock buildingBlock);
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

      public IMoBiCommand AddNewTagTo(IEntity entity, IBuildingBlock buildingBlock)
      {
         var tag = _dialogCreator.AskForInput("New Tag for Container", "New Tag", string.Empty, entity.Tags.Select(x => x.Value), getUsedTags());
         if (string.IsNullOrEmpty(tag))
            return new MoBiEmptyCommand();

         return new AddTagCommand(tag, entity, buildingBlock).Run(_context);
      }

      private IEnumerable<string> getUsedTags() => _tagVisitor.AllTags();

      public IMoBiCommand RemoveTagFrom(TagDTO tagDTO, IEntity entity, IBuildingBlock buildingBlock)
      {
         return new RemoveTagCommand(tagDTO.Value, entity, buildingBlock).Run(_context);
      }
   }
}