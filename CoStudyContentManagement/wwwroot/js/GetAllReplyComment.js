﻿$(document).ready(
    function () {
        var dt = $('#commentTable').DataTable(
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
                    url: API_URL + 'Cms/reply-comment/paged',
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
                    { data: 'index', name: 'index', width: '5%', className: 'text-center' },
                    { data: 'oid', name: 'oid', width: '5%', className: '' },
                    { data: 'author_name', name: 'author_name', width: '10%', className: '' },
                    { data: 'created_date', name: 'created_date', width: '5%', className: 'text-center' },
                    { data: 'downvote_count', name: 'downvote_count', width: '5%', className: 'text-center' },
                    { data: 'upvote_count', name: 'upvote_count', width: '5%', className: 'text-center' },
                    { data: 'status_name', name: 'status', width: '5%', className: 'text-center' },
                    { data: 'oid', name: 'activity', width: '5%', className: 'text-center' },
                    { data: 'content', name: 'content', width: '5%', className: 'd-none' },
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
                        "targets": [6],
                        "render": function (data, type, row) {
                            return getStatusButton(data);
                        }
                    },
                    {
                        "targets": [7],
                        "render": function (data, type, row) {
                            var htmlString = '<button  id="btn_detail" style="border-radius:100px" class="btn btn-info"><i class="fas fa-info-circle"></i></button> '
                                + '<button  id="btn_update" style="border-radius:100px" class="btn btn-danger"><i class="fas fa-pencil-alt"></i></button>';
                            return htmlString;
                        }
                    }

                ]
            }
        );
    });

$(document).ready(function () {


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

    $("#btnSearch").click(
        function () {
            var oTable = $("#commentTable").DataTable();
            oTable.columns(1).search($('#txt_id').val());
            oTable.columns(2).search($('#txt_author_name').val());
            oTable.columns(3).search($('#dt_createddate').val());
            oTable.columns(8).search($('#txt_content').val());
            oTable.draw();
        }
    );

    $('#btnClear').click(function () {
        var oTable = $("#commentTable").DataTable();
        $('#txt_id').val('');
        $('#txt_author_name').val('');
        $('#dt_createddate').val('');
        $('#txt_content').val('');
        oTable.columns(1).search($('#txt_id').val());
        oTable.columns(2).search($('#txt_author_name').val());
        oTable.columns(3).search($('#dt_createddate').val());
        oTable.columns(8).search($('#txt_content').val());
        oTable.draw();
    });

    $('#commentTable tbody').on('click', '#btn_detail', function () {
        var table = $('#commentTable').DataTable();
        var data = table.row($(this).parents('tr')).data();
        goToReplyCommentDetail(data.oid);
    });
    
});

function goToReplyCommentDetail(id) {
    if (id != '')
        window.location.replace('detail?replyCommentId=' + id);
    else alert("Vui lòng chọn đối tượng. ");
}