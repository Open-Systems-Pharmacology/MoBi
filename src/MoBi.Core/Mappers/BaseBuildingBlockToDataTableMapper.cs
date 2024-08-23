using System;
using System.Collections.Generic;
using System.Data;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Mappers
{
   public abstract class BaseBuildingBlockToDataTableMapper<TBuildingBlock> : IMapper<TBuildingBlock, List<DataTable>> where TBuildingBlock : IBuildingBlock
   {
      protected static readonly string _path = AppConstants.Captions.ContainerPath;
      protected static readonly string _value = AppConstants.Captions.Value;

      public List<DataTable> MapFrom(TBuildingBlock buildingBlock) =>
         buildingBlockToDataTable(buildingBlock);

      private List<DataTable> buildingBlockToDataTable(TBuildingBlock buildingBlock)
      {
         var dt = GenerateEmptyDataTable();
         var elements = GetElements(buildingBlock);

         foreach (var element in elements)
         {
            var row = dt.Rows.Add();
            row[_path] = element.ContainerPath;
            row[_value] = element.ConvertToDisplayUnit(element.Value);
            SetSpecificColumns(row, element);
         }

         return new List<DataTable> { dt };
      }

      protected abstract IEnumerable<PathAndValueEntity> GetElements(TBuildingBlock buildingBlock);
      protected abstract DataTable GenerateEmptyDataTable();
      protected abstract void SetSpecificColumns(DataRow row, PathAndValueEntity element);
   }
}