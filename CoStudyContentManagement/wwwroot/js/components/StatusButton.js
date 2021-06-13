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