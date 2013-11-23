using System;
using System.Linq.Expressions;
using FluentAssertions;

namespace Altruistic.Tests
{
    public class UtilityTests
    {
        public class GetMethodTests
        {
            public void GivenAMethodCallExpression_ShouldExtractMethodFromTheExpression()
            {
                // Arrange
                Expression<Func<string, string>> expression = s => s.Insert(3, "abc");

                // Act
                var method = Utility.GetMethod(expression);

                // Assert
                method.Name.Should().Be("Insert");
            }

            public void GivenAFunc_ShouldExtractMethodFromTheFunc()
            {
                // Arrange
                Func<string> func = DummyMethod;

                // Act
                var method = Utility.GetMethod(func);

                // Assert
                method.Name.Should().Be("DummyMethod");
            }

            public string DummyMethod()
            {
                return String.Empty;
            }


            public void GivenANonMethodCallExpression_ShouldThrowAnArgumentException()
            {
                // Arrange
                Expression<Func<string, int>> expression = s => s.Length;

                // Act
                Action act = () => Utility.GetMethod(expression);

                // Assert
                act.ShouldThrow<ArgumentException>();
            }
        }
    }
}
