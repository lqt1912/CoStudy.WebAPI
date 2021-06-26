$(document).ready(
    function () {
        var dt = $('#levelTable').DataTable(
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
                lengthMenu: [[5, 10, 20, 50], [5, 10, 20, 50]],
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
                    url: API_URL + 'MasterData/level/all',
                    type: 'POST',
                    method: 'POST',
                    contentType: 'application/json',
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('Authorization', getConfig());
                    },
                    data: function (d) {
                        var settings = $('#levelTable').dataTable().fnSettings();
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
                    { data: 'name', name: 'name', width: '10%', className: '' },
                    { data: 'description', name: 'description', width: '5%', className: 'text-center' },
                    { data: 'order', name: 'order', width: '5%', className: 'text-center' },
                    { data: 'is_active', name: 'is_active', width: '10%', className: 'text-center' },
                    { data: 'created_date', name: 'created_date', width: '5%', className: 'text-center' },
                    { data: 'oid', name: 'oid', width: '10%', className: '' },
                ],
                columnDefs: [
                    {
                        targets: [5],
                        render: function (data) {
                            return getName(data);
                        }
                    },
                    {
                        "targets": [6],
                        "render": function (data, type, row) {
                            return (new Date(data)).toLocaleDateString();
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
    $('select[name$="_length"]').addClass('form-control custom-form-control');
    $('#btnAdd').click(function () {
        $('#detail_id').val('<<auto generate>>');
        $('#detail_name').val('');
        $('#detail_name').attr('readonly', false);

        $('#detail_description').val('');
        $('#detail_description').attr('readonly', false);

        $('#detail_order').val('');
        $('#detail_order').attr('readonly', false);

        $('#detail_status').val('true');
        $('#detail_status').attr('disabled', false);

        $('#btn-pre-save').text("Lưu");
        $('#btn-pre-save').addClass('btn btn-primary');

        $('#detailModal').modal('show');
    });

    $('#btn-pre-save').click(function () {
        $('#detailModal').modal('hide');
        $('#saveConfirmModal').modal('show');
    });

    $('#btnSave').click(function () {

        var _data = JSON.stringify({
            name: $('#detail_name').val(),
            description: $('#detail_description').val(),
            order: parseInt($('#detail_order').val()),
            is_active: ($('#detail_status').val() === 'true')
        });

        $.ajax({
            type: 'POST',
            url: API_URL + 'MasterData/level/add',
            data: _data,
            dataType: 'json',
            contentType: 'application/json',
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', getConfig());
            },
            success: function (response) {
                $('#levelTable').DataTable().ajax.reload();
                $('#messageToast').text("Thêm mới thành công. ");
                $('.toast').toast('show');
            },
            error: function (response) {
                console.log(response);
            }
        });
    });

    $('#levelTable tbody').on('click', '#btn_detail', function () {
        var table = $('#levelTable').DataTable();
        var data = table.row($(this).parents('tr')).data();
        getLevelById(data.oid);
        $('#detailModal').modal('show');
    });

    $('#levelTable tbody').on('click', '#btn_update', function () {
        var table = $('#levelTable').DataTable();
        var data = table.row($(this).parents('tr')).data();
        getLevelById(data.oid);
        $('#detailModal').modal('hide');
        $('#updateModal').modal('show');
    });

    $('#btn-pre-approve').click(function () {
        $('#updateModal').modal('hide');
        $('#saveConfirmModal').modal('show');
    });

    $('#btnSave').click(function () {
        var _data = JSON.stringify({
            oid: $('#update_id').val(),
            name: $('#update_name').val(),
            description: $('#update_description').val(),
            order: parseInt($('#update_order').val()),
            is_active: ($('#update_status').val() === 'true')
        });
        $.ajax({
            type: 'PUT',
            url: API_URL + 'MasterData/level/update',
            data: _data,
            dataType: 'json',
            contentType: 'application/json',
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', getConfig());
            },
            success: function (response) {
                $('#levelTable').DataTable().ajax.reload();
                $('.toast').toast('show');
            },
            error: function (response) {
                console.log(response);
            }
        });
    });

});

function getLevelById(id) {
    $.ajax({
        type: 'GET',
        url: API_URL + 'MasterData/level/' + id,
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', getConfig());
        },
        success: function (response) {
            if (response.code === 401)
                goToLoginPage();

            $('#detail_id').val(response.result.oid);
            $('#detail_id').attr('readonly', true);

            $('#detail_name').val(response.result.name);
            $('#detail_name').attr('readonly', true);

            $('#detail_description').val(response.result.description);
            $('#detail_description').attr('readonly', true);

            $('#detail_order').val(response.result.order);
            $('#detail_order').attr('readonly', true);

            $('#detail_status').val(response.result.is_active.toString());
            $('#detail_status').attr('disabled', true);

            $('#update_id').val(response.result.oid);
            $('#update_name').val(response.result.name);
            $('#update_description').val(response.result.description);
            $('#update_order').val(response.result.order);
            $('#update_status').val(response.result.is_active.toString());
        }
    });
}

function getName(name) {
    if (name === true)
        return '<span class="status-active">Đang hoạt động</span>';
    return '<span class="status-blocked">Không hoạt động</span>';
}