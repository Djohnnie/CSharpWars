using System;
using Adic.Container;

namespace Assets.Scripts.Helpers
{
    public static class InjectionContainerExtensions
    {
        public static IInjectionContainer Group(
            this IInjectionContainer container,
            string groupName,
            Action<IInjectionContainer> func)
        {
            func(container);
            return container;
        }
    }
}