namespace EM.Services.Mapping
{
    using System.Reflection;

    using AutoMapper;

    using EM.Services.Mapping.Core;

    using Microsoft.Extensions.DependencyInjection;

    public static class MapperExtensions
    {
        private const string ModelCreationExceptionMessage = "Cannot create instance of type {0}. It should have a parameterless constructor";

        public static void AddAutoMapper(this IServiceCollection serviceCollection, params Assembly[] assembliesToScan)
        {
            serviceCollection.AddSingleton(AutoMapperFactory(assembliesToScan));
        }

        private static IMapper AutoMapperFactory(params Assembly[] assembliesToScan)
        {
            var configurationExpression = new MapperConfigurationExpression();

            foreach (var assembly in assembliesToScan)
            {
                var mappingFromInfos = GetGenericMaps(assembly, typeof(IMapFrom<>));
                RegisterMaps(configurationExpression, mappingFromInfos);

                var mappingToInfos = GetGenericMaps(assembly, typeof(IMapTo<>));
                RegisterMaps(configurationExpression, mappingToInfos);

                var customMappings = GetCustomMaps(assembly);
                RegisterCustomMaps(configurationExpression, customMappings);
            }

            var configuration = new MapperConfiguration(configurationExpression);

            return new Mapper(configuration);
        }

        private record class MapInfo(Type From, Type To);

        private static MapInfo[] GetGenericMaps(Assembly assembly, Type assignableType)
        {
            return assembly
                .GetTypes()
                .SelectMany(t => t
                    .GetInterfaces()
                    .Where(i => i.IsGenericType &&
                                i.GetGenericTypeDefinition() == assignableType)
                    .Select(i => assignableType switch
                    {
                        _ when assignableType.GetGenericTypeDefinition() == typeof(IMapFrom<>) =>
                            new MapInfo(i.GenericTypeArguments[0], t),
                        _ when assignableType.GetGenericTypeDefinition() == typeof(IMapTo<>) =>
                            new MapInfo(t, i.GenericTypeArguments[0]),
                        _ => throw new NotSupportedException(),
                    })
                    .ToArray())
                .ToArray();
        }

        private static Type[] GetCustomMaps(Assembly assembly)
        {
            return assembly
                    .GetTypes()
                    .Where(x => x.IsAssignableTo(typeof(ICustomMap)))
                    .ToArray();
        }

        private static void RegisterMaps(MapperConfigurationExpression configuration, MapInfo[] mappingInfos)
        {
            foreach (var mappingInfo in mappingInfos)
            {
                configuration.CreateMap(mappingInfo.From, mappingInfo.To, MemberList.None);
            }
        }

        private static void RegisterCustomMaps(MapperConfigurationExpression configuration, Type[] customMappings)
        {
            foreach (var customMap in customMappings)
            {
                ICustomMap? instance = Activator.CreateInstance(customMap) as ICustomMap;

                if (instance is not null)
                {
                    instance.ConfigureMap(configuration);

                    continue;
                }

                throw new InvalidOperationException(string.Format(ModelCreationExceptionMessage, customMap.Name));
            }
        }
    }
}
