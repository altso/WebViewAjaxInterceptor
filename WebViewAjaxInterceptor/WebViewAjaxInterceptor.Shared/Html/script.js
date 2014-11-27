function log(message) {
    document.querySelector('#status').innerHTML += '<br />' + message;
}

log('script loaded');

document.querySelector('#action').onclick = function () {
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
    xhr.open('GET', 'ajax/data.anyextension', true);
    xhr.send();
}
