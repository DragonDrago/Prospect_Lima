<template>
  <transition name="fade">
    <div
      v-show="isModalOpen"
      class="modal-small modal-outer"
      @click.stop="handleClose($event)"
    >
      <transition name="slide-fade">
        <div class="modal" v-if="isModalOpen">
          <div class="modal-title"><slot name="title" /></div>

          <slot name="top" />
          <div class="modal-close" @click="$emit('closeModal')">&times;</div>
        </div>
      </transition>
    </div>
  </transition>
</template>

<script>
export default {
  emits: ["closeModal"],
  props: ["isModalOpen", "modalProps"],
  methods: {
    handleClose(e) {
      if (!e.target.closest(".modal")) {
        this.$emit("closeModal");
      }
    },
  },
};
</script>
