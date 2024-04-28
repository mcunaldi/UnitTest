using CalculatorLibrary;

using FluentAssertions;

using Xunit;

namespace ValueSamples.Tests.UnitTest;
public class ValueSamplesTests
{
    //Arrange
    private readonly CalculatorLibrary.ValueSamples _sut = new();


    [Fact]
    public void StringAssertionExample()
    {
        //Act
        var fullName = _sut.FullName;

        //Assert
        fullName.Should().Be("Mehmet Can ÜNALDI");
        fullName.Should().NotBeEmpty();
        fullName.Should().StartWith("Mehmet");
        fullName.Should().EndWith("ÜNALDI");
    }

    [Fact]
    public void NumberAssertionExample()
    {
        //Act
        var age = _sut.Age;

        //Assert
        age.Should().Be(34);
        age.Should().BePositive();
        age.Should().BeGreaterThan(20);
        age.Should().BeLessThanOrEqualTo(35);
        age.Should().BeInRange(20,50);
    }

    [Fact]
    public void ObjectAssertionExample()
    {
        //Act
        var expectedUser = new User()
        {
            FullName = "Mehmet Can ÜNALDI",
            Age = 34,
            DateOfBirth = new(1989, 09, 03)
        };

        var user = _sut.user;

        //Assert 
        user.Should().BeEquivalentTo(expectedUser); //out - ref
    }

}
