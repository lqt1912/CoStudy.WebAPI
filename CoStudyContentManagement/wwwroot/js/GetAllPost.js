$(document).ready(
    function () {
        console.log(getConfig());
        //jQuery DataTables initialization
        var dt = $('#postTable').DataTable({
            paging: true,
            processing: true,
            dom: 'Blirtp',
            buttons: [
                'excel', 'pdf', 'print'
            ],
            serverSide: true, // for process on server side
            orderMulti: true,
            searching: true,
            bSort: false,
            bScrollCollapse: true,
            bAutoWidth: false,
            lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
            language: {
                info: "Hiển thị _START_ đến _END_ của _TOTAL_ mục",
                infoFiltered: "",
                lengthMenu: "Hiển thị _MENU_ mục",
                search: "Search",
                processing:
                    '<div class="border" style="background-color: #717571; font-size: 20px;line-height: 4; font-weight: bold">Đang tải dữ liệu...</div>',
                paginate: {
                    previous: "Đầu",
                    next: "Tiếp theo",
                    last: "Cuối"
                },
            },
            ajax: {
                url: API_URL + "Cms/post/paged",
                type: "POST",
                method: "POST",
                contentType: "application/json",
                dataType: "json",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('Authorization', getConfig());
                },
                data: function (d) {
                    var settings = $("#postTable").dataTable().fnSettings();
                    d.start = settings._iDisplayStart;
                    d.length = settings._iDisplayLength;

                    return JSON.stringify(d);
                },

                dataSrc: function (response) {
                    if (response.code === 401)
                        goToLoginPage();
                    if (response.data.length > 0) {

                        response.recordsTotal = response.recordsTotal;
                        response.recordsFiltered = response.recordsFiltered;

                        console.log(response);
                        return response.data;
                    } else {
                        return [];
                    };

                },
                dataFilter: function (data) {
                    return data;
                },
                error: function (status) {
                    console.log(status);
                }
            },
            columns: [
                {
                    data: "index",
                    name: "index",
                    width: "2%",
                    className: "text-center"
                },
                {
                    data: "oid",
                    name: "oid",
                    width: "3%"
                },
                {
                    data: "author_name",
                    name: "author_name",
                    width: "15%"
                },
                {
                    data: "title",
                    "name": "title",
                    width: "10%"
                },
                {
                    "data": "created_date",
                    "name": "created_date",
                    width: "10%"
                },
                {
                    "data": "upvote",
                    "name": "upvote",
                    width: "5%",
                    className: "text-center"
                },
                {
                    "data": "downvote",
                    "name": "downvote",
                    "width": "5%", className: "text-center"
                },               //index 4
                {
                    "data": "comments_count",
                    "name": "comments_count",
                    width: "10%", className: "text-center"

                },   //index 5
                {
                    name: "status_name",
                    data: "status_name",
                    width: "15%", className: "text-center"
                },
                {
                    name: "activity",
                    data: "status_name",
                    width: "10%"
                }
            ],
            columnDefs: [

                {
                    "targets": [4],
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
                    "className": '',
                    "render": function (data, type, row) {
                        return getButtonStatusOnTable(data);
                    }
                }
            ],
            initComplete: function () {
                // Apply the search
                this.api().columns().every(function (index) {
                    var that = this;
                    $('input', this.footer()).on('keyup change clear', function () {
                        if (that.search() !== this.value) {
                            that.column(index - 1).search(this.value).draw();
                        }
                    });
                });
            }
        });

        $('#postTable tfoot th').each(function () {
            if ($(this).attr('id') === 'foot_author' || $(this).attr('id') === 'foot_header') {
                var title = $(this).text();
                $(this).html('<input type="text" style="line-height: 0,5" class="form-control" placeholder="' + title + '" />');
            }
        });
    }
);

$(document).ready(
    function () {
        $('select[name$="_length"]').addClass('form-control custom-form-control');
        $('#slcStatus').on('change', function () {
            var oTable = $("#postTable").DataTable();
            oTable.columns(9).search(this.value);
            oTable.draw();
        });


        var table = $('#postTable').DataTable();
        $('#postTable tbody').on('click', 'tr', function () {
            if ($(this).hasClass('selected')) {
                $(this).removeClass('selected');
            }
            else {
                table.$('tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });

        $("#btnSearch").click(
            function () {
                oTable = $("#postTable").DataTable();
                oTable.columns(2).search($('#txt_title').val());
                oTable.columns(1).search($('#txt_author_name').val());
                oTable.columns(4).search($('#txt_date').val());
                oTable.draw();
            }
        );

        $("#btnClear").click(
            function () {

                table = $("#postTable").DataTable();
                $("#txt_title").val('');
                $("#txt_author_name").val('');
                oTable.columns(2).search($('#txt_title').val());
                oTable.columns(1).search($('#txt_author_name').val());
                oTable.draw();
            }
        );

        $('#postTable tbody').on('click', '#btn_detail', function () {
            var data = table.row($(this).parents('tr')).data();
            goToPostDetail(data.oid);
        });

        $('#postTable tbody').on('click', '#btn_delete', function () {
            var data = table.row($(this).parents('tr')).data();
            $('#id_to_delete').val(data.oid);
            $('#deleteConfirmModal').modal('show');
        });

        $('#postTable tbody').on('click', '#btn_block', function () {
            var data = table.row($(this).parents('tr')).data();
            $('#id_to_block').val(data.oid);
            $('#blockConfirmModal').modal('show');
        });

        $('#postTable tbody').on('click', '#btn_active', function () {
            var data = table.row($(this).parents('tr')).data();
            $('#id_to_active').val(data.oid);
            $('#activeConfirmModal').modal('show');
        });


        $('#btnDelete').click(function () {
            modifyPost($('#id_to_delete').val(), 4);
            $('#toastDiv').html(getToast('toast-delete', 'Xóa bài viết thành công. '));
            $('.toast').toast('show');

        });

        $('#btnBlock').click(function () {
            modifyPost($('#id_to_block').val(), 2);
            $('#toastDiv').html(getToast('toast-update', 'Cập nhật bài viết thành công. '));
            $('.toast').toast('show');

        });

        $('#btnActive').click(function () {
            modifyPost($('#id_to_active').val(), 0);
            $('#toastDiv').html(getToast('toast-active', 'Kích hoạt bài viết thành công. '));
            $('.toast').toast('show');

        });
    }
);

function goToPostDetail(id) {
    if (id != '')
        window.location.replace('detail?postId=' + id);
    else alert('Vui lòng chọn đối tượng. ');
}

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
            var oTable = $('#postTable').DataTable().ajax.reload();
        },
        error: function (response) {
            console.log(response);
        }
    });
}