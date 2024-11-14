document.addEventListener("DOMContentLoaded", function () {
    loadRoutes();

    const routeModalElement = document.getElementById("routeModal");
    const routeModal = new bootstrap.Modal(routeModalElement);

    document.getElementById("openAddRouteButton")?.addEventListener("click", () => {
        openAddRouteModal(routeModal);
    });
});

async function loadRoutes() {
    try {
        const response = await fetch('/api/BusSchedule/GetRoutes');
        if (!response.ok) throw new Error('Failed to load routes');

        const routes = await response.json();
        const routesTableBody = document.getElementById('getRoutes');
        routesTableBody.innerHTML = '';

        routes.forEach(route => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td class="text-center">${route.routeId}</td>
                <td>${route.fromLocation}</td>
                <td>${route.toLocation}</td>
                <td class="text-center">${formatDuration(route.estimatedDuration)}</td>
                <td class="text-center">
                    <button class="btn btn-info btn-sm btn-custom edit-route-btn" data-id="${route.routeId}">Edit</button>
                    <button class="btn btn-danger btn-sm btn-custom delete-route-btn" data-id="${route.routeId}">Delete</button>
                </td>
            `;
            routesTableBody.appendChild(row);
        });

        document.querySelectorAll('.edit-route-btn').forEach(button => {
            button.addEventListener('click', () => openEditRouteModal(button.getAttribute('data-id')));
        });

        document.querySelectorAll('.delete-route-btn').forEach(button => {
            button.addEventListener('click', () => openDeleteRouteModal(button.getAttribute('data-id')));
        });
    } catch (error) {
        console.error(error);
        alert('Error loading routes');
    }
}

function formatDuration(duration) {
    const [hours, minutes] = duration.split(':');
    return `${parseInt(hours)} hrs ${parseInt(minutes)} mins`;
}

function openAddRouteModal(modalInstance) {
    const form = document.getElementById("routeForm");
    form.reset();
    form.action = "/api/BusSchedule/AddRoute";
    history.pushState(null, "", "/api/BusSchedule/AddRoute");
    modalInstance.show();

    document.getElementById("routeModal").addEventListener("hidden.bs.modal", function onModalClose() {
        history.pushState(null, "", "/api/BusSchedule/ManageBusSchedules");
        document.getElementById("routeModal").removeEventListener("hidden.bs.modal", onModalClose);
    });
}

window.openEditRouteModal = async function (routeId) {
    const modalInstance = new bootstrap.Modal(document.getElementById("routeModal"));
    const form = document.getElementById("routeForm");
    form.reset();
    form.action = `/api/BusSchedule/EditRoute/${routeId}`;

    try {
        const response = await fetch(`/api/BusSchedule/GetRouteById/${routeId}`);
        if (response.ok) {
            const route = await response.json();
            document.getElementById("FromLocation").value = route.fromLocation;
            document.getElementById("ToLocation").value = route.toLocation;
            const [hours, minutes] = route.estimatedDuration.split(":");
            document.getElementById("estimatedHours").value = parseInt(hours);
            document.getElementById("estimatedMinutes").value = parseInt(minutes);
        } else {
            alert("Failed to fetch route details.");
            return;
        }
    } catch (error) {
        console.error("Error fetching route data:", error);
        return;
    }

    history.pushState(null, "", `/api/BusSchedule/EditRoute/${routeId}`);
    modalInstance.show();

    document.getElementById("routeModal").addEventListener("hidden.bs.modal", function onModalClose() {
        history.pushState(null, "", "/api/BusSchedule/ManageBusSchedules");
        document.getElementById("routeModal").removeEventListener("hidden.bs.modal", onModalClose);
    });
}

function openDeleteRouteModal(routeId) {
    const deleteModalElement = document.getElementById("deleteRouteModal");
    const deleteModal = new bootstrap.Modal(deleteModalElement);

    document.getElementById("confirmDeleteRouteButton").onclick = () => confirmDeleteRoute(routeId, deleteModal);

    history.pushState(null, "", `/api/BusSchedule/DeleteRoute/${routeId}`);
    deleteModal.show();

    deleteModalElement.addEventListener("hidden.bs.modal", function onModalClose() {
        history.pushState(null, "", "/api/BusSchedule/ManageBusSchedules");
        deleteModalElement.removeEventListener("hidden.bs.modal", onModalClose);
    });
}

async function confirmDeleteRoute(routeId, modalInstance) {
    try {
        const response = await fetch(`/api/BusSchedule/DeleteRoute/${routeId}`, { method: 'DELETE' });
        if (response.ok) {
            modalInstance.hide();
            history.pushState(null, "", "/api/BusSchedule/ManageBusSchedules");
            loadRoutes();
        } else {
            const result = await response.json();
            alert(result.Message || "Failed to delete route");
        }
    } catch (error) {
        console.error("Error deleting route:", error);
        alert("An error occurred while deleting the route");
    }
}

function saveRoute() {
    const hours = parseInt(document.getElementById('estimatedHours').value) || 0;
    const minutes = parseInt(document.getElementById('estimatedMinutes').value) || 0;

    if (isNaN(hours) || isNaN(minutes)) {
        alert("Please enter valid hours and minutes.");
        return false;
    }

    document.getElementById("EstimatedDuration").value = `${hours}:${minutes.toString().padStart(2, "0")}:00`;
    return true;
}