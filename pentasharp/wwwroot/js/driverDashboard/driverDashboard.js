let meterInterval;
let elapsedTime = 0;

function loadBookings() {
    fetch('/api/TaxiDriver/GetBookings')
        .then(response => response.json())
        .then(bookings => {
            const bookingsContainer = document.getElementById("availableBookings");
            bookingsContainer.innerHTML = "";
            bookings.forEach(booking => {
                const bookingItem = document.createElement("div");
                bookingItem.className = "booking-item mb-3";
                bookingItem.innerHTML = `
                    <p><strong>Passenger:</strong> ${booking.passengerName}</p>
                    <p><strong>Pickup:</strong> ${booking.pickupLocation}</p>
                    <p><strong>Dropoff:</strong> ${booking.dropoffLocation}</p>
                    <p><strong>Scheduled Time:</strong> ${booking.bookingTime}</p>
                    <button class="btn btn-success w-100" onclick="claimBooking(${booking.bookingId})">Claim Booking</button>
                `;
                bookingsContainer.appendChild(bookingItem);
            });
        });
}

function claimBooking(bookingId) {
    document.getElementById("currentStatus").textContent = "Claimed";
    document.getElementById("currentStatus").classList.replace("bg-warning", "bg-info");
    document.getElementById("startTripButton").style.display = "block";
    document.getElementById("fareDisplay").style.display = "none";

    fetch(`/api/TaxiDriverStartTrip`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ bookingId })
    })
        .then(response => response.json())
        .then(data => {
            if (!data.success) throw new Error(data.message);
        })
        .catch(error => alert(`Error claiming booking: ${error.message}`));
}

function startTrip() {
    elapsedTime = 0;
    document.getElementById("currentStatus").textContent = "In Progress";
    document.getElementById("currentStatus").classList.replace("bg-info", "bg-primary");
    document.getElementById("startTripButton").style.display = "none";
    document.getElementById("endTripButton").style.display = "block";
    document.getElementById("taxiMeter").style.display = "block";

    meterInterval = setInterval(updateMeter, 1000);
}

function endTrip(bookingId) {
    clearInterval(meterInterval);
    const fare = calculateFare();
    document.getElementById("fareAmount").textContent = fare.toFixed(2);
    document.getElementById("currentStatus").textContent = "Completed";
    document.getElementById("currentStatus").classList.replace("bg-primary", "bg-success");
    document.getElementById("endTripButton").style.display = "none";
    document.getElementById("taxiMeter").style.display = "none";
    document.getElementById("fareDisplay").style.display = "block";

    fetch(`/api/TaxiDriverEndTrip`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ bookingId, fare })
    })
        .then(response => response.json())
        .then(data => {
            if (!data.success) throw new Error(data.message);
            loadBookings();
        })
        .catch(error => alert(`Error ending trip: ${error.message}`));
}

function updateMeter() {
    elapsedTime++;
    const hours = String(Math.floor(elapsedTime / 3600)).padStart(2, "0");
    const minutes = String(Math.floor((elapsedTime % 3600) / 60)).padStart(2, "0");
    const seconds = String(elapsedTime % 60).padStart(2, "0");
    document.getElementById("meterTime").textContent = `${hours}:${minutes}:${seconds}`;
}

function calculateFare() {
    return (elapsedTime / 60) * 0.5;
}

document.addEventListener("DOMContentLoaded", loadBookings);