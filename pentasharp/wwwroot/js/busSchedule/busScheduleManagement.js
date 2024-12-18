document.addEventListener("DOMContentLoaded", function () {
    const manageDropdown = document.getElementById("manageDropdown");
    const busRoutesSection = document.getElementById("busRoutesSection");
    const busScheduleSection = document.getElementById("busScheduleSection");
    const routeModal = new bootstrap.Modal(document.getElementById("routeModal"));
    const scheduleModal = new bootstrap.Modal(document.getElementById("scheduleModal"));
    const getRoutesTable = document.getElementById("getRoutes");
    const getSchedulesTable = document.getElementById("getSchedules");
    const routeDropdown = document.getElementById("RouteId");
    const busDropdown = document.getElementById("BusId");
    const deleteRouteButton = document.getElementById("deleteRouteButton");
    const deleteScheduleButton = document.getElementById("deleteScheduleButton");
    const availableSeatsInput = document.getElementById("AvailableSeats");
    let currentEditRouteId = null;
    let currentEditScheduleId = null;

    manageDropdown.addEventListener("change", function () {
        if (this.value === "busRoutes") {
            busRoutesSection.classList.remove("d-none");
            busScheduleSection.classList.add("d-none");
        } else {
            busRoutesSection.classList.add("d-none");
            busScheduleSection.classList.remove("d-none");
        }
    });

    function fetchRoutes() {
        return fetch("/api/BusSchedule/GetRoutes")
            .then(response => response.json())
            .then(data => {
                getRoutesTable.innerHTML = data.map(route => `
                    <tr>
                        <td>${route.fromLocation}</td>
                        <td>${route.toLocation}</td>
                        <td>${route.estimatedDuration}</td>
                        <td>
                            <button class="btn btn-primary btn-sm edit-route" data-id="${route.routeId}">Edit</button>
                        </td>
                    </tr>
                `).join("");
                populateRouteDropdown(data);
                attachRouteEventListeners();
            });
    }

    function fetchSchedules() {
        return Promise.all([
            fetch("/api/BusSchedule/GetSchedules").then(response => response.json()),
            fetch("/api/BusSchedule/GetRoutes").then(response => response.json()),
            fetch("/Business/BusCompany/GetBuses").then(response => response.json())
        ]).then(([schedules, routes, buses]) => {
            const routeMap = routes.reduce((map, route) => {
                map[route.routeId] = `${route.fromLocation} - ${route.toLocation}`;
                return map;
            }, {});

            const busMap = buses.reduce((map, bus) => {
                map[bus.busId] = `${bus.busNumber} - ${bus.companyName}`;
                return map;
            }, {});

            getSchedulesTable.innerHTML = schedules.map(schedule => `
                <tr>
                    <td>${routeMap[schedule.routeId] || "Unknown Route"}</td>
                    <td>${busMap[schedule.busId] || "Unknown Bus"}</td>
                    <td>${new Date(schedule.departureTime).toLocaleString()}</td>
                    <td>${new Date(schedule.arrivalTime).toLocaleString()}</td>
                    <td>${schedule.price}</td>
                    <td>${schedule.availableSeats}</td>
                    <td>${schedule.status === 0 ? "Scheduled" : schedule.status}</td>
                    <td>
                        <button class="btn btn-primary btn-sm edit-schedule" data-id="${schedule.scheduleId}">Edit</button>
                    </td>
                </tr>
            `).join("");
            attachScheduleEventListeners();
        });
    }

    function fetchBuses() {
        return fetch("/Business/BusCompany/GetBuses")
            .then(response => response.json())
            .then(data => {
                populateBusDropdown(data);
            });
    }

    function populateRouteDropdown(routes) {
        routeDropdown.innerHTML = `<option value="" disabled selected>Select a route</option>` + routes.map(route => `
            <option value="${route.routeId}">${route.fromLocation} - ${route.toLocation}</option>
        `).join("");
    }

    function populateBusDropdown(buses) {
        busDropdown.innerHTML = `<option value="" disabled selected>Select a bus</option>` + buses.map(bus => `
            <option value="${bus.busId}" data-capacity="${bus.capacity}">${bus.busNumber} (${bus.companyName})</option>
        `).join("");
    }

    busDropdown.addEventListener("change", function () {
        const selectedBus = this.options[this.selectedIndex];
        const capacity = selectedBus.getAttribute("data-capacity");
        if (capacity) {
            availableSeatsInput.value = capacity;
        }
    });

    function attachRouteEventListeners() {
        document.querySelectorAll(".edit-route").forEach(button => {
            button.addEventListener("click", function () {
                openEditRoute(this.dataset.id);
            });
        });
    }

    function attachScheduleEventListeners() {
        document.querySelectorAll(".edit-schedule").forEach(button => {
            button.addEventListener("click", function () {
                openEditSchedule(this.dataset.id);
            });
        });
    }

    document.getElementById("openAddRouteButton").addEventListener("click", function () {
        currentEditRouteId = null;
        document.getElementById("routeForm").reset();
        deleteRouteButton.style.display = "none";
        routeModal.show();
    });

    document.getElementById("openAddScheduleButton").addEventListener("click", function () {
        currentEditScheduleId = null;
        document.getElementById("scheduleForm").reset();
        deleteScheduleButton.style.display = "none";
        fetchRoutes().then(() => fetchBuses().then(() => scheduleModal.show()));
    });

    function openEditRoute(routeId) {
        fetch("/api/BusSchedule/GetRoutes")
            .then(response => response.json())
            .then(data => {
                const route = data.find(r => r.routeId == routeId);
                if (route) {
                    document.getElementById("FromLocation").value = route.fromLocation;
                    document.getElementById("ToLocation").value = route.toLocation;
                    const [hours, minutes] = route.estimatedDuration.split(":");
                    document.getElementById("estimatedHours").value = hours;
                    document.getElementById("estimatedMinutes").value = minutes;
                    currentEditRouteId = routeId;
                    deleteRouteButton.style.display = "block";
                    routeModal.show();
                }
            });
    }

    function openEditSchedule(scheduleId) {
        fetch("/api/BusSchedule/GetSchedules")
            .then(response => response.json())
            .then(data => {
                const schedule = data.find(s => s.scheduleId == scheduleId);
                if (schedule) {
                    document.getElementById("RouteId").value = schedule.routeId;
                    fetchBuses().then(() => {
                        document.getElementById("BusId").value = schedule.busId;
                    });
                    document.getElementById("DepartureTime").value = schedule.departureTime;
                    document.getElementById("Price").value = schedule.price;
                    document.getElementById("AvailableSeats").value = schedule.availableSeats;
                    currentEditScheduleId = scheduleId;
                    deleteScheduleButton.style.display = "block";
                    scheduleModal.show();
                }
            });
    }

    deleteRouteButton.addEventListener("click", function () {
        if (!currentEditRouteId) return;
        fetch(`/api/BusSchedule/DeleteRoute/${currentEditRouteId}`, { method: "DELETE" })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    routeModal.hide();
                    fetchRoutes();
                    fetchSchedules();
                }
            });
    });

    deleteScheduleButton.addEventListener("click", function () {

        fetch(`/api/BusSchedule/DeleteSchedule/${currentEditScheduleId}`, {
            method: "DELETE",
            headers: { "Content-Type": "application/json" }
        })
            .then(response => {
                if (!response.ok) {
                    return response.json().then(data => {
                        throw new Error(data.message || "Failed to delete the schedule.");
                    });
                }
                return response.json();
            })
            .then(data => {
                if (data.success) {
                    scheduleModal.hide();
                    fetchSchedules(); 
                    fetchRoutes();  
                } else {
                    alert(data.message || "Failed to delete the schedule.");
                }
            })
            .catch(error => {
                console.error("Error deleting schedule:", error);
            });
    });

    document.getElementById("routeForm").addEventListener("submit", function (e) {
        e.preventDefault();
        const formData = {
            FromLocation: document.getElementById("FromLocation").value.trim(),
            ToLocation: document.getElementById("ToLocation").value.trim(),
        };
        const hours = parseInt(document.getElementById("estimatedHours").value, 10);
        const minutes = parseInt(document.getElementById("estimatedMinutes").value, 10);
        const url = currentEditRouteId
            ? `/api/BusSchedule/EditRoute/${currentEditRouteId}?hours=${hours}&minutes=${minutes}`
            : `/api/BusSchedule/AddRoute?hours=${hours}&minutes=${minutes}`;

        fetch(url, {
            method: currentEditRouteId ? "PUT" : "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(formData),
        }).then(response => response.json())
            .then(data => {
                if (data.success) {
                    routeModal.hide();
                    fetchRoutes();
                }
            });
    });

    document.getElementById("scheduleForm").addEventListener("submit", function (e) {
        e.preventDefault();
        const formData = {
            RouteId: document.getElementById("RouteId").value,
            BusId: document.getElementById("BusId").value,
            DepartureTime: document.getElementById("DepartureTime").value,
            Price: parseFloat(document.getElementById("Price").value),
            AvailableSeats: parseInt(document.getElementById("AvailableSeats").value, 10),
        };
        const url = currentEditScheduleId
            ? `/api/BusSchedule/EditSchedule/${currentEditScheduleId}`
            : "/api/BusSchedule/AddSchedule";

        fetch(url, {
            method: currentEditScheduleId ? "PUT" : "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(formData),
        }).then(response => response.json())
            .then(data => {
                if (data.success) {
                    scheduleModal.hide();
                    fetchSchedules();
                }
            });
    });

    fetchRoutes();
    fetchSchedules();
});