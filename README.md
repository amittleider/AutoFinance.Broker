# Interactive Brokers (TWS) API wrapper
This project is a wrapper around the Interactive Brokers C# client API. It aims to make TWS APIs easier to use by making the multithreaded design look single-threaded to the user. It's in dotnet core, so use it in Linux or Windows.

## Warning - Important
Running integration tests will place real orders in TWS. Only run integration tests against a paper trading account.

## How to use
Each call to the API requires its corresponding controller object. All controller objects are initialized with the same parameters. 
```C#
// Initialize a controller object
TwsObjectFactory twsObjectFactory = new TwsObjectFactory();
TwsConnectionController connectionController = new TwsConnectionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, "localhost", 7462, 1);
ITwsNextOrderIdController nextOrderIdController = new TwsNextOrderIdController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
TwsOrderPlacementController orderPlacementController = new TwsOrderPlacementController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler);
TwsRequestIdGenerator twsRequestIdGenerator = new TwsRequestIdGenerator();
TwsExecutionController executionController = new TwsExecutionController(twsObjectFactory.ClientSocket, twsObjectFactory.TwsCallbackHandler, twsRequestIdGenerator);

// Ensure the connection is made with TWS before calling any controller methods
await connectionController.EnsureConnectedAsync();
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

int orderId = await nextOrderIdController.GetNextValidIdAsync();
bool successfullyPlaced = await orderPlacementController.PlaceOrderAsync(orderId, contract, order);
```

Get a list of the current positions
```C#
List<PositionStatusEventArgs> positionStatusEvents = await positionsController.RequestPositions();
```

Get various account information
```C#
string accountId = "DU1052488";
ConcurrentDictionary<string, string> accountUpdates = await accountUpdatesController.GetAccountDetailsAsync(accountId);
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
List<HistoricalDataEventArgs> historicalDataEvents = await historicalDataController.GetHistoricalDataAsync(contract, queryTime, "1 M", "1 day", "MIDPOINT");
```

Cancel an order
```C#
bool cancelationAcknowledged = await orderCancelationController.CancelOrderAsync(orderId);
```

Place a bracket order (Entry + Stop loss + Take profit), where the entry order is pegged to the market.
```C#
bool orderAck = await twsBracketOrderPlacementController.PlaceBracketOrder(contract, entryAction, quantity, takePrice, stopPrice);
```

## Build + Nuget
[![Build Status](https://dev.azure.com/amittleider/AutoFinance.Broker/_apis/build/status/amittleider.AutoFinance.Broker?branchName=master)](https://dev.azure.com/amittleider/AutoFinance.Broker/_build/latest?definitionId=5&branchName=master)

[Download the Nuget package of this project here](https://www.nuget.org/packages/AutoFinance.Broker/)
