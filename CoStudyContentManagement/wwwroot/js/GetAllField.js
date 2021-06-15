$(document).ready(
    function () {
        var dt = $('#fieldTable').DataTable(
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
                    url: API_URL + 'MasterData/field/all',
                    type: 'POST',
                    method: 'POST',
                    contentType: 'application/json',
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('Authorization', getConfig());
                    },
                    data: function (d) {
                        var settings = $('#fieldTable').dataTable().fnSettings();
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
                    { data: 'value', name: 'value', width: '10%', className: '' },
                    { data: 'status_name', name: 'status_name', width: '5%', className: 'text-center' },
                    { data: 'oid', name: 'activity', width: '15%', className: 'text-center' },
                ],
                columnDefs: [
                    {
                        "targets": [3],
                        render: function(data, type, row) {
                            return getStatusButton(data);
                        }
                    },
                     {
                        "targets": [4],
                        "render": function (data, type, row) {
                            var htmlString = '<button  id="btn_detail" style="border-radius:100px" class="btn btn-info"><i class="fas fa-info-circle"></i></button> '
                                + '<button  id="btn_update" style="border-radius:100px" class="btn btn-danger"><i class="fas fa-pencil-alt"></i></button>';
                            return htmlString;
                        }
                    }
                 ]
            }
        );

        $('#fieldTable tbody').on('click', '#btn_detail', function () {
            var table = $('#fieldTable').DataTable();
            var data = table.row($(this).parents('tr')).data();
            getFieldById(data.oid);
            $('#detail_status').parent('div').parent('div').show();
            $('#detail_id').parent().parent().show();
            $('#detail_id').attr('readonly', true);
            $('#detail_value').attr('readonly', true);
            $('#detail_status').attr('disabled', true);
            $('#btn-pre-add').hide();
            $('#btn-pre-update').hide();
            $('#btn-close').show();

            $('#detailModal').modal('show');
        });

        $('#fieldTable tbody').on('click', '#btn_update', function () {
            var table = $('#fieldTable').DataTable();
            var data = table.row($(this).parents('tr')).data();
            getFieldById(data.oid);
            $('#detail_status').parent('div').parent('div').show();
            $('#detail_id').parent().parent().show();
            $('#detail_value').attr('readonly', false);
            $('#detail_status').attr('disabled', false);
            $('#btn-close').show();
            $('#btn-pre-update').show();
            $('#btn-pre-add').hide();

            $('#detailModal').modal('show');
        });

        $('#btn-pre-update').click(function() {
            $('#detailModal').modal('hide');
            $('#editConfirmModal').modal('show');
        });

        $('#btnEdit').click(function() {
            var _data = JSON.stringify({
                oid:$('#detail_id').val(),
                value: $('#detail_value').val(),
                status: (parseInt($('#detail_status').val()))
            });
            $.ajax({
                type: 'PUT',
                url:API_URL +'MasterData/field/update',
                dataType: 'json',
                contentType: 'application/json',
                data: _data,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('Authorization', getConfig());
                },
                success: function(respone) {
                    $('#fieldTable').DataTable().ajax.reload();
                    $('#messageToast').text("Cập nhật thành công. ");
                    $('.toast').toast('show');
                }
            });
        });

        $('#btnAdd').click(function() {
            $('#detailModal').modal('show');
            $('#detail_value').val('');
            $('#detail_value').attr('readonly', false);
            $('#detail_status').parent('div').parent('div').hide();
            $('#detail_id').parent().parent().hide();

            $('#btn-close').show();
            $('#btn-pre-add').show();
            $('#btn-pre-update').hide();
        });

        $('#btn-pre-add').click(function () {
            $('#detailModal').hide();
            $('#saveConfirmModal').modal('show');
        });

        $('#btnSave').click(function() {
            var _data = JSON.stringify({
                value: $('#detail_value').val()
            });
            $.ajax({
                type: 'POST',
                url: API_URL + 'Field',
                dataType: 'json',
                contentType: 'application/json',
                data: _data,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('Authorization', getConfig());
                },
                success: function (respone) {
                    $('#fieldTable').DataTable().ajax.reload();
                    $('#messageToast').text("Thêm mới thành công. ");
                    $('.toast').toast('show');
                }
            });
        });
        $('select[name$="_length"]').addClass('form-control custom-form-control');
    });

function getFieldById(id) {
    $.ajax({
        type: 'GET',
        url: API_URL + 'MasterData/field/'+id,
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', getConfig());
        },
        success: function (response) {
            $('#detail_id').val(response.result.oid);
            $('#detail_value').val(response.result.value);
            $('#detail_status').val(response.result.status);
        },
        error: function (response) {
            console.log(response);
        }
    });
}
