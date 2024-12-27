let meterInterval;
let elapsedTime = 0;
let isBooking = false;

function claimBooking(passenger, pickup, dropoff, status, time, bookingId) {
    updateCurrentTrip(passenger, pickup, dropoff, status, time, 0, bookingId); 
    isBooking = true;
}

function loadaaa() {
    fetch('/Driver/Bookings')
        .then(response => response.json())
        .then(data => {
            const bookings = data;
            const bookingList = document.getElementById("availableBookings");
            bookingList.innerHTML = "";

            bookings.forEach(booking => {
                console.log("Booking object:", booking);
                const status = booking.status === "Pending" ? "Pending" : "Confirmed";
                const list = document.createElement("div");
                list.innerHTML = `
                    <div class="booking-item mb-3">
                        <p><strong>Passenger:</strong> ${booking.passengerName}</p>
                        <p><strong>Pickup:</strong> ${booking.pickupLocation}</p>
                        <p><strong>Dropoff:</strong> ${booking.dropoffLocation}</p>
                        <p><strong>Scheduled Time:</strong>${booking.bookingTime}</p>
                        <button class="btn btn-success w-100" onclick="claimBooking('${booking.passengerName}', '${booking.pickupLocation}', '${booking.dropoffLocation}', '${status}', '${booking.bookingTime}', ${booking.bookingId})">
                            Claim Booking
                        </button>
                    </div>
                `;
                bookingList.appendChild(list);
            });
        })
        .catch(() => {
            alert("Error loading bookings.");
        });
}

function updateCurrentTrip(passenger, pickup, dropoff, status, time, fare, bookingId) {
    fetch(`/Driver/ClaimBooking/${bookingId}`, {
        method: 'POST'
    })
        .then(response => {
            if (response.ok) {
                response.json().then(data => {
                    console.log(data); 
                    document.getElementById("currentPassenger").textContent = passenger;
                    document.getElementById("currentPickup").textContent = pickup;
                    document.getElementById("currentDropoff").textContent = dropoff;
                    document.getElementById("currentStatus").textContent = `Confirmed`;
                    document.getElementById("currentStatus").classList.replace("bg-warning", "bg-info");
                    document.getElementById("assignedFare").textContent = fare.toFixed(2);
                    document.getElementById("startTripButton").style.display = "block";
                    document.getElementById("fareDisplay").style.display = "none";
                    document.getElementById("startTripButton").setAttribute("data-booking-id", bookingId);
                    console.log(data.message || "Booking claimed successfully!");
                });
            } else {
                response.json().then(data => {
                    console.error("Error response:", data);
                    alert(data.message || "Unable to claim booking.");
                });
            }
        })
        .catch(() => {
            console.error("Fetch error:", err); 
            alert("An error occurred while claiming the booking.");
        });
}

function startTrip() {
    const bookingId = document.getElementById("startTripButton").getAttribute("data-booking-id");

    fetch(`/Driver/StartTrip/${bookingId}`, {
        method: 'POST'
    })
        .then(response => {
            if (response.ok) {
                response.json().then(data => {
                    console.log(data);

                    document.getElementById("currentStatus").textContent = "In Progress";
                    document.getElementById("currentStatus").classList.replace("bg-info", "bg-primary");
                    document.getElementById("startTripButton").style.display = "none";
                    document.getElementById("endTripButton").style.display = "block";

                    document.getElementById("taxiMeter").style.display = "block";
                    elapsedTime = 0;
                    meterInterval = setInterval(updateMeter, 1000);

                    console.log(data.message || "Trip started successfully!");
                });
            } else {
                response.json().then(data => {
                    console.error("Error response:", data);
                    alert(data.message || "Unable to start the trip.");
                });
            }
        })
        .catch(err => {
            console.error("Fetch error:", err);
            alert("An error occurred while starting the trip.");
        });

    loadaaa();
}

function endTrip() {
    if (isBooking) {
        clearInterval(meterInterval);
        const fare = calculateFare();
        document.getElementById("fareAmount").textContent = fare.toFixed(2);

        const bookingId = document.getElementById("startTripButton").getAttribute("data-booking-id");

        fetch(`/Driver/EndTrip/${bookingId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(fare),
        })
            .then(response => {
                if (response.ok) {
                    response.json().then(data => {
                        console.log(data);

                        document.getElementById("currentStatus").textContent = "Completed";
                        document.getElementById("currentStatus").classList.replace("bg-primary", "bg-success");
                        document.getElementById("endTripButton").style.display = "none";
                        document.getElementById("taxiMeter").style.display = "none";
                        document.getElementById("fareDisplay").style.display = "block";

                        console.log(data.message || "Trip ended successfully!");
                    });
                } else {
                    response.json().then(data => {
                        console.error("Error response:", data);
                        alert(data.message || "Unable to end the trip.");
                    });
                }
            })
            .catch(err => {
                console.error("Fetch error:", err);
                alert("An error occurred while ending the trip.");
            });
    } else {
        document.getElementById("fareAmount").textContent = document.getElementById("assignedFare").textContent;
        document.getElementById("fareDisplay").style.display = "block";
        document.getElementById("currentStatus").textContent = "Completed";
        document.getElementById("currentStatus").classList.replace("bg-primary", "bg-success");
        document.getElementById("endTripButton").style.display = "none";
        document.getElementById("taxiMeter").style.display = "none";
    }
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

document.addEventListener("DOMContentLoaded", loadaaa);

function loadReservations() {
    fetch('/Driver/Reservations')
        .then(response => response.json())
        .then(data => {
            const reservations = data.data; 
            const reservationsList = document.getElementById("availableReservations");
            reservationsList.innerHTML = "";

            reservations.forEach(reservation => {
                const listItem = document.createElement("div");
                listItem.classList.add("reservation-item", "mb-3");
                listItem.innerHTML = `
                <div id="reservation-${reservation.reservationId}" class="reservation-item mb-3">
                    <p><strong>Passenger:</strong> ${reservation.passengerName}</p>
                    <p><strong>Pickup:</strong> ${reservation.pickupLocation}</p>
                    <p><strong>Dropoff:</strong> ${reservation.dropoffLocation}</p>
                    <p><strong>Reservation Time:</strong> ${reservation.reservationDate} at ${reservation.reservationTime}</p>
                    <p><strong>Base Fare:</strong> ${reservation.fare ? `$${reservation.fare.toFixed(2)}` : 'N/A'}</p>
                    <button class="btn btn-primary w-100" onclick="acceptReservation('${reservation.passengerName}', ${reservation.reservationId}, '${reservation.pickupLocation}', '${reservation.dropoffLocation}', ${reservation.fare})">
                        Accept Reservation
                    </button>
                </div>
                `;
                reservationsList.appendChild(listItem);
            });
        })
        .catch(err => {
            console.error("Error loading reservations:", err);
            alert("An error occurred while loading reservations.");
        });
}

function acceptReservation(passengerName, reservationId, pickupLocation, dropoffLocation, fare) {
    fetch(`/Driver/AcceptReservation/${reservationId}`, {
        method: 'POST'
    })
        .then(response => {
            if (response.ok) {
                response.json().then(data => {
                    console.log(data); 
                    console.log(data.message || "Reservation accepted successfully!");

                    updateCurrentReservation(passengerName, reservationId, pickupLocation, dropoffLocation, fare);

                    loadReservations();
                });
            } else {
                response.json().then(data => {
                    console.error("Error response:", data);
                    alert(data.message || "Unable to accept the reservation.");
                });
            }
        })
        .catch(err => {
            console.error("Fetch error:", err);
            alert("An error occurred while accepting the reservation.");
        });
}

function updateCurrentReservation(passengerName, reservationId, pickupLocation, dropoffLocation, fare) {

    document.getElementById("currentPassenger").textContent = passengerName;
    document.getElementById("currentPickup").textContent = pickupLocation;
    document.getElementById("currentDropoff").textContent = dropoffLocation;
    document.getElementById("currentStatus").textContent = "Confirmed";
    document.getElementById("currentStatus").classList.replace("bg-warning", "bg-info");
    document.getElementById("assignedFare").textContent = fare.toFixed(2);
    document.getElementById("startReservationTripButton").style.display = "block";
    document.getElementById("fareDisplay").style.display = "none";
    document.getElementById("startReservationTripButton").setAttribute("data-reservation-id", reservationId);
}

function startReservationTrip() {
    const reservationId = document.getElementById("startReservationTripButton").getAttribute("data-reservation-id");

    fetch(`/Driver/StartTripReservation/${reservationId}`, {
        method: 'POST'
    })
        .then(response => {
            if (response.ok) {
                response.json().then(data => {
                    console.log(data);

                    document.getElementById("currentStatus").textContent = "In Progress";
                    document.getElementById("currentStatus").classList.replace("bg-info", "bg-primary");
                    document.getElementById("startReservationTripButton").style.display = "none";
                    document.getElementById("endReservationTripButton").style.display = "block";

                    document.getElementById("taxiMeter").style.display = "block";
                    elapsedTime = 0;
                    meterInterval = setInterval(updateMeter, 1000);

                    console.log(data.message || "Trip started successfully!");
                    loadReservations();
                });
            } else {
                response.json().then(data => {
                    console.error("Error response:", data);
                    alert(data.message || "Unable to start the trip.");
                });
            }
        })
        .catch(err => {
            console.error("Fetch error:", err);
            alert("An error occurred while starting the trip.");
        });
}

function endReservationTrip() {
    const reservationId = document.getElementById("startReservationTripButton").getAttribute("data-reservation-id");

    fetch(`/Driver/EndReservationTrip/${reservationId}`, {
        method: 'POST'
    })
        .then(response => {
            if (response.ok) {
                response.json().then(data => {
                    console.log(data);

                    document.getElementById("currentStatus").textContent = "Completed";
                    document.getElementById("currentStatus").classList.replace("bg-primary", "bg-success");
                    document.getElementById("endReservationTripButton").style.display = "none";
                    document.getElementById("taxiMeter").style.display = "none";

                    console.log(data.message || "Reservation trip ended successfully!");
                });
            } else {
                response.json().then(data => {
                    console.error("Error response:", data);
                    alert(data.message || "Unable to end the reservation trip.");
                });
            }
        })
        .catch(err => {
            console.error("Fetch error:", err);
            alert("An error occurred while ending the reservation trip.");
        });
}

document.addEventListener("DOMContentLoaded", loadReservations);