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

    const driversSection = document.getElementById("driversSection");
    const driversTableBody = document.getElementById("driversTableBody");

    let deleteCallback = null;

    const fetchEntities = (url, renderFunction) => {
        fetch(url)
            .then((response) => response.json())
            .then((data) => renderFunction(data))
            .catch((error) => {
                console.error(`Error fetching from ${url}:`, error);
            });
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
            <td>${taxi.driverName || "No Driver Assigned"}</td>
            <td>${taxi.companyName}</td>
            <td>
                <button class="btn btn-warning btn-sm edit-taxi" 
                        data-id="${taxi.taxiId}" 
                        data-license="${taxi.licensePlate}" 
                        data-driver-id="${taxi.driverId || 0}" 
                        data-company-id="${taxi.taxiCompanyId}">
                    Edit
                </button>
                <button class="btn btn-danger btn-sm delete-taxi" 
                        data-id="${taxi.taxiId}">
                    Delete
                </button>
            </td>
        </tr>
    `).join("");
        attachListeners();
    };

    const renderDrivers = (drivers) => {
        driversTableBody.innerHTML = drivers.map((driver) => `
            <tr>
                <td>${driver.firstName}</td>
                <td>${driver.lastName}</td>
                <td>${driver.email}</td>
                <td>${driver.companyName}</td>
                <td>
                    <button class="btn btn-warning btn-sm edit-driver" 
                        data-id="${driver.userId}" 
                        data-firstname="${driver.firstName}" 
                        data-lastname="${driver.lastName}" 
                        data-email="${driver.email}">
                        Edit
                    </button>
                    <button class="btn btn-danger btn-sm delete-driver" data-id="${driver.userId}">Delete</button>
                </td>
            </tr>
        `).join("");
        attachListeners();
    };

    const saveEntity = (url, method, data) => {
        fetch(url, {
            method,
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data),
        })
            .then(async (response) => {
                if (!response.ok) {
                    const errorResponse = await response.text();
                    throw new Error(errorResponse);
                }
                return response.json();
            })
            .then((data) => {
                if (data.success) {
                    entityModal.hide();
                    refreshAll();
                } else {
                    alert(data.message || "An error occurred while saving the entity.");
                }
            })
            .catch((error) => {
                console.error("Error saving entity:", error);
                alert("An error occurred while saving the entity. Please check the console for details.");
            });
    };

    const deleteEntity = () => {
        if (deleteCallback) {
            deleteCallback();
            deleteCallback = null;
            deleteModal.hide();
        }
    };

    const refreshAll = () => {
        fetchEntities("/api/TaxiCompany/GetCompanies", renderCompanies);
        fetchEntities("/api/TaxiCompany/GetTaxis", renderTaxis);
        fetchEntities("/api/TaxiCompany/GetDrivers", renderDrivers);
    };

    const attachListeners = () => {

        document.querySelectorAll(".edit-company").forEach((button) => {
            button.addEventListener("click", () => {
                modalTitle.textContent = "Edit Company";

                const companyId = button.dataset.id;

                fetch(`/api/TaxiCompany/GetTaxiCompanyUser/${companyId}`)
                    .then((response) => response.json())
                    .then((data) => {
                        if (!data.success) {
                            alert(data.message || "An error occurred while fetching company user.");
                            return;
                        }

                        const user = data.data.user;
                        console.log("User:", user);

                        const userDisplay = user
                            ? `<p class="form-control mb-3 border"><strong>Assigned User: </strong>${user.firstName} ${user.lastName}</p>
                               <input type="hidden" id="userId" value="${user.userId}" />`
                            : `<p class="form-control mb-3 border"><strong>Assigned User: </strong> None</p>`;

                        entityForm.innerHTML = `
                    ${userDisplay}
                    <input type="text" class="form-control mb-3" id="companyName" value="${button.dataset.name}" placeholder="Company Name" required />
                    <input type="text" class="form-control mb-3" id="contactInfo" value="${button.dataset.contact}" placeholder="Contact Info" required />
                    <button type="submit" class="btn btn-primary">Save</button>
                `;

                        entityForm.onsubmit = (e) => {
                            e.preventDefault();
                            const companyName = document.getElementById("companyName").value.trim();
                            const contactInfo = document.getElementById("contactInfo").value.trim();
                            const userId = document.getElementById("userId") ? document.getElementById("userId").value : null;

                            if (companyName && contactInfo) {
                                saveEntity(`/api/TaxiCompany/EditCompany/${companyId}`, "PUT", {
                                    companyName,
                                    contactInfo,
                                    userId: userId ? parseInt(userId, 10) : null
                                });
                            } else {
                                alert("Please fill in all fields.");
                            }
                        };

                        entityModal.show();
                    })
                    .catch((error) => {
                        console.error("Error fetching company user:", error);
                        alert("An error occurred while fetching company user.");
                    });
            });
        });

        document.querySelectorAll(".edit-taxi").forEach((button) => {
            button.addEventListener("click", () => {
                modalTitle.textContent = "Edit Taxi";
                const taxiId = button.dataset.id;

                fetch(`/api/TaxiCompany/GetAvailableDrivers/${taxiId}`)
                    .then((response) => response.json())
                    .then((data) => {
                        if (!data.success) {
                            alert(data.message || "An error occurred while fetching available drivers.");
                            return;
                        }

                        const currentDriverId = parseInt(button.dataset.driverId, 10);
                        const drivers = data.drivers;

                        let driverOptions = `
                <option value="0" ${!currentDriverId ? "selected" : ""}>No Driver Assigned</option>
                `;

                        drivers.forEach((driver) => {
                            const isSelected = parseInt(driver.userId, 10) === currentDriverId;
                            driverOptions += `
                    <option value="${driver.userId}" ${isSelected ? "selected" : ""}>
                        ${driver.firstName} ${driver.lastName}
                    </option>
                `;
                        });

                        entityForm.innerHTML = `
                <input type="text" class="form-control mb-3" id="licensePlate" value="${button.dataset.license}" placeholder="License Plate" required />
                <select class="form-select mb-3" id="driverSelect" required>
                    ${driverOptions}
                </select>
                <button type="submit" class="btn btn-primary">Save</button>
                `;

                        entityForm.onsubmit = (e) => {
                            e.preventDefault();
                            const licensePlate = document.getElementById("licensePlate").value.trim();
                            const driverId = parseInt(document.getElementById("driverSelect").value, 10);

                            if (licensePlate) {
                                saveEntity(`/api/TaxiCompany/EditTaxi/${taxiId}`, "PUT", {
                                    licensePlate,
                                    DriverId: driverId > 0 ? driverId : null, 
                                });
                            } else {
                                alert("Please enter a valid license plate.");
                            }
                        };

                        entityModal.show();
                    })
                    .catch((error) => {
                        console.error("Error fetching drivers:", error);
                        alert("An error occurred while fetching drivers.");
                    });
            });
        });

        document.querySelectorAll(".delete-company, .delete-taxi").forEach((button) => {
            button.addEventListener("click", () => {
                const entityType = button.classList.contains("delete-company") ? "Company" : "Taxi";
                const entityId = button.dataset.id;

                deleteCallback = () => {
                    fetch(`/api/TaxiCompany/Delete${entityType}/${entityId}`, { method: "DELETE" })
                        .then(response => response.json())
                        .then(data => {
                            if (data.success) {
                                refreshAll();
                            } else {
                                alert(data.message || `Failed to delete ${entityType}.`);
                            }
                        })
                        .catch(error => {
                            console.error(`Error deleting ${entityType}:`, error);
                            alert(`An error occurred while deleting the ${entityType}.`);
                        });
                };
                deleteModal.show();
            });
        });

        document.querySelectorAll(".edit-driver").forEach((button) => {
            button.addEventListener("click", () => {
                modalTitle.textContent = "Edit Driver";

                entityForm.innerHTML = `
                    <input type="text" class="form-control mb-3" id="firstName" value="${button.dataset.firstname}" placeholder="First Name" required/>
                    <input type="text" class="form-control mb-3" id="lastName" value="${button.dataset.lastname}" placeholder="Last Name" required/>
                    <input type="email" class="form-control mb-3" id="email" value="${button.dataset.email}" placeholder="Email" required/>
                    <input type="password" class="form-control mb-3" id="password" placeholder="New Password (leave empty if not changing)" />
                    <button type="submit" class="btn btn-primary">Save</button>`;

                entityForm.onsubmit = (e) => {
                    e.preventDefault();
                    const firstName = document.getElementById("firstName").value.trim();
                    const lastName = document.getElementById("lastName").value.trim();
                    const email = document.getElementById("email").value.trim();
                    const password = document.getElementById("password").value;

                    if (firstName && lastName && email) {
                        saveEntity(`/api/TaxiCompany/EditDriver/${button.dataset.id}`, "PUT", {
                            firstName,
                            lastName,
                            email,
                            password: password || null
                        });
                    } else {
                        alert("Please fill in all required fields.");
                    }
                };
                entityModal.show();
            });
        });

        document.querySelectorAll(".delete-driver").forEach((button) => {
            button.addEventListener("click", () => {
                deleteCallback = () => {
                    fetch(`/api/TaxiCompany/DeleteDriver/${button.dataset.id}`, { method: "DELETE" })
                        .then(response => response.json())
                        .then(data => {
                            if (data.success) {
                                refreshAll();
                            } else {
                                alert(data.message || "Failed to delete Driver.");
                            }
                        })
                        .catch(error => {
                            console.error("Error deleting driver:", error);
                            alert("An error occurred while deleting the driver.");
                        });
                };
                deleteModal.show();
            });
        });

        confirmDeleteButton.addEventListener("click", deleteEntity);
    };

    addEntityButton.addEventListener("click", () => {
        const selectedEntity = entitySelect.value;

        if (selectedEntity === "companies") {
            modalTitle.textContent = "Add Company";
            fetch("/api/TaxiCompany/GetTaxiCompanyUsers")
                .then((response) => response.json())
                .then((users) => {
                    const userOptions = users.map((user) =>
                        `<option value="${user.userId}">${user.firstName} ${user.lastName}</option>`
                    ).join("");

                    entityForm.innerHTML = `
                    <input type="text" class="form-control mb-3" id="companyName" placeholder="Company Name" required/>
                    <input type="text" class="form-control mb-3" id="contactInfo" placeholder="Contact Info" required/>
                    <select class="form-select mb-3" id="userSelect" required>
                        <option value="" disabled selected>Select User</option>
                        ${userOptions}
                    </select>
                    <button type="submit" class="btn btn-primary">Save</button>`;
                    entityForm.onsubmit = (e) => {
                        e.preventDefault();
                        const companyName = document.getElementById("companyName").value.trim();
                        const contactInfo = document.getElementById("contactInfo").value.trim();
                        const userSelect = document.getElementById("userSelect").value;

                        if (companyName && contactInfo && userSelect) {
                            saveEntity("/api/TaxiCompany/AddCompany", "POST", {
                                companyName,
                                contactInfo,
                                userId: parseInt(userSelect, 10),
                            });
                        } else {
                            alert("Please fill in all required fields.");
                        }
                    };
                    entityModal.show();
                })
                .catch((error) => {
                    console.error("Error fetching Taxi Company Users:", error);
                    alert("An error occurred while fetching Taxi Company Users.");
                });
        } else if (selectedEntity === "taxis") {
            modalTitle.textContent = "Add Taxi";

            fetch("/api/TaxiCompany/GetCompany")
                .then((response) => response.json())
                .then((company) => {
                    fetch("/api/TaxiCompany/GetAvailableDrivers")
                        .then((response) => response.json())
                        .then((data) => {
                            if (!data.success) {
                                alert(data.message || "An error occurred while fetching drivers.");
                                return;
                            }

                            const drivers = data.drivers;

                            console.log("drivers", drivers);
                            const driverOptions = `
                        <option value="0" selected>No Driver Assigned</option>
                        ${drivers.map(driver =>
                                `<option value="${driver.userId}">${driver.firstName} ${driver.lastName}</option>`
                            ).join("")}
                    `;

                            entityForm.innerHTML = `
                        <p class="form-control mb-3 border"><strong>Company: </strong>${company.companyName}</p>
                        <input type="hidden" id="taxiCompanyId" name="taxiCompanyId" value="${company.taxiCompanyId}" />
                        <input type="text" class="form-control mb-3" id="licensePlate" placeholder="License Plate" required />
                        <select class="form-select mb-3" id="driverSelect" required>
                            ${driverOptions}
                        </select>
                        <button type="submit" class="btn btn-primary">Save</button>`;

                            entityForm.onsubmit = (e) => {
                                e.preventDefault();
                                const licensePlate = document.getElementById("licensePlate").value.trim();
                                const selectedDriverId = parseInt(document.getElementById("driverSelect").value, 10);
                                const taxiCompanyId = parseInt(document.getElementById("taxiCompanyId").value, 10);

                                if (licensePlate) {
                                    saveEntity("/api/TaxiCompany/AddTaxi", "POST", {
                                        licensePlate,
                                        DriverId: selectedDriverId > 0 ? selectedDriverId : null,
                                        TaxiCompanyId: taxiCompanyId,
                                    });
                                    console.log("Payload sent to AddTaxi:", {
                                        licensePlate,
                                        DriverId: selectedDriverId > 0 ? selectedDriverId : null,
                                        TaxiCompanyId: taxiCompanyId,
                                    });
                                } else {
                                    alert("Please enter a valid license plate.");
                                }
                            };

                            entityModal.show();
                        })
                        .catch((error) => {
                            console.error("Error fetching drivers:", error);
                            alert("An error occurred while fetching drivers.");
                        });
                })
                .catch((error) => {
                    console.error("Error fetching company:", error);
                    alert("An error occurred while fetching company details.");
                });
        }
        else if (selectedEntity === "drivers") {
            modalTitle.textContent = "Add Driver";
            fetch("/api/TaxiCompany/GetCompany")
                .then((response) => response.json())
                .then((company) => {
                    entityForm.innerHTML = `
                    <p class="form-control mb-3 border"><strong>Company: </strong>${company.companyName}</p>
                    <input type="text" class="form-control mb-3" id="firstName" placeholder="First Name" required/>
                    <input type="text" class="form-control mb-3" id="lastName" placeholder="Last Name" required/>
                    <input type="email" class="form-control mb-3" id="email" placeholder="Email" required/>
                    <input type="password" class="form-control mb-3" id="password" placeholder="Password" required/>
                    <button type="submit" class="btn btn-primary">Save</button>`;
                    entityForm.onsubmit = (e) => {
                        e.preventDefault();
                        const firstName = document.getElementById("firstName").value.trim();
                        const lastName = document.getElementById("lastName").value.trim();
                        const email = document.getElementById("email").value.trim();
                        const password = document.getElementById("password").value;

                        if (firstName && lastName && email && password) {
                            saveEntity("/api/TaxiCompany/AddDriver", "POST", {
                                firstName,
                                lastName,
                                email,
                                password
                            });
                        } else {
                            alert("Please fill in all required fields.");
                        }
                    };
                    entityModal.show();
                })
                .catch((error) => {
                    console.error("Error fetching company:", error);
                    alert("An error occurred while fetching company details.");
                });
        }
    });

    entitySelect.addEventListener("change", () => {
        const isCompany = entitySelect.value === "companies";
        const isTaxi = entitySelect.value === "taxis";
        const isDriver = entitySelect.value === "drivers";

        companiesSection.classList.toggle("d-none", !isCompany);
        taxisSection.classList.toggle("d-none", !isTaxi);
        driversSection.classList.toggle("d-none", !isDriver);

        if (isCompany) {
            addEntityButton.textContent = "Add Company";
        } else if (isTaxi) {
            addEntityButton.textContent = "Add Taxi";
        } else if (isDriver) {
            addEntityButton.textContent = "Add Driver";
        }
    });

    refreshAll();
});