using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace MoBi.Core.Domain.Services
{
   public abstract class BaseModuleRegisterVisitor
   {
      protected readonly IMoBiContext _context;

      protected BaseModuleRegisterVisitor(IMoBiContext context)
      {
         _context = context;
      }

      public void Visit(IBuildingBlock buildingBlock)
      {
         VisitAction(buildingBlock);
      }

      public void Visit(Module module)
      {
         VisitAction(module);
         module.AllBuildingBlocks().Each(Visit);
      }

      protected abstract void VisitAction(IBuildingBlock buildingBlock);
      protected abstract void VisitAction(Module buildingBlock);
   }

   public interface IModuleUnregisterVisitor : IVisitor<Module>, IVisitor<IBuildingBlock>
   {
   }

   public class ModuleUnregisterVisitor : BaseModuleRegisterVisitor, IModuleUnregisterVisitor
   {
      public ModuleUnregisterVisitor(IMoBiContext context) : base(context)
      {
      }

      protected override void VisitAction(IBuildingBlock buildingBlock)
      {
         _context.Unregister(buildingBlock);
      }

      protected override void VisitAction(Module module)
      {
         _context.Unregister(module);
      }
   }

   public interface IModuleRegisterVisitor : IVisitor<Module>, IVisitor<IBuildingBlock>
   {
   }

   public class ModuleRegisterVisitor : BaseModuleRegisterVisitor, IModuleRegisterVisitor
   {
      public ModuleRegisterVisitor(IMoBiContext context) : base(context)
      {
      }

      protected override void VisitAction(IBuildingBlock buildingBlock)
      {
         _context.Register(buildingBlock);
      }

      protected override void VisitAction(Module module)
      {
         _context.Register(module);
      }
   }
}