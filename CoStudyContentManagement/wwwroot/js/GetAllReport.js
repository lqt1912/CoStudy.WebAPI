$(document).ready(
    function () {
        var dt = $('#reportTable').DataTable(
            {
                paging: true,
                processing: true,
                dom: 'lirtp',
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
                    url: API_URL + 'Cms/report/paged',
                    type: 'POST',
                    method: 'POST',
                    contentType: 'application/json',
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('Authorization', getConfig());
                    },
                    data: function (d) {
                        var settings = $('#reportTable').dataTable().fnSettings();
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
                    { data: 'index', name: 'index', width: '1%', className: 'text-center' },
                    { data: 'oid', name: 'oid', width: '5%', className: '' },
                    { data: 'author_name', name: 'author_name', width: '7%', className: '' },
                    { data: 'object_type', name: 'object_type', width: '5%', className: '' },
                    { data: 'report_reason', name: 'report_reason', width: '10%', className: '' },
                    { data: 'created_date', name: 'created_date', width: '5%', className: '' },
                    { data: 'approve_status', name: 'approve_status', width: '5%', className: 'text-center' },
                    { data: 'approved_by_name', name: 'approved_by_name', width: '5%', className: 'text-center' },
                    { data: 'approve_date', name: 'approve_date', width: '5%', className: 'text-center' },
                    { data: 'oid', name: 'test  ', width: '5%', className: 'text-center' }

                ],
                columnDefs:
                    [
                        {
                            "targets": [1],
                            "className": "text-center",
                            "render": function (data, type, row) {
                                return '***' + data.substring(20).toUpperCase();
                            }
                        },
                        {
                            "targets": [3],
                            "render": function (data, type, row) {
                                var a = row.object_type;
                                return a.substring(40);
                            }
                        },
                        {
                            "targets": [4],
                            "className": '',
                            "render": function (data, type, row) {
                                return row.report_reason[0].detail;
                            }
                        },
                        {
                            "targets": [5],
                            "className": "text-center",
                            "render": function (data, type, row) {
                                var d = new Date(row.created_date);
                                return d.toUTCString();
                            }
                        },
                        {
                            "targets": [8],
                            "className": "text-center",
                            "render": function (data, type, row) {
                                if (data == null)
                                    return '';
                                var d = new Date(row.approve_date);
                                return d.toUTCString();
                            }
                        },
                        {
                            "targets": [9],
                            "className": "text-center",
                            "render": function (data, type, row) {
                                var htmlString = '<button onclick="showModal()" class="btn btn-info" id="btn_detail" style="border-radius:100px;"><i class="fa fa-info-circle" aria-hidden="true"></i></button>';
                                return htmlString;
                            }
                        }

                    ]
            }
        );
    });

$(document).ready(
    function () {
        var table = $('#reportTable').DataTable();
        $('#reportTable tbody').on('click',
            'tr',
            function () {
                if ($(this).hasClass('selected')) {
                    $(this).removeClass('selected');
                } else {
                    table.$('tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                };
                var loggingSelected = table.rows('.selected').data();
                var dataIds = [];
                for (var i = 0; i < loggingSelected.length; i++)
                    dataIds.push(loggingSelected[i].oid);
                var data = {
                    ids: dataIds
                };
                getReportById(data.ids[0]);
            });
    });

function getReportById(id) {
    $.ajax({
        type: 'GET',
        url: API_URL + 'Report/' + id,
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', getConfig());
        },
        success: function (response) {
            $('#detail_id').val(response.result.oid.toUpperCase());
            $('#detail_author_name').val(response.result.author_name);
            $('#detail_author_email').val(response.result.author_email);
            $('#detail_type').val(response.result.object_type.substring(40));
            $('#detail_object_id').val(response.result.object_id.toUpperCase());

            var d_report_reason = '';
            for (var i = 0; i < response.result.report_reason.length; i++) {
                d_report_reason = d_report_reason + '<li>' + response.result.report_reason[i].detail + '</li>';
            }
            $('#detail_report_reason').html(d_report_reason);

            var report_status = response.result.is_approved;
            if (report_status === true) {
                $('#btn-pre-approve').css('display', 'none');
            } else {
                $('#btn-pre-approve').css('display', 'block');
            }
        },
        error: function (response) {
            console.log(response);
        }
    });
}

function showModal() {
    $('#detailModal').modal('show');
}

function goToAuthorDetail() {
    setTimeout(function () {
        window.open(CLIENT_URL + 'User/detail?email=' + $('#detail_author_email').val());
    }, 500);
};

function goToObjectDetail() {
    var templateUrl = "";
    var objectType = $('#detail_type').val();
    switch (objectType) {
        case 'Post':
            templateUrl = 'Post/detail?postId=';
            break;
        case 'Comment':
            templateUrl = 'Comment/detail?commentId=';
            break;
        case 'ReplyComment':
            templateUrl = 'ReplyComment/detail?replyCommentid=';
            break;
        default:
            alert('Đã có lỗi xảy ra. ');
    }
    setTimeout(function () {
        window.open(CLIENT_URL + templateUrl + $('#detail_object_id').val());
    }, 500);
}

$(document).ready(function () {
    $('select[name$="_length"]').addClass('form-control custom-form-control');

    $("#btnSearch").click(
        function () {
            var oTable = $("#reportTable").DataTable();

            oTable.columns(1).search($('#txt_id').val());
            oTable.columns(2).search($('#txt_author_name').val());
            oTable.columns(5).search($('#dt_createddate').val());
            oTable.columns(6).search($('#slcStatus').val());
            oTable.draw();
        }
    );

    $("#btnClear").click(
        function () {
            var oTable = $("#reportTable").DataTable();
            $("#txt_id").val('');
            $("#txt_author_name").val('');
            $("#dt_createddate").val('all');

            oTable.columns(1).search($('#txt_id').val());
            oTable.columns(2).search($('#txt_author_name').val());
            oTable.columns(5).search($('#dt_createddate').val());
            oTable.columns(6).search($('#slcStatus').val());
            oTable.draw();
        }
    );

    $('#btn-pre-approve').click(function () {
        $('#detailModal').modal('hide');
        $('#approveConfirmModal').modal('show');
        $('#id_to_delete').val($('#detail_id').val().toLowerCase());
    });


    $('#btnConfirm').click(function () {
        $.ajax({
            type: 'POST',
            url: API_URL + 'Report/approve-report',
            dataType: 'json',
            contentType: 'application/json',
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', getConfig());
            },
            data: JSON.stringify([$('#id_to_delete').val()]),
            success: function (response) {
                $('#reportTable').DataTable().ajax.reload;
                $('.toast').toast('show');
            },
            error: function (response) {

            }
        });

    });
});