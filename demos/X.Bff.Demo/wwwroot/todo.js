const todos = document.getElementById("todos");

window.addEventListener("load", async () => {
    let result = await fetch(new Request("/todos"));

    if (result.ok) {
        let data = await result.json();
        data.forEach(item => addRow(item));
    }
    else if (result.status !== 401) {
        // 401 == not logged in
        error(result.status)
    }
});

function error(msg) {
    let alert = document.querySelector(".alert");
    let alertMsg = document.querySelector("#errText");

    if (msg) {
        alert.classList.remove("hide");
        alertMsg.innerText = msg;
    }
    else {
        alert.classList.add("hide");
        alertMsg.innerText = '';
    }
}

function addRow(item) {
    let row = document.createElement("tr");
    row.dataset.id = item.id;
    todos.appendChild(row);

    function addCell(row, text) {
        let cell = document.createElement("td");
        cell.innerText = text;
        row.appendChild(cell);
    }

    addCell(row, item.id);
    addCell(row, item.date);
    addCell(row, item.name);
}
