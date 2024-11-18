const taxiSelect = document.getElementById("taxiSelect");
const addTaxiEntityButton = document.getElementById("addTaxiEntityButton");
const taxiCompaniesSection = document.getElementById("taxiCompaniesSection");
const taxisSection = document.getElementById("taxisSection");
const addTaxiModalLabel = document.getElementById("addTaxiModalLabel");
const addTaxiForm = document.getElementById("addTaxiForm");

taxiSelect.addEventListener("change", (e) => {
    if (e.target.value === "companies") {
        taxiCompaniesSection.classList.remove("d-none");
        taxisSection.classList.add("d-none");
        addTaxiEntityButton.textContent = "Add Taxi Company";
        addTaxiEntityButton.setAttribute("data-bs-target", "#addTaxiModal");
        addTaxiModalLabel.textContent = "Add Taxi Company";
        addTaxiForm.innerHTML = `
            <div class="mb-3">
                <label for="companyName" class="form-label">Company Name</label>
                <input type="text" class="form-control" id="companyName" placeholder="Enter company name" />
            </div>
            <div class="mb-3">
                <label for="contactInfo" class="form-label">Contact Info</label>
                <input type="text" class="form-control" id="contactInfo" placeholder="Enter contact info" />
            </div>
            <button type="submit" class="btn btn-primary">Save</button>
        `;
    } else if (e.target.value === "taxis") {
        taxiCompaniesSection.classList.add("d-none");
        taxisSection.classList.remove("d-none");
        addTaxiEntityButton.textContent = "Add Taxi";
        addTaxiEntityButton.setAttribute("data-bs-target", "#addTaxiModal");
        addTaxiModalLabel.textContent = "Add Taxi";
        addTaxiForm.innerHTML = `
            <div class="mb-3">
                <label for="licensePlate" class="form-label">License Plate</label>
                <input type="text" class="form-control" id="licensePlate" placeholder="Enter license plate" />
            </div>
            <div class="mb-3">
                <label for="driverName" class="form-label">Driver Name</label>
                <input type="text" class="form-control" id="driverName" placeholder="Enter driver name" />
            </div>
            <div class="mb-3">
                <label for="taxiCompanySelect" class="form-label">Taxi Company</label>
                <select class="form-select" id="taxiCompanySelect">
                    <option selected disabled>Select company</option>
                    <option value="1">Example Taxi Company</option>
                </select>
            </div>
            <button type="submit" class="btn btn-primary">Save</button>
        `;
    }
});

taxiSelect.dispatchEvent(new Event("change"));