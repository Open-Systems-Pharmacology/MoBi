using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core
{
   internal class concern_for_TemplateResolverTask : ContextSpecification<TemplateResolverTask>
   {
      protected IBuildingBlockRepository _buildingBlockRepository;
      protected IMoBiProjectRetriever _moBiProjectRetriever;
      protected IMoBiContext _context;
      protected MoBiProject _moBiProject;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _moBiProjectRetriever = new MoBiProjectRetriever(_context);
         _buildingBlockRepository = new BuildingBlockRepository(_moBiProjectRetriever);
         _moBiProject = new MoBiProject();
         A.CallTo(_context).WithReturnType<MoBiProject>().Returns(_moBiProject);
         A.CallTo(_context).WithReturnType<IProject>().Returns(_moBiProject);
         sut = new TemplateResolverTask(_buildingBlockRepository, _moBiProjectRetriever);
      }
   }

   internal class When_finding_templates_for_a_simulation : concern_for_TemplateResolverTask
   {
      private IBuildingBlock _resolvedSpatialStructure;
      private IndividualBuildingBlock _simulationIndividual;
      private IndividualBuildingBlock _templateIndividual;
      private IBuildingBlock _resolvedIndividual;
      private IBuildingBlock _resolvedReaction;
      private Module _templateModule;
      private Module _simulationModule;
      private Module _resolvedModule;

      protected override void Context()
      {
         base.Context();

         _simulationIndividual = new IndividualBuildingBlock().WithName("name");
         _templateIndividual = new IndividualBuildingBlock().WithName("name");

         _simulationModule = createNewModuleWithBuildingBlocks("the module");
         _templateModule = createNewModuleWithBuildingBlocks("the module");
         var unrelatedModule = createNewModuleWithBuildingBlocks("unrelated module");

         _moBiProject.AddModule(_templateModule);
         _moBiProject.AddModule(unrelatedModule);
         _moBiProject.AddIndividualBuildingBlock(_templateIndividual);
      }

      private Module createNewModuleWithBuildingBlocks(string name)
      {
         return new Module { new MoBiSpatialStructure().WithName("name"), new MoBiReactionBuildingBlock().WithName("name") }.WithName(name);
      }

      protected override void Because()
      {
         _resolvedSpatialStructure = sut.TemplateBuildingBlockFor(_simulationModule.SpatialStructure);
         _resolvedIndividual = sut.TemplateBuildingBlockFor(_simulationIndividual);
         _resolvedReaction = sut.TemplateBuildingBlockFor(_simulationModule.Reactions);
         _resolvedModule = sut.TemplateModuleFor(_simulationModule);
      }

      [Observation]
      public void the_template_is_resolved()
      {
         _resolvedSpatialStructure.ShouldBeEqualTo(_templateModule.SpatialStructure);
         _resolvedIndividual.ShouldBeEqualTo(_templateIndividual);
         _resolvedReaction.ShouldBeEqualTo(_templateModule.Reactions);
         _resolvedModule.ShouldBeEqualTo(_templateModule);
      }
   }
}