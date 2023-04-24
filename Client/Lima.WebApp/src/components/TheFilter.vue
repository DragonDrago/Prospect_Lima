<template>
  <div class="filter">
    <div class="filter-left">
      <i class="icofont-chart-bar-graph"></i>
      <div class="filter-name"><slot name="name"></slot></div>
    </div>
    <div class="filter-bar" :class="{ fullbar: fullbar }">
      <button
        class="filter-btn__left btn btn-blue"
        @click="$emit('filterClicked')"
      >
        {{ $t("filter") }}
      </button>
      <ul class="filter-menu">
        <li class="filter-menu__items" v-for="item in filterItems" :key="item">
          <p>{{ item }}</p>
          <span @click="$emit('removeItem', item)">&times;</span>
        </li>
      </ul>
    </div>
    <button
      class="filter-btn"
      @click="$emit('filterAdd')"
      :class="{ 'no-filter': isfilter }"
    >
      <i class="icofont-plus-square"></i>
    </button>
  </div>
</template>

<script>
export default {
  props: ["filterItems", "fullbar", "isfilter"],
  emits: ["filterClicked", "filterAdd", "removeItem"],
};
</script>

<style></style>
