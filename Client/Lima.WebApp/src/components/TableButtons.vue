<template>
  <div class="table-action">
    <button class="table-btn table-btn__edit" @click="$emit('edit')">
      <i class="icofont-pencil-alt-1"></i>
    </button>
    <button
      v-if="deleteButton"
      class="table-btn table-btn__delete"
      @click="$emit('delete')"
    >
      <i class="icofont-trash"></i>
    </button>
    <button
      v-if="isLocation"
      class="table-btn table-btn__location"
      @click="$emit('locationClicked')"
    >
      <i class="icofont-location-pin"></i>
    </button>
  </div>
</template>

<script>
export default {
  emits: ["edit", "delete", "locationClicked"],
  props: ["deleteButton", "isLocation"],
};
</script>
