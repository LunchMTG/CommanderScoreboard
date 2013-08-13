using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DI
{
    public class Container
    {
        public static Container Global { get { return Nested.instance; } }

        private readonly IDictionary<Type, LinkedList<Type>> _register;
        private readonly IDictionary<Type, object> _instanceRegister;

        public Container()
        {
            _register = new Dictionary<Type, LinkedList<Type>>();
            _instanceRegister = new Dictionary<Type, object>();
        }

        public void Register<I, C>()
        {
            Type i = typeof(I);
            Type c = typeof(C);

            ValidateInterfaceType(i, true);
            ValidateConcreateType(c, true);

            if (_register.ContainsKey(i))
            {
                _register[i].AddLast(c);
            }
            else
            {
                LinkedList<Type> types = new LinkedList<Type>();
                types.AddLast(c);
                _register.Add(i, types);
            }
        }

        public void RegisterInstance<I>(I instance)
        {
            Type i = typeof(I);
            ValidateInterfaceType(i, true);

            if (_instanceRegister.ContainsKey(i))
                throw new RegisterException(i.Name + " instance already registered");
            _instanceRegister.Add(i, instance);
        }

        public T Resolve<T>()
        {
            Type t = typeof(T);

            return (T)(ResolveInstance(t) ?? Resolve(t));
        }

        private object ResolveInstance(Type t)
        {
            if (_instanceRegister.ContainsKey(t))
                return _instanceRegister[t];
            return null;
        }

        private object Resolve(Type t)
        {
            if (t.IsInterface)
            {
                if (!_register.ContainsKey(t))
                    throw new ResolveException(String.Format("Unable to resolve type {0}. You must register the interface first", t.Name));

                Type c = _register[t].First();

                return Create(c);
            }
            else
            {                
                var ctrs = t.GetConstructors();

                ValidateConcreateType(t, false, ctrs);

                return Create(t, ctrs);
            }
        }

        private object Create(Type c, ConstructorInfo[] constructors = null)
        {
            constructors = constructors ?? c.GetConstructors();

            if (constructors.Length == 0)
                throw new ResolveException(String.Format("Type {0} must have a public constructor", c.Name));

            ConstructorInfo constructor = constructors.First();
            var cParameters = constructor.GetParameters();
            object[] parameters = new object[cParameters.Length];
            for (int i = 0; i < cParameters.Length; i++)
            {
                ParameterInfo pi = cParameters[i];
                parameters[i] = ResolveInstance(pi.ParameterType) ?? Resolve(pi.ParameterType);
            }

            return constructor.Invoke(parameters);
        }


        private static void ValidateInterfaceType(Type i, bool registering)
        {
            if (!i.IsInterface)
            {
                string message = String.Format("Type I must be an interface", i.Name);
                if (registering) throw new RegisterException(message);
                else throw new ResolveException(message);
            }
        }

        private static void ValidateConcreateType(Type c, bool registering, ConstructorInfo[] constructors = null)
        {
            if (c.IsInterface || c.IsAbstract)
            {
                string message = String.Format("Type {0} must be not be abstract or an interface", c.Name);
                if (registering) throw new RegisterException(message);
                else throw new ResolveException(message);
            }
            if ((constructors ?? c.GetConstructors()).Length == 0)
            {
                string message = String.Format("Type {0} must have a public constructor", c.Name);
                if (registering) throw new RegisterException(message);
                else throw new ResolveException(message);
            }
        }

        class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested() { }
            internal static readonly Container instance = new Container();
        }
    }
}
