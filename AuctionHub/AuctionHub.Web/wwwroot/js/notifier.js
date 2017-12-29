var notifier = notifier || {};

(function () {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-bottom-center",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "2000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
})();


notifier.showMessage = function (message, type) {
    switch (type.toLowerCase()) {
        case "warnin":
            notifier.showWarning(message);
            break;
        case "error":
            notifier.showError(message);
            break;
        case "success":
            notifier.showSuccess(message);
            break;
        default:
            notifier.showInfo(message);
            break;
    }
}
notifier.showInfo = function (message) {
    toastr.info(message);
}
notifier.showError = function (message) {
    toastr.error(message);
}
notifier.showSuccess = function (message) {
    toastr.success(message);
}
notifier.showWarning = function (message) {
    toastr.warning(message);
}