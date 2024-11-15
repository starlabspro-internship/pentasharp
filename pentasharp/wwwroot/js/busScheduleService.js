(function () {
    document.addEventListener("DOMContentLoaded", initialize);

    let scheduleIdToDelete = null;

    function initialize() {
        loadSchedules();
        loadRoutes();
        loadBuses();
        setupEventListeners();
    }

    function setupEventListeners() {
        const addScheduleButton = document.getElementById("openAddScheduleButton");
        if (addScheduleButton) {
            addScheduleButton.addEventListener("click", openAddScheduleModal);
        }

        const scheduleForm = document.getElementById("scheduleForm");
        if (scheduleForm) {
            scheduleForm.addEventListener("submit", function (event) {
                event.preventDefault();
                saveSchedule();
            });
        }

        const confirmDeleteButton = document.getElementById("confirmDeleteButton");
        if (confirmDeleteButton) {
            confirmDeleteButton.addEventListener("click", deleteSchedule);
        }
    }

    async function loadSchedules() {
        try {
            const response = await fetch('/api/BusSchedule/GetAllSchedules');
            if (!response.ok) throw new Error('Failed to load schedules');
            const schedules = await response.json();
            populateSchedulesTable(schedules);
        } catch {
            alert('Error loading schedules');
        }
    }

    function populateSchedulesTable(schedules) {
        const tableBody = document.getElementById('getSchedules');
        tableBody.innerHTML = '';
        schedules.forEach(schedule => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${schedule.routeName}</td>
                <td class="text-center">${schedule.busNumber} (${schedule.companyName})</td>
                <td class="text-center">${new Date(schedule.departureTime).toLocaleString()}</td>
                <td class="text-center">${new Date(schedule.arrivalTime).toLocaleString()}</td>
                <td class="text-center">${schedule.price.toFixed(2)}</td>
                <td class="text-center">
                    <span class="badge bg-success badge-custom">${schedule.status}</span>
                </td>
                <td class="text-center">${schedule.availableSeats}</td>
                <td class="text-center">
                    <button class="btn btn-info btn-sm btn-custom edit-schedule-btn" data-id="${schedule.scheduleId}">Edit</button>
                    <button class="btn btn-danger btn-sm btn-custom delete-schedule-btn" data-id="${schedule.scheduleId}">Delete</button>
                </td>
            `;
            tableBody.appendChild(row);
            row.querySelector('.edit-schedule-btn').addEventListener('click', () => openEditScheduleModal(schedule.scheduleId));
            row.querySelector('.delete-schedule-btn').addEventListener('click', () => openDeleteModal(schedule.scheduleId));
        });
    }

    async function loadRoutes() {
        try {
            const response = await fetch('/api/BusSchedule/GetAllRoutes');
            if (!response.ok) throw new Error('Failed to load routes');
            const routes = await response.json();
            populateDropdown('RouteId', routes, route => ({ value: route.routeId, text: route.routeName }));
        } catch {
            alert('Error loading routes');
        }
    }

    async function loadBuses() {
        try {
            const response = await fetch('/api/BusScheduleCompany/GetAllBuses');
            if (!response.ok) throw new Error('Failed to load buses');
            const buses = await response.json();
            populateDropdown('BusId', buses, bus => ({ value: bus.busId, text: `${bus.busNumber} (${bus.companyName})` }));
        } catch {
            alert('Error loading buses');
        }
    }

    function populateDropdown(dropdownId, items, getItemAttributes) {
        const dropdown = document.getElementById(dropdownId);
        dropdown.innerHTML = '<option value="" selected disabled>Select an option</option>';
        items.forEach(item => {
            const option = document.createElement('option');
            const attributes = getItemAttributes(item);
            option.value = attributes.value;
            option.textContent = attributes.text;
            dropdown.appendChild(option);
        });
    }

    function openAddScheduleModal() {
        const modal = new bootstrap.Modal(document.getElementById("scheduleModal"));
        const form = document.getElementById("scheduleForm");
        form.reset();
        form.action = "/api/BusSchedule/AddSchedule";
        history.pushState(null, "", "/AddSchedule");
        modal.show();
    }

    async function openEditScheduleModal(scheduleId) {
        const modal = new bootstrap.Modal(document.getElementById("scheduleModal"));
        const form = document.getElementById("scheduleForm");
        form.reset();
        form.action = `/api/BusSchedule/EditSchedule/${scheduleId}`;
        history.pushState(null, "", `/EditSchedule/${scheduleId}`);
        try {
            const response = await fetch(`/api/BusSchedule/GetScheduleById/${scheduleId}`);
            if (!response.ok) throw new Error('Failed to fetch schedule details');
            const schedule = await response.json();
            populateScheduleForm(schedule);
        } catch {
            alert('Error fetching schedule details');
        }
        modal.show();
    }

    function populateScheduleForm(schedule) {
        document.getElementById("RouteId").value = schedule.routeId;
        document.getElementById("BusId").value = schedule.busId;
        document.getElementById("DepartureTime").value = new Date(schedule.departureTime).toISOString().slice(0, 16);
        document.getElementById("Price").value = schedule.price;
        document.getElementById("AvailableSeats").value = schedule.availableSeats;
    }

    async function saveSchedule() {
        const form = document.getElementById("scheduleForm");
        const formData = new FormData(form);
        const schedule = {
            routeId: formData.get("RouteId"),
            busId: formData.get("BusId"),
            departureTime: formData.get("DepartureTime"),
            price: parseFloat(formData.get("Price")),
            availableSeats: parseInt(formData.get("AvailableSeats"), 10),
        };
        try {
            const response = await fetch(form.action, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(schedule),
            });
            loadSchedules();
            if (!response.ok) throw new Error('Failed to save schedule');
            closeModal();
            const result = await response.json();
       
        } catch {
            alert("Error saving schedule");
        }
    }

    function closeModal() {
        const modal = bootstrap.Modal.getInstance(document.getElementById("scheduleModal"));
        if (modal) modal.hide();
        history.pushState(null, "", "/api/BusSchedule/ManageBusSchedules");
    }

    function openDeleteModal(scheduleId) {
        scheduleIdToDelete = scheduleId;
        const modal = new bootstrap.Modal(document.getElementById("confirmDeleteModal"));
        modal.show();
    }

    async function deleteSchedule() {
        if (!scheduleIdToDelete) return;
        try {
            const response = await fetch(`/api/BusSchedule/DeleteSchedule/${scheduleIdToDelete}`, {
                method: "DELETE",
            });
            if (!response.ok) throw new Error('Failed to delete schedule');
            scheduleIdToDelete = null;
            loadSchedules();
        } catch {
            alert("Error deleting schedule");
        }
        const modal = bootstrap.Modal.getInstance(document.getElementById("confirmDeleteModal"));
        if (modal) modal.hide();
    }
})();