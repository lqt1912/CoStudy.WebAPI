$(document).ready(
    function () {
        getCommentDetailById();
    }
);

function getCommentDetailById() {
    $.ajax({
        type: 'GET',
        url: API_URL + 'Cms/reply-comment/' + $('#replyCommentId').val(),
        beforeSend: function(xhr) {
            xhr.setRequestHeader('Authorization', getConfig());
        },
        success: function (response) {
            if (response.result === null || response.code === 400)
            {
                alert("Không tìm thấy phẩn hồi. ");
                $('[id*="Table_wrapper"]').removeClass("loading");
            }
            $('#oid').val(response.result.oid);
            $('#post_id').val(response.result.post_id);
            $('#content').val(response.result.content);
            $('#author_id').val(response.result.author_id);
            $('#author_name').val(response.result.author_name);
            $('#status').val(response.result.status_name);
            $('#parent_id').val(response.result.parent_id);

            var created_date = new Date(response.result.created_date);
            $('#created_date').val(created_date.toUTCString());

            var modified_date = new Date(response.result.modified_date);
            $('#modified_date').val(modified_date.toUTCString());
            $('#upvote_count').val(response.result.upvote_count);
            $('#downvote_count').val(response.result.downvote_count);

            var arrButton = [];
            switch (response.result.status) { 
            case StatusEnum.Active:
                arrButton = ['delete', 'block'];
                break;
            case StatusEnum.Deleted:
                arrButton = [];
                break;
            case StatusEnum.Blocked:
                arrButton = ['delete', 'active'];
                break;
            default:
                arrButton = [];
            }

            $('#button_bottom').html(getButtonWithObjectName('phản hồi', arrButton));

            $('#btn-pre-delete').click(function () {
                $('#id_to_delete').val($('#oid').val());
                $('#deleteConfirmModal').modal('show');
            });

            $('#btn-pre-block').click(function () {
                $('#id_to_block').val($('#oid').val());
                $('#blockConfirmModal').modal('show');
            });

            $('#btn-pre-active').click(function () {
                $('#id_to_active').val($('#oid').val());
                $('#activeConfirmModal').modal('show');
            });
        },
        error: function (response) {
            console.log(response);
        }
    });
}

$(document).ready(
    function () {
        $('#btnDelete').click(function () {
            modifyReplyComment($('#id_to_delete').val(), 4);
        });

        $('#btnBlock').click(function () {
            modifyReplyComment($('#id_to_block').val(), 2);
        });

        $('#btnActive').click(function () {
            modifyReplyComment($('#id_to_active').val(), 0);
        });
    }
);

function goToParentDetail() {
    setTimeout(function () {
        window.open(CLIENT_URL+ 'Comment/detail?commentId=' + $('#parent_id').val());
        },
        500);
};

function modifyReplyComment(oid, _status) {
    $.ajax({
        type: 'PUT',
        url: API_URL + 'Comment/modified-reply-status',
        dataType: 'json',
        contentType: 'application/json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', getConfig());
        },
        data: JSON.stringify({
            comment_id: oid.toLowerCase(),
            status: _status
        }),
        success: function (response) {
            if (response.code === 401)
                alert("Unauthorized. ");
            getCommentDetailById();
        },
        error: function (response) {
            console.log(response);
        }
    });
}