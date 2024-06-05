//import Chart from 'chart.js/auto'

function renderData() {
    return Array.from({ length: 12 }, (v, i) => {
        return {
            month: 'Tháng '+ (i+1),
            count: Math.floor(Math.random() * 100) + 1
        }
    });
}
function getRndInteger(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

 function barchart() {
    const data = renderData();
    const data2 = renderData();

    var maxValue = data.reduce(function (prev, current) {
        return (prev > current.count) ? prev : current.count;
    }, 0);
    var maxValue2 = data2.reduce(function (prev, current) {
        return (prev > current.count) ? prev : current.count;
    }, 0);

    maxValue = maxValue > maxValue2 ? maxValue : maxValue2;

    new Chart(
        document.getElementById('acquisitions'),
        {
            type: 'bar',
            data: {
                labels: data.map(row => row.month),
                datasets: [
                    {
                        label: 'Xin ra ngoài',
                        data: data.map(row => row.count),
                        borderColor: '#36A2EB',
                        backgroundColor: '#9BD0F5',
                    },
                    {
                        label: 'Xin về sớm',
                        data: renderData().map(row => row.count),
                        borderColor: '#FF6384',
                        backgroundColor: '#FFB1C1',
                    }
                ],
                barPercentage: 0.5,
                //barThickness: 6,
                //maxBarThickness: 8,
                //minBarLength: 2,
            },
            
            options: {
                scales: {
                    y: {
                        max: maxValue + 10,
                    }
                },
                plugins: {
                    title: {
                        display: true,
                        text: 'Biểu đồ',
                        padding: {
                            top: 10,
                            bottom: 30
                        }
                    },
                    datalabels: {
                        anchor: 'end', // 'start', 'center', 'end' (vị trí của nhãn dữ liệu trên thanh)
                        align: 'top', // 'top', 'center', 'bottom' (cách thức căn chỉnh của nhãn dữ liệu)
                        color: '#000', // Màu sắc của nhãn dữ liệu
                        font: {
                            weight: 'light', // Độ đậm của font chữ
                            size: 14 
                        },
                    }
                }
            },
            plugins: [ChartDataLabels],
        }
    );
}

function linechart () {
   var data1 = renderData()

    new Chart(
        document.getElementById('line-chart'),
        {
            type: 'line',
            data: {
                labels: data1.map(d => d.month),
                datasets: [{
                    label: 'Tỉ lệ tăng ca %',
                    data: data1.map(d => getRndInteger(70, 100)),
                    fill: false,
                    borderColor: '#5366e3',
                    backgroundColor: '#5366e3',
                    showLabelBackdrop:false  ,
                    tension: 0.1
                },
                {
                    label: '# Tỷ lệ đi làm %',
                    data: data1.map(d => getRndInteger(80, 100)),
                    fill: false,
                    borderColor: '#02b506',
                    backgroundColor: '#02b506',
                    tension: 0.1    
                },
                ]
            },
            options: {
                scales: {
                    x: {
                        display: true, // Hiển thị trục x
                        grid: {
                            display: true // Hiển thị lưới trục x
                        }
                    },
                    y: {
                        display: true, 
                        suggestedMax: 105,
                        grid: {
                            display: true // Hiển thị lưới trục y
                        }         ,
                        beginAtZero: true
                    }
                },
                plugins: {
                    datalabels: {
                        anchor: 'center', // 'start', 'center', 'end' (vị trí của nhãn dữ liệu trên thanh)
                        align: 'top', // 'top', 'center', 'bottom' (cách thức căn chỉnh của nhãn dữ liệu)
                        //color: '#000', // Màu sắc của nhãn dữ liệu
                        font: {
                            weight: 'light', // Độ đậm của font chữ
                            size: 14 // Kích thước font chữ
                        },
                        labels: {
                            title: {
                                font: {
                                    weight: 'bold'
                                }
                            },
                            value: {
                                color: 'green'
                            }
                        }
                    }
                }
            },
            plugins: [ChartDataLabels],
        }
    );
};

$.ajax({
    url: "/AccessManage/Report",
    method: "POST",
    success: function (response) {
        console.log(response);

        $('.loading').fadeOut('slow');
        linechart();
        barchart();
    },
    error: function (xhr, status, error) {

        console.error("Error:", error);
    }
});

