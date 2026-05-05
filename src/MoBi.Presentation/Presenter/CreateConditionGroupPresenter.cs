using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter;

public interface ICreateConditionGroupPresenter : IDisposablePresenter
{
   ConditionGroup CreateConditionGroup();

   void AddOperand();

   void RemoveOperand(EditConditionGroupOperandDTO operand);

   IReadOnlyList<TagType> SelectableTagTypes { get; }

   string DisplayNameFor(TagType tagType);
}

public class CreateConditionGroupPresenter : AbstractDisposablePresenter<ICreateConditionGroupView, ICreateConditionGroupPresenter>, ICreateConditionGroupPresenter
{
   private readonly ITagVisitor _tagVisitor;

   private readonly IEditConditionGroupDTOToConditionGroupMapper _criteriaMapper;

   //freshly built each time the modal opens so the previous run's edits don't leak in.
   private EditConditionGroupDTO _dto;

   public IReadOnlyList<TagType> SelectableTagTypes { get; } = new[]
   {
      TagType.Match,
      TagType.NotMatch,
      TagType.MatchAll,
      TagType.InContainer,
      TagType.NotInContainer,
      TagType.InParent,
      TagType.InChildren,
   };

   public CreateConditionGroupPresenter(
      ICreateConditionGroupView view,
      ITagVisitor tagVisitor,
      IEditConditionGroupDTOToConditionGroupMapper criteriaMapper) : base(view)
   {
      _tagVisitor = tagVisitor;
      _criteriaMapper = criteriaMapper;
   }

   public ConditionGroup CreateConditionGroup()
   {
      _view.Caption = AppConstants.Captions.CreateConditionGroup;
      _dto = new EditConditionGroupDTO(_tagVisitor.AllTags().ToList());
      _view.BindTo(_dto);
      _view.Display();

      if (_view.Canceled)
         return null;

      return _criteriaMapper.MapFrom(_dto);
   }

   public void AddOperand()
   {
      _dto.Operands.Add(new EditConditionGroupOperandDTO());
      _view.BindTo(_dto);
   }

   public void RemoveOperand(EditConditionGroupOperandDTO operand)
   {
      if (operand == null)
         return;
      _dto.Operands.Remove(operand);
      _view.BindTo(_dto);
   }

   public string DisplayNameFor(TagType tagType)
   {
      switch (tagType)
      {
         case TagType.Match:
            return AppConstants.Match;
         case TagType.NotMatch:
            return AppConstants.NotMatch;
         case TagType.InContainer:
            return AppConstants.InContainer;
         case TagType.NotInContainer:
            return AppConstants.NotInContainer;
         case TagType.MatchAll:
            return AppConstants.MatchAll;
         case TagType.InParent:
            return AppConstants.InParent;
         case TagType.InChildren:
            return AppConstants.InChildren;
         case TagType.ConditionGroup:
            return AppConstants.ConditionGroup;
         default:
            return tagType.ToString();
      }
   }
}