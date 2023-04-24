<template>
  <div class="chart">
    <h2 class="chart-title">{{ chart_title }}</h2>
    <canvas id="chart"></canvas>
  </div>
</template>

<script>
import Chart from "chart.js";
import ChartData from "@/use/ChartData.js";

export default {
  name: "chart",
  props: ["chart_title"],
  data() {
    return {
      ChartData,
    };
  },
  mounted() {
    const ctx = document.getElementById("chart");
    new Chart(ctx, this.ChartData);
  },
};
</script>
