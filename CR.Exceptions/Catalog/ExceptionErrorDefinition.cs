namespace CR.Exceptions.Catalog;

internal sealed record class ExceptionErrorDefinition(
    CrError[] Errors,
    Func<CrError[], CrException>? ExceptionFactory = null);