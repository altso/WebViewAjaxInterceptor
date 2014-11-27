function log(message) {
    document.querySelector('#status').innerHTML += '<br />' + message;

}

log('script loaded');
document.querySelector('#action').onclick = function () {
    log('yay!');
}
