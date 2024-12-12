document.addEventListener("DOMContentLoaded", () => {
    const userId = document.getElementById("UserId").value;
    const selectedRole = document.getElementById("Role").dataset.selectedValue;
    const selectedBusinessType = document.getElementById("BusinessType").dataset.selectedValue;

    fetch(`/Authenticate/GetEnums`)
        .then((response) => response.json())
        .then((data) => {
            populateDropdown("Role", data.roles, selectedRole);
            populateDropdown("BusinessType", data.businessTypes, selectedBusinessType);
        });


    function populateDropdown(id, options, selectedValue) {
        const dropdown = document.getElementById(id);
        dropdown.innerHTML = `<option value="">Select ${id }</option>`;
        options.forEach((option) => {
            const isSelected = option.value == selectedValue ? "selected" : "";
            dropdown.innerHTML += `<option value="${option.value}" ${isSelected}>${option.text}</option>`;
        });
    }

    const form = document.getElementById("editUserForm");
    form.addEventListener("submit", (event) => {
        event.preventDefault();
        const formData = new FormData(form);

        fetch(`/Authenticate/EditUser`, {
            method: "POST",
            body: formData,
        })
            .then((response) => {
                if (response.ok) {
                    window.location.href = "/Authenticate/UserList";
                } else {
                    alert("Error updating user.");
                }
            })
            .catch((error) => {
                console.error("Error:", error);
            });
    });
});