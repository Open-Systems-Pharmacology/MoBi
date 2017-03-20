using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Commands
{
   internal class CloneManagerForBuildingBlockForSpecs : ICloneManagerForBuildingBlock
   {
      public T Clone<T>(T objectToClone) where T : class, IUpdatable
      {
         return objectToClone;
      }

      public DataRepository Clone(DataRepository dataRepository)
      {
         return dataRepository;
      }

      public T Clone<T>(T objectToClone, IFormulaCache formulaCache) where T : class, IObjectBase
      {
         return objectToClone;
      }

      public T CloneBuildingBlock<T>(T buildingBlock) where T : class, IBuildingBlock
      {
         return buildingBlock;
      }

      public IFormulaCache FormulaCache { get; set; }
   }
}