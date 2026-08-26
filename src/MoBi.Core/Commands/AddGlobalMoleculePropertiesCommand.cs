using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Commands
{
   public class AddGlobalMoleculePropertiesCommand : AddObjectBaseCommand<IContainer, MoBiSpatialStructure>
   {
      public AddGlobalMoleculePropertiesCommand(MoBiSpatialStructure spatialStructure, IContainer moleculeProperties) : base(spatialStructure, moleculeProperties, spatialStructure)
      {
      }

      protected override void AddTo(IContainer moleculeProperties, MoBiSpatialStructure spatialStructure, IMoBiContext context)
      {
         spatialStructure.GlobalMoleculeDependentProperties = moleculeProperties;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveGlobalMoleculePropertiesCommand(_parent, _itemToAdd).AsInverseFor(this);
      }
   }

   public class RemoveGlobalMoleculePropertiesCommand : RemoveObjectBaseCommand<IContainer, MoBiSpatialStructure>
   {
      public RemoveGlobalMoleculePropertiesCommand(MoBiSpatialStructure spatialStructure, IContainer moleculeProperties) : base(spatialStructure, moleculeProperties, spatialStructure)
      {
      }

      protected override void RemoveFrom(IContainer moleculeProperties, MoBiSpatialStructure spatialStructure, IMoBiContext context)
      {
         spatialStructure.GlobalMoleculeDependentProperties = null;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddGlobalMoleculePropertiesCommand(_parent, _itemToRemove).AsInverseFor(this);
      }
   }
}