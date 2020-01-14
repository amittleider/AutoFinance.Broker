// Copyright (c) Andrew Mittleider. All Rights Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.

namespace AutoFinance.Broker.IntegrationTests.InteractiveBrokers
{
    using System;
    using System.Threading;
    using AutoFinance.Broker.InteractiveBrokers.Constants;
    using IBApi;
    using Moq;
    using Xunit;

    /// <summary>
    /// Tests the TWS EClientSocket communication.
    /// </summary>
    public class EClientSocketTests
    {
        /// <summary>
        /// Test that the wrapper properly connects to TWS.
        /// These tests require that TWS be running on the local machine.
        /// They also require that the API is enabled, and that 127.0.0.1 and 0:0:0:0:0:0:0:1 are in the Allowed Hosts section of the TWS configuration.
        /// The setting can be found in the TWS window under "Account -> Global Settings -> API".
        /// </summary>
        [Fact]
        public void TwsConnection_Should_BeEstablished()
        {
            // Setup
            Mock<EWrapper> mockTwsWrapper = new Mock<EWrapper>();
            mockTwsWrapper.Setup(mock => mock.connectAck());
            mockTwsWrapper.Setup(mock => mock.connectionClosed());

            EReaderMonitorSignal signal = new EReaderMonitorSignal();
            EClientSocket clientSocket = new EClientSocket(mockTwsWrapper.Object, signal);

            // Call
            clientSocket.eConnect("localhost", 7462, 2);
            clientSocket.eDisconnect();

            // Assert
            mockTwsWrapper.Setup(mock => mock.connectAck());
            mockTwsWrapper.Setup(mock => mock.connectionClosed());
        }

        /// <summary>
        /// Test how to obtain a contract through the API
        /// Simply test that a contract is returned through the callback API with a Mock
        /// </summary>
        [Fact]
        public void ContractDetailsRequest_Should_Callback()
        {
            // Setup
            Mock<EWrapper> mockTwsWrapper = new Mock<EWrapper>();
            mockTwsWrapper.Setup(mock => mock.contractDetails(It.IsAny<int>(), It.IsAny<ContractDetails>()));
            mockTwsWrapper.Setup(mock => mock.contractDetailsEnd(It.IsAny<int>()));

            EReaderMonitorSignal signal = new EReaderMonitorSignal();
            EClientSocket clientSocket = new EClientSocket(mockTwsWrapper.Object, signal);

            clientSocket.eConnect("localhost", 7462, 2);

            // Create a reader to consume messages from the TWS. The EReader will consume the incoming messages and put them in a queue
            // Be very careful with the order. The EReader constructor must be called after a call to clientSocket.eConnect().
            var reader = new EReader(clientSocket, signal);
            reader.Start();

            // Process messages when the signal comes
            Contract contract = new Contract();
            contract.SecType = TwsContractSecType.Future;
            contract.Symbol = TwsSymbol.Dax;
            contract.Exchange = TwsExchange.Dtb;
            contract.Currency = TwsCurrency.Eur;
            contract.Multiplier = "25";
            contract.LastTradeDateOrContractMonth = "201809";

            Thread thread = new Thread(
             () =>
             {
                 while (true)
                 {
                     signal.waitForSignal();
                     reader.processMsgs();
                 }
             })
            { IsBackground = true };
            thread.Start();

            // Call
            clientSocket.reqContractDetails(220, contract);

            // Wait for the response and tear down the connection
            Thread.Sleep(1000); // Let it run for a second
            clientSocket.eDisconnect();

            // Assert
            mockTwsWrapper.Verify(mock => mock.contractDetails(It.IsAny<int>(), It.IsAny<ContractDetails>()), Times.Once());
            mockTwsWrapper.Verify(mock => mock.contractDetailsEnd(It.IsAny<int>()), Times.Once());
        }

        /// <summary>
        /// Test receiving account updates
        /// Test that the account updates callback method is called on a Mock
        /// </summary>
        [Fact]
        public void RequestAccountUpdates_Should_Callback()
        {
            // Setup
            Mock<EWrapper> mockTwsWrapper = new Mock<EWrapper>();
            mockTwsWrapper.Setup(mock => mock.updateAccountValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            mockTwsWrapper.Setup(mock => mock.updatePortfolio(It.IsAny<Contract>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<string>()));
            mockTwsWrapper.Setup(mock => mock.accountDownloadEnd(It.IsAny<string>()));

            EReaderMonitorSignal signal = new EReaderMonitorSignal();
            EClientSocket clientSocket = new EClientSocket(mockTwsWrapper.Object, signal);

            clientSocket.eConnect("localhost", 7462, 2);

            // Create a reader to consume messages from the TWS. The EReader will consume the incoming messages and put them in a queue
            // Be very careful with the order. The EReader constructor must be called after a call to clientSocket.eConnect().
            var reader = new EReader(clientSocket, signal);
            reader.Start();

            Thread thread = new Thread(
             () =>
             {
                 while (true)
                 {
                     signal.waitForSignal();
                     reader.processMsgs();
                 }
             })
            { IsBackground = true };
            thread.Start();

            // Call
            clientSocket.reqAccountUpdates(true, "DU1052488"); // The 'subscription' parameter set to false does nothing

            // Wait for the response and tear down the connection
            Thread.Sleep(1000);
            clientSocket.eDisconnect();

            // Assert
            // The updateAccountValue and updatePortfolio are called many time, accountDownloadEnd is called once
            // Here is an example of the calls:
            // EWrapper.connectAck()
            // EWrapper.managedAccounts("DU348954")
            // EWrapper.nextValidId(1)
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:jfarm")
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:usfuture")
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:eufarm")
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:cashfarm")
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:usfarm.us")
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:usfarm")
            // EWrapper.error(-1, 2106, "HMDS data farm connection is OK:euhmds")
            // EWrapper.error(-1, 2106, "HMDS data farm connection is OK:hkhmds")
            // EWrapper.error(-1, 2106, "HMDS data farm connection is OK:ushmds")
            // EWrapper.updateAccountValue("AccountCode", "DU348954", null, "DU348954")
            // EWrapper.updateAccountValue("AccountOrGroup", "DU348954", "BASE", "DU348954")
            // EWrapper.updateAccountValue("AccountOrGroup", "DU348954", "EUR", "DU348954")
            // EWrapper.updateAccountValue("AccountOrGroup", "DU348954", "GBP", "DU348954")
            // EWrapper.updateAccountValue("AccountOrGroup", "DU348954", "JPY", "DU348954")
            // EWrapper.updateAccountValue("AccountOrGroup", "DU348954", "USD", "DU348954")
            // EWrapper.updateAccountValue("AccountReady", "true", null, "DU348954")
            // EWrapper.updateAccountValue("AccountType", "CORPORATION", null, "DU348954")
            // EWrapper.updateAccountValue("AccruedCash", "-468", "BASE", "DU348954")
            // EWrapper.updateAccountValue("AccruedCash", "-468.45", "EUR", "DU348954")
            // EWrapper.updateAccountValue("AccruedCash", "0", "GBP", "DU348954")
            // EWrapper.updateAccountValue("AccruedCash", "0", "JPY", "DU348954")
            // EWrapper.updateAccountValue("AccruedCash", "-397", "USD", "DU348954")
            // EWrapper.updateAccountValue("AccruedCash-C", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("AccruedCash-S", "-468.45", "EUR", "DU348954")
            // EWrapper.updateAccountValue("AccruedDividend", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("AccruedDividend-C", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("AccruedDividend-S", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("AvailableFunds", "221720.10", "EUR", "DU348954")
            // EWrapper.updateAccountValue("AvailableFunds-C", "210820.79", "EUR", "DU348954")
            // EWrapper.updateAccountValue("AvailableFunds-S", "10899.31", "EUR", "DU348954")
            // EWrapper.updateAccountValue("Billable", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("Billable-C", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("Billable-S", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("BuyingPower", "886880.39", "EUR", "DU348954")
            // EWrapper.updateAccountValue("CashBalance", "228437", "BASE", "DU348954")
            // EWrapper.updateAccountValue("CashBalance", "362554", "EUR", "DU348954")
            // EWrapper.updateAccountValue("CashBalance", "64331", "GBP", "DU348954")
            // EWrapper.updateAccountValue("CashBalance", "70516", "JPY", "DU348954")
            // EWrapper.updateAccountValue("CashBalance", "-245925", "USD", "DU348954")
            // EWrapper.updateAccountValue("CorporateBondValue", "0", "BASE", "DU348954")
            // EWrapper.updateAccountValue("CorporateBondValue", "0", "EUR", "DU348954")
            // EWrapper.updateAccountValue("CorporateBondValue", "0", "GBP", "DU348954")
            // EWrapper.updateAccountValue("CorporateBondValue", "0", "JPY", "DU348954")
            // EWrapper.updateAccountValue("CorporateBondValue", "0", "USD", "DU348954")
            // EWrapper.updateAccountValue("Currency", "BASE", "BASE", "DU348954")
            // EWrapper.updateAccountValue("Currency", "EUR", "EUR", "DU348954")
            // EWrapper.updateAccountValue("Currency", "GBP", "GBP", "DU348954")
            // EWrapper.updateAccountValue("Currency", "JPY", "JPY", "DU348954")
            // EWrapper.updateAccountValue("Currency", "USD", "USD", "DU348954")
            // EWrapper.updateAccountValue("Cushion", "0.972589", null, "DU348954")
            // EWrapper.updateAccountValue("DayTradesRemaining", "-1", null, "DU348954")
            // EWrapper.updateAccountValue("DayTradesRemainingT+1", "-1", null, "DU348954")
            // EWrapper.updateAccountValue("DayTradesRemainingT+2", "-1", null, "DU348954")
            // EWrapper.updateAccountValue("DayTradesRemainingT+3", "-1", null, "DU348954")
            // EWrapper.updateAccountValue("DayTradesRemainingT+4", "-1", null, "DU348954")
            // EWrapper.updateAccountValue("EquityWithLoanValue", "227968.97", "EUR", "DU348954")
            // EWrapper.updateAccountValue("EquityWithLoanValue-C", "210820.79", "EUR", "DU348954")
            // EWrapper.updateAccountValue("EquityWithLoanValue-S", "17148.19", "EUR", "DU348954")
            // EWrapper.updateAccountValue("ExcessLiquidity", "221720.10", "EUR", "DU348954")
            // EWrapper.updateAccountValue("ExcessLiquidity-C", "210820.79", "EUR", "DU348954")
            // EWrapper.updateAccountValue("ExcessLiquidity-S", "10899.31", "EUR", "DU348954")
            // EWrapper.updateAccountValue("ExchangeRate", "1.00", "BASE", "DU348954")
            // EWrapper.updateAccountValue("ExchangeRate", "1.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("ExchangeRate", "1.1395157", "GBP", "DU348954")
            // EWrapper.updateAccountValue("ExchangeRate", "0.0076238", "JPY", "DU348954")
            // EWrapper.updateAccountValue("ExchangeRate", "0.8456267", "USD", "DU348954")
            // EWrapper.updateAccountValue("FullAvailableFunds", "221720.10", "EUR", "DU348954")
            // EWrapper.updateAccountValue("FullAvailableFunds-C", "210820.79", "EUR", "DU348954")
            // EWrapper.updateAccountValue("FullAvailableFunds-S", "10899.31", "EUR", "DU348954")
            // EWrapper.updateAccountValue("FullExcessLiquidity", "221720.10", "EUR", "DU348954")
            // EWrapper.updateAccountValue("FullExcessLiquidity-C", "210820.79", "EUR", "DU348954")
            // EWrapper.updateAccountValue("FullExcessLiquidity-S", "10899.31", "EUR", "DU348954")
            // EWrapper.updateAccountValue("FullInitMarginReq", "6248.88", "EUR", "DU348954")
            // EWrapper.updateAccountValue("FullInitMarginReq-C", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("FullInitMarginReq-S", "6248.88", "EUR", "DU348954")
            // EWrapper.updateAccountValue("FullMaintMarginReq", "6248.88", "EUR", "DU348954")
            // EWrapper.updateAccountValue("FullMaintMarginReq-C", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("FullMaintMarginReq-S", "6248.88", "EUR", "DU348954")
            // EWrapper.updateAccountValue("FundValue", "0", "BASE", "DU348954")
            // EWrapper.updateAccountValue("FundValue", "0", "EUR", "DU348954")
            // EWrapper.updateAccountValue("FundValue", "0", "GBP", "DU348954")
            // EWrapper.updateAccountValue("FundValue", "0", "JPY", "DU348954")
            // EWrapper.updateAccountValue("FundValue", "0", "USD", "DU348954")
            // EWrapper.updateAccountValue("FutureOptionValue", "0", "BASE", "DU348954")
            // EWrapper.updateAccountValue("FutureOptionValue", "0", "EUR", "DU348954")
            // EWrapper.updateAccountValue("FutureOptionValue", "0", "GBP", "DU348954")
            // EWrapper.updateAccountValue("FutureOptionValue", "0", "JPY", "DU348954")
            // EWrapper.updateAccountValue("FutureOptionValue", "0", "USD", "DU348954")
            // EWrapper.updateAccountValue("FuturesPNL", "0", "BASE", "DU348954")
            // EWrapper.updateAccountValue("FuturesPNL", "0", "EUR", "DU348954")
            // EWrapper.updateAccountValue("FuturesPNL", "0", "GBP", "DU348954")
            // EWrapper.updateAccountValue("FuturesPNL", "0", "JPY", "DU348954")
            // EWrapper.updateAccountValue("FuturesPNL", "0", "USD", "DU348954")
            // EWrapper.updateAccountValue("FxCashBalance", "0", "BASE", "DU348954")
            // EWrapper.updateAccountValue("FxCashBalance", "0", "EUR", "DU348954")
            // EWrapper.updateAccountValue("FxCashBalance", "0", "GBP", "DU348954")
            // EWrapper.updateAccountValue("FxCashBalance", "0", "JPY", "DU348954")
            // EWrapper.updateAccountValue("FxCashBalance", "0", "USD", "DU348954")
            // EWrapper.updateAccountValue("GrossPositionValue", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("GrossPositionValue-S", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("Guarantee", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("Guarantee-C", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("Guarantee-S", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("IndianStockHaircut", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("IndianStockHaircut-C", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("IndianStockHaircut-S", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("InitMarginReq", "6248.88", "EUR", "DU348954")
            // EWrapper.updateAccountValue("InitMarginReq-C", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("InitMarginReq-S", "6248.88", "EUR", "DU348954")
            // EWrapper.updateAccountValue("IssuerOptionValue", "0", "BASE", "DU348954")
            // EWrapper.updateAccountValue("IssuerOptionValue", "0", "EUR", "DU348954")
            // EWrapper.updateAccountValue("IssuerOptionValue", "0", "GBP", "DU348954")
            // EWrapper.updateAccountValue("IssuerOptionValue", "0", "JPY", "DU348954")
            // EWrapper.updateAccountValue("IssuerOptionValue", "0", "USD", "DU348954")
            // EWrapper.updateAccountValue("Leverage-S", "0.00", null, "DU348954")
            // EWrapper.updateAccountValue("LookAheadAvailableFunds", "221720.10", "EUR", "DU348954")
            // EWrapper.updateAccountValue("LookAheadAvailableFunds-C", "210820.79", "EUR", "DU348954")
            // EWrapper.updateAccountValue("LookAheadAvailableFunds-S", "10899.31", "EUR", "DU348954")
            // EWrapper.updateAccountValue("LookAheadExcessLiquidity", "221720.10", "EUR", "DU348954")
            // EWrapper.updateAccountValue("LookAheadExcessLiquidity-C", "210820.79", "EUR", "DU348954")
            // EWrapper.updateAccountValue("LookAheadExcessLiquidity-S", "10899.31", "EUR", "DU348954")
            // EWrapper.updateAccountValue("LookAheadInitMarginReq", "6248.88", "EUR", "DU348954")
            // EWrapper.updateAccountValue("LookAheadInitMarginReq-C", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("LookAheadInitMarginReq-S", "6248.88", "EUR", "DU348954")
            // EWrapper.updateAccountValue("LookAheadMaintMarginReq", "6248.88", "EUR", "DU348954")
            // EWrapper.updateAccountValue("LookAheadMaintMarginReq-C", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("LookAheadMaintMarginReq-S", "6248.88", "EUR", "DU348954")
            // EWrapper.updateAccountValue("LookAheadNextChange", "0", null, "DU348954")
            // EWrapper.updateAccountValue("MaintMarginReq", "6248.88", "EUR", "DU348954")
            // EWrapper.updateAccountValue("MaintMarginReq-C", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("MaintMarginReq-S", "6248.88", "EUR", "DU348954")
            // EWrapper.updateAccountValue("MoneyMarketFundValue", "0", "BASE", "DU348954")
            // EWrapper.updateAccountValue("MoneyMarketFundValue", "0", "EUR", "DU348954")
            // EWrapper.updateAccountValue("MoneyMarketFundValue", "0", "GBP", "DU348954")
            // EWrapper.updateAccountValue("MoneyMarketFundValue", "0", "JPY", "DU348954")
            // EWrapper.updateAccountValue("MoneyMarketFundValue", "0", "USD", "DU348954")
            // EWrapper.updateAccountValue("MutualFundValue", "0", "BASE", "DU348954")
            // EWrapper.updateAccountValue("MutualFundValue", "0", "EUR", "DU348954")
            // EWrapper.updateAccountValue("MutualFundValue", "0", "GBP", "DU348954")
            // EWrapper.updateAccountValue("MutualFundValue", "0", "JPY", "DU348954")
            // EWrapper.updateAccountValue("MutualFundValue", "0", "USD", "DU348954")
            // EWrapper.updateAccountValue("NLVAndMarginInReview", "false", null, "DU348954")
            // EWrapper.updateAccountValue("NetDividend", "0", "BASE", "DU348954")
            // EWrapper.updateAccountValue("NetDividend", "0", "EUR", "DU348954")
            // EWrapper.updateAccountValue("NetDividend", "0", "GBP", "DU348954")
            // EWrapper.updateAccountValue("NetDividend", "0", "JPY", "DU348954")
            // EWrapper.updateAccountValue("NetDividend", "0", "USD", "DU348954")
            // EWrapper.updateAccountValue("NetLiquidation", "227968.97", "EUR", "DU348954")
            // EWrapper.updateAccountValue("NetLiquidation-C", "210820.79", "EUR", "DU348954")
            // EWrapper.updateAccountValue("NetLiquidation-S", "17148.19", "EUR", "DU348954")
            // EWrapper.updateAccountValue("NetLiquidationByCurrency", "227969", "BASE", "DU348954")
            // EWrapper.updateAccountValue("NetLiquidationByCurrency", "362421", "EUR", "DU348954")
            // EWrapper.updateAccountValue("NetLiquidationByCurrency", "64331", "GBP", "DU348954")
            // EWrapper.updateAccountValue("NetLiquidationByCurrency", "70516", "JPY", "DU348954")
            // EWrapper.updateAccountValue("NetLiquidationByCurrency", "-246321", "USD", "DU348954")
            // EWrapper.updateAccountValue("NetLiquidationUncertainty", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("OptionMarketValue", "0", "BASE", "DU348954")
            // EWrapper.updateAccountValue("OptionMarketValue", "0", "EUR", "DU348954")
            // EWrapper.updateAccountValue("OptionMarketValue", "0", "GBP", "DU348954")
            // EWrapper.updateAccountValue("OptionMarketValue", "0", "JPY", "DU348954")
            // EWrapper.updateAccountValue("OptionMarketValue", "0", "USD", "DU348954")
            // EWrapper.updateAccountValue("PASharesValue", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("PASharesValue-C", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("PASharesValue-S", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("PostExpirationExcess", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("PostExpirationExcess-C", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("PostExpirationExcess-S", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("PostExpirationMargin", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("PostExpirationMargin-C", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("PostExpirationMargin-S", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("PreviousDayEquityWithLoanValue", "16574.92", "EUR", "DU348954")
            // EWrapper.updateAccountValue("PreviousDayEquityWithLoanValue-S", "16574.92", "EUR", "DU348954")
            // EWrapper.updateAccountValue("RealCurrency", "BASE", "BASE", "DU348954")
            // EWrapper.updateAccountValue("RealCurrency", "EUR", "EUR", "DU348954")
            // EWrapper.updateAccountValue("RealCurrency", "GBP", "GBP", "DU348954")
            // EWrapper.updateAccountValue("RealCurrency", "JPY", "JPY", "DU348954")
            // EWrapper.updateAccountValue("RealCurrency", "USD", "USD", "DU348954")
            // EWrapper.updateAccountValue("RealizedPnL", "0", "BASE", "DU348954")
            // EWrapper.updateAccountValue("RealizedPnL", "0", "EUR", "DU348954")
            // EWrapper.updateAccountValue("RealizedPnL", "0", "GBP", "DU348954")
            // EWrapper.updateAccountValue("RealizedPnL", "0", "JPY", "DU348954")
            // EWrapper.updateAccountValue("RealizedPnL", "0", "USD", "DU348954")
            // EWrapper.updateAccountValue("RegTEquity", "17148.19", "EUR", "DU348954")
            // EWrapper.updateAccountValue("RegTEquity-S", "17148.19", "EUR", "DU348954")
            // EWrapper.updateAccountValue("RegTMargin", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("RegTMargin-S", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("SMA", "57744.28", "EUR", "DU348954")
            // EWrapper.updateAccountValue("SMA-S", "57744.28", "EUR", "DU348954")
            // EWrapper.updateAccountValue("SegmentTitle-C", "UK Commodities", null, "DU348954")
            // EWrapper.updateAccountValue("SegmentTitle-S", "UK Securities", null, "DU348954")
            // EWrapper.updateAccountValue("StockMarketValue", "0", "BASE", "DU348954")
            // EWrapper.updateAccountValue("StockMarketValue", "0", "EUR", "DU348954")
            // EWrapper.updateAccountValue("StockMarketValue", "0", "GBP", "DU348954")
            // EWrapper.updateAccountValue("StockMarketValue", "0", "JPY", "DU348954")
            // EWrapper.updateAccountValue("StockMarketValue", "0", "USD", "DU348954")
            // EWrapper.updateAccountValue("TBillValue", "0", "BASE", "DU348954")
            // EWrapper.updateAccountValue("TBillValue", "0", "EUR", "DU348954")
            // EWrapper.updateAccountValue("TBillValue", "0", "GBP", "DU348954")
            // EWrapper.updateAccountValue("TBillValue", "0", "JPY", "DU348954")
            // EWrapper.updateAccountValue("TBillValue", "0", "USD", "DU348954")
            // EWrapper.updateAccountValue("TBondValue", "0", "BASE", "DU348954")
            // EWrapper.updateAccountValue("TBondValue", "0", "EUR", "DU348954")
            // EWrapper.updateAccountValue("TBondValue", "0", "GBP", "DU348954")
            // EWrapper.updateAccountValue("TBondValue", "0", "JPY", "DU348954")
            // EWrapper.updateAccountValue("TBondValue", "0", "USD", "DU348954")
            // EWrapper.updateAccountValue("TotalCashBalance", "228437", "BASE", "DU348954")
            // EWrapper.updateAccountValue("TotalCashBalance", "362554", "EUR", "DU348954")
            // EWrapper.updateAccountValue("TotalCashBalance", "64331", "GBP", "DU348954")
            // EWrapper.updateAccountValue("TotalCashBalance", "70516", "JPY", "DU348954")
            // EWrapper.updateAccountValue("TotalCashBalance", "-245925", "USD", "DU348954")
            // EWrapper.updateAccountValue("TotalCashValue", "228437.42", "EUR", "DU348954")
            // EWrapper.updateAccountValue("TotalCashValue-C", "210820.79", "EUR", "DU348954")
            // EWrapper.updateAccountValue("TotalCashValue-S", "17616.63", "EUR", "DU348954")
            // EWrapper.updateAccountValue("TotalDebitCardPendingCharges", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("TotalDebitCardPendingCharges-C", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("TotalDebitCardPendingCharges-S", "0.00", "EUR", "DU348954")
            // EWrapper.updateAccountValue("TradingType-S", "STKNOPT", null, "DU348954")
            // EWrapper.updateAccountValue("UnrealizedPnL", "0", "BASE", "DU348954")
            // EWrapper.updateAccountValue("UnrealizedPnL", "0", "EUR", "DU348954")
            // EWrapper.updateAccountValue("UnrealizedPnL", "0", "GBP", "DU348954")
            // EWrapper.updateAccountValue("UnrealizedPnL", "0", "JPY", "DU348954")
            // EWrapper.updateAccountValue("UnrealizedPnL", "0", "USD", "DU348954")
            // EWrapper.updateAccountValue("WarrantValue", "0", "BASE", "DU348954")
            // EWrapper.updateAccountValue("WarrantValue", "0", "EUR", "DU348954")
            // EWrapper.updateAccountValue("WarrantValue", "0", "GBP", "DU348954")
            // EWrapper.updateAccountValue("WarrantValue", "0", "JPY", "DU348954")
            // EWrapper.updateAccountValue("WarrantValue", "0", "USD", "DU348954")
            // EWrapper.updateAccountValue("WhatIfPMEnabled", "true", null, "DU348954")
            // EWrapper.updatePortfolio(CASH EUR USD, 83996, 1.18255495, 99329.89, 1.23875, -4720.16, 0, "DU348954")
            // EWrapper.updateAccountTime("12:01")
            // EWrapper.updatePortfolio(CASH GBP USD, 62500, 1.34754, 84221.25, 1.4328, -5328.75, 0, "DU348954")
            // EWrapper.updateAccountTime("12:01")
            // EWrapper.updateAccountTime("12:01")
            // EWrapper.accountDownloadEnd("DU348954")
            // EWrapper.connectionClosed()
            mockTwsWrapper.Verify(mock => mock.updateAccountValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            mockTwsWrapper.Verify(mock => mock.updatePortfolio(It.IsAny<Contract>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<string>()));
            mockTwsWrapper.Verify(mock => mock.accountDownloadEnd(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Test placing an order to the IB API
        /// This test is commented because it depends on an order ID which must come from the TWS connection.
        /// See the invokations list below.
        /// </summary>
        [Fact]
        public void PlaceOrder_Should_Callback()
        {
            // Setup
            // Initialize the contract that will be traded
            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Future,
                Symbol = TwsSymbol.Dax,
                Exchange = TwsExchange.Dtb,
                Currency = TwsCurrency.Eur,
                Multiplier = "25",
                LastTradeDateOrContractMonth = "201809"
            };

            // Initialize the order
            Order order = new Order
            {
                Action = "BUY",
                OrderType = "MKT",
                TotalQuantity = 1
            };

            Mock<EWrapper> mockTwsWrapper = new Mock<EWrapper>();
            int orderId = 0; // This will be changed on the callback from nextValidId
            int clientId = 2;
            mockTwsWrapper.Setup(mock => mock.nextValidId(It.IsAny<int>())).Callback<int>((oid) =>
            {
                orderId = oid;
            });
            mockTwsWrapper.Setup(mock => mock.openOrder(orderId, It.IsAny<Contract>(), It.IsAny<Order>(), It.IsAny<OrderState>()));
            mockTwsWrapper.Setup(mock => mock.orderStatus(orderId, It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<double>(), clientId, It.IsAny<string>()));

            EReaderMonitorSignal signal = new EReaderMonitorSignal();
            EClientSocket clientSocket = new EClientSocket(mockTwsWrapper.Object, signal);

            clientSocket.eConnect("localhost", 7462, 2);

            // Create a reader to consume messages from the TWS. The EReader will consume the incoming messages and put them in a queue
            // Be very careful with the order. The EReader constructor must be called after a call to clientSocket.eConnect().
            var reader = new EReader(clientSocket, signal);
            reader.Start();

            Thread thread = new Thread(
             () =>
             {
                 while (true)
                 {
                     signal.waitForSignal();
                     reader.processMsgs();
                 }
             })
            { IsBackground = true };
            thread.Start();

            // Wait for the next valid order ID to come in
            // And always wait 5 seconds before sending a new order to IB
            Thread.Sleep(5000);

            // Call
            clientSocket.placeOrder(orderId, contract, order);

            // Wait for the order to be placed and the callbacks to be called
            Thread.Sleep(1000);

            // Wait for the response and tear down the connection
            clientSocket.eDisconnect();

            // Under normal circumstances
            // EWrapper.connectAck()
            // EWrapper.managedAccounts("DU348954")
            // EWrapper.nextValidId(1)
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:jfarm")
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:usfuture")
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:eufarm")
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:cashfarm")
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:usfarm.us")
            // EWrapper.error(-1, 2103, "Market data farm connection is broken:usfarm")
            // EWrapper.error(-1, 2106, "HMDS data farm connection is OK:euhmds")
            // EWrapper.error(-1, 2106, "HMDS data farm connection is OK:hkhmds")
            // EWrapper.error(-1, 2105, "HMDS data farm connection is broken:ushmds")
            // EWrapper.openOrder(1, FUT DAX EUR DTB, Order, OrderState)
            // EWrapper.orderStatus(1, "PreSubmitted", 0, 1, 0, 1324695212, 0, 0, 2, "locate")
            // EWrapper.execDetails(-1, FUT DAX EUR DTB, Execution)
            // EWrapper.openOrder(1, FUT DAX EUR DTB, Order, OrderState)
            // EWrapper.orderStatus(1, "Filled", 1, 0, 12997, 1324695212, 0, 12997, 2, null)
            // EWrapper.openOrder(1, FUT DAX EUR DTB, Order, OrderState)
            // EWrapper.orderStatus(1, "Filled", 1, 0, 12997, 1324695212, 0, 12997, 2, null)
            // EWrapper.commissionReport(CommissionReport)

            // When the order will cross a related resting order
            // EWrapper.error(104, 399, "Order Message:BUY 1 DAX JUN'18 Warning: your order will not be placed at the exchange until 2018 - 05 - 28 08:00:00 MET")}
            // EWrapper.openOrder(104, FUT DAX EUR DTB, Order, OrderState)}
            // EWrapper.orderStatus(104, "PreSubmitted", 0, 1, 0, 1752774138, 0, 0, 2, "locate")}
            // EWrapper.openOrder(104, FUT DAX EUR DTB, Order, OrderState)}
            // EWrapper.orderStatus(104, "PendingCancel", 0, 1, 0, 1752774138, 0, 0, 2, null)}
            // EWrapper.openOrder(104, FUT DAX EUR DTB, Order, OrderState)}
            // EWrapper.orderStatus(104, "PendingCancel", 0, 1, 0, 1752774138, 0, 0, 2, null)}
            // EWrapper.orderStatus(104, "Cancelled", 0, 1, 0, 1752774138, 0, 0, 2, null)}
            // EWrapper.error(104, 202, "Order Canceled - reason:Reject: This order would cross a related resting order in this or an affiliated account.")}
            mockTwsWrapper.Verify(mock => mock.openOrder(orderId, It.IsAny<Contract>(), It.IsAny<Order>(), It.IsAny<OrderState>()), Times.AtLeastOnce);
            mockTwsWrapper.Verify(mock => mock.orderStatus(orderId, It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<double>(), clientId, It.IsAny<string>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Test placing an order to the IB API
        /// This test is commented because it depends on an order ID which must come from the TWS connection.
        /// See the invokations list below.
        /// </summary>
        [Fact]
        public void TwoOrdersPlaced_Should_Callback()
        {
            // Setup
            // Initialize the contract that will be traded
            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Future,
                Symbol = TwsSymbol.Dax,
                Exchange = TwsExchange.Dtb,
                Currency = TwsCurrency.Eur,
                Multiplier = "25",
                LastTradeDateOrContractMonth = "201809"
            };

            // Initialize the order
            Order order = new Order
            {
                Action = "BUY",
                OrderType = "MKT",
                TotalQuantity = 1
            };

            Mock<EWrapper> mockTwsWrapper = new Mock<EWrapper>();
            int orderId = 0; // This will be changed on the callback from nextValidId
            int clientId = 2;
            mockTwsWrapper.Setup(mock => mock.nextValidId(It.IsAny<int>())).Callback<int>((oid) =>
            {
                orderId = oid;
            });
            mockTwsWrapper.Setup(mock => mock.openOrder(orderId, It.IsAny<Contract>(), It.IsAny<Order>(), It.IsAny<OrderState>()));
            mockTwsWrapper.Setup(mock => mock.orderStatus(orderId, It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<double>(), clientId, It.IsAny<string>()));

            EReaderMonitorSignal signal = new EReaderMonitorSignal();
            EClientSocket clientSocket = new EClientSocket(mockTwsWrapper.Object, signal);

            clientSocket.eConnect("localhost", 7462, 2);

            // Create a reader to consume messages from the TWS. The EReader will consume the incoming messages and put them in a queue
            // Be very careful with the order. The EReader constructor must be called after a call to clientSocket.eConnect().
            var reader = new EReader(clientSocket, signal);
            reader.Start();

            Thread thread = new Thread(
             () =>
             {
                 while (true)
                 {
                     signal.waitForSignal();
                     reader.processMsgs();
                 }
             })
            { IsBackground = true };
            thread.Start();

            // Wait for the next valid order ID to come in
            Thread.Sleep(1000);

            // Call
            clientSocket.placeOrder(orderId, contract, order);
            clientSocket.placeOrder(orderId + 1, contract, order);

            // Wait for the order to be placed and the callbacks to be called
            // Sometimes this takes a while for IB to respond
            Thread.Sleep(7000);

            // Wait for the response and tear down the connection
            clientSocket.eDisconnect();

            mockTwsWrapper.Verify(mock => mock.openOrder(orderId, It.IsAny<Contract>(), It.IsAny<Order>(), It.IsAny<OrderState>()), Times.AtLeastOnce);
            mockTwsWrapper.Verify(mock => mock.openOrder(orderId + 1, It.IsAny<Contract>(), It.IsAny<Order>(), It.IsAny<OrderState>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Test placing an order to the IB API
        /// This test is commented because it depends on an order ID which must come from the TWS connection.
        /// See the invokations list below.
        /// </summary>
        [Fact]
        public void CancelOrder_Should_Callback()
        {
            // Setup
            // Initialize the contract that will be traded
            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Future,
                Symbol = TwsSymbol.Dax,
                Exchange = TwsExchange.Dtb,
                Currency = TwsCurrency.Eur,
                Multiplier = "25",
                LastTradeDateOrContractMonth = "201809"
            };

            // Initialize the order
            Order order = new Order
            {
                Action = "BUY",
                OrderType = "LMT",
                TotalQuantity = 1,
                LmtPrice = 1,
            };

            Mock<EWrapper> mockTwsWrapper = new Mock<EWrapper>();
            int orderId = 0; // This will be changed on the callback from nextValidId
            int clientId = 2;
            mockTwsWrapper.Setup(mock => mock.nextValidId(It.IsAny<int>())).Callback<int>((oid) =>
            {
                orderId = oid;
            });

            EReaderMonitorSignal signal = new EReaderMonitorSignal();
            EClientSocket clientSocket = new EClientSocket(mockTwsWrapper.Object, signal);

            clientSocket.eConnect("localhost", 7462, 2);

            // Create a reader to consume messages from the TWS. The EReader will consume the incoming messages and put them in a queue
            // Be very careful with the order. The EReader constructor must be called after a call to clientSocket.eConnect().
            var reader = new EReader(clientSocket, signal);
            reader.Start();

            Thread thread = new Thread(
             () =>
             {
                 while (true)
                 {
                     signal.waitForSignal();
                     reader.processMsgs();
                 }
             })
            { IsBackground = true };
            thread.Start();

            // Wait for the next valid order ID to come in
            // And always wait 5 seconds before sending a new order to IB
            Thread.Sleep(5000);

            // Place a test order
            clientSocket.placeOrder(orderId, contract, order);

            // Wait for TWS
            Thread.Sleep(1000);

            // Call the test method
            clientSocket.cancelOrder(orderId);

            // Wait for the order to be cancelled and the callbacks to be called
            Thread.Sleep(1000);

            // Wait for the response and tear down the connection
            clientSocket.eDisconnect();

            // EWrapper.connectAck()
            // EWrapper.managedAccounts("DU348954")
            // EWrapper.nextValidId(141)
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:jfarm")
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:usfuture")
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:eufarm")
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:cashfarm")
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:usfarm.us")
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:usfarm")
            // EWrapper.error(-1, 2106, "HMDS data farm connection is OK:euhmds")
            // EWrapper.error(-1, 2106, "HMDS data farm connection is OK:hkhmds")
            // EWrapper.error(-1, 2106, "HMDS data farm connection is OK:ushmds")
            // EWrapper.error(141, 399, "Order Message: BUY 1 DAX JUN'18 Warning: your order will not be placed at the exchange until 2018 - 05 - 28 08:00:00 MET")
            // EWrapper.openOrder(141, FUT DAX EUR DTB, Order, OrderState)
            // EWrapper.orderStatus(141, "PreSubmitted", 0, 1, 0, 1375368034, 0, 0, 2, "locate")
            // EWrapper.openOrder(141, FUT DAX EUR DTB, Order, OrderState)
            // EWrapper.orderStatus(141, "Submitted", 0, 1, 0, 1375368034, 0, 0, 2, null)
            // EWrapper.orderStatus(141, "Cancelled", 0, 1, 0, 1375368034, 0, 0, 2, null)
            // EWrapper.error(141, 202, "Order Canceled - reason:")
            // EWrapper.connectionClosed()
            mockTwsWrapper.Verify(mock => mock.orderStatus(orderId, "Cancelled", It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<double>(), clientId, It.IsAny<string>()), Times.Once);
            mockTwsWrapper.Verify(mock => mock.error(orderId, 202, It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Tests requesting historical data from TWS
        /// </summary>
        [Fact]
        public void ReqHistoricalData_Should_Callback()
        {
            // Setup
            // Initialize the contract that will be traded
            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Future,
                Symbol = TwsSymbol.Dax,
                Exchange = TwsExchange.Dtb,
                Currency = TwsCurrency.Eur,
                Multiplier = "25",
                LastTradeDateOrContractMonth = "201809"
            };

            Mock<EWrapper> mockTwsWrapper = new Mock<EWrapper>();
            EReaderMonitorSignal signal = new EReaderMonitorSignal();
            EClientSocket clientSocket = new EClientSocket(mockTwsWrapper.Object, signal);

            clientSocket.eConnect("localhost", 7462, 2);

            // Create a reader to consume messages from the TWS. The EReader will consume the incoming messages and put them in a queue
            // Be very careful with the order. The EReader constructor must be called after a call to clientSocket.eConnect().
            var reader = new EReader(clientSocket, signal);
            reader.Start();

            Thread thread = new Thread(
             () =>
             {
                 while (true)
                 {
                     signal.waitForSignal();
                     reader.processMsgs();
                 }
             })
            { IsBackground = true };
            thread.Start();

            // Wait for the next valid order ID to come in
            Thread.Sleep(1000);

            // Call
            int requestId = 4001;
            string queryTime = DateTime.Now.AddMonths(-6).ToString("yyyyMMdd HH:mm:ss");
            clientSocket.reqHistoricalData(requestId, contract, queryTime, "1 M", "1 day", "MIDPOINT", 1, 1, null);

            // Wait for the order to be placed and the callbacks to be called
            Thread.Sleep(1000);

            // Wait for the response and tear down the connection
            clientSocket.eDisconnect();

            // Assert
            // EWrapper.connectAck()
            // EWrapper.managedAccounts("DU348954")
            // EWrapper.nextValidId(88)
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:jfarm")
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:usfuture")
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:eufarm")
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:cashfarm")
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:usfarm.us")
            // EWrapper.error(-1, 2104, "Market data farm connection is OK:usfarm")
            // EWrapper.error(-1, 2106, "HMDS data farm connection is OK:euhmds")
            // EWrapper.error(-1, 2106, "HMDS data farm connection is OK:hkhmds")
            // EWrapper.error(-1, 2106, "HMDS data farm connection is OK:ushmds")
            // EWrapper.historicalData(4001, "20171026", 12996, 13182.75, 12954.25, 13172.75, -1, -1, -1, False)
            // EWrapper.historicalData(4001, "20171027", 13185.75, 13265.25, 13185.75, 13233, -1, -1, -1, False)
            // EWrapper.historicalData(4001, "20171030", 13223, 13269.75, 13222.75, 13246, -1, -1, -1, False)
            // EWrapper.historicalData(4001, "20171101", 13346.5, 13507.5, 13333.25, 13484.25, -1, -1, -1, False)
            // EWrapper.historicalData(4001, "20171102", 13456.25, 13488.25, 13421.75, 13486.25, -1, -1, -1, False)
            // EWrapper.historicalData(4001, "20171103", 13497.5, 13524, 13446.25, 13488.5, -1, -1, -1, False)
            // EWrapper.historicalData(4001, "20171106", 13494, 13501.5, 13458.25, 13488.75, -1, -1, -1, False)
            // EWrapper.historicalData(4001, "20171107", 13527.75, 13551.5, 13355.25, 13387.75, -1, -1, -1, False)
            // EWrapper.historicalData(4001, "20171108", 13382.75, 13437.75, 13361.75, 13411, -1, -1, -1, False)
            // EWrapper.historicalData(4001, "20171109", 13408.5, 13421, 13121.25, 13199.25, -1, -1, -1, False)
            // EWrapper.historicalData(4001, "20171110", 13206.5, 13232.75, 13123.75, 13140.5, -1, -1, -1, False)
            // EWrapper.historicalData(4001, "20171113", 13141, 13178.5, 12974.5, 13122.75, -1, -1, -1, False)
            // EWrapper.historicalData(4001, "20171114", 13094.25, 13153.25, 13014.25, 13054.75, -1, -1, -1, False)
            // EWrapper.historicalData(4001, "20171115", 12996.5, 13013.75, 12860.75, 12972, -1, -1, -1, False)
            // EWrapper.historicalData(4001, "20171116", 13032.5, 13111.5, 13020.25, 13099.5, -1, -1, -1, False)
            // EWrapper.historicalData(4001, "20171117", 13074.5, 13104.25, 12982.25, 12994.75, -1, -1, -1, False)
            // EWrapper.historicalData(4001, "20171120", 12887.5, 13101.5, 12872.25, 13067.75, -1, -1, -1, False)
            // EWrapper.historicalData(4001, "20171121", 13071.5, 13225.75, 13038, 13189.25, -1, -1, -1, False)
            // EWrapper.historicalData(4001, "20171122", 13193, 13207.25, 12979.75, 12994, -1, -1, -1, False)
            // EWrapper.historicalData(4001, "20171123", 12987, 13067, 12918, 13022.75, -1, -1, -1, False)
            // EWrapper.historicalData(4001, "20171124", 13017.75, 13178.75, 12993.75, 13082.5, -1, -1, -1, False)
            // EWrapper.historicalDataEnd(4001, "20171025  16:53:27", "20171125  16:53:27")
            mockTwsWrapper.Verify(mock => mock.historicalData(requestId, It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<double>(), It.IsAny<bool>()));
            mockTwsWrapper.Verify(mock => mock.historicalDataEnd(requestId, It.IsAny<string>(), It.IsAny<string>()));
        }

        /// <summary>
        /// Tests requesting historical data from TWS
        /// </summary>
        [Fact]
        public void ReqRealtimeBars_Should_SubscribeForever()
        {
            // Setup
            // Initialize the contract that will be traded
            Contract contract = new Contract
            {
                SecType = TwsContractSecType.Future,
                Symbol = TwsSymbol.Dax,
                Exchange = TwsExchange.Dtb,
                Currency = TwsCurrency.Eur,
                Multiplier = "25",
                LastTradeDateOrContractMonth = "201809"
            };

            Mock<EWrapper> mockTwsWrapper = new Mock<EWrapper>();
            EReaderMonitorSignal signal = new EReaderMonitorSignal();
            EClientSocket clientSocket = new EClientSocket(mockTwsWrapper.Object, signal);

            clientSocket.eConnect("localhost", 7462, 2);

            // Create a reader to consume messages from the TWS. The EReader will consume the incoming messages and put them in a queue
            // Be very careful with the order. The EReader constructor must be called after a call to clientSocket.eConnect().
            var reader = new EReader(clientSocket, signal);
            reader.Start();

            Thread thread = new Thread(
             () =>
             {
                 while (true)
                 {
                     signal.waitForSignal();
                     reader.processMsgs();
                 }
             })
            { IsBackground = true };
            thread.Start();

            // Call
            int requestId = 4001;
            clientSocket.reqRealTimeBars(requestId, contract, 5, "TRADES", false, null); // "TRADES", "ASK", "BID", or "MIDPOINT".

            // Wait for the realtime bar to come in, which could take 5 seconds
            Thread.Sleep(5005);

            // Wait for the response and tear down the connection
            clientSocket.eDisconnect();

            // Assert
            mockTwsWrapper.Verify(mock => mock.realtimeBar(4001, It.IsAny<long>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<long>(), It.IsAny<double>(), It.IsAny<int>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Test receiving account updates
        /// Test that the account updates callback method is called on a Mock
        /// </summary>
        [Fact]
        public void RequestAllOpenOrders_Should_Callback()
        {
            // Setup
            Mock<EWrapper> mockTwsWrapper = new Mock<EWrapper>();
            mockTwsWrapper.Setup(mock => mock.openOrder(It.IsAny<int>(), It.IsAny<Contract>(), It.IsAny<Order>(), It.IsAny<OrderState>()));
            mockTwsWrapper.Setup(mock => mock.orderStatus(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<double>(), It.IsAny<int>(), It.IsAny<string>()));
            mockTwsWrapper.Setup(mock => mock.openOrderEnd());

            EReaderMonitorSignal signal = new EReaderMonitorSignal();
            EClientSocket clientSocket = new EClientSocket(mockTwsWrapper.Object, signal);

            clientSocket.eConnect("localhost", 7462, 2);

            // Create a reader to consume messages from the TWS. The EReader will consume the incoming messages and put them in a queue
            // Be very careful with the order. The EReader constructor must be called after a call to clientSocket.eConnect().
            var reader = new EReader(clientSocket, signal);
            reader.Start();

            Thread thread = new Thread(
             () =>
             {
                 while (true)
                 {
                     signal.waitForSignal();
                     reader.processMsgs();
                 }
             })
            { IsBackground = true };
            thread.Start();

            Thread.Sleep(1000);

            // Call
            clientSocket.reqAllOpenOrders();

            // Wait for the response and tear down the connection
            Thread.Sleep(1000);
            clientSocket.eDisconnect();

            // Assert
            // The openOrder and orderStatus are called many times, openOrderEnd is called once
            // Here is an example of the calls:
            // EWrapper.openOrder(0, FUT ESTX50 EUR DTB, Order, OrderState)
            // EWrapper.orderStatus(0, "Submitted", 0, 1, 0, 1149884527, 0, 0, 0, null)
            // EWrapper.openOrderEnd()
            mockTwsWrapper.Verify(mock => mock.openOrderEnd(), Times.Once);
        }

        /// <summary>
        /// Test receiving account updates
        /// Test that the account updates callback method is called on a Mock
        /// </summary>
        [Fact]
        public void RequsetAllPositions_Should_Callback()
        {
            // Setup
            Mock<EWrapper> mockTwsWrapper = new Mock<EWrapper>();
            mockTwsWrapper.Setup(mock => mock.position(It.IsAny<string>(), It.IsAny<Contract>(), It.IsAny<double>(), It.IsAny<double>()));
            mockTwsWrapper.Setup(mock => mock.positionEnd());

            EReaderMonitorSignal signal = new EReaderMonitorSignal();
            EClientSocket clientSocket = new EClientSocket(mockTwsWrapper.Object, signal);

            clientSocket.eConnect("localhost", 7462, 2);

            // Create a reader to consume messages from the TWS. The EReader will consume the incoming messages and put them in a queue
            // Be very careful with the order. The EReader constructor must be called after a call to clientSocket.eConnect().
            var reader = new EReader(clientSocket, signal);
            reader.Start();

            Thread thread = new Thread(
             () =>
             {
                 while (true)
                 {
                     signal.waitForSignal();
                     reader.processMsgs();
                 }
             })
            { IsBackground = true };
            thread.Start();

            Thread.Sleep(1000);

            // Call
            clientSocket.reqPositions();

            // Wait for the response and tear down the connection
            Thread.Sleep(1000);
            clientSocket.eDisconnect();

            // Assert
            // The openOrder and orderStatus are called many times, openOrderEnd is called once
            // Here is an example of the calls:
            // EWrapper.position("DU1052488", FUT ESTX50 EUR, 0, 0)}
            // EWrapper.position("DU1052488", FUT DAX EUR, 2, 63905.8)}
            // EWrapper.positionEnd()}
            // EWrapper.connectionClosed()}
            mockTwsWrapper.Verify(mock => mock.positionEnd(), Times.Once);
        }

        /// <summary>
        /// Test receiving executions
        /// Test that the account updates callback method is called on a Mock
        /// </summary>
        [Fact]
        public void RequsetAllExecutions_Should_Callback()
        {
            // Setup
            Mock<EWrapper> mockTwsWrapper = new Mock<EWrapper>();
            mockTwsWrapper.Setup(mock => mock.execDetails(It.IsAny<int>(), It.IsAny<Contract>(), It.IsAny<Execution>()));
            mockTwsWrapper.Setup(mock => mock.execDetailsEnd(It.IsAny<int>()));

            EReaderMonitorSignal signal = new EReaderMonitorSignal();
            EClientSocket clientSocket = new EClientSocket(mockTwsWrapper.Object, signal);

            clientSocket.eConnect("localhost", 7462, 2);

            // Create a reader to consume messages from the TWS. The EReader will consume the incoming messages and put them in a queue
            // Be very careful with the order. The EReader constructor must be called after a call to clientSocket.eConnect().
            var reader = new EReader(clientSocket, signal);
            reader.Start();

            Thread thread = new Thread(
             () =>
             {
                 while (true)
                 {
                     signal.waitForSignal();
                     reader.processMsgs();
                 }
             })
            { IsBackground = true };
            thread.Start();

            Thread.Sleep(1000);

            // Call
            clientSocket.reqExecutions(2, new ExecutionFilter());

            // Wait for the response and tear down the connection
            Thread.Sleep(1000);
            clientSocket.eDisconnect();

            // Assert
            // The openOrder and orderStatus are called many times, openOrderEnd is called once
            // Here is an example of the calls:
            // EWrapper.execDetails(2, FUT ESTX50 EUR DTB, Execution)
            // EWrapper.execDetails(2, FUT DAX EUR DTB, Execution)
            // EWrapper.execDetails(2, FUT DAX EUR DTB, Execution)
            // EWrapper.commissionReport(CommissionReport)
            // EWrapper.commissionReport(CommissionReport)
            // EWrapper.execDetailsEnd(2)
            mockTwsWrapper.Verify(mock => mock.execDetailsEnd(2), Times.Once);
        }
    }
}
