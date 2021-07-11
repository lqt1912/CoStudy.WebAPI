$(document).ready(
    function () {
        getPostDetailById();
    }
);

function getPostDetailById() {
    
    $.ajax(
        {
            type: 'GET',
            url: API_URL + 'Cms/post/' + $('#postId').val(),
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', getConfig())
            },
            success: function (response) {
                if (response.code === 400)
                {
                    alert(response.message);
                    window.$body.removeClass("loading");
                }

                if (response.result === null)
                {
                    alert("Không tìm thấy bài viết");
                    window.$body.removeClass("loading");
                }
                $('#oid').val(response.result.oid.toUpperCase());
                $('#author_id').val(response.result.author_id.toUpperCase());
                $('#author_name').val(response.result.author_name);
                $('#author_email').val(response.result.author_email);
                $('#title').val(response.result.title);
                $('#upvote').val(response.result.upvote);
                $('#downvote').val(response.result.downvote);
                $('#comments_count').val(response.result.comments_count);

                var created_date = new Date(response.result.created_date);
                $('#created_date').val(created_date.toUTCString());

                var d_string_contents = ' <div class="col-12 row pt-3"><b class="col-2">Loại nội dung</b><b class="col-10">Chi tiết nội dung</b></div>';
                for (var i = 0; i < response.result.string_contents.length; i++) {
                    d_string_contents = d_string_contents + '<div class="col-12 row pt-3"><div class="col-2">' + getContentTypeName(response.result.string_contents[i].content_type) + '</div><div class="col-10">' + response.result.string_contents[i].content + '</div> </div>';
                }
                $("#string_contents").html(d_string_contents);

                var d_image_contents = "";
                var d_media_rows = "<ul>";
                for (var i = 0; i < response.result.image_contents.length; i++) {
                    if (response.result.image_contents[i].media_type === 0) {
                        d_image_contents = d_image_contents + '<div class="col-3"><img class="w-100" src="' + response.result.image_contents[i].image_hash + '"/><p class="text-center">' + response.result.image_contents[i].discription + '</p></div>';
                    } else if (response.result.image_contents[i].media_type === 1) {
                        console.log(response.result.image_contents[i].media_type);
                        console.log(response.result.image_contents[i].image_hash);
                        console.log(response.result.image_contents[i].discription);

                        d_media_rows = d_media_rows + '<li><a href="' + response.result.image_contents[i].image_hash + '">' + response.result.image_contents[i].discription + '</a></li>';
                    }
                }

                var d_fields = "";
                for (var i = 0; i < response.result.field.length; i++) {
                    d_fields = d_fields + ' <div class="col-12 form-group row justify-content-around"><label for="status" class="col-sm-4 col-form-label">' + response.result.field[i].field_name
                        + '</label ><div class="col-sm-4 row"><input type="text" readonly class="form-control" id="" value="' + response.result.field[i].level_description
                        + '"/>  </div>  </div ></div >';
                }

                $("#fields").html(d_fields);
                $("#image_contents").html(d_image_contents);
                $('#media_rows').html(d_media_rows + '</ul>');
                $('#status_name').val(response.result.status_name);

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

                $('#button_bottom').html(getButtonWithObjectName('bài viết', arrButton));

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
        }
    );
};

function goToAuthorDetail() {
    setTimeout(function () {
        window.open(CLIENT_URL + 'User/detail?email=' + $('#author_email').val());
    },
    500);
};

function getContentTypeName(parameter) {
    if (parameter === 0)
        return "Văn bản";
    return "Mã nguồn";
}

$(document).ready(
    function () {
        $('#btnDelete').click(function () {
            modifyPost($('#id_to_delete').val(), 4);
        });

        $('#btnBlock').click(function () {
            modifyPost($('#id_to_block').val(), 2);
        });

        $('#btnActive').click(function () {
            modifyPost($('#id_to_active').val(), 0);
        });
    }
);

function modifyPost(oid, _status) {
    $.ajax({
        type: 'POST',
        url: API_URL + 'Post/modified-post-status',
        dataType: 'json',
        contentType: 'application/json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', getConfig());
        },
        data: JSON.stringify({
            post_id: oid.toLowerCase(),
            status: _status
        }),
        success: function (response) {
            if (response.code === 401)
                alert("Unauthorized. ");
            getPostDetailById();
        },
        error: function (response) {
            console.log(response);
        }
    });
}