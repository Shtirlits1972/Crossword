function openDiv(divId) {

    var div = document.getElementById("div" + divId);
    var openHref = document.getElementById('href' + divId);
    var hrefText = openHref.innerText;

    if (hrefText === "open") {
        div.setAttribute("style", "display: block; overflow: auto; max-width: 300px; border: solid 1px blue; padding: 5px;");
        openHref.innerText = "close";
    }
    else if (hrefText === "close") {
        div.setAttribute("style", "display: none; overflow: auto;  max-width: 300px; border: solid 1px blue; padding: 5px;");
        openHref.innerText = "open";
    }

}