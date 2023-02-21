using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation
{
   public class concern_for_CreateModulePresenter : ContextSpecification<CreateModulePresenter>
   {
      protected ICreateModuleDTOToModuleMapper _mapper;
      private IMoBiContext _context;
      protected ICreateModuleView _view;
      protected IMoBiProject _project;

      protected override void Context()
      {
         _project = new MoBiProject();
         _view = A.Fake<ICreateModuleView>();
         _mapper = A.Fake<ICreateModuleDTOToModuleMapper>();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         

         sut = new CreateModulePresenter(_view, _context, _mapper);
      }
   }

   public class When_creating_a_new_module_and_the_view_is_not_canceled : concern_for_CreateModulePresenter
   {
      private Module _result;
      private Module _module;

      protected override void Context()
      {
         base.Context();
         _module = new Module();
         A.CallTo(() => _view.Canceled).Returns(false);
         A.CallTo(() => _mapper.MapFrom(A<CreateModuleDTO>._)).Returns(_module);
      }

      protected override void Because()
      {
         _result = sut.CreateModule();
      }

      [Observation]
      public void the_module_mapper_should_create_the_module()
      {
         A.CallTo(() => _mapper.MapFrom(A<CreateModuleDTO>._)).MustHaveHappened();
      }
   }

   public class When_creating_a_new_module_and_the_view_is_canceled : concern_for_CreateModulePresenter
   {
      private Module _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         _result = sut.CreateModule();
      }

      [Observation]
      public void the_module_should_not_be_added_to_the_project()
      {
         _result.ShouldBeNull();
      }
   }
}
