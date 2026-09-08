using MyWPFApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWPFApp.Tests;

public class CalculatorTests
{
    [Test]
    [Arguments(10, 5, 15)]
    [Arguments(-20, 5, -15)]
    [Arguments(-3, 0, -3)]
    [Arguments(0, 0, 0)]
    public async Task Sum_ReturnsCorrectSum(int a, int b, int expected)
    {
        // Arrange

        // Act
        int result = Calculator.Sum(a, b);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }
}
