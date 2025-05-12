using System.Reflection;
using AutoMapper;

namespace DevPack.Core.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        const string mappingMethodName = nameof(IMapFrom<object>.Mapping);

        var mapFromType = typeof(IMapFrom<>);
        var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(HasInterface)).ToList();
        var argumentTypes = new Type[] { typeof(Profile) };
        
        types.ForEach(type =>
        {
            var instance = Activator.CreateInstance(type);
            var methodInfo = type.GetMethod(mappingMethodName);
            
            if (methodInfo is not null)
            {
                methodInfo.Invoke(instance, new object[] { this });
            }
            else
            {
                var interfaces = type.GetInterfaces().Where(HasInterface).ToList();
                
                if (interfaces.Any())
                {
                    interfaces.ForEach(@interface =>
                    {
                        var interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes);
                        interfaceMethodInfo?.Invoke(instance, new object[] { this });
                    });
                }
            }
        });
        return;


        bool HasInterface(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType;
    }
}