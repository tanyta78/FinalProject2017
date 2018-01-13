var transportType = signalR.TransportType.WebSockets;
var logger = new signalR.ConsoleLogger(signalR.LogLevel.Information);
var auctionHub = new signalR.HttpConnection(`https://${document.location.host}/AuctionHub`, { transport: transportType, logger: logger });
var auctionConnection = new signalR.HubConnection(auctionHub, logger);

auctionConnection.onClosed = e => { console.log("connection closed") };

auctionConnection.on("Bid", (bidderName, bid) => {
    $("#result").text(bid + " by " + bidderName);
});

auctionConnection.on("Comment", (author, comment, commentDate, newCommentId) => {
    var isAuthor = window.loggedInUser === author;
    var commentHtml = $(`<div class="card-panel" comment-id=${newCommentId} ${isAuthor ? "ondblclick=commentEditMode(" + newCommentId + ")" : ""}>
                        <div class="card-content flow-text">${author} : <span id=commentContent>${comment}<span></div>
                        <p class="small"><i>${commentDate}</i></p>
                        ${isAuthor ? '<a class="btn btn-sm" onclick="deleteComment(' + newCommentId + ')">Delete</a>' : ''}
                    </div>`);
    var commentsArea = $("#comments");
    commentsArea.prepend(commentHtml);
});
auctionConnection.on("DeleteComment", (commentId) => {
    $(`div[comment-id=${commentId}]`).remove();
});
auctionConnection.on("EditComment", (id, newContent) => {
    //var newContent = $(`div[comment-id=${id}] textarea`).val();
    $(`div[comment-id=${id}] #commentContent`).text(newContent);
    closeEditMode();
});
auctionConnection.start().catch(err => { console.log("connection error") });
