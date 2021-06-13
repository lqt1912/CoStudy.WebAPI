function getButtonWithObjectName(name, actionList) {

    var _delete = '<button class="btn btn-outline-danger mr-2" id="btn-pre-delete">Xóa ' + name + '</button>';
    var _active = '<button class="btn btn-primary mr-2 " id="btn-pre-active">Mở ' + name + '</button>';
    var _block = '<button class="btn btn-danger mr-2" id="btn-pre-block">Chặn ' + name + '</button>';
    var _buttons = '';

    if (actionList.includes('active'))
        _buttons = _buttons + _active;

    if (actionList.includes('block'))
        _buttons = _buttons + _block;

    if (actionList.includes('delete'))
        _buttons = _buttons + _delete;

    return '<div class="row d-flex flex-row-reverse">' + _buttons + '</div>';
   
}