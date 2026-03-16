using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace AuthServer.Service;

public static class ObjectMapper
{
    private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
    {
        var expression = new MapperConfigurationExpression();
        expression.AddProfile<MapProfile>();
        var config = new MapperConfiguration(expression, NullLoggerFactory.Instance);
        return config.CreateMapper();
    });

    public static IMapper Mapper => lazy.Value;
}
