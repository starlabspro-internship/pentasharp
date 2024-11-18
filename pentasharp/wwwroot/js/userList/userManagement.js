function populateEditModal(firstName, lastName, email, userId) {
    document.getElementById("editFirstName").value = firstName;
    document.getElementById("editLastName").value = lastName;
    document.getElementById("editEmail").value = email;
    document.getElementById("editUserId").value = userId;
}

function setDeleteUserId(userId) {
    document.getElementById("confirmDeleteBtn").setAttribute("data-user-id", userId);
}

document.getElementById("confirmDeleteBtn").addEventListener("click", function () {
    const userId = this.getAttribute("data-user-id");
    console.log(`User with ID ${userId} deleted`);
    const deleteModal = new bootstrap.Modal(document.getElementById("deleteModal"));
    deleteModal.hide();
});