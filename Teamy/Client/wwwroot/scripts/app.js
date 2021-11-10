function showAlert(message) {
    alert(message);
}

window.clipboardCopy = {
    copyText: function (text) {
        navigator.clipboard.writeText(text).then(function () {
            console.log("Copied to clipboard!");
        })
            .catch(function (error) {
                alert(error);
            });
    }
};


//usage
//var cookieEventId = await JS.InvokeAsync < string > ("ReadCookie.ReadCookie", "participationEventId");
//await JS.InvokeAsync < object > ("WriteCookie.WriteCookie", "participationEventId", participation.EventId, DateTime.Now.AddMinutes(1));
window.WriteCookie = {

    WriteCookie: function (name, value, days) {

        var expires;
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toGMTString();
        }
        else {
            expires = "";
        }
        document.cookie = name + "=" + value + expires + "; path=/";
    }
}
window.ReadCookie = {
    ReadCookie: function (cname) {
        var name = cname + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }
}