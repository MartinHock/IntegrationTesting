﻿using FluentAssertions;
using System;
using Xunit;

namespace TestingTechniques.Tests.Unit;

public class ValueSamplesTests
{
    private readonly ValueSamples _sut = new();

    [Fact]
    public void StringAssertionExample()
    {
        var fullName = _sut.FullName;

        fullName.Should().Be("Nick Chapsas");
        fullName.Should().NotBeEmpty();
        fullName.Should().StartWith("Nick");
        fullName.Should().EndWith("Chapsas");
    }

    [Fact]
    public void NumberAssertionExample()
    {
        var age = _sut.Age;

        age.Should().Be(21);
        age.Should().BePositive();
        age.Should().BeGreaterThan(20);
        age.Should().BeLessOrEqualTo(21);
        age.Should().BeInRange(18, 60);
    }

    [Fact]
    public void DateAssertionExample()
    {
        // the commented out code does not work any longer

        // eventually the following might Help

        //public static class DateOnlyAssertionExtensions
        //{
        //    public static void BeInRange(this DateOnlyAssertions assertions, DateOnly start, DateOnly end, string because = "", params object[] becauseArgs)
        //    {
        //        Execute.Assertion
        //            .ForCondition(assertions.Subject >= start && assertions.Subject <= end)
        //            .BecauseOf(because, becauseArgs)
        //            .FailWith("Expected {context:date} to be within {0} and {1}{reason}, but found {2}.", start, end, assertions.Subject);
        //    }
        //}

        var dateOfBirth = _sut.DateOfBirth;

        dateOfBirth.Should().Be(new DateOnly(2000, 6, 9));
        // dateOfBirth.Should().BeInRange(new DateOnly(2000, 1, 1), new DateOnly(2001, 1, 1));
        // dateOfBirth.Should().BeGreaterThan(new DateOnly(2000, 1, 1));
    }

    [Fact]
    public void ObjectAssertionExample()
    {
        var expected = new User
        {
            FullName = "Nick Chapsas",
            Age = 21,
            DateOfBirth = new DateOnly(2000, 6, 9)
        };

        var user = _sut.AppUser;

        //Assert.Equal(expected, user);
        //user.Should().Be(expected);
        user.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void EnumerableObjectsAssertionExample()
    {
        var expected = new User
        {
            FullName = "Nick Chapsas",
            Age = 21,
            DateOfBirth = new DateOnly(2000, 6, 9)
        };

        var users = _sut.Users.As<User[]>();

        users.Should().ContainEquivalentOf(expected);
        users.Should().HaveCount(3);
        users.Should().Contain(x => x.FullName.StartsWith("Nick") && x.Age > 5);
    }

    [Fact]
    public void EnumerableNumbersAssertionExample()
    {
        var numbers = _sut.Numbers.As<int[]>();

        numbers.Should().Contain(5);
    }

    [Fact]
    public void ExceptionThrownAssertionExample()
    {
        var calculator = new Calculator();

        Action result = () => calculator.Divide(1, 0);

        result.Should()
            .Throw<DivideByZeroException>()
            .WithMessage("Attempted to divide by zero.");
    }

    [Fact]
    public void EventRaisedAssertionExample()
    {
        var monitorSubject = _sut.Monitor();

        _sut.RaiseExampleEvent();

        monitorSubject.Should().Raise("ExampleEvent");
    }

    [Fact]
    public void TestingInternalMembersExample()
    {
        var number = _sut.InternalSecretNumber;

        number.Should().Be(42);
    }
}