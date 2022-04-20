/*  
 *  Developed under MIT Licence for academic purposes.
 *
 *  Author: Fikri Aydemir
 *  Date  :	21/04/2022 
 */

using IonClassLibrary.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IonClassLibrary.Filter
{
    public static class TypeInfoDeducerExtensions
    {

        private const string STR_IonOptions = "IonOptions";

        public static IServiceCollection AddIonOptions(this IServiceCollection collection, IConfiguration configuration)
        {
            var ionOptions = configuration.GetOptions<IonOptions>(STR_IonOptions);
            if (ionOptions != null && ionOptions.EnableIon) return collection.AddTypeToConfig<IonOptions>(configuration, STR_IonOptions);
            else return collection;
        }

        private static TModel GetOptions<TModel>(this IConfiguration configuration, string sectionName) where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(sectionName).Bind(model);
            return model;
        }


        private static IServiceCollection AddTypeToConfig<T>(this IServiceCollection collection, IConfiguration configuration, string section) where T : class
        {
            var value = configuration.GetSection(section).Get<T>();

            if (value == null)
                throw new System.Exception($"Please enter '{section}' in your config");

            collection.AddSingleton(value);
            return collection;

        }

        public static IEnumerable<ConstructorInfo> GetAllConstructors(this TypeInfo typeInfo)
            => typeInfo.GetAll(ti => ti.DeclaredConstructors);

        public static IEnumerable<EventInfo> GetAllEvents(this TypeInfo typeInfo)
            => typeInfo.GetAll(ti => ti.DeclaredEvents);

        public static IEnumerable<FieldInfo> GetAllFields(this TypeInfo typeInfo)
            => typeInfo.GetAll(ti => ti.DeclaredFields);

        public static IEnumerable<MemberInfo> GetAllMembers(this TypeInfo typeInfo)
            => typeInfo.GetAll(ti => ti.DeclaredMembers);

        public static IEnumerable<MethodInfo> GetAllMethods(this TypeInfo typeInfo)
            => typeInfo.GetAll(ti => ti.DeclaredMethods);

        public static IEnumerable<TypeInfo> GetAllNestedTypes(this TypeInfo typeInfo)
            => typeInfo.GetAll(ti => ti.DeclaredNestedTypes);

        public static IEnumerable<PropertyInfo> GetAllProperties(this TypeInfo typeInfo)
            => typeInfo.GetAll(ti => ti.DeclaredProperties);

        private static IEnumerable<T> GetAll<T>(this TypeInfo typeInfo, Func<TypeInfo, IEnumerable<T>> accessor)
        {
            while (typeInfo != null)
            {
                foreach (var t in accessor(typeInfo))
                {
                    yield return t;
                }

                typeInfo = typeInfo.BaseType?.GetTypeInfo();
            }
        }
    }
}
