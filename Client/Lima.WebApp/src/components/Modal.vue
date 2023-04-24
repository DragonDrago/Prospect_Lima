<template>
  <transition name="fade">
    <div
      v-show="isModalOpen"
      class="modal-outer"
      @click.stop="handleClose($event)"
    >
      <transition name="slide-fade">
        <div ref="modal" class="modal" v-if="isModalOpen">
          <div class="modal-title"><slot name="title" /></div>
          <slot name="top" />
          <form @submit.prevent="$emit('submitHandler')">
            <slot />
            <slot name="confirm" />
            <div class="modal-buttons">
              <slot name="button"></slot>
            </div>
          </form>
          <div class="modal-close" @click="$emit('closeModal')">&times;</div>
        </div>
      </transition>
    </div>
  </transition>
</template>

<script>
export default {
  emits: ["closeModal", "submitHandler"],
  props: ["isModalOpen"],
  methods: {
    handleClose(e) {
      if (!e.target.closest(".modal")) {
        this.$emit("closeModal");
      }
    },
  },
};
</script>
