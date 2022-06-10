function log() {
    document.getElementById("results").innerText = "";

    Array.prototype.forEach.call(arguments, function (msg) {
        if (typeof msg !== "undefined") {
            if (msg instanceof Error) {
                msg = "Error: " + msg.message;
            } else if (typeof msg !== "string") {
                msg = JSON.stringify(msg, null, 2);
            }
            document.getElementById("results").innerText += msg + "\r\n";
        }
    });
}

let userClaims = null;

(async function () {
    var req = new Request("/bff/user", {
        headers: new Headers({
            "X-CSRF": "1",
        }),
    });

    try {
        var resp = await fetch(req);
        if (resp.ok) {
            userClaims = await resp.json();

            log("user logged in");
            document.getElementById("country").disabled = false;
            document.getElementById("state").disabled = false;
            document.getElementById("newName").disabled = false;
            document.getElementById("newCode").disabled = false;
            document.getElementById("submit").disabled = false;
        } else if (resp.status === 401) {
            log("user not logged in");
            document.getElementById("country").disabled = true;
            document.getElementById("state").disabled = true;
            document.getElementById("newName").disabled = true;
            document.getElementById("newCode").disabled = true;
            document.getElementById("submit").disabled = true;
        }
    } catch (e) {
        log("error checking user status");
    }
})();

document.getElementById("login").addEventListener("click", login, false);
document.getElementById("logout").addEventListener("click", logout, false);

function login() {
    window.location = "/bff/login";
}

function logout() {
    if (userClaims) {
        var logoutUrl = userClaims.find(
            (claim) => claim.type === "bff:logout_url"
        ).value;
        window.location = logoutUrl;
    } else {
        window.location = "/bff/logout";
    }
}