
// TODO: Add front-end loader support
var ajax = {
    request: null,

    execute: function(url, data, successCallback, completeCallback) {

        if (ajax.request != null) {
            ajax.request.abort();
            ajax.request = null;
        }

        ajax.request = $.ajax({
            url: url,
            data: data,
            success: function(response) {
                successCallback(response);
            },
            error: function(e) {
                if (e.statusText != "abort") {
                    console.log(e);
                    notification.show("Something went wrong: " + e.responseText, 'error');
                }
            },
            complete: function(e) {
                if (completeCallback != undefined && completeCallback != null) {
                    completeCallback();
                }
            }
        });
    }
};