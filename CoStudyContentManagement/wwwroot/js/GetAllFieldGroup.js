$(document).ready(
    function () {
        var dt = $('#fieldGroupTable').DataTable(
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
                    url: API_URL + 'MasterData/field-group/all',
                    type: 'POST',
                    method: 'POST',
                    contentType: 'application/json',
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('Authorization', getConfig());
                    },
                    data: function (d) {
                        var settings = $('#fieldGroupTable').dataTable().fnSettings();
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
                    { data: 'group_name', name: 'group_name', width: '5%', className: '' },
                    { data: 'fields', name: 'fields', width: '10%', className: '' },
                    { data: 'status_name', name: 'status_name', width: '5%', className: 'text-center' },
                    { data: 'oid', name: 'activity', width: '5%', className: '' }
                ],
                columnDefs: [
                    {
                        "targets": [3],
                        "className": 'text-center',
                        "render": function (data, type, row) {
                            return data.length;
                        }
                    },
                    {
                        "targets": [5],
                        "render": function (data, type, row) {
                            var htmlString = '<button  id="btn_detail" style="border-radius:100px" class="btn btn-info">Chi tiết</button> ';
                            return htmlString;
                        }
                    }
                ]
            }
        );


    });

function getAllField(existFields, skip, count) {
    $.ajax({
        type: 'GET',
        url: API_URL + 'Field?skip=' + skip + '&count=' + count,
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', getConfig());
        },
        success: function (response) {
            var d_field_rows = '';
            for (var i = 0; i < response.result.length; i++) {
                if (existFields.includes(response.result[i].oid)) {
                    d_field_rows = d_field_rows + '<div class="form-group col-6 row">' + '<div class="col-sm-3">' + '<input style="font-size: 8px" type="checkbox" id="' + response.result[i].oid + '" checked="checked"  class="form-control" />' + '</div>' + '<label class="col-sm-9 col-form-label infomation-label-header">' + response.result[i].value + '</label>' + '</div >';
                } else {
                    d_field_rows = d_field_rows + '<div class="form-group col-6 row">' + '<div class="col-sm-3">' + '<input style="font-size: 8px" type="checkbox" id="' + response.result[i].oid + '" class="form-control" />' + '</div>' + '<label class="col-sm-9 col-form-label infomation-label-header">' + response.result[i].value + '</label>' + '</div >';
                }
            }
            $('#field_rows').html(d_field_rows);
        },
        error: function (response) {
            console.log(response);
        }
    });
}

$(document).ready(function () {
    $('select[name$="_length"]').addClass('form-control custom-form-control');
    $('#fieldGroupTable tbody').on('click', '#btn_detail', function () {
        var table = $('#fieldGroupTable').DataTable();
        $('#fieldGroupId').val(table.row($(this).parents('tr')).data().oid);

        $('#fieldGroupName').val(table.row($(this).parents('tr')).data().group_name);
        var existFields = table.row($(this).parents('tr')).data().fields.map(x => x.oid);
        getAllField(existFields, 0, 200);
        $('#btn-pre-add').hide();
        $('#btn-pre-save').show();
        $('#detailModal ').modal('show');
    });

    $('#btn-pre-save').click(function () {
        $('#detailModal').modal('hide');
        $('#saveConfirmModal').modal('show');
        $('#btnSaveConfirm').show();
        $('#btnAddConfirm').hide();
    });

    $('#btnAdd').click(function() {
        $('#saveConfirmModal').modal('hide');
        getAllField([], 0, 200);
        $('#fieldGroupName').val('');
        $('#btn-pre-add').show();
        $('#btn-pre-save').hide();
        $('#detailModal').modal('show');
    });

    $('#btn-pre-add').click(function() {
        $('#detailModal').modal('hide');
        $('#saveConfirmModal').modal('show');
        $('#btnSaveConfirm').hide();
        $('#btnAddConfirm').show();
    });
    $('#btnAddConfirm').click(function() {
        addNew();
    });
});

function addNew() {
    alert('addnew');
    var checkboxes = document.querySelectorAll("input[type=checkbox]:checked");
    var fields = [];
    for (var i = 0; i < checkboxes.length; i++) {
        fields.push(checkboxes[i].getAttribute('id'));
    }
    var _data = JSON.stringify({
        group_name: $('#fieldGroupName').val(),
        field_ids: fields
    });

    $.ajax({
        type: 'POST',
        url: API_URL + 'FieldGroup',
        dataType: 'json',
        contentType: 'application/json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', getConfig());
        },
        data: _data,
        success: function (response) {
            $('#fieldGroupTable').DataTable().ajax.reload();
            $('#messageToast').text("Thêm mới thành công. ");
            $('.toast').toast('show');
        },
        error: function (response) {
            console.log(response);
        }
    });
}

function saveInfo() {
    var checkboxes = document.querySelectorAll("input[type=checkbox]:checked");
    var fields = [];
    for (var i = 0; i < checkboxes.length; i++) {
        fields.push(checkboxes[i].getAttribute('id'));
    }
    var _data = JSON.stringify({
        group_id: $('#fieldGroupId').val(),
        group_name: $('#fieldGroupName').val(),
        field_ids: fields
    });

    $.ajax({
        type: 'POST',
        url: API_URL + 'Field/add-to-group',
        dataType: 'json',
        contentType: 'application/json',
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', getConfig());
        },
        data: _data,
        success: function (response) {
            $('#fieldGroupTable').DataTable().ajax.reload();
            $('#messageToast').text("Chỉnh sửa thành công. ");
            $('.toast').toast('show');
        },
        error: function (response) {
            console.log(response);
        }
    });
}