function CommentSubmitByClick(PostId, x, y) {
    //here "y" is represent this is thacher user who comments on own post
    var CommentBoxIDName = "CommentBox" + PostId;
    var Comment = $("#" + CommentBoxIDName).val().trim();

    if (Comment == "") Comment = null;

    if (PostId != null && Comment != null) {
        $.ajax({
            url: '@Url.Action("CommentSubmitByClick", "ShareContents")',
            data: { PostId: PostId, x: x, Comment: Comment, Flag: y },
            success: function (result) {

                var CommentList = "";
                for (var i = 0; i < result.length; i++) {
                    //Comment Deleation  previlige check
                    if (result[i]['UserId'] == x) {

                        CommentList = CommentList + "<div>" +
                            "<div class='commentdeleationdiv'><a onclick='return DeleteComment(" + result[i]['CommentId'] + "," + result[i]['ContentId'] + "," + x + ")'>x</a></div>" +
                            " <div><div class='CommentImg'> <img src='../" + result[i]['PhotoPath'] + "' width='40' height='40' /></div>" +
                            "<div class=''><h5><a href=''>" + result[i]['UserName'] + "</a> " + result[i]['Comment'] + "</h5>"
                            + "<p  style='color: slategrey'>" + result[i]['CommentTime'] + "</p></div></div";
                    } else {

                        CommentList = CommentList + "<div>" +
                            " <div><div class='CommentImg'> <img src='../" + result[i]['PhotoPath'] + "' width='40' height='40' /></div>" +
                            "<div class=''><h5><a href=''>" + result[i]['UserName'] + "</a> " + result[i]['Comment'] + "</h5>"
                            + "<p  style='color: slategrey'>" + result[i]['CommentTime'] + "</p></div></div";
                    }

                }
                $("#" + CommentBoxIDName).val("");
                $("#" + PostId).empty();
                $("#" + PostId).append(CommentList);
                $("#" + PostId).collapse();
            }
        });
    }

};

function white_space(field) {
    field.value = field.value.replace(/^\\s+/, "");
}
function FocusButton(Id) {
    var SubmitBtnID = "SubmitBtn" + Id;
    $("#" + SubmitBtnID).focus();
}




//@*<script src="~/Scripts/bootstrap-confirmation.js"></script>*@
//@*Thanks Button Action*@

    function ThanksButton(PostId, x) {
        if (PostId != null) {
            $.ajax({
                url: '@Url.Action("ThanksButtonClick", "ShareContents")',
                data: { PostId: PostId, x: x, Flag: 0 },
                success: function (result) {

                    if (result['Status'] == 1) {
                        $("#ThanksButton" + PostId).val = '';
                        $("#ThanksButton" + PostId).val('Thanked');
                        $("#CountThanks" + PostId).empty();
                        $("#CountThanks" + PostId).append(" " + result['ThanksCount'] + '&nbsp; Peoples Thanked');
                    }
                    else {
                        $("#ThanksButton" + PostId).val = '';
                        $("#ThanksButton" + PostId).val('Thanks');
                        $("#CountThanks" + PostId).empty();
                        $("#CountThanks" + PostId).append(" " + result['ThanksCount'] + ' &nbsp; Peoples Thanked');
                    }

                }

            });
        }
    };



//@*photo view dialog*@

    function PhotoViewbyClick(e) {
        var photo = "<img src='" + e.src + "'/>";

        $("#ViewPhotoPopup").empty();
        $("#ViewPhotoPopup").append(photo)
        $("#ViewPhotoPopup").dialog({
            resizable: false,
            height: 600,
            modal: true,
            minWidth: 900,
            buttons: {
                Close: function () {
                    $(this).dialog("close");
                }
            }
        });
    }


//@*Comment Deleation*@

    function DeleteComment(x, y, user) {

        if (x > 0) {
            var con = confirm("Are you sure want to delete");
            if (con == true) {
                $.ajax({
                    url: '@Url.Action("CommentDeletebyClick", "ShareContents")',
                    data: { CommentId: x, ContentId: y },
                    success: function (result) {

                        var CommentList = "";
                        for (var i = 0; i < result.length; i++) {

                            ////Comment Deleation  Check
                            ////Comment Deleation  previlige check
                            if (result[i]['UserId'] == user) {
                                CommentList = CommentList + "<div>" +
                                    "<div class='commentdeleationdiv'><a onclick='return DeleteComment(" + result[i]['CommentId'] + "," + result[i]['ContentId'] + "," + user + ")'>x</a></div>" +
                                    " <div><div class='CommentImg'> <img src='../" + result[i]['PhotoPath'] + "' width='40' height='40' /></div>" +
                                    "<div class=''><h5><a href=''>" + result[i]['UserName'] + "</a> " + result[i]['Comment'] + "</h5>"
                                    + "<p  style='color: slategrey'>" + result[i]['CommentTime'] + "</p></div></div";
                            } else {


                                CommentList = CommentList + "<div>" +
                                    " <div><div class='CommentImg'> <img src='../" + result[i]['PhotoPath'] + "' width='40' height='40' /></div>" +
                                    "<div class=''><h5><a href=''>" + result[i]['UserName'] + "</a> " + result[i]['Comment'] + "</h5>"
                                    + "<p  style='color: slategrey'>" + result[i]['CommentTime'] + "</p></div></div";
                            }
                        }

                        $("#" + y).empty();
                        $("#" + y).append(CommentList);
                        $("#" + y).collapse();

                    }

                });
            }

        }

    }
