using System;
using System.Linq.Expressions;
using FluentAssertions;
using Moq;

namespace Altruistic.Tests
{
    public class MockingWrapperTests
    {
        public class CreateUniqueKeyTests
        {
            public void WhenMethodHasNoParameters_ShouldReturnTypeAndMethodName()
            {
                // Arrange
                Expression<Func<string, string>> expression = s => s.Normalize();
                var mockingWrapper = new MockingWrapper<ITestData>(new Mock<ITestData>());
                var method = Utility.GetMethod(expression);

                // Act
                var uniqueName = mockingWrapper.CreateUniqueKey(typeof (string), method);

                // Assert
                uniqueName.Should().Be("ITestDataNormalize");
            }

            // figure out what's wrong with the custom Fixie convention
            public void WhenMethodHasParameters_ShouldIncludeParamterTypes()
            {
                // Arrange
                Expression<Func<string, string>> expression = s => s.Replace("tr", "rt");
                var mockingWrapper = new MockingWrapper<ITestData>(new Mock<ITestData>());
                var method = Utility.GetMethod(expression);

                // Act
                var uniqueName = mockingWrapper.CreateUniqueKey(typeof(string), method);

                // Assert
                uniqueName.Should().Be("ITestDataReplaceStringString");
            }

            public void WhenMethodHasParameters_ShouldIncludeParamterTypes2()
            {
                // Arrange
                Expression<Func<string, string>> expression = s => s.Replace('.', '_');
                var mockingWrapper = new MockingWrapper<ITestData>(new Mock<ITestData>());
                var method = Utility.GetMethod(expression);

                // Act
                var uniqueName = mockingWrapper.CreateUniqueKey(typeof(string), method);

                // Assert
                uniqueName.Should().Be("ITestDataReplaceCharChar");
            }

//            public IEnumerable<TestData> ParameterisedMethodsTestData
//            {
//                get
//                {
//                    yield return new TestData { MethodExpression = s => s.Replace("tr", "rt"), ExpectedKey = "StringReplaceStringString"};
//                    yield return new TestData { MethodExpression = s => s.Replace('.', '_'), ExpectedKey = "StringReplaceStringString" };
//                }
//            }
//
            public interface ITestData
            {
                 
            }
            public class TestData
            {
                public Expression<Func<string, string>> MethodExpression { get; set; }
                public string ExpectedKey { get; set; }
            }
        }
    }

    
}
