<template>
  <div class="empty" :class="{ opened: !item.length }" style="opacity: 0">
    <img src="@/assets/icons/empty_folder.svg" alt="empty_folder" />
    <p>{{ $t("empty") }}</p>
  </div>
</template>

<script>
export default {
  props: ["item"],
};
</script>

<style></style>
