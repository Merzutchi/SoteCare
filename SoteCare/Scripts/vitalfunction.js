// Chart rendering on page load
document.addEventListener("DOMContentLoaded", function () {
    const vitalData = window.vitalData; // This data comes from the Razor view

    const labels = vitalData.Dates; // X-axis labels (dates)
    const heartRates = vitalData.HeartRates; // Data for Heart Rate
    const systolicBP = vitalData.SystolicBP; // Data for Systolic Blood Pressure
    const diastolicBP = vitalData.DiastolicBP; // Data for Diastolic Blood Pressure

    // canvas context for rendering the chart
    const ctx = document.getElementById('vitalChart').getContext('2d');

    // Create the Chart.js line chart
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

    // Add form toggle functionality
    const addButton = document.getElementById("showAddForm");
    const addForm = document.getElementById("addVitalFunctionForm");

    if (addButton && addForm) {
        addButton.addEventListener("click", function () {
            if (addForm.style.display === "none" || addForm.style.display === "") {
                addForm.style.display = "block";
            } else {
                addForm.style.display = "none";
            }
        });
    }
});