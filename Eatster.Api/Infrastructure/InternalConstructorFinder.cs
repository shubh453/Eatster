using Autofac.Core.Activators.Reflection;
using System;
using System.Linq;
using System.Reflection;

namespace Eatster.Api.Infrastructure
{
    internal class InternalConstructorFinder : IConstructorFinder
    {
        public ConstructorInfo[] FindConstructors(Type t) => t.GetTypeInfo().DeclaredConstructors.Where(c => !c.IsPrivate && !c.IsPublic).ToArray();
    }
}