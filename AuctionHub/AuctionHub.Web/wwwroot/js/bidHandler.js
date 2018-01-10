var transportType = signalR.TransportType.WebSockets;
var logger = new signalR.ConsoleLogger(signalR.LogLevel.Information);
var auctionHub = new signalR.HttpConnection(`https://${document.location.host}/bid`, { transport: transportType, logger: logger });
var auctionConnection = new signalR.HubConnection(auctionHub, logger);

auctionConnection.onClosed = e => { console.log("connection closed") };

auctionConnection.on("Bid", (bidderName, bid) => {
    $("#result").text(bid + " by " + bidderName);
});

auctionConnection.start().catch(err => { console.log("connection error") });

function bid(bidderName, auctionId) {
    var value = $("#bid").val();
    var url = `https://localhost:44346/Bid/Create?auctionId=${auctionId}&value=${value}`
    $.ajax({
        method: "GET",
        url: url,
        success: () => {
            auctionConnection.invoke("Bid", bidderName, value);
            notifier.showSuccess("Bid successful");
        },
        error: (error) => {
            notifier.showError(error.responseText);
        }
    });
}