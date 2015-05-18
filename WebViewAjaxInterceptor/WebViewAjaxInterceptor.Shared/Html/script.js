function log(message) {
    document.querySelector('#status').innerHTML += '<br />' + message;
}

log('script loaded');

var actions = document.querySelectorAll('.action'), i, length;
for (i = 0, length = actions.length; i < length; i++) {
    actions[i].onclick = function() {
        var xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function() {
            switch (this.readyState) {
            case 0:
                log('request not initialized');
                break;
            case 1:
                log('server connection established');
                break;
            case 2:
                log('request received');
                break;
            case 3:
                log('processing request');
                break;
            case 4:
                log('request finished and response is ready');
                break;
            }
        };
        xhr.onerror = function(e) {
            log('error');
        };
        var url = this.getAttribute('data-url'),
            method = this.getAttribute('data-method') || 'GET';
        log('sending ' + method + ' ' + url);
        try {
            xhr.open(method, url, true);
            xhr.send();

        } catch (e) {
            log(e);
        }
    }
}
