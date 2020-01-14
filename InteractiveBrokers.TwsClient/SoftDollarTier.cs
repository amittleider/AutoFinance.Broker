using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBApi
{
    public class SoftDollarTier
    {
        public string Name { get; private set; }
        public string Value { get; private set; }
        public string DisplayName { get; private set; }

        public SoftDollarTier(string name, string value, string displayName)
        {
            this.Name = name;
            this.Value = value;
            this.DisplayName = displayName;
        }

        public override bool Equals(object obj)
        {
            SoftDollarTier b = obj as SoftDollarTier;

            if (object.Equals(b, null))
                return false;

            return string.Compare(Name, b.Name, true) == 0 && string.Compare(Value, b.Value, true) == 0;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + Value.GetHashCode();
        }

        public static bool operator ==(SoftDollarTier left, SoftDollarTier right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SoftDollarTier left, SoftDollarTier right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}
