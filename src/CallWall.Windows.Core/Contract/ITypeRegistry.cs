using System.Collections.Generic;

namespace CallWall.Windows.Contract
{
    public interface ITypeRegistry
    {
        void RegisterTypeAsSingleton<TFrom, TTo>() 
            where TTo : TFrom;
        
        void RegisterTypeAsTransient<TFrom, TTo>() 
            where TTo : TFrom;

        /// <summary>
        /// Used to register a type as a named registration of a Base/Shared type.
        /// </summary>
        /// <typeparam name="TFrom">The base interface to be registered as a named instance to.</typeparam>
        /// <typeparam name="TTo">The actual implementation type</typeparam>
        /// <remarks>
        /// This is normally used for registering a type that is to be used in a collection of implementations i.e. is injected 
        /// as part of an <see cref="IEnumerable{T}"/>. The name used will be the full type name of the <typeparamref name="TTo"/>.
        /// </remarks>
        void RegisterCompositeAsSingleton<TFrom, TTo>()
            where TTo : TFrom;

        /// <summary>
        /// Used to register a type as a named registration of a Base/Shared type and also as the default implementation of a specialized interface.
        /// </summary>
        /// <typeparam name="TBase">The base interface to be registered as a named instance to.</typeparam>
        /// <typeparam name="TSpecialization">The specialized interface.</typeparam>
        /// <typeparam name="TImplementation">The actual implementation type</typeparam>
        /// <remarks>
        /// This is normally used for registering a type that is to be used in a collection of implementations i.e. is injected 
        /// as part of an <see cref="IEnumerable{T}"/>, but that implementation also needs to be referenced directly. You could use
        /// name but then you have nasty strings in your code, and potentially you need to extend the base interface for the specialization.
        /// The name used will be the full type name of the <typeparamref name="TImplementation"/>.
        /// </remarks>
        void RegisterCompositeAsSingleton<TBase, TSpecialization, TImplementation>()
            where TImplementation : TSpecialization
            where TSpecialization : TBase;
    }
}
