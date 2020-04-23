# Interactive Brokers (TWS) API wrapper
This project is a wrapper around the Interactive Brokers C# client API. It aims to make TWS APIs easier to use by making the multithreaded design look single-threaded to the user. It's in dotnet core, so use it in Linux or Windows.

## Warning - Important
Running integration tests will place real orders in TWS. Only run integration tests against a paper trading account.

## How to use
Use the `TwsController` object to call into Tws methods.
```C#
TwsObjectFactory twsObjectFactory = new TwsObjectFactory("localhost", 7462, 1);
TwsController twsController = twsObjectFactory.TwsController;

await twsController.EnsureConnectedAsync();
```

Place an order
```C#
Contract contract = new Contract
{
    SecType = TwsContractSecType.Stock,
    Symbol = "MSFT",
    Exchange = TwsExchange.Smart,
    PrimaryExch = TwsExchange.Island,
};

Order order = new Order
{
    Action = "BUY",
    OrderType = "MKT",
    TotalQuantity = 1
};

int orderId = await twsController.GetNextValidIdAsync();
bool successfullyPlaced = await twsController.PlaceOrderAsync(orderId, contract, order);
```

Get a list of the current positions
```C#
List<PositionStatusEventArgs> positionStatusEvents = await twsController.RequestPositions();
```

Get various account information
```C#
string accountId = "DU1052488";
ConcurrentDictionary<string, string> accountUpdates = await twsController.GetAccountDetailsAsync(accountId);
```

Get historical data
```C#
Contract contract = new Contract
{
    SecType = TwsContractSecType.Stock,
    Symbol = "MSFT",
    Exchange = TwsExchange.Smart,
    PrimaryExch = TwsExchange.Island,
};

string queryTime = DateTime.Now.AddMonths(-6).ToString("yyyyMMdd HH:mm:ss");

// Call
List<HistoricalDataEventArgs> historicalDataEvents = await twsController.GetHistoricalDataAsync(contract, queryTime, "1 M", "1 day", "MIDPOINT");
```

Cancel an order
```C#
bool cancelationAcknowledged = await twsController.CancelOrderAsync(orderId);
```

Place a bracket order (Entry + Stop loss + Take profit), where the entry order is pegged to the market.
```C#
bool orderAck = await twsController.PlaceBracketOrder(contract, entryAction, quantity, takePrice, stopPrice);
```

Check out the tests in [TwsControllerBaseTests.cs](../master/AutoFinance.Broker.IntegrationTests/InteractiveBrokers/Controllers/TwsControllerBaseTests.cs) and [TwsControllerTests.cs](../master/AutoFinance.Broker.IntegrationTests/InteractiveBrokers/Controllers/TwsControllerTests.cs) for more examples. A full list of implemented APIs can be found in [the TwsController](../master/AutoFinance.Broker/InteractiveBrokers/Controllers/TwsController.cs).

## Build + Nuget
[![Build Status](https://dev.azure.com/amittleider/AutoFinance.Broker/_apis/build/status/amittleider.AutoFinance.Broker?branchName=master)](https://dev.azure.com/amittleider/AutoFinance.Broker/_build/latest?definitionId=5&branchName=master)

[Download the Nuget package of this project here](https://www.nuget.org/packages/AutoFinance.Broker/)
