﻿using System;
using System.Collections.Generic;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure.IO.GraphSON;
using Xunit;

namespace Gremlin.Net.UnitTest.Structure.IO.GraphSON
{
    public class GraphSONWriterTests
    {
        [Fact]
        public void ArraySerializationTest()
        {
            var writer = CreateStandardGraphSONWriter();
            var array = new[] {5,6};

            var serializedGraphSON = writer.WriteObject(array);

            var expectedGraphSON = "[{\"@type\":\"g:Int32\",\"@value\":5},{\"@type\":\"g:Int32\",\"@value\":6}]";
            Assert.Equal(expectedGraphSON, serializedGraphSON);
        }

        [Fact]
        public void ListSerializationTest()
        {
            var writer = CreateStandardGraphSONWriter();
            var list = new List<int> {5, 6};

            var serializedGraphSON = writer.WriteObject(list.ToArray());

            var expectedGraphSON = "[{\"@type\":\"g:Int32\",\"@value\":5},{\"@type\":\"g:Int32\",\"@value\":6}]";
            Assert.Equal(expectedGraphSON, serializedGraphSON);
        }

        [Fact]
        public void DictionarySerializationTest()
        {
            var writer = CreateStandardGraphSONWriter();
            var dictionary = new Dictionary<string, dynamic>
            {
                {"age", new List<int> {29}},
                {"name", new List<string> {"marko"}}
            };

            var serializedDict = writer.WriteObject(dictionary);

            var expectedGraphSON = "{\"age\":[{\"@type\":\"g:Int32\",\"@value\":29}],\"name\":[\"marko\"]}";
            Assert.Equal(expectedGraphSON, serializedDict);

        }

        [Fact]
        public void EnumSerializationTest()
        {
            var writer = CreateStandardGraphSONWriter();

            var serializedEnum = writer.WriteObject(T.label);

            var expectedGraphSON = "{\"@type\":\"g:T\",\"@value\":\"label\"}";
            Assert.Equal(expectedGraphSON, serializedEnum);
        }

        [Fact]
        public void PredicateWithSingleValueSerializationTest()
        {
            var writer = CreateStandardGraphSONWriter();
            var predicate = new TraversalPredicate("lt", 5);

            var serializedPredicate = writer.WriteObject(predicate);

            var expectedGraphSON =
                "{\"@type\":\"g:P\",\"@value\":{\"predicate\":\"lt\",\"value\":{\"@type\":\"g:Int32\",\"@value\":5}}}";
            Assert.Equal(expectedGraphSON, serializedPredicate);
        }

        [Fact]
        public void PredicateSerializationTest()
        {
            var writer = CreateStandardGraphSONWriter();
            var predicate = new TraversalPredicate("within", new List<int> {1, 2});

            var serializedPredicate = writer.WriteObject(predicate);

            var expectedGraphSON =
                "{\"@type\":\"g:P\",\"@value\":{\"predicate\":\"within\",\"value\":[{\"@type\":\"g:Int32\",\"@value\":1},{\"@type\":\"g:Int32\",\"@value\":2}]}}";
            Assert.Equal(expectedGraphSON, serializedPredicate);
        }

        [Fact]
        public void CustomSerializationTest()
        {
            var customSerializerByType = new Dictionary<Type, IGraphSONSerializer>
            {
                {typeof(TestClass), new TestGraphSONSerializer {TestNamespace = "NS"} }
            };
            var writer = new GraphSONWriter(customSerializerByType);
            var testObj = new TestClass {Value = "test"};

            var serialized = writer.WriteObject(testObj);

            Assert.Equal("{\"@type\":\"NS:TestClass\",\"@value\":\"test\"}", serialized);
        }

        private GraphSONWriter CreateStandardGraphSONWriter()
        {
            return new GraphSONWriter();
        }
    }

    internal enum T
    {
        label
    }

    internal class TestGraphSONSerializer : IGraphSONSerializer
    {
        public string TestNamespace { get; set; }

        public Dictionary<string, dynamic> Dictify(dynamic objectData, GraphSONWriter writer)
        {
            return GraphSONUtil.ToTypedValue(nameof(TestClass), objectData.Value, TestNamespace);
        }
    }
}