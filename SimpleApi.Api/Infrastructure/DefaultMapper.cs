using SimpleApi.Domain.Services;

namespace SimpleApi.API.Infrastructure;

public class DefaultMapper : IMapper
{
    private readonly AutoMapper.IMapper _mapper;

    public DefaultMapper(AutoMapper.IMapper mapper)
    {
        _mapper = mapper;
    }

    public R Map<T, R>(T t)
    {
        return _mapper.Map<T, R>(t);
    }
}