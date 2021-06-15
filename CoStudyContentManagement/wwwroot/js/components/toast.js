function getToast(type, text) {
    return '<div aria-live="polite" aria-atomic="true">' +
        '<div style="position: absolute; top: 0; right: 1rem;">' +
        '<div class="toast custom-toast '+type + '" role="alert" aria-live="assertive" aria-atomic="true" data-delay="4000">' +
        '<div class="toast-body"> ' +
        '<button type="button" class="ml-2 mb-1 close" data-dismiss="toast" aria-label="Close"> ' +
        '<span aria-hidden="true">×</span> ' +
        '</button><p id="messageToast">' +text + '</p>' +
        '</div></div></div></div>';
}