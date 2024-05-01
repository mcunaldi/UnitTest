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

    [Fact]
    public void EnumerableObjectAssertionExample()
    {
        //Arrange
        var expected = new User
        {
            FullName = "Taner Saydam",
            Age = 34,
            DateOfBirth = new(1989, 09, 03)
        };

        //Act
        var users = _sut.Users.As<User[]>();    

        //Assert
        users.Should().ContainEquivalentOf(expected);
        users.Should().HaveCount(3);
        users.Should().Contain(x => x.FullName.StartsWith("Tahir") && x.Age > 5);

    }

    [Fact]
    public void EnumerableNumberAssertionExample()
    {
        //Act
        var numbers = _sut.Numbers.As<int[]>();

        //Assert
        numbers.Should().Contain(5);
    }

    [Fact]
    public void ExceptionThrownAssertionExample()
    {
        //Act
        Action result = () => _sut.Divide(1, 0);

        //Assert
        result.Should().Throw<DivideByZeroException>().WithMessage("Attempted to divide by zero.");
    }

    [Fact]
    public void EventRaisedAssertionExample()
    {
        //Arrange
        var monitorSubject = _sut.Monitor(); //eventi monitorize edip göstermeyi sağlar. Eventleri çağırmak için.

        //Act
        _sut.RaiseExampleEvent();

        //Assert
        monitorSubject.Should().Raise("ExampleEvent");
    }

    [Fact]
    public void TestingInternalMembersExample()
    {
        //Act
        var number = _sut.InternalSecretNumber;

        //Assert

        number.Should().Be(42);
    }




}
