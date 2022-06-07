namespace EM.Services.Mapping
{
    using AutoMapper;

    public interface ICustomMap
    {
        void ConfigureMap(IProfileExpression configuration);
    }
}
