function comment(author, auctionId) {
    var value = $("#comment").val();
    $("#comment").val("");

    var url = `https://localhost:44346/Comment/Add?id=${auctionId}&comment=${value}`
    $.ajax({
        method: "GET",
        url: url,
        success: (response) => {
            auctionConnection.invoke("Comment", author, value, response.publishDate, response.newCommentId);
        },
        error: (error) => {
            notifier.showError(error.responseText);
        }
    });
}

function deleteComment(id) {
    var url = `https://localhost:44346/Comment/Delete/${id}`
    $.ajax({
        method: "GET",
        url: url,
        success: () => {
            auctionConnection.invoke("DeleteComment", id);
            notifier.showSuccess("Comment deleted");
        },
        error: (error) => {
            notifier.showError(error.responseText);
        }
    });
}

function commentEditMode(id) {
    $("div[class=contentUpdate]").remove();
    $(`div[comment-id=${id}]`).addClass("edit-mode");
    var selector = `div[comment-id=${id}] span[id=commentContent]`;
    var content = $(selector).text();
    $(selector).append(`<div class='contentUpdate'>
                            <textarea>${content}</textarea>
                            <a class="btn btn-default" onclick="editComment(${id})">Edit</a>
                        </div>`);
}

function closeEditMode() {
    $("div[class=contentUpdate]").remove();
    $(".edit-mode").removeClass("edit-mode");
}

function editComment(id) {
    var url = `https://localhost:44346/Comment/Edit`
    var newContent = $(`div[comment-id=${id}] textarea`).val();
    $.ajax({
        method: "POST",
        data: { Id: id, NewContent: newContent },
        url: url,
        success: () => {
            auctionConnection.invoke("EditComment", id);
        },
        error: (error) => {
            notifier.showError(error.responseText);
        }
    });
}

$(document).click(function (e) {
    if (!$(e.target).parents('.edit-mode').length) {
        closeEditMode();
    }
    if (!$(e.target).is(".edit-mode")) {

    }
});