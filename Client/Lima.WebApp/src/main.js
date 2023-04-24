import Vue from "vue";
import "./assets/scss/main.scss";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import "./interceptors/axios";
import Paginate from "vuejs-paginate";
import YmapPlugin from "vue-yandex-maps";
import i18n from "./i18n";
import EmptyComponentToggle from "./mixins/EmptyComponentToggle";
import VueCompositionAPI from "@vue/composition-api";

Vue.config.productionTip = false;

const settings = {
  apiKey: process.env.VUE_APP_YANDEX_KEY,
  lang: "ru_RU",
  coordorder: "latlong",
  enterprise: false,
  version: "2.1",
};

Vue.prototype.$grants = JSON.parse(localStorage.getItem("GRANTS")) || [];

Vue.use(YmapPlugin, settings);
Vue.use(VueCompositionAPI);
Vue.mixin(EmptyComponentToggle);
Vue.component("paginate", Paginate);

new Vue({
  router,
  store,
  i18n,
  render: (h) => h(App),
}).$mount("#app");
