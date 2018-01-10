var transportType = signalR.TransportType.WebSockets;
var logger = new signalR.ConsoleLogger(signalR.LogLevel.Information);
var auctionHub = new signalR.HttpConnection(`https://${document.location.host}/AuctionHub`, { transport: transportType, logger: logger });
var auctionConnection = new signalR.HubConnection(auctionHub, logger);

auctionConnection.onClosed = e => { console.log("connection closed") };

auctionConnection.on("Bid", (bidderName, bid) => {
    $("#result").text(bid + " by " + bidderName);
});

auctionConnection.on("Comment", (author, comment, commentDate) => {
    var comment = $(`<div class="card-panel">
                        <div class="card-content flow-text">${author} : ${comment}</div>
                        <p class="small"><i>${commentDate}</i></p>
                    </div>`);
    var commentsArea = $("#comments");
    commentsArea.prepend(comment);
});

auctionConnection.start().catch(err => { console.log("connection error") });

function bid(bidderName, auctionId) {
    var value = $("#bid").val();
    $("#bid").val("");
    var url = `https://localhost:44346/Bid/Create?auctionId=${auctionId}&value=${value}`
    $.ajax({
        method: "GET",
        url: url,
        success: () => {
            auctionConnection.invoke("Bid", bidderName, value);
        },
        error: (error) => {
            notifier.showError(error.responseText);
        }
    });
}

function comment(author, auctionId) {
    var value = $("#comment").val();
    $("#comment").val("");

    var url = `https://localhost:44346/Comment/Add?id=${auctionId}&comment=${value}`
    $.ajax({
        method: "GET",
        url: url,
        success: (publishDate) => {
            auctionConnection.invoke("Comment", author, value, publishDate);
        },
        error: (error) => {
            notifier.showError(error.responseText);
        }
    });
}