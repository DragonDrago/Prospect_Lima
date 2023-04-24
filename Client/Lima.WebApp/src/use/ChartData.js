const days = [];
for (let i = 1; i <= 31; i++) {
  days.push(i);
}

export const ChartData = {
  type: "line",
  data: {
    labels: days,
    datasets: [
      {
        label: "Продажи за месяц",
        data: [
          0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 50, 45, 50, 55, 60, 65,
          70, 75, 70, 65, 60, 65, 70, 75, 80, 85, 90, 95, 100,
        ],
        borderColor: "#5b71e8",
        pointBackgroundColor: "#5b71e8",
        borderWidth: 1,
      },
    ],
  },
  options: {
    responsive: true,
    maintainAspectRatio: false,
    lineTension: 1,
    legend: {
      display: false,
    },
    tooltips: {
      callbacks: {
        label: function (tooltipItem) {
          return tooltipItem.yLabel;
        },
      },
    },
    scales: {
      yAxes: [
        {
          ticks: {
            beginAtZero: true,
          },
        },
      ],
    },
  },
};

export default ChartData;
