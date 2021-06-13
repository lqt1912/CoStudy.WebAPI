$(document).ready(
    function () {
        console.log(getConfig());
        //jQuery DataTables initialization
        var dt = $('#postTable').DataTable({
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
                    data: "oid",
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
                    "className": 'text-center',
                    "render": function (data, type, row) {
                        var htmlString = '<button  id="btn_detail" style="border-radius:100px" class="btn btn-info"><i class="fa fa-info-circle" aria-hidden="true"></i></button> ';
                        return htmlString;
                    }
                }
            ]
        });

        $('#postTable tfoot th').each(function () {
            switch ($(this).attr('id')) {
                case 'tfoot_id':
                case 'tfoot_author':
                case 'tfoot_header':
                    {
                        var title = $(this).text();
                        $(this).html('<input type="text" style="line-height: 0,5" class="form-control" placeholder="' + title + '" />');
                    }
            }
        });
    }
);

$(document).ready(
    function () {
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
    }
);

function goToPostDetail(id) {
    if (id != '')
        window.location.replace('detail?postId=' + id);
    else alert('Vui lòng chọn đối tượng. ');
}