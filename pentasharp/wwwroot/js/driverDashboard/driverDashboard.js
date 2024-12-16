let meterInterval;
let elapsedTime = 0;
let isBooking = false;

function claimBooking(passenger, pickup, dropoff, time) {
    updateCurrentTrip(passenger, pickup, dropoff, "Booking", time, 0);
    isBooking = true;
}

function acceptReservation(passenger, pickup, dropoff, time, fare) {
    updateCurrentTrip(passenger, pickup, dropoff, "Reservation", time, fare);
    document.getElementById("assignedFare").textContent = fare.toFixed(2);
    isBooking = false;
}

function updateCurrentTrip(passenger, pickup, dropoff, type, time, fare) {
    document.getElementById("currentPassenger").textContent = passenger;
    document.getElementById("currentPickup").textContent = pickup;
    document.getElementById("currentDropoff").textContent = dropoff;
    document.getElementById("currentStatus").textContent = `${type} Claimed`;
    document.getElementById("currentStatus").classList.replace("bg-warning", "bg-info");
    document.getElementById("assignedFare").textContent = fare.toFixed(2);
    document.getElementById("startTripButton").style.display = "block";
    document.getElementById("fareDisplay").style.display = "none";
}

function startTrip() {
    document.getElementById("currentStatus").textContent = "In Progress";
    document.getElementById("currentStatus").classList.replace("bg-info", "bg-primary");
    document.getElementById("startTripButton").style.display = "none";
    document.getElementById("endTripButton").style.display = "block";

    if (isBooking) {
        document.getElementById("taxiMeter").style.display = "block";
        elapsedTime = 0;
        meterInterval = setInterval(updateMeter, 1000);
    }
}

function endTrip() {
    if (isBooking) {
        clearInterval(meterInterval);
        const fare = calculateFare();
        document.getElementById("fareAmount").textContent = fare.toFixed(2);
    } else {
        document.getElementById("fareAmount").textContent = document.getElementById("assignedFare").textContent;
    }
    document.getElementById("fareDisplay").style.display = "block";
    document.getElementById("currentStatus").textContent = "Completed";
    document.getElementById("currentStatus").classList.replace("bg-primary", "bg-success");
    document.getElementById("endTripButton").style.display = "none";
    document.getElementById("taxiMeter").style.display = "none";
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