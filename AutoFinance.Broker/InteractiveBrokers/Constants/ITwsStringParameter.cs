// Licensed under the Apache License, Version 2.0.

namespace AutoFinance.Broker.InteractiveBrokers.Constants
{
    /// <summary>
    /// String parameter conversion for TWS parameters
    /// </summary>
    public interface ITwsStringParameter
    {
        /// <summary>
        /// Conver to the TWS string parameter version
        /// </summary>
        /// <returns>The TWS parameter</returns>
        string ToTwsParameter();
    }
}
