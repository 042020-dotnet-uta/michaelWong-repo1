const createModal = document.getElementById("create-modal");
document.getElementById("btn-create").addEventListener("click", () => {
    createModal.style.display = "block";
});
window.addEventListener("click", function(event) {
    if (event.target == createModal) {
        createModal.style.display = "none";
    }
});