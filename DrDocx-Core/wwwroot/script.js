function submitForm() {
    for (let field of document.querySelectorAll(":disabled")) {
        field.disabled = false;
    }
  document.getElementById("form-submit").click();
}

function toggleEdit(buttonId, fieldIds) {
  let butt = document.getElementById(buttonId);
  butt.innerText = "Save";
  for(let fieldId of fieldIds) {
    let field = document.getElementById(fieldId);
    field.parentElement.MaterialTextfield.enable();
  }
  butt.onclick = submitForm;
}

function editTable(buttonId, rowSelector) {
  let butt = document.getElementById(buttonId);
  butt.innerText = "Save";
  for(let row of document.querySelector(rowSelector)) {
    let raw = row.childNodes[1];
    let scaled = row.childNodes[2];
  }
  butt.onclick = submitForm;
}