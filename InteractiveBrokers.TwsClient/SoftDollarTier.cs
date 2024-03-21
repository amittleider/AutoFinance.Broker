/* Copyright (C) 2019 Interactive Brokers LLC. All rights reserved. This code is subject to the terms
 * and conditions of the IB API Non-Commercial License or the IB API Commercial License, as applicable. */

namespace IBApi
{
    /**
     * @class SoftDollarTier
     * @brief A container for storing Soft Dollar Tier information
     */
    public class SoftDollarTier
    {
        /**
         * @brief The name of the Soft Dollar Tier
         */
        public string Name { get; set; }

        /**
         * @brief The value of the Soft Dollar Tier
         */
        public string Value { get; set; }

        /**
         * @brief The display name of the Soft Dollar Tier
         */
        public string DisplayName { get; set; }

        public SoftDollarTier(string name, string value, string displayName)
        {
            Name = name;
            Value = value;
            DisplayName = displayName;
        }

        public SoftDollarTier()
            : this(null, null, null)
        {
        }

        public override bool Equals(object obj)
        {
            SoftDollarTier b = obj as SoftDollarTier;

            if (Equals(b, null))
                return false;

            return string.Compare(Name, b.Name, true) == 0 && string.Compare(Value, b.Value, true) == 0;
        }

        public override int GetHashCode()
        {
            return (Name ?? "").GetHashCode() + (Value ?? "").GetHashCode();
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
            return DisplayName;
        }
    }
}
