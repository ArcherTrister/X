window.addEventListener("load", async () => {

    var username = document.querySelector("#username");

    try {
        var resp = await fetch(new Request("/bff/me"));
        if (resp.ok) {
            document.querySelector(".logged-in").classList.remove("hide");

            let claims = await resp.json();
            console.log("user claims", claims);

            let sidClaim = claims.find(claim => claim.type === 'sid');
            if (sidClaim) {
                document.getElementById("logoutLink").href = "/bff/logout?sid=" + sidClaim.value;
            }

            let name = claims.find(claim => claim.type === 'name') || claims.find(claim => claim.type === 'sub');
            console.log(name);
            username.innerText = "logged in as: " + (name && name.value);

        } else if (resp.status === 401) {
            document.querySelector(".not-logged-in").classList.remove("hide");
            username.innerText = "(not logged in)";
        }
    }
    catch (e) {
        console.log("error checking user status");
    }
});

document.querySelector(".show_session").addEventListener("click", async () => {
    try {
        var resp = await fetch(new Request("/bff/me"));
        if (resp.ok) {
            let claims = await resp.json();

            document.querySelector(".modal-title").innerText = "Session Claims";

            let body = document.querySelector("#claims");
            body.innerHTML = "";

            claims.forEach(claim => {
                var c1 = document.createElement("td");
                c1.innerText = claim.type;
                var c2 = document.createElement("td");
                c2.innerText = claim.value;
                var r = document.createElement("tr");
                r.appendChild(c1);
                r.appendChild(c2);
                body.appendChild(r);
            });

            var modal = document.querySelector(".modal");
            var myModal = new bootstrap.Modal(modal);
            myModal.toggle();
        }
        else if (resp.status === 401) {
        }

    }
    catch (e) {
        console.log("error checking user session");
    }
});
