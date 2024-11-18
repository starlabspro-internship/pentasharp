const entitySelect = document.getElementById("entitySelect");
const addEntityButton = document.getElementById("addEntityButton");
const companiesSection = document.getElementById("companiesSection");
const busesSection = document.getElementById("busesSection");
const addModalLabel = document.getElementById("addModalLabel");
const addEntityForm = document.getElementById("addEntityForm");

entitySelect.addEventListener("change", (e) => {
    if (e.target.value === "companies") {
        companiesSection.classList.remove("d-none");
        busesSection.classList.add("d-none");
        addEntityButton.textContent = "Add Company";
        addModalLabel.textContent = "Add Company";
        addEntityForm.innerHTML = `
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
    } else if (e.target.value === "buses") {
        companiesSection.classList.add("d-none");
        busesSection.classList.remove("d-none");
        addEntityButton.textContent = "Add Bus";
        addModalLabel.textContent = "Add Bus";
        addEntityForm.innerHTML = `
            <div class="mb-3">
                <label for="busNumber" class="form-label">Bus Number</label>
                <input type="text" class="form-control" id="busNumber" placeholder="Enter bus number" />
            </div>
            <div class="mb-3">
                <label for="capacity" class="form-label">Capacity</label>
                <input type="number" class="form-control" id="capacity" placeholder="Enter capacity" />
            </div>
            <div class="mb-3">
                <label for="companySelect" class="form-label">Select Company</label>
                <select class="form-select" id="companySelect">
                    <option selected disabled>Select company</option>
                    <option value="1">Example Company</option>
                </select>
            </div>
            <button type="submit" class="btn btn-primary">Save</button>
        `;
    }
});

entitySelect.dispatchEvent(new Event("change"));