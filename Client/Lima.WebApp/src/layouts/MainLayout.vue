<template>
  <div class="app-main-layout">
    <the-navbar @makeFalse="makeFalse" @removeElement="removeElement" />
    <the-sidebar :dropdownList="dropdownList" @clickHandler="clickHandler" />
    <div class="app-page">
      <router-view />
    </div>
  </div>
</template>

<script>
import TheNavbar from "@/components/TheNavbar.vue";
import TheSidebar from "@/components/TheSidebar.vue";

export default {
  components: { TheNavbar, TheSidebar },
  data() {
    return {
      dropdownList: [
        { item: false },
        { item: false },
        { item: false },
        { item: false },
        { item: false },
        { item: false },
      ],
    };
  },
  methods: {
    makeFalse() {
      this.dropdownList.forEach((list) => (list.item = false));
    },
    clickHandler(idx) {
      if (!document.body.classList.contains("small-sidenav")) {
        this.dropdownList[idx].item = !this.dropdownList[idx].item;
      } else {
        this.makeFalse();
      }
    },
    removeElement() {
      document.body.classList.remove("mobile-sidenav");
    },
  },
};
</script>
