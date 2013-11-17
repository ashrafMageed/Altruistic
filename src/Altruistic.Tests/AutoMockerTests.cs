using System;
using FluentAssertions;

namespace Altruistic.Tests
{
    public class AutoMockerTests
    {
        public void Create_WhenCreatingSUTWithNoParameters_ShouldReturnANewSUT()
        {
            // Arrange
            var sutCreator = new SUTCreator();
            var expectedSUT = new SUT();

            // Act
            var sut = sutCreator.Create<SUT>();

            // Assert
            sut.ShouldBeEquivalentTo(expectedSUT);
        }

        public void Create_WhenCreatingSUTWithValueParameters_ShouldReturnANewSUTWithParametersSetToZero()
        {
            // Arrange
            var sutCreator = new SUTCreator();
            var expectedSUT = new SUTWithPrimitiveParameters(0);

            // Act
            var sut = sutCreator.Create<SUTWithPrimitiveParameters>();

            // Assert
            sut.ShouldBeEquivalentTo(expectedSUT);
        }

        public void Create_WhenCreatingSUTWithMultipleValueTypePrimitiveParameters_ShouldReturnANewSUTWithParametersSetToZero()
        {
            // Arrange
            var sutCreator = new SUTCreator();
            var expectedSUT = new SUTWithPrimitiveParameters2(0,0,0,0,0);

            // Act
            var sut = sutCreator.Create<SUTWithPrimitiveParameters2>();

            // Assert
            sut.ShouldBeEquivalentTo(expectedSUT);
        }

        public void Create_WhenCreatingSUTWithReferenceTypes_ShouldCreateThemAsMockDependencies()
        {
            // Arrange
            var sutCreator = new SUTCreator();

            // Act
            var sut = sutCreator.Create<SUTWithReferenceDependencies>();

            // Assert
            sut.AbstractClassDependency.Should().BeAssignableTo<AbstractClassDependency>();
            sut.TestInterfaceDependency.Should().BeAssignableTo<ITestInterfaceDependency>();
        }

        public void Create_WhenSutHasMultipleConstructors_ShouldUseTheOneWithTheMostParameters()
        {
            // Arrange
            var sutCreator = new SUTCreator();

            // Act
            var sut = sutCreator.Create<SUTWithMultipleConstructors>();

            // Assert
            sut.AbstractClassDependency.Should().BeAssignableTo<AbstractClassDependency>();
            sut.TestInterfaceDependency.Should().BeAssignableTo<ITestInterfaceDependency>();
        }

        #region Sample Test Classes

        public class SUTWithMultipleConstructors
        {
            public ITestInterfaceDependency TestInterfaceDependency { get; set; }
            public AbstractClassDependency AbstractClassDependency { get; set; }

            public SUTWithMultipleConstructors()
            {
            }

            public SUTWithMultipleConstructors(ITestInterfaceDependency testInterfaceDependency)
            {
                TestInterfaceDependency = testInterfaceDependency;
            }

            public SUTWithMultipleConstructors(ITestInterfaceDependency testInterfaceDependency,
                                               AbstractClassDependency abstractClassDependency)
            {
                TestInterfaceDependency = testInterfaceDependency;
                AbstractClassDependency = abstractClassDependency;
            }
        }

        public class SUTWithReferenceDependencies
        {
            public ITestInterfaceDependency TestInterfaceDependency { get; set; }
            public AbstractClassDependency AbstractClassDependency { get; set; }

            public SUTWithReferenceDependencies(ITestInterfaceDependency testInterfaceDependency,
                                                AbstractClassDependency abstractClassDependency)
            {
                TestInterfaceDependency = testInterfaceDependency;
                AbstractClassDependency = abstractClassDependency;
            }
        }

        public class SUTWithPrimitiveParameters2
        {
            public long Test { get; set; }
            public int Test2 { get; set; }
            public double Test3 { get; set; }
            public Int16 Test4 { get; set; }
            public float Test5 { get; set; }


            public SUTWithPrimitiveParameters2(long test, int test2, double test3, Int16 test4, float test5)
            {
                Test = test;
                Test2 = test2;
                Test3 = test3;
                Test4 = test4;
                Test5 = test5;
            }
        }

        public class SUTWithPrimitiveParameters
        {
            public long Test { get; set; }

            public SUTWithPrimitiveParameters(long test)
            {
                Test = test;
            }
        }

        public class SUT
        {
            public string test { get; private set; }
        }
    }

    public abstract class AbstractClassDependency2
    {

    }

    public abstract class AbstractClassDependency
    {
    }

    public interface ITestInterfaceDependency
    {
    }

    public interface ITestInterfaceDependency2
    {
    }

    #endregion

}
