using NUnit.Framework;
using CalcLibrary; 

namespace CalcLibraryTests
{
    [TestFixture]
    public class CalculatorTests
    {
        SimpleCalculator calculator;

        [SetUp]
        public void Init()
        {
            calculator = new SimpleCalculator();
        }

        [TearDown]
        public void Cleanup()
        {
            calculator = null;
        }

        [TestCase(2.0, 3.0, 5.0)]
        [TestCase(-1.5, -1.5, -3.0)]
        [TestCase(0.0, 0.0, 0.0)]
        public void TestAddition(double a, double b, double expected)
        {
            double result = calculator.Addition(a, b);
            Assert.That(result, Is.EqualTo(expected).Within(0.001)); 
        }
    }
}
