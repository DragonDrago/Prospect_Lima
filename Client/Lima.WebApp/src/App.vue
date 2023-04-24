<template>
  <div id="app">
    <component :is="layout"></component>
  </div>
</template>
<script>
import MainLayout from "./layouts/MainLayout.vue";
import EmptyLayout from "./layouts/EmptyLayout.vue";
import axios from "axios";

export default {
  components: { MainLayout, EmptyLayout },
  computed: {
    layout() {
      return (this.$route.meta.layout || "empty") + "-layout";
    },
  },
  data() {
    return {
      windowWidth: 0,
    };
  },
  methods: {
    getWindowWidth(event) {
      this.windowWidth = document.documentElement.clientWidth;
      this.windowWidth < 865
        ? document.body.classList.add("small-sidenav")
        : document.body.classList.remove("small-sidenav");

      if (this.windowWidth < 575)
        document.body.classList.remove("small-sidenav");
    },
  },
  beforeDestroy() {
    window.removeEventListener("resize", this.getWindowWidth);
  },
  async mounted() {
    this.$nextTick(function () {
      window.addEventListener("resize", this.getWindowWidth);

      //Init
      this.getWindowWidth();
    });
    // const { data } = await axios.get(`settings/company`, {
    //   headers: {
    //     Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
    //   },
    // });
    // console.log(data);
    navigator.geolocation.getCurrentPosition(
      (pos) => {
        console.log("Success");
      },
      (err) => {
        console.log(
          "Вам необходимо резрешить получать информацию о том где вы находитесь"
        );
      },
      {
        enableHightAccuracy: true,
        timeout: 5000,
        maximumAge: 0,
      }
    );
  },
};
</script>
