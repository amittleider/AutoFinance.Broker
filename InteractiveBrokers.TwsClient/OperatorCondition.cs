using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBApi
{
    public abstract class OperatorCondition : OrderCondition
    {
        protected abstract string Value { get; set; }
        public Boolean IsMore { get; set; }

        const string header = " is ";

        public override string ToString()
        {
            return header + (IsMore ? ">= " : "<= ") + Value;
        }

        protected override bool TryParse(string cond)
        {
            if (!cond.StartsWith(header))
                return false;

            try
            {
                cond = cond.Replace(header, "");

                if (!cond.StartsWith(">=") && !cond.StartsWith("<="))
                    return false;

                IsMore = cond.StartsWith(">=");

                if (base.TryParse(cond.Substring(cond.LastIndexOf(" "))))
                    cond = cond.Substring(0, cond.LastIndexOf(" "));

                Value = cond.Substring(3);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public override void Deserialize(IDecoder inStream)
        {
            base.Deserialize(inStream);

            IsMore = inStream.ReadBoolFromInt();
            Value = inStream.ReadString();
        }

        public override void Serialize(System.IO.BinaryWriter outStream)
        {
            base.Serialize(outStream);
            outStream.AddParameter(IsMore);
            outStream.AddParameter(Value);
        }
    }
}
