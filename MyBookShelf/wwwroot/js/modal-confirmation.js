// MyBookShelf - Confirmation de suppression
// Remplit la modale de confirmation avec le titre et l'identifiant
// du livre selectionne. JavaScript natif, aucune dependance
// autre que Bootstrap (deja present).

document.addEventListener("DOMContentLoaded", function () {
    var modal = document.getElementById("delete-confirmation-modal");
    if (!modal) {
        return;
    }

    var titleTarget = modal.querySelector("[data-book-title-target]");
    var idInput = modal.querySelector("input[name='id']");

    document.querySelectorAll("[data-delete-book]").forEach(function (button) {
        button.addEventListener("click", function () {
            if (titleTarget) {
                titleTarget.textContent = button.getAttribute("data-book-title") || "";
            }
            if (idInput) {
                idInput.value = button.getAttribute("data-book-id") || "";
            }
        });
    });
});
