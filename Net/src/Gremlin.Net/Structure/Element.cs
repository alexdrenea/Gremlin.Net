﻿using System;

namespace Gremlin.Net.Structure
{
    public abstract class Element : IEquatable<Element>
    {
        protected Element(object id, string label)
        {
            Id = id;
            Label = label;
        }

        public object Id { get; }
        public string Label { get; }

        public bool Equals(Element other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Element) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}