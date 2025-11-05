using System.Collections.Generic;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Services;
using MoBi.HelpersForTests;
using MoBi.Presentation.Presenter;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Presentation
{
   public abstract class concern_for_TagVisitor : ContextSpecification<ITagVisitor>
   {
      private IBuildingBlockRepository _projectRetriever;
      protected SpatialStructure _spatialStructure;
      private IMoBiProjectRetriever _moBiProjectRetriever;

      protected override void Context()
      {
         _moBiProjectRetriever = A.Fake<IMoBiProjectRetriever>();
         _projectRetriever = new BuildingBlockRepository(_moBiProjectRetriever);
         sut = new TagVisitor(_projectRetriever);

         _spatialStructure = new MoBiSpatialStructure();
         var topContainer = new Container().WithName("Top");
         topContainer.AddTag("Top");
         var sub1 = new Container().WithName("Sub1");
         sub1.AddTag("sub");
         var sub2 = new Container().WithName("Sub2");
         sub2.Add(new Parameter().WithName(AppConstants.Param));
         sub2.Add(new DistributedParameter().WithName("Param2"));
         sub2.AddTag("sub");
         topContainer.Add(sub1);
         topContainer.Add(sub2);

         var neighborHoods = new Container().WithName("N");
         neighborHoods.AddTag("N");
         _spatialStructure.NeighborhoodsContainer = neighborHoods;
         _spatialStructure.AddTopContainer(topContainer);

         var molecules = new Container().WithName(Constants.MOLECULE_PROPERTIES);
         molecules.AddTag(Constants.MOLECULE_PROPERTIES);
         _spatialStructure.GlobalMoleculeDependentProperties = molecules;

         var mobiProject = DomainHelperForSpecs.NewProject();
         mobiProject.AddModule(new Module { _spatialStructure });

         A.CallTo(() => _moBiProjectRetriever.Current).Returns(mobiProject);
      }
   }

   internal class When_getting_a_list_of_tags_from_a_spatial_structure : concern_for_TagVisitor
   {
      private IEnumerable<string> _result;

      protected override void Because()
      {
         _result = sut.AllTagsFrom(_spatialStructure);
      }

      [Observation]
      public void should_return_all_distinct_tags()
      {
         _result.ShouldContain("Top", "sub", "N", Constants.MOLECULE_PROPERTIES);
      }

      [Observation]
      public void should_return_all_container_names()
      {
         _result.ShouldContain("Top", "Sub1", "Sub2");
      }

      [Observation]
      public void should_return_all_parameter_names()
      {
         _result.ShouldContain(AppConstants.Param, "Param2");
      }
   }

   internal class When_getting_all_tags_defined_in_the_project : concern_for_TagVisitor
   {
      private IEnumerable<string> _result;

      protected override void Because()
      {
         _result = sut.AllTags();
      }

      [Observation]
      public void should_return_all_distinct_tags()
      {
         _result.ShouldContain("Top", "sub", "N", Constants.MOLECULE_PROPERTIES);
      }

      [Observation]
      public void should_return_all_container_names()
      {
         _result.ShouldContain("Top", "Sub1", "Sub2");
      }

      [Observation]
      public void should_return_all_parameter_names()
      {
         _result.ShouldContain(AppConstants.Param, "Param2");
      }
   }
}