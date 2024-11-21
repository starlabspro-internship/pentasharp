// main.js

// Function to handle the search form submission
function handleSearch(event) {
    event.preventDefault(); // Prevent form submission

    const pickupLocation = document.getElementById('pickupLocation').value;
    const dropoffLocation = document.getElementById('dropoffLocation').value;
    const reservationTime = document.getElementById('reservationTime').value;
    const reservationDate = document.getElementById('reservationDate').value;
    const passengerCount = document.getElementById('passengerCount').value;

    // Make a POST request to the server to search for available taxis
    fetch('http://localhost:5053/IncomingTaxiReservation/Search', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            PickupLocation: pickupLocation,
            DropoffLocation: dropoffLocation,
            ReservationTime: reservationTime,
            ReservationDate: reservationDate,
            PassengerCount: passengerCount,
        }),
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to fetch taxi reservations');
            }
            return response.json();
        })
        .then(data => {
            const resultsContainer = document.getElementById('resultsContainer');
            resultsContainer.innerHTML = ''; // Clear any previous results

            // Loop through the data and create result cards
            data.forEach(taxiCompany => {
                const resultCard = `
                <div class="container my-3" style="max-width: 600px;">
                    <div class="card shadow-sm border-0 p-4 rounded-4">
                        <div class="row align-items-center">
                            <div class="col-4 d-flex align-items-center justify-content-center">
                                <i class="fas fa-taxi fa-3x text-warning"></i>
                            </div>
                            <div class="col-5">
                                <h5 class="fw-bold">Taxi: ${taxiCompany.TaxiCompany}</h5>
                                <h5 class="fw-bold">Contact: ${taxiCompany.Contact}</h5>
                                <p class="text-muted">Reliable and comfortable</p>
                            </div>
                            <div class="col-3 text-end">
                                <button class="btn btn-primary rounded-pill px-4 py-2 shadow" 
                                    onclick="openReservationModal('${pickupLocation}', '${dropoffLocation}', '${reservationDate}', 
                                    '${reservationTime}', '${passengerCount}', '${taxiCompany.TaxiCompany}', 
                                    '${taxiCompany.Contact}', '${taxiCompany.id}')">Reserve Now</button>
                            </div>
                        </div>
                    </div>
                </div>`;
                resultsContainer.insertAdjacentHTML('beforeend', resultCard);
            });
        })
        .catch(error => {
            console.error('Error:', error);
            alert('An error occurred while searching for taxis. Please try again.');
        });
}

// Function to open the reservation modal
function openReservationModal(pickupLocation, dropoffLocation, reservationDate, reservationTime, passengerCount, companyName, contactInfo, taxiId) {
    document.getElementById('modalPickupLocation').textContent = pickupLocation;
    document.getElementById('modalDropoffLocation').textContent = dropoffLocation;
    document.getElementById('modalReservationDate').textContent = reservationDate;
    document.getElementById('modalReservationTime').textContent = reservationTime;
    document.getElementById('modalPassengerCount').textContent = passengerCount;
    document.getElementById('modalCompanyname').textContent = companyName;
    document.getElementById('modalContactInfo').textContent = contactInfo;

    // Store the selected taxiId for reservation confirmation
    window.selectedTaxiId = taxiId;

    document.getElementById('confirmationMessage').style.display = 'none';
    document.getElementById('confirmButton').style.display = 'inline-block';

    const reservationModal = new bootstrap.Modal(document.getElementById('reservationModal'));
    reservationModal.show();
}

// Function to confirm the reservation
function confirmReservation() {
    document.getElementById('confirmationMessage').style.display = 'block';
    document.getElementById('confirmButton').style.display = 'none';

    const pickupLocation = document.getElementById('modalPickupLocation').textContent;
    const dropoffLocation = document.getElementById('modalDropoffLocation').textContent;
    const reservationDate = document.getElementById('modalReservationDate').textContent;
    const reservationTime = document.getElementById('modalReservationTime').textContent; // Fixed typo
    const passengerCount = document.getElementById('modalPassengerCount').textContent;
    const companyName = document.getElementById('modalCompanyname').textContent;
    const contactInfo = document.getElementById('modalContactInfo').textContent;

    const tripStartTime = new Date(`${reservationDate}T${reservationTime}Z`);

    const reservationData = {
        PickupLocation: pickupLocation,
        DropoffLocation: dropoffLocation,
        ReservationDate: reservationDate,
        ReservationTime: reservationTime,
        PassengerCount: passengerCount,
        CompanyName: companyName,
        ContactInfo: contactInfo,
        TaxiId: window.selectedTaxiId,
        TripStartTime: tripStartTime,
    };

    fetch('http://localhost:5053/IncomingTaxiReservation/Confirm', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(reservationData),
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to confirm reservation');
            }
            return response.json();
        })
        .then(data => {
            alert('Your reservation has been confirmed!');
        })
        .catch(error => {
            console.error('Error:', error);
            alert('An error occurred while confirming your reservation.');
        });
}

// Add event listener to the search form
document.getElementById('searchForm').addEventListener('submit', handleSearch);