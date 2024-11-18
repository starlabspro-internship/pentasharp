document.getElementById("manageDropdown").addEventListener("change", (event) => {
    const value = event.target.value;
    const busRoutesSection = document.getElementById("busRoutesSection");
    const busScheduleSection = document.getElementById("busScheduleSection");

    if (value === "busRoutes") {
        busRoutesSection.classList.remove("d-none");
        busScheduleSection.classList.add("d-none");
    } else if (value === "busSchedule") {
        busRoutesSection.classList.add("d-none");
        busScheduleSection.classList.remove("d-none");
    }
});

document.getElementById("openAddRouteButton").addEventListener("click", () => {
    document.getElementById("FromLocation").value = "";
    document.getElementById("ToLocation").value = "";
    document.getElementById("estimatedHours").value = "";
    document.getElementById("estimatedMinutes").value = "";
    document.getElementById("deleteRouteButton").style.display = "none";
    document.getElementById("routeModalLabel").textContent = "Add Route";
    new bootstrap.Modal(document.getElementById("routeModal")).show();
});

document.getElementById("openAddScheduleButton").addEventListener("click", () => {
    document.getElementById("RouteId").value = "";
    document.getElementById("BusId").value = "";
    document.getElementById("DepartureTime").value = "";
    document.getElementById("Price").value = "";
    document.getElementById("AvailableSeats").value = "";
    document.getElementById("deleteScheduleButton").style.display = "none";
    document.getElementById("scheduleModalLabel").textContent = "Add Schedule";
    new bootstrap.Modal(document.getElementById("scheduleModal")).show();
});

document.getElementById("routeForm").addEventListener("submit", (event) => {
    event.preventDefault();
    const routeData = {
        fromLocation: document.getElementById("FromLocation").value,
        toLocation: document.getElementById("ToLocation").value,
        estimatedDuration: `${document.getElementById("estimatedHours").value} hours ${document.getElementById("estimatedMinutes").value} minutes`
    };
    console.log(routeData);
    new bootstrap.Modal(document.getElementById("routeModal")).hide();
});

document.getElementById("scheduleForm").addEventListener("submit", (event) => {
    event.preventDefault();
    const scheduleData = {
        routeId: document.getElementById("RouteId").value,
        busId: document.getElementById("BusId").value,
        departureTime: document.getElementById("DepartureTime").value,
        price: document.getElementById("Price").value,
        availableSeats: document.getElementById("AvailableSeats").value
    };
    console.log(scheduleData);
    new bootstrap.Modal(document.getElementById("scheduleModal")).hide();
});