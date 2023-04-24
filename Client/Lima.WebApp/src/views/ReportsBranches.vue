<template>
  <div class="reports">
    <div class="filter-left">
      <i class="icofont-chart-bar-graph"></i>
      <div class="filter-name">{{ $t("report_by_branches") }}</div>
    </div>
    <div class="reports-page">
      <div class="reports-page__top">
        <input type="text" :placeholder="$t('regions')" class="input" />
        <input type="text" :placeholder="$t('date_h')" class="input" />
        <input type="text" :placeholder="$t('date_o')" class="input" />
      </div>
      <div class="statistics-menu statistic-reports">
        <div class="statistics-menu__card">
          <i class="icofont-user-alt-6"></i>
          <div class="statistics-menu__content">
            <div class="statistics-menu__title">
              {{ $t("reserved_for_the_amount_of") }}
            </div>
            <div class="statistics-menu__num">{{ prettify(3000000) }}</div>
          </div>
        </div>
        <div class="statistics-menu__card">
          <i class="icofont-user-alt-6"></i>
          <div class="statistics-menu__content">
            <div class="statistics-menu__title">
              {{ $t("prepayments_in_the_amount_of") }}
            </div>
            <div class="statistics-menu__num">{{ prettify(1500000) }}</div>
          </div>
        </div>
        <div class="statistics-menu__card">
          <i class="icofont-user-alt-6"></i>
          <div class="statistics-menu__content">
            <div class="statistics-menu__title">
              {{ $t("balances_in_the_amount_of") }}
            </div>
            <div class="statistics-menu__num">{{ prettify(1500000) }}</div>
          </div>
        </div>
        <div class="statistics-menu__card">
          <i class="icofont-user-alt-6"></i>
          <div class="statistics-menu__content">
            <div class="statistics-menu__title">
              {{ $t("sold_for_the_amount_of") }}
            </div>
            <div class="statistics-menu__num">{{ prettify(1500000) }}</div>
          </div>
        </div>
      </div>
      <div class="reports-charts">
        <pie-chart>
          <template #title>{{ $t("visits_to_pharmacies") }}</template>
          <template #text>
            {{ $t("total") }} <br />
            200
          </template>
        </pie-chart>
        <pie-chart>
          <template #title>{{ $t("visits_to_wholesalers") }}</template>
          <template #text>
            {{ $t("total") }} <br />
            200
          </template>
        </pie-chart>
        <pie-chart>
          <template #title>{{ $t("visits_to_doctors") }}</template>
          <template #text>
            {{ $t("total") }} <br />
            200
          </template>
        </pie-chart>
        <pie-chart>
          <template #title>{{
            $t("visits_to_the_medical_institution")
          }}</template>
          <template #text>
            {{ $t("total") }} <br />
            200
          </template>
        </pie-chart>
        <pie-chart>
          <template #title>{{ $t("tasks") }}</template>
          <template #text>
            {{ $t("total") }} <br />
            200
          </template>
        </pie-chart>
      </div>
    </div>
  </div>
</template>

<script>
import PieChart from "@/components/PieChart.vue";
import { prettify } from "../use/PrettifySum";
export default {
  data() {
    return {
      prettify,
    };
  },
  components: { PieChart },
};
</script>

<style></style>
