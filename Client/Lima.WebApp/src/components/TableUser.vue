<template>
  <div class="table-user">
    <img :src="img" :alt="name" />
    <p>{{ name }}</p>
  </div>
</template>

<script>
export default {
  props: ["img", "name"],
};
</script>

<style></style>
