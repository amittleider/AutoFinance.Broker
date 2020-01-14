/* Copyright (C) 2013 Interactive Brokers LLC. All rights reserved.  This code is subject to the terms
 * and conditions of the IB API Non-Commercial License or the IB API Commercial License, as applicable. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace IBApi
{
    /**
    * @brief Delta-Neutral Underlying Component.
    */
    public class UnderComp
    {
        private int conId;
        private double delta;
        private double price;

        /**
         * @brief The unique contract identifier specifying the security. Used for Delta-Neutral Combo contracts.
         */
        public int ConId
        {
            get { return conId; }
            set { conId = value; }
        }

        /**
        * @brief The underlying stock or future delta. Used for Delta-Neutral Combo contracts.
        */
        public double Delta
        {
            get { return delta; }
            set { delta = value; }
        }

        /**
        * @brief The price of the underlying. Used for Delta-Neutral Combo contracts.
        */
        public double Price
        {
            get { return price; }
            set { price = value; }
        }
    }
}
