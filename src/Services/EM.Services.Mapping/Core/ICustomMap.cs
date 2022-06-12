namespace EM.Services.Mapping.Core
{
    using AutoMapper;

    public interface ICustomMap
    {
        void ConfigureMap(IProfileExpression configuration);
    }
}
