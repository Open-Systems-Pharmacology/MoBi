using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mapper
{
   internal class concern_for_MoleculeToMoleculeSelectionDTOMapper : ContextSpecification<MoleculeToMoleculeSelectionDTOMapper>
   {
      protected override void Context()
      {
         sut = new MoleculeToMoleculeSelectionDTOMapper();
      }
   }

   internal class When_mapping_dtos_from_building_blocks : concern_for_MoleculeToMoleculeSelectionDTOMapper
   {
      private MoleculeBuildingBlock _buildingBlock;
      private IReadOnlyList<MoleculeSelectionDTO> _dtos;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new MoleculeBuildingBlock
         {
            new MoleculeBuilder()
         }.WithName("BuildingBlock");

         new Module().WithName("Module").Add(_buildingBlock);
      }

      protected override void Because()
      {
         _dtos = sut.MapFrom(_buildingBlock);
      }

      [Observation]
      public void dtos_should_all_have_the_building_block_name_qualified_with_module_name()
      {
         _dtos.Each(x => x.BuildingBlock.ShouldBeEqualTo(_buildingBlock.ToString()));
      }

      [Observation]
      public void molecules_should_all_be_unselected()
      {
         _dtos.Each(x => x.Selected.ShouldBeFalse());
      }
   }
}
