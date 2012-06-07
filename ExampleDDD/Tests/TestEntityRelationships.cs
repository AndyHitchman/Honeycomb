using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DDD;
using NUnit.Framework;
using PersonContext.DogAggregate;

namespace Tests
{
    [TestFixture]
    public class TestEntityRelationships
    {
        private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance;

        [Test]
        // Try to find any aggregate root or entity that refers to an entity in a different aggregate        
        public void EntityShouldNotReferenceEntitiesOutOfBounds()
        {
            var allEntitiesAndAggregates = new List<Type>();

            string binPath = System.AppDomain.CurrentDomain.BaseDirectory;

            var allAssemblies = Directory.GetFiles(binPath, "*.dll").Select(Assembly.LoadFile).ToList();

            Func<Type, bool> isEntityType =
                (type) =>
                type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == (typeof (Entity<>));
            Func<Type, bool> isAggregateRootType =
                (type) =>
                type.BaseType != null && type.BaseType == (typeof(AggregateRoot));

            foreach (var referencedAssembly in allAssemblies)
            {
                var entitiesAndAggregatesForAssembly = referencedAssembly.GetTypes().Where(_ => isEntityType(_) || isAggregateRootType(_)).ToList();
                allEntitiesAndAggregates.AddRange(entitiesAndAggregatesForAssembly);
            }

            foreach (var entityAggregateType in allEntitiesAndAggregates)
            {
                Func<Type, Type, bool> isBadEntityAccessFromEntity = null;
                isBadEntityAccessFromEntity = (badType, entityConatainerType) =>
                                              (isEntityType(badType) && badType.BaseType.GetGenericArguments() != entityConatainerType.BaseType.GetGenericArguments()) ||
                                              badType.IsGenericType && badType.GetGenericArguments().Any(_ => isBadEntityAccessFromEntity(_, entityConatainerType));


                Func<Type, Type, bool> isBadEntityAccessFromAggregate = null;
                isBadEntityAccessFromAggregate = (badType, aggregateConatainerType) => (isEntityType(badType) && badType.BaseType.GetGenericArguments().Any(_ => _ != aggregateConatainerType)) ||
                                            badType.IsGenericType && badType.GetGenericArguments().Any(_ => isBadEntityAccessFromAggregate(_, aggregateConatainerType));

                Func<Type, Type, bool> isBadAggregateAccessFromEntity = null;
                isBadAggregateAccessFromEntity = (badType, entityConatainerType) => (isAggregateRootType(badType) && entityConatainerType.BaseType.GetGenericArguments().Any(_ => _ != badType)) ||
                                                                                    (badType.IsGenericType && badType.GetGenericArguments().Any(_ => isBadAggregateAccessFromEntity(_, entityConatainerType)));

                Func<Type, bool> badMemberType = type =>
                                                 isBadAggregateAccessFromEntity(type, entityAggregateType) ||
                                                 isBadEntityAccessFromAggregate(type, entityAggregateType) ||
                                                 isBadAggregateAccessFromEntity(type, entityAggregateType);




                var hasInvalidReference = entityAggregateType.GetProperties(bindingFlags).Any(_ => badMemberType(_.PropertyType)) ||
                                          entityAggregateType.GetFields(bindingFlags).Any(_ => badMemberType(_.FieldType));
                
                if (hasInvalidReference)
                {
                    Assert.True(!hasInvalidReference);
                }
            }
        }
    }
}
