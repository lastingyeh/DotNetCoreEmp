function confirmDelete(id, isShow) {
    var deleteSpan = 'deleteSpan_' + id;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + id;

    if (isShow) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    } else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}