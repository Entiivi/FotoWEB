function reloadCSS() {
    var links = document.querySelectorAll("link[rel='stylesheet']");
    links.forEach(link => {
        link.href = link.href.split("?")[0] + "?" + new Date().getTime();
    });
}
