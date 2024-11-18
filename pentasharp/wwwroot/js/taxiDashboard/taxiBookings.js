const dummyPassengers = [
    { name: "John Doe", pickup: "Central Station", dropoff: "Downtown Mall", time: "08:30 AM", status: "Pending", driver: "Driver A" },
    { name: "Jane Smith", pickup: "City Square", dropoff: "Airport", time: "10:00 AM", status: "In Progress", driver: "Driver B" }
];

function loadPassengers(passengers) {
    const passengerList = document.getElementById("passengerList");
    passengerList.innerHTML = "";
    passengers.forEach((passenger, index) => {
        const row = document.createElement("tr");
        row.innerHTML = `
            <td>${passenger.name}</td>
            <td>${passenger.pickup}</td>
            <td>${passenger.dropoff}</td>
            <td>${passenger.time}</td>
            <td>
                <span class="badge bg-${passenger.status === "Pending"
                ? "warning"
                : passenger.status === "In Progress"
                    ? "primary"
                    : passenger.status === "Completed"
                        ? "success"
                        : "danger"
            }">${passenger.status}</span>
            </td>
            <td>${passenger.driver}</td>
            <td>
                <button class="btn btn-sm btn-outline-success" onclick="confirmBooking(${index})">Confirm</button>
                <button class="btn btn-sm btn-outline-danger" onclick="cancelBooking(${index})">Cancel</button>
            </td>
        `;
        passengerList.appendChild(row);
    });
}

function confirmBooking(index) {
    if (dummyPassengers[index].status === "Pending") {
        dummyPassengers[index].status = "In Progress";
        loadPassengers(dummyPassengers);
    }
}

function cancelBooking(index) {
    dummyPassengers.splice(index, 1);
    loadPassengers(dummyPassengers);
}

function saveBookingChanges() {
    console.log("Changes saved.");
}

document.addEventListener("DOMContentLoaded", () => {
    loadPassengers(dummyPassengers);
});