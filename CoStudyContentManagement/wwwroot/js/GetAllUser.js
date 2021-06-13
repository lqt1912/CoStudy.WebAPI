$(document).ready(
    function () {
        getConfig();
        var dt = $("#userTable").DataTable(
            {
                paging: true,
                processing: true,
                dom: 'lirtp',
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
                    processing: '<div class="border" style="background-color: #717571; font-size: 20px;line-height: 4; font-weight: bold">Đang tải dữ liệu...</div>',
                    paginate: {
                        previous: "Đầu",
                        next: "Tiếp theo",
                        last: "Cuối"
                    },
                },
                ajax: {
                    url: "https://localhost:44323/api/Cms/user/paged",
                    type: "POST",
                    method: "POST",
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader('Authorization', getConfig());
                    },
                    contentType: "application/json",
                    dataType: "json",
                    data: function (d) {
                        var settings = $("#userTable").dataTable().fnSettings();
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
                        }
                        else {
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
                        width: "5%"
                    },
                    {
                        data: "full_name",
                        name: "full_name",
                        width: "15%"
                    },
                    {
                        data: "email",
                        name: "email",
                        width: "10%"
                    },
                    {
                        data: "phone_number",
                        name: "phone_number",
                        width: "10%"
                    },
                    {
                        data: "full_address",
                        name: "full_address",
                        width: "25%"
                    },
                    {
                        data: "date_of_birth",
                        name: "date_of_birth",
                        width: "10%"
                    },
                    {
                        data: "created_date",
                        name: "created_date",
                        width: "10%"
                    },
                    {
                        name: "status_name",
                        data: "status_name",
                        width: "5%"
                    },
                    {
                        name: "activity",
                        data: "email"
                    }
                ],
                columnDefs: [
                    {
                        "targets": [5],
                        "render": function (data, type, row) {
                            var d = new Date(row.date_of_birth);
                            return d.toLocaleDateString();
                        }
                    },
                    {
                        "targets": [6],
                        "render": function (data, type, row) {
                            var d = new Date(row.created_date);
                            return d.toLocaleDateString();
                        }
                    },
                    {
                        "targets": [7],
                        "render": function (data, type, row) {
                            return getStatusButton(data);
                        }
                    },
                    {
                        "targets": [8],
                        "className": 'text-center',
                        "render": function (data, type, row) {
                            var htmlString = '<button  id="btn_detail" style="border-radius:100px" class="btn btn-info"><i class="fa fa-info-circle" aria-hidden="true"></i></button> ';
                            return htmlString;
                        }
                    }
                ],
                initComplete: function () {
                    // Apply the search
                    this.api().columns().every(function () {
                        var that = this;

                        $('input', this.footer()).on('keyup change clear', function () {
                            if (that.search() !== this.value) {
                                that
                                    .search(this.value)
                                    .draw();
                            }
                        });
                    });
                }
            });

        $('#userTable tfoot th').each(function () {
            switch ($(this).attr('id')) {
                case 'tfoot_name':
                case 'tfoot_email':
                case 'tfoot_phone':
                case 'tfoot_address':
                {
                    var title = $(this).text();
                    $(this).html('<input type="text" style="line-height: 0,5" class="form-control" placeholder="' + title  + '" />');
                }
            }
        });
    }
);

$(document).ready(
    function () {

        $("#btnSearch").click(
            function () {
                var oTable = $("#userTable").DataTable();
                oTable.columns(1).search($('#txtName').val());
                oTable.columns(2).search($('#txtEmail').val());
                oTable.columns(3).search($('#txtPhoneNumber').val());
                oTable.columns(4).search($('#txtAddress').val());
                oTable.draw();
            }
        );

        $("#btnClear").click(
            function () {
                table = $("#userTable").DataTable();
                $("#txtName").val('');
                $("#txtEmail").val('');
                $("#txtPhoneNumber").val('');
                $('#txtAddress').val('');
                oTable.columns(1).search($('#txtName').val());
                oTable.columns(2).search($('#txtEmail').val());
                oTable.columns(3).search($('#txtPhoneNumber').val());
                oTable.columns(4).search($('#txtAddress').val());
                oTable.draw();
            }
        );

        var table = $('#userTable').DataTable();

        $('#userTable tbody').on('click', '#btn_detail', function () {
            var data = table.row($(this).parents('tr')).data();
            goToUserDetail(data.email);
        });

        $('#userTable tbody').on('click', '#btn_delete', function () {
            var data = table.row($(this).parents('tr')).data();
            $('#id_to_delete').val(data.email);
            $('#deleteConfirmModal').modal('show');
        });

        $('#btnDelete').click(
            function () {
                alert("Delete " + $('#id_to_delete').val());
            });
    }
);

function goToDetail() {
    var table = $('#userTable').DataTable();
    var loggingSelected = table.rows('.selected').data();
    var dataIds = [];
    for (var i = 0; i < loggingSelected.length; i++)
        dataIds.push(loggingSelected[i].email);
    var data = {
        ids: dataIds
    };
    goToUserDetail(data.ids[0]);
}

function goToUserDetail(email) {
    setTimeout(function () {
        window.open(CLIENT_URL + 'User/detail?email=' + email);
    }, 500);
}
