using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_AddObjectBaseItemCommand : ContextSpecification<AddObjectBaseCommand<IObjectBase, IObjectBase>>
   {
      protected IMoBiContext _context;
      protected IObjectBase _value;
      protected IObjectBase _parent;
      protected IBuildingBlock _buidingBlock;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _parent = A.Fake<IObjectBase>().WithName("Parent").WithId("Parent");
         _value = A.Fake<IObjectBase>().WithName("Value").WithId("Value");
         _buidingBlock = A.Fake<IBuildingBlock>();
         sut = new TestAddObjectBaseCommand(_parent, _value, _buidingBlock);
      }

      protected static IObjectBase Value { get; set; }

      protected static IObjectBase Parent { get; set; }

      public class TestAddObjectBaseCommand : AddObjectBaseCommand<IObjectBase, IObjectBase>
      {
         public TestAddObjectBaseCommand(IObjectBase parent, IObjectBase itemToAdd, IBuildingBlock buildingBlock) : base(parent, itemToAdd, buildingBlock)
         {
         }

         protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
         {
            return null;
         }

         protected override void AddTo(IObjectBase child, IObjectBase parent, IMoBiContext context)
         {
            Parent = parent;
            Value = child;
         }
      }
   }

   internal class When_executing_an_AddObjectBaseCommand : concern_for_AddObjectBaseItemCommand
   {
      private AddedEvent _event;

      protected override void Context()
      {
         base.Context();
         sut.Silent = false;
         A.CallTo(() => _context.PublishEvent(A<AddedEvent<IObjectBase>>._))
            .Invokes(x => _event = x.GetArgument<AddedEvent<IObjectBase>>(0));
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_publish_added_event()
      {
         _event.ShouldNotBeNull();
         _event.AddedObject.ShouldBeEqualTo(_value);
         _event.Parent.ShouldBeEqualTo(_parent);
      }

      [Observation]
      public void should_have_add_value_to_parent()
      {
         Parent.ShouldBeEqualTo(_parent);
         Value.ShouldBeEqualTo(_value);
      }
   }
}