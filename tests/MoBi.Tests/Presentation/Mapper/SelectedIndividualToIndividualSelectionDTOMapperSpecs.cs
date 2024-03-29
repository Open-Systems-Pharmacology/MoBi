﻿using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mapper
{
   public class concern_for_SelectedIndividualToIndividualSelectionDTOMapper : ContextSpecification<SelectedIndividualToIndividualSelectionDTOMapper>
   {
      private IMoBiProjectRetriever _projectRetriever;
      private IBuildingBlockRepository _buildingBlockRepository;
      protected MoBiProject _moBiProject;

      protected override void Context()
      {
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();
         _buildingBlockRepository = new BuildingBlockRepository(_projectRetriever);
         _moBiProject = new MoBiProject();
         A.CallTo(() => _projectRetriever.Current).Returns(_moBiProject);
         
         sut = new SelectedIndividualToIndividualSelectionDTOMapper(_buildingBlockRepository);
      }
   }

   public class When_mapping_a_selected_individual_to_an_individual_selection_dto : concern_for_SelectedIndividualToIndividualSelectionDTOMapper
   {
      private IndividualSelectionDTO _result;
      private IndividualBuildingBlock _projectIndividual1;
      private IndividualBuildingBlock _configurationIndividual;
      private IndividualBuildingBlock _projectIndividual2;

      protected override void Context()
      {
         base.Context();
         _projectIndividual1 = new IndividualBuildingBlock().WithName("common name");
         _projectIndividual2 = new IndividualBuildingBlock().WithName("uncommon name");
         _configurationIndividual = new IndividualBuildingBlock().WithName("common name");
         _moBiProject.AddIndividualBuildingBlock(_projectIndividual1);
         _moBiProject.AddIndividualBuildingBlock(_projectIndividual2);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_configurationIndividual);
      }

      [Observation]
      public void should_return_a_dto_with_the_name_of_the_individual()
      {
         _result.SelectedIndividualBuildingBlock.ShouldBeEqualTo(_projectIndividual1);
         _result.AllIndividuals.ShouldOnlyContain(_projectIndividual1, _projectIndividual2, NullIndividual.NullIndividualBuildingBlock);
      }
   }
}
