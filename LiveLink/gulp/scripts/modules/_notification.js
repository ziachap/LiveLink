var notification = {
    show: function(text, type) {
        if (type == null) {
            type = 'info';
        }
        new Noty({
            text: text,
            layout: 'bottomLeft',
            theme: 'relax',
            type: type,
            timeout: 2000
        }).show();
    }
};

$(function() {
    notification.show('Notifications ready!');
});