using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Mapper
{
   public abstract class concern_for_ParameterToDTOParameterMapper : ContextSpecification<IParameterToParameterDTOMapper>
   {
      protected IFormulaToFormulaBuilderDTOMapper _formulaMapper;
      protected ITagToTagDTOMapper _tagMappper;
      protected IGroupRepository _groupRepository;
      protected IFavoriteRepository _favoriteRepository;
      protected IEntityPathResolver _entityPathResolver;

      protected override void Context()
      {
         _formulaMapper = A.Fake<IFormulaToFormulaBuilderDTOMapper>();
         _tagMappper = A.Fake<ITagToTagDTOMapper>();
         _groupRepository= A.Fake<IGroupRepository>();
         _favoriteRepository = A.Fake<IFavoriteRepository>();
         _entityPathResolver = A.Fake<IEntityPathResolver>();
         sut = new ParameterToParameterDTOMapper(_formulaMapper,_tagMappper,_groupRepository,_favoriteRepository,_entityPathResolver);
      }
   }

   public class When_mapping_a_Parameter_without_RHS : concern_for_ParameterToDTOParameterMapper
   {
      private IParameter _parameter;
      private IFormula _formula;
      private string _name;
      private IDimension _dimension;
      private ParameterBuildMode _parameterBuildMode;
      private ParameterDTO _result;
      protected Tag _tag;
      protected Tag _otherTag;
      private IGroup _group;

      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
         _formula = A.Fake<IFormula>();
         _dimension = A.Fake<IDimension>();
         _group= A.Fake<IGroup>();  
         _parameter.Formula = _formula;
         _parameter.RHSFormula = null;
         _name = "Para";
         _parameter.Name = _name;
         _parameter.GroupName = "TOTO";
         _parameter.Persistable = true;
         _parameter.Visible = true;
         _parameter.Dimension = _dimension;
         _parameterBuildMode = ParameterBuildMode.Global;
         _parameter.BuildMode = _parameterBuildMode;
         _tag = new Tag("Unsinn");
         _otherTag = new Tag("Quatsch");
         _parameter.CanBeVariedInPopulation = true;
         A.CallTo(() => _parameter.Tags).Returns(new Tags(new[] {_tag, _otherTag}));
         A.CallTo(() => _groupRepository.GroupByName(_parameter.GroupName)).Returns(_group);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_parameter).DowncastTo<ParameterDTO>();
      }
   
      [Observation]
      public void should_ask_to_map_formulas()
      {
         A.CallTo(() => _formulaMapper.MapFrom(_formula)).MustHaveHappened();
      }
      [Observation]
      public void should_ask_map_all_Tags()
      {
         A.CallTo(() => _tagMappper.MapFrom(_tag)).MustHaveHappened();
         A.CallTo(() => _tagMappper.MapFrom(_otherTag)).MustHaveHappened();
      }

      [Observation]
      public void should_set_right_properties()
      {
         _result.Name.ShouldBeEqualTo(_name);
         _result.Persistable.ShouldBeTrue();
         _result.IsAdvancedParameter.ShouldBeFalse();
         _result.Dimension.ShouldBeEqualTo(_dimension);
         _result.BuildMode.ShouldBeEqualTo(_parameterBuildMode);
         _result.HasRHS.ShouldBeFalse();
         _result.CanBeVariedInPopulation.ShouldBeTrue();
         _result.Group.ShouldBeEqualTo(_group);
      }
   }


   public class When_mapping_a_Parameter_with_RHS : concern_for_ParameterToDTOParameterMapper
   {
      private IParameter _parameter;
      private IFormula _formula;
      private IFormula _rhsFormula;
      private string _name;
      private IDimension _dimension;
      private ParameterBuildMode _parameterBuildMode;
      private ParameterDTO _result;
      private Tag _tag;
      private Tag _otherTag;

      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
         _formula = A.Fake<IFormula>();
         _rhsFormula = A.Fake<IFormula>();
         _dimension = A.Fake<IDimension>();
         _parameter.Formula = _formula;
         _parameter.RHSFormula = _rhsFormula;
         _name = "Para";
         _parameter.Name = _name;
         _parameter.Persistable = true;
         _parameter.Visible = true;
         _parameter.Dimension = _dimension;
         _parameterBuildMode = ParameterBuildMode.Global;
         _parameter.BuildMode = _parameterBuildMode;
         _tag = new Tag("Unsinn");
         _otherTag = new Tag("Quatsch");
         A.CallTo(() => _parameter.Tags).Returns(new Tags(new[] { _tag, _otherTag }));
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_parameter).DowncastTo<ParameterDTO>();
      }

      [Observation]
      public void should_ask_to_map_formulas()
      {
         A.CallTo(() => _formulaMapper.MapFrom(_formula)).MustHaveHappened();
         A.CallTo(() => _formulaMapper.MapFrom(_rhsFormula)).MustHaveHappened();
      }

      [Observation]
      public void should_ask_map_all_Tags()
      {
         A.CallTo(() => _tagMappper.MapFrom(_tag)).MustHaveHappened();
         A.CallTo(() => _tagMappper.MapFrom(_otherTag)).MustHaveHappened();
      }

      [Observation]
      public void should_set_right_properties()
      {
         _result.Name.ShouldBeEqualTo(_name);
         _result.Persistable.ShouldBeTrue();
         _result.IsAdvancedParameter.ShouldBeFalse();
         _result.Dimension.ShouldBeEqualTo(_dimension);
         _result.BuildMode.ShouldBeEqualTo(_parameterBuildMode);
         _result.HasRHS.ShouldBeTrue();
      }
   }

   public class When_mapping_a_Favorite_Parameter : concern_for_ParameterToDTOParameterMapper
   {
      private IParameter _parameter;
      private IFormula _formula;
      private string _name;
      private IDimension _dimension;
      private ParameterBuildMode _parameterBuildMode;
      private ParameterDTO _result;
      private Tag _tag;
      private Tag _otherTag;
      private IObjectPath _path;

      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
         _formula = A.Fake<IFormula>();
         _dimension = A.Fake<IDimension>();
         _path= A.Fake<IObjectPath>();
         _parameter.Formula = _formula;
         _name = "Para";
         _parameter.Name = _name;
         _parameter.Persistable = true;
         _parameter.Visible = true;
         _parameter.Dimension = _dimension;
         _parameterBuildMode = ParameterBuildMode.Global;
         _parameter.BuildMode = _parameterBuildMode;
         _tag = new Tag("Unsinn");
         _otherTag = new Tag("Quatsch");
         A.CallTo(() => _entityPathResolver.ObjectPathFor(_parameter, false)).Returns(_path);
         A.CallTo(() => _favoriteRepository.Contains(_path)).Returns(true);
         A.CallTo(() => _parameter.Tags).Returns(new Tags(new[] { _tag, _otherTag }));
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_parameter).DowncastTo<ParameterDTO>();
      }

      [Observation]
      public void should_ask_to_map_formulas()
      {
         A.CallTo(() => _formulaMapper.MapFrom(_formula)).MustHaveHappened();
      }

      [Observation]
      public void should_ask_map_all_Tags()
      {
         A.CallTo(() => _tagMappper.MapFrom(_tag)).MustHaveHappened();
         A.CallTo(() => _tagMappper.MapFrom(_otherTag)).MustHaveHappened();
      }

      [Observation]
      public void should_set_right_properties()
      {
         _result.Name.ShouldBeEqualTo(_name);
         _result.Persistable.ShouldBeTrue();
         _result.IsAdvancedParameter.ShouldBeFalse();
         _result.Dimension.ShouldBeEqualTo(_dimension);
         _result.BuildMode.ShouldBeEqualTo(_parameterBuildMode);
         _result.HasRHS.ShouldBeTrue();
         _result.IsFavorite.ShouldBeEqualTo(true);
      }
   }
}	