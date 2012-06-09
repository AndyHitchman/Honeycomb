using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DDD;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TestEntityRelationships
    {
        private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance;

        Func<Type, bool> isEntityType =
            (type) =>
            type.BaseType != null && type.BaseType.IsGenericType &&
            type.BaseType.GetGenericTypeDefinition() == (typeof(Entity<>));
        Func<Type, bool> isAggregateRootType =
            (type) =>
            type.BaseType != null && type.BaseType == (typeof(AggregateRoot));

        private Func<Type, Type, bool> isBadEntityAccessFromEntity = null;
        private Func<Type, Type, bool> isBadEntityAccessFromAggregate = null;
        private Func<Type, Type, bool> isBadAggregateAccessFromEntity = null;
        private Func<Type, Type, bool> badMemberType = null;

        [SetUp]
        public void TestSetup()
        {
            isBadEntityAccessFromEntity = (badType, entityConatainerType) =>
                                              (isEntityType(badType) && isEntityType(entityConatainerType) && !badType.BaseType.GetGenericArguments().SequenceEqual(entityConatainerType.BaseType.GetGenericArguments())) ||
                                              badType.IsGenericType && badType.GetGenericArguments().Any(_ => isBadEntityAccessFromEntity(_, entityConatainerType));

            isBadEntityAccessFromAggregate = (badType, aggregateConatainerType) => 
                    (isEntityType(badType) && isAggregateRootType(aggregateConatainerType) && badType.BaseType.GetGenericArguments().Any(_ => _ != aggregateConatainerType)) ||
                                        badType.IsGenericType && badType.GetGenericArguments().Any(_ => isBadEntityAccessFromAggregate(_, aggregateConatainerType));

            /*
             * This one isn't needed because we don't care if entities access other aggregate roots. Thought it would a useful definition though.
             */
            isBadAggregateAccessFromEntity = (badType, entityConatainerType) => 
                (isAggregateRootType(badType) && isEntityType(entityConatainerType) && entityConatainerType.BaseType.GetGenericArguments().Any(_ => _ != badType)) ||
                (badType.IsGenericType && badType.GetGenericArguments().Any(_ => isBadAggregateAccessFromEntity(_, entityConatainerType)));

            badMemberType = (type, entityContainerType) =>
                                             isBadEntityAccessFromEntity(type, entityContainerType) ||
                                             isBadEntityAccessFromAggregate(type, entityContainerType);
        }

        [Test]
        // Try to find any aggregate root or entity that refers to an entity in a different aggregate        
        public void EntityShouldNotReferenceEntitiesOutOfBounds()
        {
            var allEntitiesAndAggregates = getAllEntitiesAndAggregates();

            foreach (var entityAggregateType in allEntitiesAndAggregates)
            {
                var hasInvalidReference = entityAggregateType.GetProperties(bindingFlags).Any(_ => badMemberType(_.PropertyType, entityAggregateType)) ||
                                          entityAggregateType.GetFields(bindingFlags).Any(_ => badMemberType(_.FieldType, entityAggregateType)) ||
                                          entityAggregateType.GetMethods(bindingFlags).Any(_ => 
                                              _.GetParameters().Any(par => badMemberType(par.ParameterType, entityAggregateType)) ||
                                              badMemberType(_.ReturnType, entityAggregateType));
                
                if (hasInvalidReference)
                {
                    Assert.True(!hasInvalidReference);
                }
            }
        }

        private List<Type> getAllEntitiesAndAggregates()
        {
            var allEntitiesAndAggregates = new List<Type>();

            string binPath = System.AppDomain.CurrentDomain.BaseDirectory;

            var allAssemblies = Directory.GetFiles(binPath, "*.dll").Select(Assembly.LoadFile).ToList();

            foreach (var referencedAssembly in allAssemblies)
            {
                var entitiesAndAggregatesForAssembly =
                    referencedAssembly.GetTypes().Where(_ => isEntityType(_) || isAggregateRootType(_)).ToList();
                allEntitiesAndAggregates.AddRange(entitiesAndAggregatesForAssembly);
            }

            return allEntitiesAndAggregates;
        }
    }
}
