using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBApi
{
    public interface IDecoder
    {
        double ReadDouble();
        double ReadDoubleMax();
        long ReadLong();
        int ReadInt();
        int ReadIntMax();
        bool ReadBoolFromInt();
        string ReadString();
    }
}
