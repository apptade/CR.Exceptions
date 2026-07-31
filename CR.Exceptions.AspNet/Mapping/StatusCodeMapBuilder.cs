namespace CR.Exceptions.AspNet.Mapping;

public sealed class StatusCodeMapBuilder : ExceptionTypeMapBuilder<int>
{
    public StatusCodeMap Build()
    {
        return new(BuildMap());
    }
}