using CR.Exceptions.Mapping;
using CR.Exceptions.Tests.Shared;

namespace CR.Exceptions.Tests.Component;

public sealed class ExceptionTranslatorTests
{
    private static readonly TestInternalException ExistentException = new();
    private static readonly TestUnknownException NonExistentException = new();

    [Fact]
    public void TryTranslate_ShouldReturn_TrueAndException_WhenExceptionExists()
    {
        var translator = GetDefaultTranslator();
        var result = translator.TryTranslate(ExistentException, out var exception);

        Assert.True(result);
        Assert.NotNull(exception);
        Assert.IsType<TestUnknownException>(exception);
    }

    [Fact]
    public void TryTranslate_ShouldReturn_FalseAndNull_WhenExceptionDoesNotExist()
    {
        var translator = GetDefaultTranslator();
        var result = translator.TryTranslate(NonExistentException, out var exception);

        Assert.False(result);
        Assert.Null(exception);
    }

    [Fact]
    public void Translate_ShouldReturn_Exception_WhenExceptionExists()
    {
        var translator = GetDefaultTranslator();
        var exception = translator.Translate(ExistentException);

        Assert.NotNull(exception);
        Assert.IsType<TestUnknownException>(exception);
    }

    [Fact]
    public void Translate_ShouldThrow_WhenExceptionDoesNotExist()
    {
        var translator = GetDefaultTranslator();

        Assert.Throws<KeyNotFoundException>(() => translator.Translate(NonExistentException));
    }

    private static ExceptionTranslator GetDefaultTranslator()
    {
        return new ExceptionTranslatorBuilder()
            .Map<TestInternalException>(() => new TestUnknownException())
            .Build();
    }
}