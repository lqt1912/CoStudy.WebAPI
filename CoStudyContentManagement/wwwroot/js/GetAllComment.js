$(document).ready(
    function () {
        var dt = $('#commentTable').DataTable(
            {
                paging: true,
                processing: true,
                dom: 'Blirtp',
                buttons: [
                    'excel', 'pdf', 'print'
                ],
                serverSide: true,
                searching: true,
                orderMulti: true,
                bSort: false,
                bScrollCollapse: true,
                bAutoWidth: true,
                lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
                language: {
                    info: "Hiển thị _START_ đến _END_ của _TOTAL_ mục",
                    infoFiltered: "",
                    lengthMenu: "Hiển thị _MENU_ mục",
                    search: "Search",
                    processing: '<div class="border" style="background-color: #717571; font-size: 20px;line-height: 4; font-weight: bold">Đang tải dữ liệu...</div>',
                    paginate: {
                        previous: "Đầu",
                        next: "Tiếp theo",
                        last: "Cuối"
                    },
                },
                ajax: {
                    url: 'https://localhost:44323/api/Cms/comment/paged',
                    type: 'POST',
                    method: 'POST',
                    contentType: 'application/json',
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('Authorization', getConfig())
                    },
                    data: function (d) {
                        var settings = $('#commentTable').dataTable().fnSettings();
                        d.start = settings._iDisplayStart;
                        d.length = settings._iDisplayLength;
                        return JSON.stringify(d);
                    },
                    dataSrc: function (response) {
                        if (response.data.length > 0) {
                            response.recordsTotal = response.recordsTotal;
                            response.recordsFiltered = response.recordsFiltered;
                            return response.data;
                        }
                        else return [];
                    },
                    dataFilter: function (data) {
                        return data;
                    },
                    error: function (status) {
                        console.log(status);
                    }
                },
                columns: [
                    { data: 'index', name: 'index', width: '2%', className: 'text-center' },
                    { data: 'oid', name: 'oid', width: '2%', className: '' },
                    { data: 'author_name', name: 'author_name', width: '7%', className: '' },
                    { data: 'created_date', name: 'created_date', width: '5%', className: 'text-center' },
                    { data: 'contain_image', name: 'contain_image', width: '2%', className: 'text-center' },
                    { data: 'downvote_count', name: 'downvote_count', width: '2%', className: 'text-center' },
                    { data: 'upvote_count', name: 'upvote_count', width: '3%', className: 'text-center' },
                    { data: 'replies_count', name: 'replies_count', width: '2%', className: 'text-center' },
                    { data: 'status_name', name: 'status_name', width: '5%', className: 'text-center' },
                    { data: 'status_name', name: 'activity', width: '5%', className: '' },
                    { data: 'content', name: 'content', width: '5%', className: 'd-none' }
                ],
                columnDefs: [
                    {
                        "targets": [3],
                        "className": "text-center",
                        "render": function (data, type, row) {
                            var d = new Date(row.created_date);
                            return d.toLocaleDateString();
                        }
                    },
                    {
                        "targets": [8],
                        "render": function (data, type, row) {
                            return getStatusButton(data);
                        }
                    },
                    {
                        "targets": [9],
                        "className": "",
                        "render": function (data, type, row) {
                            return getButtonStatusOnTable(data);
                        }
                    }
                ],
                initComplete: function () {
                    // Apply the search
                    this.api().columns().every(function () {
                        var that = this;
                        $('input', this.footer()).on('keyup change clear', function () {
                            if (that.search() !== this.value) {
                                //alert(this.value);
                                that
                                    .search(this.value)
                                    .draw();
                            }
                        });
                    });
                }
            }
        );

        $('#commentTable tfoot th').each(function () {
            if ($(this).attr('id') === 'foot_id' || $(this).attr('id') ==='foot_author') {
                var title = $(this).text();
                $(this).html('<input type="text" style="line-height: 0,5" class="form-control" placeholder="' + title + '" />');
            }
        });
    }
);

$(document).ready(
    function () {
        var table = $("#commentTable").DataTable();
        $('#btnDelete').click(function () {
            modifyComment($('#id_to_delete').val(), 4);
            $('#toastDiv').html(getToast('toast-delete', 'Xóa bình luận thành công. '));
            $('.toast').toast('show');
        });

        $('#btnBlock').click(function () {
            modifyComment($('#id_to_block').val(), 2);
            $('#toastDiv').html(getToast('toast-update', 'Cập nhật bình luận thành công. '));
            $('.toast').toast('show');
        });

        $('#btnActive').click(function () {
            modifyComment($('#id_to_active').val(), 0);
            $('#toastDiv').html(getToast('toast-active', 'Kích hoạt bình luận thành công. '));
            $('.toast').toast('show');
        });

        $('#commentTable tbody').on('click', '#btn_detail', function () {
            var data = table.row($(this).parents('tr')).data();
            goToPostDetail(data.oid);
        });

        $('#commentTable tbody').on('click', '#btn_delete', function () {
            var data = table.row($(this).parents('tr')).data();
            $('#id_to_delete').val(data.oid);
            $('#deleteConfirmModal').modal('show');
        });

        $('#commentTable tbody').on('click', '#btn_block', function () {
            var data = table.row($(this).parents('tr')).data();
            $('#id_to_block').val(data.oid);
            $('#blockConfirmModal').modal('show');
        });

        $('#commentTable tbody').on('click', '#btn_active', function () {
            var data = table.row($(this).parents('tr')).data();
            $('#id_to_active').val(data.oid);
            $('#activeConfirmModal').modal('show');
        });

    }
);
$(document).ready(function () {
    $('select[name$="_length"]').addClass('form-control custom-form-control');
    $("#btnSearch").click(
        function () {
            var oTable = $("#commentTable").DataTable();
            oTable.columns(1).search($('#txt_title').val());
            oTable.columns(2).search($('#txt_author_name').val());
            oTable.columns(3).search($('#dt_createddate').val());
            oTable.columns(10).search($('#txt_content').val());
            oTable.draw();
        }
    );

    $("#btnClear").click(
        function () {
            var oTable = $("#commentTable").DataTable();
            $('#txt_title').val('');
            $('#txt_author_name').val('');
            $('#dt_createddate').val('');
            $('#txt_content').val('');
            oTable.columns(1).search('');
            oTable.columns(2).search('');
            oTable.columns(3).search('');
            oTable.columns(10).search('');
            oTable.draw();
        }
    );

    var table = $('#commentTable').DataTable();
    $('#commentTable tbody').on('click',
        'tr',
        function () {
            if ($(this).hasClass('selected')) {
                $(this).removeClass('selected');
            } else {
                table.$('tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });

    $('#commentTable tbody').on('click', '#btn_detail', function () {
        var table = $('#commentTable').DataTable();
        var data = table.row($(this).parents('tr')).data();
        goToCommentDetail(data.oid);
    });
});

function goToCommentDetail(id) {
    if (id != '')
        window.location.replace('detail?commentId=' + id);
    else alert("Vui lòng chọn đối tượng. ");
}

function modifyComment(oid, _status) {
    $.ajax({
        type: 'PUT',
        url: API_URL + 'Comment/modified-comment-status',
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
            var oTable = $('#commentTable').DataTable().ajax.reload();
        },
        error: function (response) {
            console.log(response);
        }
    });
}