
function renderData() {
    return Array.from({ length: 12 }, (v, i) => {
        return {
            label: 'Tháng ' + (i + 1),
            value: Math.floor(Math.random() * 100) + 1
        }
    });
}
function getRndInteger(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}
function getLast7Daysdata() {
    const result = [];
    const today = new Date();

    for (let i = 7; i >= 0; i--) {
        const date = new Date(today);
        date.setDate(today.getDate() - i);
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        result.push({
            label: `${month}/${day}`,
            value: getRndInteger(70, 100)
        }); 
    }

    return result;
}
const today = moment();
const currentWeekNumber = today.isoWeek();
console.log('Quý ' + today.quarter())

console.log(`Số tuần hiện tại là: ${currentWeekNumber}`);
let data = [
    {
        label: '2024',
        value: getRndInteger(70, 100)
    },
    {
        label: 'Quý 1',
        value: getRndInteger(70, 100)
    },
    {
        label: 'Quý 2',
        value: getRndInteger(70, 100)
    },
    {
        label: 'Tháng 4',
        value: getRndInteger(70, 100)
    },
    {
        label: 'Tháng 5',
        value: getRndInteger(70, 100)
    },
    {
        label: 'Tuần ' + (currentWeekNumber - 1),
        value: getRndInteger(70, 100)
    },
    {
        label: 'Tuần ' + currentWeekNumber,
        value: getRndInteger(70, 100)
    }
]
data = [...data, ...getLast7Daysdata()]

function linechart() {
    new Chart(document.getElementById('line-chart'),
        {
            type: 'line',
            data: {
                labels: data.map(d => d.label),
                datasets: [{
                    label: 'Tỉ lệ tăng ca %',
                    data: data.map(d => getRndInteger(70, 100)),
                    fill: false,
                    borderColor: '#5366e3',
                    backgroundColor: '#5366e3',
                    showLabelBackdrop: false,
                    tension: 0.1
                },
                {
                    label: 'Tỷ lệ đi làm %',
                    data: data.map(d => getRndInteger(80, 100)),
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
                        },
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
                        }
                    }
                }
            },
            plugins: [ChartDataLabels],
        }
    );
};

let promise = new Promise((resolve, reject) => {
    resolve()
})


promise.then(() => {
    linechart();
})

