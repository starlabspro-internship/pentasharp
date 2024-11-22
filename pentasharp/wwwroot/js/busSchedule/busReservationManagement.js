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
    fetch('/api/BusSchedule/GetReservations')
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to fetch reservations');
            }
            return response.json();
        })
        .then(data => populateReservations(data))
        .catch(error => {
            console.error('Error fetching reservations:', error);
            const confirmSection = document.querySelector('#confirmReservationSection .list-group');
            if (confirmSection) {
                confirmSection.innerHTML = `
                    <div class="list-group-item text-center">
                        <span class="text-muted">Failed to load reservations. Please try again later.</span>
                    </div>`;
            }
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
    if (!confirmSection) {
        console.error('Confirm reservation section not found');
        return;
    }

    confirmSection.innerHTML = '';

    if (!reservations || reservations.length === 0) {
        confirmSection.innerHTML = `
            <div class="list-group-item text-center">
                <span class="text-muted">No reservations found.</span>
            </div>`;
        return;
    }

    reservations.forEach(reservation => {
        const badgeClass = reservation.status === 0 ? 'bg-warning' : reservation.status === 1 ? 'bg-success' : 'bg-danger';
        const reservationElement = `
            <div class="list-group-item d-flex align-items-center justify-content-between shadow-sm p-3 mb-2 rounded" style="background-color: #fdfdfe;">
                <div class="schedule-details">
                    <h6 class="mb-1 fw-bold text-dark">Route: ${reservation.schedule.fromLocation} - ${reservation.schedule.toLocation}</h6>
                    <small class="text-muted">Bus Number: <b>${reservation.schedule.busNumber}</b> | Schedule ID: <b>${reservation.schedule.scheduleId}</b></small><br />
                    <small class="text-muted">Departure: <b>${new Date(reservation.schedule.departureTime).toLocaleString()}</b> | Arrival: <b>${new Date(reservation.schedule.arrivalTime).toLocaleString()}</b></small><br />
                    <small class="text-muted">Price: <b>${reservation.schedule.price}</b> | Status: <span class="badge ${badgeClass} text-white">${mapStatus(reservation.status)}</span></small>
                </div>
                <div class="action-buttons d-flex flex-column align-items-center">
                    <button class="btn btn-success btn-sm mb-2 d-flex align-items-center justify-content-center" onclick="confirmReservation(${reservation.reservationId})">
                        <i class="fas fa-check me-2"></i> Confirm
                    </button>
                    <button class="btn btn-danger btn-sm d-flex align-items-center justify-content-center" onclick="cancelReservation(${reservation.reservationId})">
                        <i class="fas fa-times me-2"></i> Cancel
                    </button>
                </div>
            </div>`;
        confirmSection.insertAdjacentHTML('beforeend', reservationElement);
    });
}

function confirmReservation(reservationId) {
    fetch('/api/BusSchedule/ConfirmReservation', {
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
                console.log("err")
            }
        })
        .catch(error => {
            console.error('Error confirming reservation:', error);
        });
}

function cancelReservation(reservationId) {
    fetch('/api/BusSchedule/CancelReservation', {
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
                console.log("err")
            }
        })
        .catch(error => {
            console.error('Error canceling reservation:', error);
        });
}