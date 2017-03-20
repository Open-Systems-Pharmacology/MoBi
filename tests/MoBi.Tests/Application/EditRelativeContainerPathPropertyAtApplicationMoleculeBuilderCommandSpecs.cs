using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Application
{
   public abstract class concern_for_EditRelativeContainerPathPropertyAtApplicationMoleculeBuilderCommandSpecs : ContextSpecification<EditRelativeContainerPathPropertyAtApplicationMoleculeBuilderCommand>
   {
      public IApplicationMoleculeBuilder _applicationMoleculeBuilder;
      public IObjectPath _newPath;
      public IObjectPath _oldPath;

      protected override void Context()
      {
         _applicationMoleculeBuilder = A.Fake<IApplicationMoleculeBuilder>();
         _applicationMoleculeBuilder.Id = "ID";
         _newPath = A.Fake<IObjectPath>();
         _oldPath = A.Fake<IObjectPath>();
         _applicationMoleculeBuilder.RelativeContainerPath = _oldPath;
         sut = new EditRelativeContainerPathPropertyAtApplicationMoleculeBuilderCommand(_applicationMoleculeBuilder, _newPath, A.Fake<IBuildingBlock>());
      }
   }

   internal class When_executing_EditRelativeContainerPathPropertyAtApplicationMoleculeBuilderCommand : concern_for_EditRelativeContainerPathPropertyAtApplicationMoleculeBuilderCommandSpecs
   {
      protected override void Because()
      {
         sut.Execute(A.Fake<IMoBiContext>());
      }

      [Observation]
      public void should_set_applicationMoleculeBuilders_realtive_container_path_to_new_path()
      {
         _applicationMoleculeBuilder.RelativeContainerPath.ShouldBeEqualTo(_newPath);
      }
   }

   internal class When_restore_EditRelativeContainerPathPropertyAtApplicationMoleculeBuilderCommand : concern_for_EditRelativeContainerPathPropertyAtApplicationMoleculeBuilderCommandSpecs
   {
      private IMoBiContext _context;

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
      }

      protected override void Because()
      {
         sut.RestoreExecutionData(_context);
      }

      [Observation]
      public void should_set_applicationMoleculeBuilders_realtive_container_path_to_new_path()
      {
         A.CallTo(() => _context.Get<IApplicationMoleculeBuilder>(_applicationMoleculeBuilder.Id)).MustHaveHappened();
      }
   }
}