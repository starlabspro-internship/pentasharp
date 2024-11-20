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
        fetch("/api/BusSchedule/GetRoutes")
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
        Promise.all([
            fetch("/api/BusSchedule/GetSchedules").then(response => response.json()),
            fetch("/api/BusSchedule/GetRoutes").then(response => response.json()),
            fetch("/api/BusCompany/GetBuses").then(response => response.json())
        ])
            .then(([schedules, routes, buses]) => {
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
            })
            .catch(error => {
                console.error("Error fetching schedules, routes, or buses:", error.message);
            });
    }

    function fetchBuses() {
        return fetch("/api/BusCompany/GetBuses")
            .then(response => {
                if (!response.ok) {
                    throw new Error("Failed to fetch buses");
                }
                return response.json();
            })
            .then(data => {
                if (data.length > 0) {
                    populateBusDropdown(data);
                } else {
                    busDropdown.innerHTML = `<option disabled>No buses available</option>`;
                }
            })
            .catch(error => {
                console.error("Error fetching buses:", error.message);
                busDropdown.innerHTML = `<option disabled>Failed to load buses</option>`;
            });
    }

    function populateRouteDropdown(routes) {
        routeDropdown.innerHTML = routes.map(route => `
            <option value="${route.routeId}">${route.fromLocation} - ${route.toLocation}</option>
        `).join("");
    }

    function populateBusDropdown(buses) {
        busDropdown.innerHTML = buses.map(bus => `
            <option value="${bus.busId}">${bus.busNumber} (${bus.companyName})</option>
        `).join("");
    }

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
        fetchRoutes();
        fetchBuses().then(() => scheduleModal.show());
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
                } else {
                    alert(data.message);
                }
            });
    });

    deleteScheduleButton.addEventListener("click", function () {
        if (!currentEditScheduleId) return;
        fetch(`/api/BusSchedule/DeleteSchedule/${currentEditScheduleId}`, { method: "DELETE" })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    scheduleModal.hide();
                    fetchSchedules();
                } else {
                    alert(data.message);
                }
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
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    routeModal.hide();
                    fetchRoutes();
                    fetchSchedules();
                } else {
                    alert(data.message);
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
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(text => {
                        throw new Error(text);
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
                    alert(data.message);
                }
            })
            .catch(error => {
                console.error("Error:", error.message);
                alert("An error occurred: " + error.message);
            });
    });

    fetchRoutes();
    fetchSchedules();
});