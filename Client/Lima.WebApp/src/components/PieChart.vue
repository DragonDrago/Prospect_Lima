<template>
  <div class="pie-chart">
    <div class="pie-chart__title"><slot name="title"></slot></div>
    <div class="pie-chart__inner">
      <div class="pie-chart__text"><slot name="text"></slot></div>
      <canvas id="pie_chart"></canvas>
    </div>
  </div>
</template>

<script>
import Chart from "chart.js";
import { PieChartData } from "@/use/PieChartData.js";

export default {
  name: "pie-chart",
  data() {
    return {
      PieChartData,
    };
  },
  mounted() {
    const ctxs = document.querySelectorAll("#pie_chart");
    ctxs.forEach((ctx) => new Chart(ctx, PieChartData));
  },
};
</script>
