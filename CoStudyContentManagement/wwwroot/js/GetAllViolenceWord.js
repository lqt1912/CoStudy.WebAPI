$(document).ready(
    function () {
        var dt = $('#violenceWordTable').DataTable(
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
                    url: API_URL + 'ViolenceWord/paged',
                    type: 'POST',
                    method: 'POST',
                    contentType: 'application/json',
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('Authorization', getConfig());
                    },
                    data: function (d) {
                        var settings = $('#violenceWordTable').dataTable().fnSettings();
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
                    { data: 'value', name: 'name', width: '10%', className: '' },
                    { data: 'oid', name: 'name', width: '10%', className: '' },
                ],
                columnDefs: [
                    {
                        "targets": [3],
                        "className":"text-center",
                        "render": function (data, type, row) {
                            var htmlString = '<button  id="btn_delete" style="border-radius:100px" class="btn btn-info"><i class="fas fa-times"></i></button> ';
                            return htmlString;
                        }
                    }                ]
            }
        );
    });

$(document).ready(
    function () {
        $('#violenceWordTable tbody').on('click', '#btn_delete', function () {
            var table = $('#violenceWordTable').DataTable();
            var data = table.row($(this).parents('tr')).data();
            deleteById(data.oid);
        });

        $('#btnAdd').click(function () {
            $('#addModal').modal("show");
        });

        $('#btn-pre-add').click(function () {
            addNewWord($('#new_value').val());
        });
    }
);

function deleteById(id) {
    $.ajax({
        type: "DELETE",
        url: API_URL + "ViolenceWord/" + id,
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', getConfig());
        },
        success: function (response) {
            if (response.code === 401)
                goToLoginPage();
            alert(response.result);
            $('#violenceWordTable').DataTable().ajax.reload();
        },
        error: function (response) {
            console.log(response);
        }
    });
}

function addNewWord(word) {
    $.ajax({
        type: "POST",
        url: API_URL + "ViolenceWord?value=" + word,
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', getConfig());
        },
        success: function (response) {
            if (response.code === 401)
                goToLoginPage();
            $('#violenceWordTable').DataTable().ajax.reload();
        },
        error: function (response) {
            console.log(response);
        }
    });
}