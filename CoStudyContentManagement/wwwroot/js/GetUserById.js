
$(document).ready(
    function () {
        getUserDetailByEmail();
    }
);

function getUserDetailByEmail() {
    $.ajax({
        type: 'GET',
        url: API_URL + 'Cms/user?email=' + $('#user_email').val(),
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', getConfig());
        },
        success: function (response) {
            $("#full_name").val(response.result.user.full_name);

            var d = new Date(response.result.user.date_of_birth);
            $("#date_of_birth").val(d.toISOString().substring(0, 10));

            $("#email").val(response.result.user.email);

            $("#phone_number").val(response.result.user.phone_number);

            $("#full_address").val(response.result.user.full_address);

            $("#avatar_hash").attr("src", response.result.user.avatar_hash);

            var d1 = new Date(response.result.user.created_date);
            $("#created_date").val(d1.toISOString().substring(0, 10));

            $("#post_count").val(response.result.user.post_count);
            $("#status").val(response.result.user.status_name);
            $("#status_id").val(response.result.user.status);

            var d_additional_infos = "";
            for (var i = 0; i < response.result.user.additional_infos.length; i++) {
                d_additional_infos = d_additional_infos + ' <div class="col-12 form-group row"><label for="status" class="col-sm-6 col-form-label">' + response.result.user.additional_infos[i].information_name
                    + '</label ><div class="col-sm-6 row"><input type="text" readonly class="form-control" id="" value="' + response.result.user.additional_infos[i].information_value
                    + '"/>  </div></div >';
            }

            $("#additional_infos").html(d_additional_infos);

            var d_fields = "";
            for (var i = 0; i < response.result.user.fields.length; i++) {
                d_fields = d_fields + ' <div class="col-12 form-group row justify-content-around"><label for="status" class="col-sm-4 col-form-label">' + response.result.user.fields[i].field_name
                    + '</label ><div class="col-sm-4 row"><input type="text" readonly class="form-control" id="" value="' + response.result.user.fields[i].level_description
                    + '"/>  </div> <div class="col-sm-4 row "><input type="text" readonly class="form-control" id="" value="' + response.result.user.fields[i].point + ' điểm "/> </div> </div ></div >';
            }

            $("#fields").html(d_fields);
            var role_num = response.result.account.role;
            if (role_num === 0)
                $("#role").val("Quản trị viên");
            else $("#role").val("Người dùng");

            $("#verificationToken").val(response.result.account.verificationToken);

            var verified_date = new Date(response.result.account.verified);
            $("#verified").val(verified_date.toISOString().substring(0, 10));

            var is_verified = response.result.account.isVerified;
            if (is_verified === true)

                $("#isVerified").val("Đã xác thực");
            else $("#isVerified").val("Chưa xác thực");

            $("#passwordHash").val("*******************");
            $("#acc_email").val(response.result.account.email);
            $('#user_id').val(response.result.user.oid);

            var arrButton = [];
            switch (parseInt($('#status_id').val())) {
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

            $('#button_bottom').html(getButtonWithObjectName('tài khoản', arrButton));

            $('#btn-pre-active').click(function () {
                $('#id_to_active').val($('#user_id').val());
                $('#activeConfirmModal').modal('show');
            });

            $('#btn-pre-delete').click(function () {
                $('#id_to_delete').val($('#user_id').val());
                $('#deleteConfirmModal').modal('show');
            });


            $('#btn-pre-block').click(function () {
                $('#id_to_block').val($('#user_id').val());
                $('#blockConfirmModal').modal('show');
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
            modifyUser($('#id_to_delete').val(), 4);
        });

        $('#btnBlock').click(function () {
            modifyUser($('#id_to_block').val(), 2);
        });

        $('#btnActive').click(function () {
            modifyUser($('#id_to_active').val(), 0);
        });
    }
);
function modifyUser(oid, _status) {
    $.ajax({
        type: 'PUT',
        url: API_URL + 'User/modified-user',
        dataType: 'json',
        contentType: 'application/json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', getConfig());
        },
        data: JSON.stringify({
            user_id: oid.toString(),
            status: _status
        }),
        success: function (response) {
            if (response.code === 401)
                alert("Unauthorized. ");
            getUserDetailByEmail();
        },
        error: function (response) {
            console.log(response);
        }
    });
}


