<template>
  <transition name="fade">
    <div
      v-show="openLocation"
      class="modal-outer"
      @click.stop="handleClose($event)"
    >
      <transition name="slide-fade">
        <div class="modal-geolocation" v-if="openLocation">
          <div class="modal-geolocation__title">
            {{ $t(title) }}
          </div>
          <div class="modal-geolocation__btns">
            <button
              @click="$emit('makeFalse')"
              class="modal-geolocation__btn btn__red"
            >
              {{ $t("no") }}
            </button>
            <button
              @click="getCoordinates"
              class="modal-geolocation__btn btn__blue"
            >
              {{ $t("yes") }}
            </button>
          </div>
        </div>
      </transition>
    </div>
  </transition>
</template>

<script>
export default {
  emits: ["makeFalse", "getCoordinates"],
  props: ["openLocation", "title"],
  methods: {
    getCoordinates() {
      const succeed = (pos) => {
        this.$emit("getCoordinates", {
          latitude: pos.coords.latitude,
          longitude: pos.coords.longitude,
        });
      };
      const failure = (err) => {
        console.log(err);
      };
      const options = {
        enableHightAccuracy: true,
        timeout: 5000,
        maximumAge: 0,
      };
      navigator.geolocation.getCurrentPosition(succeed, failure, options);
    },
    handleClose(e) {
      if (!e.target.closest(".modal-geolocation")) {
        this.$emit("makeFalse");
      }
    },
  },
};
</script>

<style></style>
