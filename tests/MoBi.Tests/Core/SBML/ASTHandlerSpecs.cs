using System;
using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using libsbmlcs;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Core.Helper;
using MoBi.IntegrationTests;
using OSPSuite.Core.Domain;

namespace MoBi.Core.SBML
{
    public abstract class ConcernForASTHandler : ContextForIntegration<ASTHandler>
    {

        protected override void Context()
        {
            var obf = IoC.Resolve<IObjectBaseFactory>();
            var opf = IoC.Resolve<IObjectPathFactory>();
            var mdf = IoC.Resolve<IMoBiDimensionFactory>();
            var ac = IoC.Resolve<IAliasCreator>();
            var sI = new SBMLInformation();
            sut = new ASTHandler(obf, opf, ac, mdf);
        }

        public class SimpleASTHandlerTests : ConcernForASTHandler
        {
            private List<string> _resList;
            private List<string> _rightresList;
            private List<string> _testList;

            protected override void Because()
            {
                _resList = new List<string>();
                _testList = new List<string>();
                _rightresList = new List<string>();

                //Leaves
                _testList.AddUnique("1");
                _testList.AddUnique("1.3");
                _testList.AddUnique("2E-4");

                //Multiop
                _testList.AddUnique("1 + 4 + 5");
                _testList.AddUnique("1 * 4 * 5");

                //Binop
                _testList.AddUnique("1-4");
                _testList.AddUnique("4^5");

                _rightresList.AddUnique("1");
                _rightresList.AddUnique("1.3");
                _rightresList.AddUnique("0.0002");
                _rightresList.AddUnique("((1 + 4) + 5)");
                _rightresList.AddUnique("1 * 4 * 5");
                _rightresList.AddUnique("((1) - (4))");
                _rightresList.AddUnique("(4 ^ (5))");


                foreach (var testcase in _testList)
                {
                    var temp = libsbml.parseFormula(testcase);
                    var res = sut.Eval(temp);
                    _resList.Add(res);
                }
            }

            [Observation]
            public void Simple_NotNullTest()
            {
                foreach (var result in _resList)
                    result.ShouldNotBeNull();
            }

            [Observation]
            public void Simple_RightResultTest()
            {
                for (var i = 0; i < _resList.Count; i++)
                {
                    _resList[i].Trim().ShouldBeEqualTo(_rightresList[i].Trim());
                }
            }
        }

        public class AdvancedASTHandlerTests : ConcernForASTHandler
        {
            private List<string> _resList;
            private List<string> _rightresList;
            private List<string> _testList;


            protected override void Because()
            {
                _resList = new List<string>();
                _testList = new List<string> { "-5", "5+ -2", "1 + (3-4)", "1 + 3/4 + 5", "1 + 3^4", "1/274", };
                _rightresList = new List<string> { "(-5)", "(5+(-2))", "(1 + ((3)-(4)))", "((1 + ((3)/(4))) + 5)", "(1 + (3^(4)))", "((1)/(274))" }; 

                //Advanced
                foreach (var testcase in _testList)
                {
                    var temp = libsbml.parseFormula(testcase);
                    _resList.Add(sut.Eval(temp));
                }
            }

            [Observation]
            public void Advanced_NotNullTest()
            {
                foreach (var result in _resList)
                    result.ShouldNotBeNull();
            }

            [Observation]
            public void Advanced_RightResultTest()
            {
                for (var i = 0; i < _testList.Count; i++)
                {
                    var expected = _rightresList[i].Replace(" ", String.Empty);
                    var reality = _resList[i].Replace(" ", String.Empty);
                    expected.ShouldBeEqualTo(reality);
                }
            }
        }

        public class LogicalReationalASTHandlerTests : ConcernForASTHandler
        {
            private List<string> _resList;
            private List<string> _testList;
            private List<string> _rightresList;

            protected override void Because()
            {
                _resList = new List<string>();
                _testList = new List<string> { "!1", "1 && 0", "1 || 0", /*"1 XOR 0",*/ "1 > 5", "1 < 5", "1 >= 5", "1 <= 5 + 4", "1 != 5" };
                _rightresList = new List<string> { "¬1", "1 & 0", "1 | 0", /*"1 XOR 0",*/ "1 > 5", "1 < 5", "1 >= 5", "1 <= (5 + 4)", "1 <> 5" };

                foreach (var testcase in _testList)
                {
                    var temp = libsbml.parseL3Formula(testcase);
                    _resList.Add(sut.Eval(temp));
                }
            }

            [Observation]
            public void Advanced_NotNullTest()
            {
                foreach (var result in _resList)
                    result.ShouldNotBeNull();
            }

            [Observation]
            public void Advanced_RightResultTest()
            {
                for (var i = 0; i < _testList.Count; i++)
                {
                    var expected = _rightresList[i].Replace(" ", String.Empty);
                    var reality = _resList[i].Replace(" ", String.Empty);
                    reality.ShouldBeEqualTo(expected);
                }
            }
        }

        public class TrigonometricASTHandlerTests : ConcernForASTHandler
        {
            private List<string> _resList;
            private List<string> _testList;

            protected override void Because()
            {
                _resList = new List<string>();
                _testList = new List<string>
                {
                    "cos(1)",
                    "acos(1)",
                    "cosh(1)",
                    "sin(1)",
                    "asin(1)",
                    "sinh(1)",
                    "tan(1)",
                    "atan(1)",
                    "tanh(1)"
                };

                foreach (var testcase in _testList)
                {
                    var temp = libsbml.parseFormula(testcase);
                    _resList.Add(sut.Eval(temp));
                }
            }

            [Observation]
            public void TrigonometricNotNullTest()
            {
                foreach (var result in _resList)
                    result.ShouldNotBeNull();
            }

            [Observation]
            public void Trigonometric_RightResultTest()
            {
                for (var i = 0; i < _testList.Count; i++)
                {
                    var expected = _testList[i].Replace(" ", String.Empty);
                    var reality = _resList[i].Replace(" ", String.Empty);
                    expected.ShouldBeEqualTo(reality);
                }
            }
        }

        public class ASTHandlerTests : ConcernForASTHandler
        {
            private List<string> _resList;
            private List<string> _minusList;
            private List<string> _minusRightResList;

            private List<string> _testResList;
            private List<string> _testList;
            private List<string> _testRightResList;

            protected override void Because()
            {
                _resList = new List<string>();
                _minusList = new List<string>();
                _minusRightResList = new List<string>();

                _testResList = new List<string>();
                _testList = new List<string>();
                _testRightResList = new List<string>();

                //Negative tests
                _minusList.Add("-1");
                _minusList.Add("-1.2");
                _minusList.Add("-(2+3)");

                _minusRightResList.Add("(-1)");
                _minusRightResList.Add("(-1.2)");
                _minusRightResList.Add("(-(2+3))");

                foreach (var testcase in _minusList)
                {
                    var temp = libsbml.parseFormula(testcase);
                    _resList.Add(sut.Eval(temp));
                }

                _testList.Add("ln(2)");
                _testRightResList.Add("ln(2)");

                _testList.Add("log(2)");
                _testRightResList.Add("ln(2)");

                _testList.Add("log(2,1)");
                _testRightResList.Add("log(2;1)");

                foreach (var testcase in _testList)
                {
                    var temp = libsbml.parseFormula(testcase);
                    _testResList.Add(sut.Eval(temp));
                }
            }

            [Observation]
            public void NegativeNumbersTest()
            {
                for (var i = 0; i < _minusList.Count; i++)
                {
                    var expected = _minusRightResList[i].Replace(" ", String.Empty);
                    var result = _resList[i].Replace(" ", String.Empty);
                    result.ShouldBeEqualTo(expected);
                }
            }

            [Observation]
            public void SpecialTest()
            {
                for (var i = 0; i < _testList.Count; i++)
                {
                    var expected = _testRightResList[i].Replace(" ", String.Empty);
                    var result = _testResList[i].Replace(" ", String.Empty);
                    result.ShouldBeEqualTo(expected);
                }
            }
        }

        public class ASTHandlerConstantsTests : ConcernForASTHandler
        {
            private List<string> _testResList;
            private List<ASTNode> _testList;
            private List<string> _testRightResList;

            protected override void Because()
            {
                _testResList = new List<string>();
                _testList = new List<ASTNode>();
                _testRightResList = new List<string>();


                var pi = new ASTNode(libsbml.AST_CONSTANT_PI);
                var e = new ASTNode(libsbml.AST_CONSTANT_E);
                var trueConstant = new ASTNode(libsbml.AST_CONSTANT_TRUE);
                var falseConstant = new ASTNode(libsbml.AST_CONSTANT_FALSE);

                _testList.Add(pi);
                _testList.Add(e);
                _testList.Add(trueConstant);
                _testList.Add(falseConstant);

                _testRightResList.Add("PI");
                _testRightResList.Add("E");
                _testRightResList.Add("1");
                _testRightResList.Add("0");

                foreach (var testcase in _testList)
                {
                    _testResList.Add(sut.Eval(testcase));
                }
            }

            [Observation]
            public void ConstantsTest()
            {
                for (var i = 0; i < _testList.Count; i++)
                {
                    var expected = _testRightResList[i].Replace(" ", String.Empty);
                    var result = _testResList[i].Replace(" ", String.Empty);
                    result.ShouldBeEqualTo(expected);
                }
            }
        }
    }
}
