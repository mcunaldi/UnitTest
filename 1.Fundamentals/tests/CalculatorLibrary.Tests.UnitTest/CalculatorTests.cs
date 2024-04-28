using FluentAssertions;

using Xunit;
using Xunit.Abstractions;

namespace CalculatorLibrary.Tests.UnitTest;

public class CalculatorTests : IDisposable, IAsyncLifetime
{
    //Arrange 
    #region Arrange
    private readonly Calculator _sut = new(); //system under test
    private readonly Guid _guid = Guid.NewGuid();
    private readonly ITestOutputHelper _outputHelper;

    public CalculatorTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
        _outputHelper.WriteLine("Constructor is working");
    }

    public async Task InitializeAsync()
    {

        _outputHelper.WriteLine("Initialize is working...");
        await Task.Delay(1);
    }

    #endregion

    [Fact]
    public void Subtract_ShouldSubtractTwoNumbers_WhenTwoNumbersAreInteger()
    {
        //Act
        var result = _sut.Subtract(2, 7);
        //Assert
        //Assert.Equal(-5, result);

        //Assert
        result.Should().Be(-5);

    }

    [Fact]
    public void Multiply_ShouldMultiplyTwoNumbers_WhenTwoNumbersAreInteger()
    {
        //Act
        var result = _sut.Multiply(1, 3);

        //Assert
        result.Should().Be(3);
    }

    [Theory]
    [InlineData(6,2,3)]
    [InlineData(8,2,4)]
    [InlineData(0,0,0, Skip = "Sýfýr sýfýra bölünemez.")]
    public void Divide_ShouldDivideTwoNumbers_WhenTwoNumbersAreInteger(int a, int b, int expected)
    {
        //Act
        var result = _sut.Divide(a, b);//0,0 5,2

        //Assert
        result.Should().Be(expected);
    }


    [Fact(Skip = "Bu metot artýk kullanýlmýyor!")]
    public void Test1()
    {
        _outputHelper.WriteLine(_guid.ToString());
    }

    [Fact(Skip = "Bu metot artýk kullanýlmýyor!")]
    public void Test2()
    {
        _outputHelper.WriteLine(_guid.ToString());
    }


    [Fact]
    public void Add_ShouldAddTwoNumbers_WhenTwoNumbersAreInteger()
    {
        //Act
        var result = _sut.Add(2, 7);

        //Assert
        //Assert.Equal(9, result);

        //Assert
        result.Should().Be(9);
        result.Should().NotBe(7);
    }

    #region Dispose
    public void Dispose() //Genelde integration testlerde yazýlýr.
    {
        _outputHelper.WriteLine("Dispose is working...");
    }


    public async Task DisposeAsync()
    {
        _outputHelper.WriteLine("Dispose is working...");
        await Task.Delay(1);
    }

    #endregion

}