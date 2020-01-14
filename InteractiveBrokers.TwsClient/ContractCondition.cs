using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBApi
{
	public abstract class ContractCondition : OperatorCondition
    {
        public int ConId { get; set; }
        public string Exchange { get; set; }

        public const string delimiter = " of ";

        public Func<int, string, string> ContractResolver { get; set; }

        public ContractCondition()
        {
            ContractResolver = (conid, exch) => conid + "(" + exch + ")";
        }

        public override string ToString()
        {
            return Type + delimiter + ContractResolver(ConId, Exchange) + base.ToString();
        }

        protected override bool TryParse(string cond)
        {
            try
            {
                if (cond.Substring(0, cond.IndexOf(delimiter)) != Type.ToString())
                    return false;

                cond = cond.Substring(cond.IndexOf(delimiter) + delimiter.Length);
                int conid;

                if (!int.TryParse(cond.Substring(0, cond.IndexOf("(")), out conid))
                    return false;

                ConId = conid;
                cond = cond.Substring(cond.IndexOf("(") + 1);
                Exchange = cond.Substring(0, cond.IndexOf(")"));
                cond = cond.Substring(cond.IndexOf(")") + 1);

                return base.TryParse(cond);
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

            ConId = inStream.ReadInt();
            Exchange = inStream.ReadString();
        }

        public override void Serialize(System.IO.BinaryWriter outStream)
        {
            base.Serialize(outStream);
            outStream.AddParameter(ConId);
            outStream.AddParameter(Exchange);
        }
    }
}
