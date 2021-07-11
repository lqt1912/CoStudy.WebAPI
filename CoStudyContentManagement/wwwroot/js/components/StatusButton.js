function getStatusButton(data) {
    switch (data) {
        case "Active":
            return '<span class="status-active">' + data + '</span>';
        case "Blocked":
            return '<span class="status-blocked">' + data + '</span>';
        case "Deleted":
            return '<span class="status-deleted">' + data + '</span>';
        default:
            return data;
    }
}

function getButtonStatusOnTable(status) {
    var _btnDetail =
        '<button  id="btn_detail" style="border-radius:100px" class="btn btn-info" data-toggle="tooltip" data-placement="bottom" title="Chi tiết"><i class="fas fa-info-circle"></i></button> ';
    var _btnDelete =
        '<button  id="btn_delete" style="border-radius:100px" class="btn btn-danger" data-toggle="tooltip" data-placement="bottom" title="Xóa"><i class="fas fa-times"></i></button> ';
    var _btnBlock =
        '<button  id="btn_block" style="border-radius:100px" class="btn btn-danger" data-toggle="tooltip" data-placement="bottom" title="Chặn"><i class="fas fa-lock"></i></button> ';
    var _btnActive =
        '<button  id="btn_active" style="border-radius:100px" class="btn btn-success" data-toggle="tooltip" data-placement="bottom" title="Mở"><i class="fas fa-check-circle"></i></button> ';
    var _buttons = _btnDetail;
    switch (status) {
        case 'Active':
            _buttons = _buttons + _btnBlock + _btnDelete;
            break;
        case 'Blocked':
            _buttons = _buttons + _btnActive +_btnDelete;
            break;
        default:
    }
    return _buttons;
}