document.addEventListener("DOMContentLoaded", function () {
    loadRoutes();

    document.getElementById("openAddScheduleButton").addEventListener("click", openAddScheduleModal);
});

async function loadRoutes() {
    try {
        const response = await fetch('/api/BusSchedule/GetAllRoutes');
        const routes = await response.json();

        const routeSelect = document.getElementById("routeId");
        routeSelect.innerHTML = '<option value="">Select Route</option>';
        routes.forEach(route => {
            const option = document.createElement("option");
            option.value = route.RouteId;
            option.text = route.RouteName;
            routeSelect.appendChild(option);
        });
    } catch (error) {
        console.error("Error loading routes:", error);
    }
}

function openAddScheduleModal() {

    document.getElementById('scheduleForm').reset();
    document.getElementById('arrivalTime').value = "";
    document.getElementById('routeIdDisplay').textContent = "";

    const scheduleModal = new bootstrap.Modal(document.getElementById('scheduleModal'));
    scheduleModal.show();
}

function calculateArrivalTime() {
    const departureTime = document.getElementById('departureTime').value;
    const routeId = document.getElementById('routeId').value;

    if (departureTime && routeId) {
        fetch(`/api/BusSchedule/GetRouteBy/${routeId}`)
            .then(response => response.json())
            .then(routeData => {
                const [depHours, depMinutes] = departureTime.split(":").map(Number);
                const [estHours, estMinutes] = routeData.EstimatedDuration.split(":").map(Number);

                const arrivalDate = new Date();
                arrivalDate.setHours(depHours + estHours);
                arrivalDate.setMinutes(depMinutes + estMinutes);

                const formattedArrivalTime = arrivalDate.toTimeString().split(" ")[0].substring(0, 5);
                document.getElementById('arrivalTime').value = formattedArrivalTime;
            })
            .catch(error => console.error("Error fetching route data:", error));
    }
}

async function saveSchedule() {
    const scheduleData = {
        RouteId: document.getElementById('routeId').value,
        DepartureTime: document.getElementById('departureTime').value,
        ArrivalTime: document.getElementById('arrivalTime').value,
        Price: parseFloat(document.getElementById('price').value),
        AvailableSeats: parseInt(document.getElementById('availableSeats').value),
        BusId: document.getElementById('busId').value
    };

    try {
        const response = await fetch('/api/BusSchedule/AddSchedule', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(scheduleData)
        });
        const result = await response.json();
        if (response.ok) {
            alert(result.Message);
            document.getElementById('scheduleForm').reset();
            document.getElementById('routeIdDisplay').textContent = "";
            $('#scheduleModal').modal('hide');
        } else {
            alert(result.Message || "Failed to add schedule");
        }
    } catch (error) {
        console.error("Error saving schedule:", error);
    }
}