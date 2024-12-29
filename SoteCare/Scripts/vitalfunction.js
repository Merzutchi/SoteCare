// Show the add form
document.getElementById("showAddForm").addEventListener("click", function () {
    const addForm = document.getElementById("addVitalFunctionForm");
    const dateTimeInput = document.getElementById("DateTimeInput");
    const now = new Date();
    const formattedDate = now.toISOString().slice(0, 16);
    dateTimeInput.value = formattedDate;
    addForm.style.display = "block";
});

// Handle form submission
document.getElementById("addVitalForm").addEventListener("submit", function (e) {
    e.preventDefault();

    const form = e.target;
    const formData = new FormData(form);

    fetch('/VitalFunctions/AddVitalFunction', {
        method: 'POST',
        body: new URLSearchParams([...formData]),
        headers: {
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        }
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                alert("Vital function added successfully!");
                updateChart(data.data); // Function to update chart with new data
                form.reset();
            } else {
                alert(`Error: ${data.message}`);
            }
        })
        .catch(error => console.error('Error:', error));
});

// Chart rendering on page load
document.addEventListener("DOMContentLoaded", function () {
    const vitalData = window.vitalData; 

    const labels = vitalData.Dates;
    const heartRates = vitalData.HeartRates;
    const systolicBP = vitalData.SystolicBP;
    const diastolicBP = vitalData.DiastolicBP;

    const ctx = document.getElementById('vitalChart').getContext('2d');
    window.myVitalChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [
                {
                    label: 'Heart Rate',
                    data: heartRates,
                    borderColor: 'blue',
                    borderWidth: 2,
                    fill: false
                },
                {
                    label: 'Systolic Blood Pressure',
                    data: systolicBP,
                    borderColor: 'red',
                    borderWidth: 2,
                    fill: false
                },
                {
                    label: 'Diastolic Blood Pressure',
                    data: diastolicBP,
                    borderColor: 'green',
                    borderWidth: 2,
                    fill: false
                }
            ]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: 'Vital Signs Over Time'
                }
            },
            scales: {
                x: {
                    title: {
                        display: true,
                        text: 'Date'
                    }
                },
                y: {
                    title: {
                        display: true,
                        text: 'Values'
                    }
                }
            }
        }
    });
});