document.getElementById('actionSelect').addEventListener('change', () => {
    const action = document.getElementById('actionSelect').value;
    document.getElementById('confirmReservationSection').style.display = action === 'Confirm Bus Reservation' ? 'block' : 'none';
    document.getElementById('viewPassengersSection').style.display = action === 'View Passengers by Route' ? 'block' : 'none';
});

function confirmSchedule(scheduleId) {
    console.log(`Schedule ${scheduleId} confirmed.`);
}

function cancelSchedule(scheduleId) {
    console.log(`Schedule ${scheduleId} cancelled.`);
}

function fetchPassengersByRoute() {
    const selectedRoute = document.getElementById('routeSelect').value;
    const passengerListContainer = document.getElementById('passengerListContainer');
    passengerListContainer.innerHTML = '';

    if (!selectedRoute) {
        passengerListContainer.innerHTML = '<tr><td colspan="5" class="text-center text-muted">Please select a route to view passengers.</td></tr>';
        return;
    }

    const passengersData = {
        'Route1': [
            { name: 'Passenger A', seats: 2, date: '2024-11-18', time: '08:00 AM' },
            { name: 'Passenger B', seats: 1, date: '2024-11-18', time: '09:00 AM' }
        ],
        'Route2': [
            { name: 'Passenger C', seats: 3, date: '2024-11-19', time: '10:00 AM' }
        ]
    };

    const selectedPassengers = passengersData[selectedRoute] || [];

    if (selectedPassengers.length === 0) {
        passengerListContainer.innerHTML = '<tr><td colspan="5" class="text-center text-muted">No passengers found for this route.</td></tr>';
        return;
    }

    selectedPassengers.forEach((passenger, index) => {
        const row = `
            <tr>
                <th scope="row">${index + 1}</th>
                <td>${passenger.name}</td>
                <td>${passenger.seats}</td>
                <td>${passenger.date}</td>
                <td>${passenger.time}</td>
            </tr>`;
        passengerListContainer.insertAdjacentHTML('beforeend', row);
    });
}