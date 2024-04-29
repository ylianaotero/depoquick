window.createBarChart = function(labels, data) {
    var ctx = document.getElementById('barChartCanvas').getContext('2d');
    
    const colors = [
        'rgba(255, 99, 132, 0.2)',
        'rgba(54, 162, 235, 0.2)',
        'rgba(255, 206, 86, 0.2)',
        'rgba(75, 192, 192, 0.2)',
        'rgba(153, 102, 255, 0.2)'  
    ];

    const bordercolors = [
        'rgba(255, 99, 132, 1)',
        'rgba(54, 162, 235, 1)',
        'rgba(255, 206, 86, 1)',
        'rgba(75, 192, 192, 1)',
        'rgba(153, 102, 255, 1)'
    ];
    var datasets = [];
    for (var i = 0; i < labels.length; i++) {
        datasets.push({
            label: labels[i],
            backgroundColor: colors[i],
            borderColor: bordercolors[i],
            borderWidth: 1, 
            data: [data[i]]
        });
    }
    
    var myChart = new Chart(ctx, {
        type: "bar",
        data: {
            labels: ["Cantidad de reservas según área"],
            datasets: datasets
        },
        options: {
            legend: { display: false },
        }
    });
};
