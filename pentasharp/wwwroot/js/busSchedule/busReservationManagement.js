document.addEventListener('DOMContentLoaded', () => {
    fetchReservations();
});

const actionSelect = document.getElementById('actionSelect');
if (actionSelect) {
    actionSelect.addEventListener('change', () => {
        const action = actionSelect.value;
        document.getElementById('confirmReservationSection').style.display = action === 'Confirm Bus Reservation' ? 'block' : 'none';
        document.getElementById('viewPassengersSection').style.display = action === 'View Passengers by Route' ? 'block' : 'none';
    });
}

function fetchReservations() {
    const confirmSection = document.querySelector('#confirmReservationSection .list-group');

    fetch('/api/BusReservation/GetReservations')
        .then(response => {
            if (!response.ok) {
                if (response.status === 404) {
                    confirmSection.innerHTML = `
                        <div class="list-group-item text-center">
                            <span class="text-muted">No reservations found. The endpoint may be missing or unavailable.</span>
                        </div>`;
                } else {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
            }
            return response.json();
        })
        .then(data => {
      
            if (!data.success) {
                confirmSection.innerHTML = `
                    <div class="list-group-item text-center">
                        <span class="text-muted">${data.message || "No reservations found."}</span>
                    </div>`;
                return;
            }

            const reservations = data.data;

            if (!reservations || reservations.length <= 0) {
                confirmSection.innerHTML = `
                    <div class="list-group-item text-center">
                        <span class="text-muted">You don’t have any reservations at the moment.</span>
                    </div>`;
                return;
            }

            populateReservations(reservations);
        })
        .catch(error => {
            console.error('Error fetching reservations:', error);
            confirmSection.innerHTML = `
                <div class="list-group-item text-center">
                    <span class="text-muted">Failed to load reservations. Please try again later.</span>
                </div>`;
        });
}

function mapStatus(statusCode) {
    switch (statusCode) {
        case 0:
            return 'Pending';
        case 1:
            return 'Confirmed';
        case 2:
            return 'Canceled';
        default:
            return 'Unknown';
    }
}

function populateReservations(reservations) {
    const confirmSection = document.querySelector('#confirmReservationSection .list-group');

    confirmSection.innerHTML = '';

    reservations.reverse().forEach(reservation => {
        const badgeClass = reservation.status === 0 ? 'bg-warning' : reservation.status === 1 ? 'bg-success' : 'bg-danger';
        const passengerCount = Math.round(reservation.totalAmount / reservation.schedule.price);
        const reservationElement = `
    <div class="reservation-card shadow-sm mb-2 rounded p-2" style="background-color: #fdfdfd; border: 1px solid #eaeaea;">
        <div class="d-flex justify-content-between align-items-center">
            <div>
                <h6 class="fw-bold text-dark m-0">${reservation.schedule.fromLocation} → ${reservation.schedule.toLocation}</h6>
                <small class="text-muted">Passenger: <b>${reservation.user.firstName} ${reservation.user.lastName}</b></small>
            </div>
            <span class="badge ${badgeClass} text-white">${mapStatus(reservation.status)}</span>
        </div>
        <hr class="my-1" />
        <div class="reservation-details">
            <small class="text-muted">
                <b>Bus:</b> ${reservation.schedule.busNumber} | <b>Schedule:</b> ${reservation.schedule.scheduleId}
            </small><br />
            <small class="text-muted">
                <b>Departure:</b> ${new Date(reservation.schedule.departureTime).toLocaleString()} | 
                <b>Arrival:</b> ${new Date(reservation.schedule.arrivalTime).toLocaleString()}
            </small><br />
            <small class="text-muted">
                <b>Price:</b> ${reservation.schedule.price} | <b>Total:</b> ${reservation.totalAmount} | 
                <b>Seats:</b> ${passengerCount}
            </small>
        </div>
        <div class="mt-2 d-flex justify-content-end gap-2">
            <button class="btn btn-success btn-sm" onclick="confirmReservation(${reservation.reservationId})">
                <i class="fas fa-check"></i> Confirm
            </button>
            <button class="btn btn-danger btn-sm" onclick="cancelReservation(${reservation.reservationId})">
                <i class="fas fa-times"></i> Cancel
            </button>
        </div>
    </div>`;
        confirmSection.insertAdjacentHTML('beforeend', reservationElement);
    });
}

function confirmReservation(reservationId) {
    fetch('/api/BusReservation/ConfirmReservation', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ ReservationId: reservationId }),
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to confirm reservation');
            }
            return response.json();
        })
        .then(data => {
            if (data.success) {
                fetchReservations();
            } else {
                console.error('Error confirming reservation');
            }
        })
        .catch(error => {
            console.error('Error confirming reservation:', error);
        });
}

function cancelReservation(reservationId) {
    fetch('/api/BusReservation/CancelReservation', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ ReservationId: reservationId }),
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to cancel reservation');
            }
            return response.json();
        })
        .then(data => {
            if (data.success) {
                fetchReservations();
            } else {
                console.error('Error canceling reservation');
            }
        })
        .catch(error => {
            console.error('Error canceling reservation:', error);
        });
}