document.addEventListener("DOMContentLoaded", () => {
    const entitySelect = document.getElementById("entitySelect");
    const addEntityButton = document.getElementById("addEntityButton");
    const companiesSection = document.getElementById("companiesSection");
    const busesSection = document.getElementById("busesSection");
    const companiesTableBody = document.getElementById("companiesTableBody");
    const busesTableBody = document.getElementById("busesTableBody");
    const modalElement = document.getElementById("entityModal");
    const deleteModalElement = document.getElementById("deleteModal");
    const modalTitle = document.getElementById("modalTitle");
    const entityForm = document.getElementById("entityForm");
    const modal = new bootstrap.Modal(modalElement);
    const deleteModal = new bootstrap.Modal(deleteModalElement);
    const confirmDeleteButton = document.getElementById("confirmDelete");

    let deleteCallback = null;

    const fetchCompanies = () => {
        fetch("/api/BusCompany/GetCompanies")
            .then((response) => response.json())
            .then((data) => renderCompanies(data));
    };

    const fetchBuses = () => {
        fetch("/api/BusCompany/GetBuses")
            .then((response) => response.json())
            .then((data) => renderBuses(data));
    };

    const refreshAll = () => {
        fetchCompanies();
        fetchBuses();
    };

    const renderCompanies = (companies) => {
        companiesTableBody.innerHTML = companies
            .map(
                (company) => `
                <tr>
                    <td>${company.companyName}</td>
                    <td>${company.contactInfo}</td>
                    <td>
                        <button class="btn btn-warning btn-sm edit-company" data-id="${company.busCompanyId}" data-name="${company.companyName}" data-contact="${company.contactInfo}">Edit</button>
                        <button class="btn btn-danger btn-sm delete-company" data-id="${company.busCompanyId}">Delete</button>
                    </td>
                </tr>`
            )
            .join("");
        attachListeners();
    };

    const renderBuses = (buses) => {
        busesTableBody.innerHTML = buses
            .map(
                (bus) => `
                <tr>
                    <td>${bus.busNumber}</td>
                    <td>${bus.companyName}</td>
                    <td>${bus.capacity}</td>
                    <td>
                        <button class="btn btn-warning btn-sm edit-bus" data-id="${bus.busId}" data-number="${bus.busNumber}" data-capacity="${bus.capacity}" data-company="${bus.busCompanyId}">Edit</button>
                        <button class="btn btn-danger btn-sm delete-bus" data-id="${bus.busId}">Delete</button>
                    </td>
                </tr>`
            )
            .join("");
        attachListeners();
    };

    const saveEntity = (url, method, data, fetchFunction) => {
        fetch(url, {
            method,
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data),
        })
            .then((response) => response.json())
            .then((result) => {
                if (result.success) {
                    fetchFunction();
                    modal.hide();
                } else {
                    alert(result.message);
                }
            });
    };

    const deleteEntity = (url) => {
        fetch(url, { method: "DELETE" })
            .then((response) => response.json())
            .then((result) => {
                if (result.success) {
                    refreshAll();
                    deleteModal.hide();
                } else {
                    alert(result.message);
                }
            });
    };

    const attachListeners = () => {
        document.querySelectorAll(".edit-company").forEach((button) => {
            button.addEventListener("click", () => {
                const id = button.getAttribute("data-id");
                const name = button.getAttribute("data-name");
                const contact = button.getAttribute("data-contact");
                openEditCompanyModal(id, name, contact);
            });
        });

        document.querySelectorAll(".delete-company").forEach((button) => {
            button.addEventListener("click", () => {
                const id = button.getAttribute("data-id");
                openDeleteModal(() => deleteCompany(id));
            });
        });

        document.querySelectorAll(".edit-bus").forEach((button) => {
            button.addEventListener("click", () => {
                const id = button.getAttribute("data-id");
                const number = button.getAttribute("data-number");
                const capacity = button.getAttribute("data-capacity");
                const company = button.getAttribute("data-company");
                openEditBusModal(id, number, capacity, company);
            });
        });

        document.querySelectorAll(".delete-bus").forEach((button) => {
            button.addEventListener("click", () => {
                const id = button.getAttribute("data-id");
                openDeleteModal(() => deleteBus(id));
            });
        });
    };

    const openDeleteModal = (callback) => {
        deleteCallback = callback;
        deleteModal.show();
    };

    confirmDeleteButton.addEventListener("click", () => {
        if (deleteCallback) {
            deleteCallback();
            deleteCallback = null;
        }
    });

    const deleteCompany = (id) => {
        deleteEntity(`/api/BusCompany/DeleteCompany/${id}`);
    };

    const deleteBus = (id) => {
        deleteEntity(`/api/BusCompany/DeleteBus/${id}`);
    };

    const openEditCompanyModal = (id, companyName, contactInfo) => {
        modalTitle.textContent = "Edit Company";
        entityForm.innerHTML = `
            <div class="mb-3">
                <label for="companyName" class="form-label">Company Name</label>
                <input type="text" class="form-control" id="companyName" value="${companyName}" required />
            </div>
            <div class="mb-3">
                <label for="contactInfo" class="form-label">Contact Info</label>
                <input type="text" class="form-control" id="contactInfo" value="${contactInfo}" required />
            </div>
            <button type="submit" class="btn btn-primary">Save</button>
        `;
        entityForm.onsubmit = (e) => {
            e.preventDefault();
            const data = {
                companyName: document.getElementById("companyName").value,
                contactInfo: document.getElementById("contactInfo").value,
            };
            saveEntity(`/api/BusCompany/EditCompany/${id}`, "PUT", data, fetchCompanies);
        };
        modal.show();
    };

    const openEditBusModal = (id, busNumber, capacity, busCompanyId) => {
        modalTitle.textContent = "Edit Bus";
        entityForm.innerHTML = `
            <div class="mb-3">
                <label for="busNumber" class="form-label">Bus Number</label>
                <input type="number" class="form-control" id="busNumber" value="${busNumber}" required />
            </div>
            <div class="mb-3">
                <label for="capacity" class="form-label">Capacity</label>
                <input type="number" class="form-control" id="capacity" value="${capacity}" required />
            </div>
            <div class="mb-3">
                <label for="companySelect" class="form-label">Select Company</label>
                <select class="form-select" id="companySelect" required></select>
            </div>
            <button type="submit" class="btn btn-primary">Save</button>
        `;
        fetch("/api/BusCompany/GetCompanies")
            .then((response) => response.json())
            .then((companies) => {
                const companySelect = document.getElementById("companySelect");
                companySelect.innerHTML = "";
                companies.forEach((company) => {
                    const option = document.createElement("option");
                    option.value = company.busCompanyId;
                    option.textContent = company.companyName;
                    companySelect.appendChild(option);
                });
                companySelect.value = busCompanyId;
            });
        entityForm.onsubmit = (e) => {
            e.preventDefault();
            const data = {
                busNumber: document.getElementById("busNumber").value,
                capacity: document.getElementById("capacity").value,
                busCompanyId: document.getElementById("companySelect").value,
            };
            saveEntity(`/api/BusCompany/EditBus/${id}`, "PUT", data, fetchBuses);
        };
        modal.show();
    };

    addEntityButton.addEventListener("click", () => {
        const isCompany = entitySelect.value === "companies";
        modalTitle.textContent = isCompany ? "Add Company" : "Add Bus";
        entityForm.innerHTML = isCompany
            ? `
                <div class="mb-3">
                    <label for="companyName" class="form-label">Company Name</label>
                    <input type="text" class="form-control" id="companyName" required />
                </div>
                <div class="mb-3">
                    <label for="contactInfo" class="form-label">Contact Info</label>
                    <input type="text" class="form-control" id="contactInfo" required />
                </div>
                <button type="submit" class="btn btn-primary">Save</button>
            `
            : `
                <div class="mb-3">
                    <label for="busNumber" class="form-label">Bus Number</label>
                    <input type="number" class="form-control" id="busNumber" required />
                </div>
                <div class="mb-3">
                    <label for="capacity" class="form-label">Capacity</label>
                    <input type="number" class="form-control" id="capacity" required />
                </div>
                <div class="mb-3">
                    <label for="companySelect" class="form-label">Select Company</label>
                    <select class="form-select" id="companySelect" required></select>
                </div>
                <button type="submit" class="btn btn-primary">Save</button>
            `;
        if (!isCompany) {
            fetch("/api/BusCompany/GetCompanies")
                .then((response) => response.json())
                .then((companies) => {
                    const companySelect = document.getElementById("companySelect");
                    companySelect.innerHTML = "";
                    companies.forEach((company) => {
                        const option = document.createElement("option");
                        option.value = company.busCompanyId;
                        option.textContent = company.companyName;
                        companySelect.appendChild(option);
                    });
                });
        }
        entityForm.onsubmit = (e) => {
            e.preventDefault();
            const data = isCompany
                ? {
                    companyName: document.getElementById("companyName").value,
                    contactInfo: document.getElementById("contactInfo").value,
                }
                : {
                    busNumber: document.getElementById("busNumber").value,
                    capacity: document.getElementById("capacity").value,
                    busCompanyId: document.getElementById("companySelect").value,
                };
            saveEntity(isCompany ? "/api/BusCompany/AddCompany" : "/api/BusCompany/AddBus", "POST", data, refreshAll);
        };
        modal.show();
    });

    entitySelect.addEventListener("change", (e) => {
        const isCompany = e.target.value === "companies";
        companiesSection.classList.toggle("d-none", !isCompany);
        busesSection.classList.toggle("d-none", isCompany);
        addEntityButton.textContent = isCompany ? "Add Company" : "Add Bus";
    });

    refreshAll();
});