document.addEventListener("DOMContentLoaded", () => {
    const taxiSelect = document.getElementById("entitySelect");
    const addTaxiEntityButton = document.getElementById("addEntityButton");
    const taxiCompaniesSection = document.getElementById("companiesSection");
    const taxisSection = document.getElementById("taxisSection");
    const taxiCompaniesTableBody = document.getElementById("companiesTableBody");
    const taxisTableBody = document.getElementById("taxisTableBody");
    const modalElement = document.getElementById("entityModal");
    const deleteModalElement = document.getElementById("deleteModal");
    const modalTitle = document.getElementById("modalTitle");
    const entityForm = document.getElementById("entityForm");
    const modal = new bootstrap.Modal(modalElement);
    const deleteModal = new bootstrap.Modal(deleteModalElement);
    const confirmDeleteButton = document.getElementById("confirmDelete");

    let deleteCallback = null;

    const fetchTaxiCompanies = () => {
        fetch("/api/TaxiCompany/GetTaxiCompanies")
            .then(response => response.json())
            .then(data => renderTaxiCompanies(data));
    };

    const fetchTaxis = () => {
        fetch("/api/Taxi/GetTaxis")
            .then(response => response.json())
            .then(data => renderTaxis(data));
    };

    const refreshAll = () => {
        fetchCompanies();
        fetchTaxis();
    };

    const renderCompanies = (companies) => {
        taxiCompaniesTableBody.innerHTML = companies.map(company => `
            <tr>
                <td>${company.companyName}</td>
                <td>${company.contactInfo}</td>
                <td>
                    <button class="btn btn-warning btn-sm edit-company" data-id="${company.taxiCompanyId}" data-name="${company.companyName}" data-contact="${company.contactInfo}">Edit</button>
                    <button class="btn btn-danger btn-sm delete-company" data-id="${company.taxiCompanyId}">Delete</button>
                </td>
            </tr>
        `).join('');
        attachListeners();
    };

    const renderTaxis = (taxis) => {
        taxisTableBody.innerHTML = taxis.map(taxi => `
            <tr>
                <td>${taxi.licensePlate}</td>
                <td>${taxi.companyName}</td>
                <td>${taxi.driverName}</td>
                <td>
                    <button class="btn btn-warning btn-sm edit-taxi" data-id="${taxi.taxiId}" data-license="${taxi.licensePlate}" data-driver="${taxi.driverName}" data-company-id="${taxi.taxiCompanyId}">Edit</button>
                    <button class="btn btn-danger btn-sm delete-taxi" data-id="${taxi.taxiId}">Delete</button>
                </td>
            </tr>
        `).join('');
        attachListeners();
    };

    const saveEntity = (url, method, data, fetchFunction) => {
        fetch(url, {
            method: method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        })
            .then(response => response.json())
            .then(result => {
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
            .then(response => response.json())
            .then(result => {
                if (result.success) {
                    refreshAll();
                    deleteModal.hide();
                } else {
                    alert(result.message);
                }
            });
    };

    const attachListeners = () => {
        document.querySelectorAll(".edit-company").forEach(button => {
            button.addEventListener("click", () => {
                const id = button.getAttribute("data-id");
                const name = button.getAttribute("data-name");
                const contact = button.getAttribute("data-contact");
                openEditModal(id, name, contact, true);
            });
        });

        document.querySelectorAll(".delete-company").forEach(button => {
            button.addEventListener("click", () => {
                const id = button.getAttribute("data-id");
                openDeleteModal(() => deleteCompany(id));
            });
        });

        document.querySelectorAll(".edit-taxi").forEach(button => {
            button.addEventListener("click", () => {
                const id = button.getAttribute("data-id");
                const license = button.getAttribute("data-license");
                const driver = button.getAttribute("data-driver");
                const companyId = button.getAttribute("data-company-id");
                openEditModal(id, license, driver, false, companyId);
            });
        });

        document.querySelectorAll(".delete-taxi").forEach(button => {
            button.addEventListener("click", () => {
                const id = button.getAttribute("data-id");
                openDeleteModal(() => deleteTaxi(id));
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
            deleteEntity(`/api/TaxiCompany/DeleteCompany/${id}`);
        };

        const deleteTaxi = (id) => {
            deleteEntity(`/api/TaxiCompany/DeleteTaxi/${id}`);
        };


        const openEditCompanyModal = (id, companyName, contactInfo) => {
            modalTitle.textContent = "Edit Company";
            entityForm.innerHTML = `
                <div class="mb-3">
                    <label for="companyName" class="form-label">Company Name</label>
                    <input type="text" class="form-control" id="companyName" value="${nameOrLicense}" required />
                </div>
                <div class="mb-3">
                    <label for="contactInfo" class="form-label">Contact Info</label>
                    <input type="text" class="form-control" id="contactInfo" value="${contactOrDriver}" required />
                </div>
                <button type="submit" class="btn btn-primary">Save</button>
            `;
            entityForm.onsubmit = (e) => {
                e.preventDefault();
                const data = {
                    companyName: document.getElementById("companyName").value,
                    contactInfo: document.getElementById("contactInfo").value,
                };
                saveEntity(`/api/TaxiCompany/EditCompany/${id}`, "PUT", data, fetchCompanies);
            };
            modal.show();
        };

    const openEditTaxiModal = (id, nameOrLicense, contactOrDriver, isCompany, companyId) => {
        modalTitle.textContent = "Edit Taxi";
        entityForm.innerHTML = `
                <div class="mb-3">
                    <label for="licensePlate" class="form-label">License Plate</label>
                    <input type="text" class="form-control" id="licensePlate" value="${nameOrLicense}" required />
                </div>
                <div class="mb-3">
                    <label for="driverName" the form-label">Driver Name</label>
                    <input type="text" class="form-control" id="driverName" value="${contactOrDriver}" required />
                </div>
                <div class="mb-3">
                    <label for="taxiCompanySelect" the form-label">Select Company</label>
                    <select class="form-select" id="taxiCompanySelect" required></select>
                </div>
                <button type="submit" class="btn btn-primary">Save</button>
            `;
            fetch("/api/TaxiCompany/GetCompanies")
                .then((response) => response.json())
                .then((companies) => {
                    const companySelect = document.getElementById("companySelect");
                    companySelect.innerHTML = "";
                    companies.forEach((company) => {
                        const option = document.createElement("option");
                        option.value = company.taxiCompanyId;
                        option.textContent = company.companyName;
                        companySelect.appendChild(option);
                    });
                    companySelect.value = taxiCompanyId;
                });
        entityForm.onsubmit = (e) => {
            e.preventDefault();
            const data = {
                licensePlate: document.getElementById("licensePlate").value,
                driverName: document.getElementById("driverName").value,
                taxiCompanyId: document.getElementById("companySelect").value,
            };
            saveEntity(`/api/TaxiCompany/EditTaxi/${id}`, "PUT", data, fetchTaxis);
        };
        modal.show();
    };

    addEntityButton.addEventListener("click", () => {
        const isCompany = taxiSelect.value === "companies";
        modalTitle.textContent = isCompany ? "Add Taxi Company" : "Add Taxi";
        entityForm.innerHTML = isCompany ?
        `
            <div class="mb-3">
                <label for="companyName" class="form-label">Company Name</label>
                <input type="text" class="form-control" id="companyName" required />
            </div>
            <div class="mb-3">
                <label for="contactInfo" class="form-label">Contact Info</label>
                <input type="text" the form-control" id="contactInfo" required />
            </div>
            <button type="submit" class="btn btn-primary">Save</button>
        ` : `
            <div class="mb-3">
                <label for="licensePlate" class="form-label">License Plate</label>
                <input type="text" class="form-control" id="licensePlate" required />
            </div>
            <div class="mb-3">
                <label for="driverName" class="form-label">Driver Name</label
                <input type="text" class="form-control" id="driverName" required />
            </div>
            <div class="mb-3">
                <label for="taxiCompanySelect" class="form-label">Select Company</label>
                <select class="form-select" id="taxiCompanySelect" required></select>
            </div>
            <button type="submit" class="btn btn-primary">Save</button>
        `;
        if (!isCompany) {
            fetch("/api/TaxiCompany/GetCompanies")
                .then((response) => response.json())
                .then((companies) => {
                    const companySelect = document.getElementById("companySelect");
                    companySelect.innerHTML = "";
                    companies.forEach((company) => {
                        const option = document.createElement("option");
                        option.value = company.taxiCompanyId;
                        option.textContent = company.companyName;
                        companySelect.appendChild(option);
                    });
                });
        }
        entityForm.onsubmit = (e) => {
            e.preventDefault();
            const data = isCompany ? {
                companyName: document.getElementById("companyName").value,
                contactInfo: document.getElementById("contactInfo").value,
            } : {
                licensePlate: document.getElementById("licensePlate").value,
                driverName: document.getElementById("driverName").value,
                taxiCompanyId: document.getElementById("taxiCompanySelect").value,
            };
            saveEntity(isCompany ? "/api/TaxiCompany/AddCompany" : "/api/TaxiCompany/AddTaxi", "POST", data, refreshAll);
        };
        modal.show();
    });

        entitySelect.addEventListener("change", (e) => {
            const isCompany = e.target.value === "companies";
            companiesSection.classList.toggle("d-none", !isCompany);
            taxisSection.classList.toggle("d-none", isCompany);
            addEntityButton.textContent = isCompany ? "Add Company" : "Add Taxi";
        });


        refreshAll();
});
