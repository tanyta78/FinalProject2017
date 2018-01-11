
function bid(bidderName, auctionId) {
    var value = $("#bid").val();
    $("#bid").val("");
    var url = `https://localhost:44346/Bid/Create?auctionId=${auctionId}&value=${value}`
    $.ajax({
        method: "GET",
        url: url,
        success: () => {
            auctionConnection.invoke("Bid", bidderName, value, commentId);
        },
        error: (error) => {
            notifier.showError(error.responseText);
        }
    });
}


