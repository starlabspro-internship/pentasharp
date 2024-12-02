document.addEventListener("DOMContentLoaded", () => {
    const entitySelect = document.getElementById("entitySelect");
    const addEntityButton = document.getElementById("addEntityButton");
    const companiesSection = document.getElementById("companiesSection");
    const taxisSection = document.getElementById("taxisSection");
    const companiesTableBody = document.getElementById("companiesTableBody");
    const taxisTableBody = document.getElementById("taxisTableBody");
    const entityModal = new bootstrap.Modal(document.getElementById("entityModal"));
    const deleteModal = new bootstrap.Modal(document.getElementById("deleteModal"));
    const modalTitle = document.getElementById("modalTitle");
    const entityForm = document.getElementById("entityForm");
    const confirmDeleteButton = document.getElementById("confirmDelete");

    let deleteCallback = null;

    const fetchEntities = (url, renderFunction) => {
        fetch(url)
            .then((response) => {
                if (!response.ok) throw new Error(`Error fetching data from ${url}`);
                return response.json();
            })
            .then((data) => renderFunction(data))
            .catch((error) => console.error("Fetch error:", error));
    };

    const renderCompanies = (companies) => {
        companiesTableBody.innerHTML = companies.map((company) => `
            <tr>
                <td>${company.companyName}</td>
                <td>${company.contactInfo}</td>
                <td>
                    <button class="btn btn-warning btn-sm edit-company" data-id="${company.taxiCompanyId}" data-name="${company.companyName}" data-contact="${company.contactInfo}">Edit</button>
                    <button class="btn btn-danger btn-sm delete-company" data-id="${company.taxiCompanyId}">Delete</button>
                </td>
            </tr>
        `).join("");
        attachListeners();
    };

    const renderTaxis = (taxis) => {
        taxisTableBody.innerHTML = taxis.map((taxi) => `
            <tr>
                <td>${taxi.licensePlate}</td>
                <td>${taxi.companyName}</td>
                <td>${taxi.driverName}</td>
                <td>
                <span class="badge ${getStatusBadgeClass(taxi.status)}">${taxi.status}</span>
            </td>
                <td>
                    <button class="btn btn-warning btn-sm edit-taxi" data-id="${taxi.taxiId}" data-license="${taxi.licensePlate}" data-driver="${taxi.driverName}" data-company-id="${taxi.taxiCompanyId}" data-status="${taxi.status}">Edit</button>
                    <button class="btn btn-danger btn-sm delete-taxi" data-id="${taxi.taxiId}">Delete</button>
                </td>
            </tr>
        `).join("");
        attachListeners();
    };

    const getStatusBadgeClass = (status) => {
        switch (status) {
            case 'Available':
                return 'bg-success';
            case 'Busy':
                return 'bg-warning';
            case 'Start':
                return 'bg-info';
            case 'End':
                return 'bg-secondary';
            default:
                return 'bg-light text-dark';
        }
    };

    const saveEntity = (url, method, data) => {
        fetch(url, {
            method,
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data),
        })
            .then((response) => {
                if (!response.ok) {
                    return response.json().then((error) => {
                        throw new Error(error.message || "An error occurred");
                    });
                }
                return response.json();
            })
            .then(() => {
                entityModal.hide();
                refreshAll();
            })
            .catch((error) => {
                displayErrorMessage(error.message); 
            });
    };

    const deleteEntity = () => {
        if (deleteCallback) {
            deleteCallback();
            deleteModal.hide();
        }
    };

    const displayErrorMessage = (message) => {
        const alertBox = document.getElementById("alertBox"); 
        alertBox.innerHTML = `
        <div class="alert alert-danger alert-dismissible fade show" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>`;
    };

    const refreshAll = () => {
        fetchEntities("/api/TaxiCompany/GetCompanies", renderCompanies);
        fetchEntities("/api/TaxiCompany/GetTaxis", renderTaxis);
    };

    const attachListeners = () => {
        document.querySelectorAll(".edit-company").forEach((button) => {
            button.addEventListener("click", () => {
                modalTitle.textContent = "Edit Company";
                entityForm.innerHTML = `
                    <input type="text" class="form-control mb-3" id="companyName" value="${button.dataset.name}" />
                    <input type="text" class="form-control mb-3" id="contactInfo" value="${button.dataset.contact}" />
                    <button type="submit" class="btn btn-primary">Save</button>`;
                entityForm.onsubmit = (e) => {
                    e.preventDefault();
                    saveEntity(`/api/TaxiCompany/EditCompany/${button.dataset.id}`, "PUT", {
                        companyName: document.getElementById("companyName").value,
                        contactInfo: document.getElementById("contactInfo").value,
                    });
                };
                entityModal.show();
            });
        });

        document.querySelectorAll(".edit-taxi").forEach((button) => {
            button.addEventListener("click", () => {
                modalTitle.textContent = "Edit Taxi";
                fetch("/api/TaxiCompany/GetCompanies")
                    .then((response) => response.json())
                    .then((companies) => {
                        const companyOptions = companies.map((company) =>
                            `<option value="${company.taxiCompanyId}" ${company.taxiCompanyId == button.dataset.companyId ? "selected" : ""}>${company.companyName}</option>`
                        ).join("");
                        entityForm.innerHTML = `
                            <div id="alertBox" class="mt-3"></div>
                            <input type="text" class="form-control mb-3" id="licensePlate" value="${button.dataset.license}" />
                            <input type="text" class="form-control mb-3" id="driverName" value="${button.dataset.driver}" />
                            <select class="form-select mb-3" id="taxiCompanySelect">${companyOptions}</select>
                            <select class="form-select mb-3" id="statusSelect"></select>
                            <button type="submit" class="btn btn-primary">Save</button>`;

                        fetch("/api/TaxiCompany/GetTaxiStatuses")
                            .then((response) => response.json())
                            .then((statuses) => {
                                const statusSelect = document.getElementById("statusSelect");
                                statusSelect.innerHTML = statuses.map((status) => `
                                    <option value="${status.name}" ${status.name === button.dataset.status ? "selected" : ""}>${status.name}</option>
                                `).join("");
                            });

                        entityForm.onsubmit = (e) => {
                            e.preventDefault();
                            saveEntity(`/api/TaxiCompany/EditTaxi/${button.dataset.id}`, "PUT", {
                                licensePlate: document.getElementById("licensePlate").value,
                                driverName: document.getElementById("driverName").value,
                                taxiCompanyId: document.getElementById("taxiCompanySelect").value,
                                status: document.getElementById("statusSelect").value,
                            });
                        };
                        entityModal.show();
                    });
            });
        });

        document.querySelectorAll(".delete-company, .delete-taxi").forEach((button) => {
            button.addEventListener("click", () => {
                deleteCallback = () => {
                    fetch(`/api/TaxiCompany/${button.classList.contains("delete-company") ? "DeleteCompany" : "DeleteTaxi"}/${button.dataset.id}`, { method: "DELETE" })
                        .then(() => refreshAll());
                };
                deleteModal.show();
            });
        });

        confirmDeleteButton.addEventListener("click", deleteEntity);
    };

    addEntityButton.addEventListener("click", () => {
        const isCompany = entitySelect.value === "companies";
        modalTitle.textContent = isCompany ? "Add Company" : "Add Taxi";
        if (isCompany) {
            entityForm.innerHTML = `
                <input type="text" class="form-control mb-3" id="companyName" placeholder="Company Name" />
                <input type="text" class="form-control mb-3" id="contactInfo" placeholder="Contact Info" />
                <button type="submit" class="btn btn-primary">Save</button>`;
        } else {
            fetch("/api/TaxiCompany/GetCompanies")
                .then((response) => response.json())
                .then((companies) => {
                    const companyOptions = companies.map((company) =>
                        `<option value="${company.taxiCompanyId}">${company.companyName}</option>`
                    ).join("");
                    entityForm.innerHTML = `
                        <div id="alertBox" class="mt-3"></div>
                        <select class="form-select mb-3" id="taxiCompanySelect">${companyOptions}</select>
                        <input type="text" class="form-control mb-3" id="licensePlate" placeholder="License Plate" />
                        <input type="text" class="form-control mb-3" id="driverName" placeholder="Driver Name" />
                        <select class="form-select mb-3" id="statusSelect"></select>
                        <button type="submit" class="btn btn-primary">Save</button>`;

                    fetch("/api/TaxiCompany/GetTaxiStatuses")
                        .then((response) => response.json())
                        .then((statuses) => {
                            const statusSelect = document.getElementById("statusSelect");
                            statusSelect.innerHTML = statuses.map((status) => `
                                <option value="${status.name}">${status.name}</option>
                            `).join("");
                        });
                });
        }
        entityForm.onsubmit = (e) => {
            e.preventDefault();
            saveEntity(isCompany ? "/api/TaxiCompany/AddCompany" : "/api/TaxiCompany/AddTaxi", "POST", {
                companyName: document.getElementById("companyName")?.value,
                contactInfo: document.getElementById("contactInfo")?.value,
                licensePlate: document.getElementById("licensePlate")?.value,
                driverName: document.getElementById("driverName")?.value,
                taxiCompanyId: document.getElementById("taxiCompanySelect")?.value,
                status: document.getElementById("statusSelect")?.value,
            });
        };
        entityModal.show();
    });

    entitySelect.addEventListener("change", () => {
        const isCompany = entitySelect.value === "companies";
        companiesSection.classList.toggle("d-none", !isCompany);
        taxisSection.classList.toggle("d-none", isCompany);
        addEntityButton.textContent = isCompany ? "Add Company" : "Add Taxi";
    });

    refreshAll();
});
