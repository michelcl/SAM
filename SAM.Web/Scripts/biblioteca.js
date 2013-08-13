$(function () {
    gravarUrlNavegacao();
});

//Metodo utilizado para gravar a navegacao do usuario e poder voltar para pagina ideal.
function gravarUrlNavegacao() {
    if ($.cookie("UrlAtual") != undefined)
        if ($.cookie("UrlAtual") != window.location.href.replace(/\?[\w\W]*/, "")) {
            $.cookie("UrlAnterior", $.cookie("UrlAtual"), { path: "/" });
        }
    $.cookie("UrlAtual", window.location.href, { path: "/" });
}

function Voltar() {
    //if($.cookie("UrlAnterior"))
    //location.href = $.cookie("UrlAnterior");
    history.back();
}

// http://www.mkyong.com/javascript/how-to-detect-ie-version-using-javascript/
function getInternetExplorerVersion()
    // Returns the version of Windows Internet Explorer or a -1
    // (indicating the use of another browser).
{
    var rv = -1; // Return value assumes failure.
    if (navigator.appName == 'Microsoft Internet Explorer') {
        var ua = navigator.userAgent;
        var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
        if (re.exec(ua) != null)
            rv = parseFloat(RegExp.$1);
    }
    return rv;
}
