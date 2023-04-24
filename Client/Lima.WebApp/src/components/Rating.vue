<template>
  <div class="rating-box">
    <h3 class="rating-title"><slot name="title"></slot></h3>
    <slot />
    <ul class="rating-list" v-if="data">
      <li>
        <div class="text">{{ $t("by_region") }}</div>
        <div class="text">{{ data.region }}</div>
        <span></span>
      </li>
      <li>
        <div class="text">{{ $t("by_doctors") }}</div>
        <div class="text">{{ data.doctor }}</div>
        <span></span>
      </li>
      <li>
        <div class="text">{{ $t("by_month") }}</div>
        <div class="text">{{ data.by_month }}</div>
        <span></span>
      </li>
      <li>
        <div class="text">{{ $t("by_organization") }}</div>
        <div class="text">{{ data.organization }}</div>
        <span></span>
      </li>
      <li>
        <div class="text">{{ $t("by_amount") }}</div>
        <div class="text">{{ prettify(data.by_profit) }}</div>
        <span></span>
      </li>
      <li>
        <div class="text">{{ $t("by_employee") }}</div>
        <div class="text">{{ data.stuff }}</div>
        <span></span>
      </li>
    </ul>
  </div>
</template>

<script>
import { prettify } from "../use/PrettifySum";
export default {
  props: ["data"],
  data() {
    return {
      prettify,
    };
  },
};
</script>

<style></style>
