﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gremlin.Net.Process.Traversal.Strategy
{
    public abstract class AbstractTraversalStrategy : ITraversalStrategy, IEquatable<AbstractTraversalStrategy>
    {
        public string StrategyName => GetType().Name;
        public Dictionary<string, dynamic> Configuration { get; } = new Dictionary<string, dynamic>();

        public bool Equals(AbstractTraversalStrategy other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return StrategyName == other.StrategyName;
        }

        public virtual void Apply(Traversal traversal)
        {
        }

        public virtual Task ApplyAsync(Traversal traversal)
        {
            return Task.CompletedTask;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((AbstractTraversalStrategy) obj);
        }

        public override int GetHashCode()
        {
            return StrategyName.GetHashCode();
        }

        public override string ToString()
        {
            return StrategyName;
        }
    }
}