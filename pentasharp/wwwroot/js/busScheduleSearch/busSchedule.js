document.addEventListener("DOMContentLoaded", function () {
    const searchBtn = document.getElementById('searchBtn');
    const busScheduleContainer = document.getElementById('busScheduleContainer');
    const resultView = document.getElementById('resultView');
    const numberOfSeatsInput = document.getElementById('numberOfSeats');
    const totalAmountInput = document.getElementById('totalAmount');
    const reservationDateInput = document.getElementById('reservationDate');
    const passengerNameInput = document.getElementById('passengerNameInput');
    const confirmBookingBtn = document.getElementById('confirmBookingBtn');
    let currentPrice = 0;
    let selectedScheduleId = null;
    let userId = null;

    searchBtn.addEventListener('click', function () {
        const from = document.getElementById('departure').value.trim();
        const to = document.getElementById('arrival').value.trim();
        const date = document.getElementById('date').value;

        if (!from || !to || !date) {
            busScheduleContainer.innerHTML = '<p class="text-center text-danger">Please fill in all fields to search schedules.</p>';
            resultView.style.display = 'block';
            resultView.style.opacity = 1;
            return;
        }

        fetch(`/api/BusSchedule/SearchSchedules?from=${from}&to=${to}&date=${date}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to fetch schedules.');
                }
                return response.json();
            })
            .then(schedules => {
                busScheduleContainer.innerHTML = '';

                if (!schedules || schedules.length === 0) {
                    busScheduleContainer.innerHTML = '<p class="text-center">No schedules found for the selected criteria.</p>';
                    resultView.style.display = 'block';
                    resultView.style.opacity = 1;
                    return;
                }

                schedules.forEach(schedule => {
                    const scheduleCard = `
                        <div class="schedule-card">
                            <div class="row align-items-center">
                                <div class="col-5 d-flex align-items-start">
                                    <div class="me-3 text-center">
                                        <p class="mb-1 text-danger fw-bold">${new Date(schedule.departureTime).toLocaleTimeString()}</p>
                                        <span class="text-muted small">${schedule.fromLocation}</span>
                                    </div>
                                    <div class="d-flex flex-column align-items-center mx-3">
                                        <span class="text-danger"><i class="fas fa-circle"></i></span>
                                        <div style="width: 2px; height: 40px; background-color: #dcdcdc;"></div>
                                        <span class="text-secondary"><i class="fas fa-map-marker-alt"></i></span>
                                    </div>
                                    <div class="ms-3 text-center">
                                        <p class="mb-1 text-dark fw-bold">${new Date(schedule.arrivalTime).toLocaleTimeString()}</p>
                                        <span class="text-muted small">${schedule.toLocation}</span>
                                    </div>
                                </div>
                                <div class="col-7 d-flex justify-content-between align-items-center">
                                    <div class="d-flex align-items-center">
                                        <i class="fas fa-bus-alt text-secondary me-2"></i>
                                        <span class="fw-bold text-dark">${schedule.busNumber}</span>
                                    </div>
                                    <div class="d-flex align-items-center">
                                        <span class="text-danger fw-bold fs-5">${schedule.price}\u20AC</span>
                                    </div>
                                    <div>
                                        <button class="btn btn-primary rounded-pill px-3 py-1" 
                                            data-bs-toggle="modal" 
                                            data-bs-target="#bookingModal" 
                                            onclick="openBookingModal('${schedule.fromLocation}', '${schedule.toLocation}', '${schedule.availableSeats}', '${schedule.status}', ${schedule.price}, ${schedule.scheduleId})">
                                            Book Now
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>`;
                    busScheduleContainer.insertAdjacentHTML('beforeend', scheduleCard);
                });

                resultView.style.display = 'block';
                setTimeout(() => {
                    resultView.style.opacity = 1;
                }, 0);
            })
            .catch(error => {
                console.error("Error fetching schedules:", error);
                busScheduleContainer.innerHTML = '<p class="text-center text-danger">An error occurred while fetching schedules. Please try again.</p>';
                resultView.style.display = 'block';
                resultView.style.opacity = 1;
            });
    });

    numberOfSeatsInput.addEventListener('input', function () {
        const numberOfSeats = parseInt(numberOfSeatsInput.value) || 0;
        const totalAmount = numberOfSeats * currentPrice;
        totalAmountInput.value = totalAmount > 0 ? `${totalAmount}\u20AC` : '';
    });

    window.openBookingModal = function (departure, arrival, availableSeats, status, price, scheduleId) {
        document.getElementById('modalDeparture').textContent = departure;
        document.getElementById('modalArrival').textContent = arrival;
        document.getElementById('modalSeats').textContent = availableSeats;
        document.getElementById('modalStatus').textContent = status;

        document.getElementById('numberOfSeats').value = '';
        document.getElementById('totalAmount').value = '';
        currentPrice = price;
        selectedScheduleId = scheduleId;

        const now = new Date();
        reservationDateInput.value = now.toISOString();

        fetch('/Authenticate/GetCurrentUser')
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to fetch current user.');
                }
                console.log("data", response);
                return response.json();
            })
            .then(data => {
                console.log("data", data.data)
                passengerNameInput.value = `${data.data.firstName} ${data.data.lastName}`;
                userId = data.data.userId;
            })
            .catch(error => {
                console.error("Error fetching user:", error);
                passengerNameInput.value = "Guest";
            });
    };

    confirmBookingBtn.addEventListener('click', function () {
        const numberOfSeats = parseInt(numberOfSeatsInput.value);
        const totalAmount = parseFloat(totalAmountInput.value.replace('€', '').trim());
        const reservationDate = reservationDateInput.value;

        if (!numberOfSeats || numberOfSeats <= 0) {
            alert("Please enter a valid number of seats.");
            return;
        }

        if (!userId) {
            alert("User not identified. Please log in.");
            return;
        }

        const data = {
            ReservationDate: reservationDate,
            NumberOfSeats: numberOfSeats,
            TotalAmount: totalAmount,
            ScheduleId: selectedScheduleId,
            UserId: userId,
        };

        fetch("/api/BusSchedule/AddReservation", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error("Failed to add reservation.");
                }
                return response.json();
            })
            .then(result => {
                if (result.success) {
                    document.getElementById("bookingModal").classList.remove("show");
                    location.reload();
                } else {
                    alert("Failed to add reservation. Please try again.");
                }
            })
            .catch(error => {
                console.error("Error adding reservation:", error);
                alert("An error occurred while adding the reservation.");
            });
    });
});