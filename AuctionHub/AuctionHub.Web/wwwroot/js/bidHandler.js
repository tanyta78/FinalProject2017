var transportType = signalR.TransportType.WebSockets;
var logger = new signalR.ConsoleLogger(signalR.LogLevel.Information);
var auctionHub = new signalR.HttpConnection(`https://${document.location.host}/bid`, { transport: transportType, logger: logger });
var auctionConnection = new signalR.HubConnection(auctionHub, logger);

auctionConnection.onClosed = e => { console.log("connection closed") };

auctionConnection.on("Bid", (bidderName, bid) => {
    $("#result").append(`<div>Bidder Name: ${bidderName} has bid ${bid} dollars</div>`);
});

auctionConnection.start().catch(err => { console.log("connection error") });

function bid(bidderName) {
    var bid = $("#bid").val();
    auctionConnection.invoke("Bid", bidderName, bid);
}