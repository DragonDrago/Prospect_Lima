<template>
  <div class="navbar">
    <div class="navbar-wrapper">
      <div class="navbar-left">
        <router-link :to="{ name: 'Home' }" class="navbar-logo">
          <img src="@/assets/image/logo.jpg" alt="logo" />
        </router-link>

        <div class="navbar-resize" @click="smallNav">
          <i class="icofont-outdent"></i>
        </div>
        <div class="navbar-resize full-screen" @click="requestFullScreen">
          <i class="icofont-expand"></i>
        </div>
        <div class="navbar-search">
          <input type="text" :placeholder="`${$t('search')}...`" />
          <span class="icofont icofont-search"></span>
        </div>
      </div>
      <div class="navbar-right">
        <div class="navbar-time">
          {{ currentTime }}
        </div>
        <div class="navbar-notif message" @click="$router.push('/messages')">
          <i class="icofont-envelope"></i>
          <span>0</span>
        </div>
        <div class="navbar-notif notification">
          <i class="notification-icon icofont-notification"></i>
          <span>0</span>
          <div
            class="navbar-notif__menu"
            :class="{ 'active-notif': activeNotif }"
          >
            <ul>
              <li>
                <div class="time">11:00</div>
                <div class="message">
                  напоминание о заплонированном посещении
                </div>
              </li>
              <li>
                <div class="time">11:00</div>
                <div class="message">вам поставлена задача</div>
              </li>
              <li class="all">
                <router-link to="/notifications">Смотреть всё</router-link>
              </li>
            </ul>
          </div>
        </div>
        <div class="navbar-notif lang" @click="isLangOpen = !isLangOpen">
          <div class="language">
            {{ languages[langIndex].name }}
          </div>
          <ul v-if="isLangOpen" class="lang-list">
            <li
              v-for="(lang, index) in languages"
              :key="lang.name"
              @click.stop="changeHandler(index, lang.value)"
            >
              <img :src="lang.img" :alt="lang.name" />{{ lang.name }}
            </li>
          </ul>
        </div>
        <div class="navbar-profile">
          <img src="no-user.jpg" alt="profile" />
          <div
            class="navbar-profile__menu"
            :class="{ 'active-profile': activeProfile }"
          >
            <ul>
              <li>
                <router-link to="/profile">{{ $t("my_profile") }}</router-link>
              </li>
              <li>
                <router-link to="/calendar">{{ $t("calendar") }}</router-link>
              </li>
              <li>
                <a href="#" @click.prevent="logout">{{ $t("exit") }}</a>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
    <teleport to="body">
      <div class="overlay-mobile" @click="$emit('removeElement')"></div>
    </teleport>
  </div>
</template>

<script>
import Teleport from "vue2-teleport";
import { mapState, mapMutations } from "vuex";
export default {
  emits: ["makeFalse", "removeElement"],
  components: { Teleport },
  data() {
    return {
      interval: null,
      activeNotif: false,
      activeProfile: false,
      windowWidth: 0,
      langIndex: +localStorage.getItem("localeIndex") || 0,
      isLangOpen: false,
    };
  },
  computed: {
    ...mapState(["currentTime"]),
    languages: () => [
      {
        name: "RU",
        img: "./ru.jpg",
        value: "ru",
      },
      { name: "УЗ", img: "./uz.gif", value: "uz" },
      { name: "O'Z", img: "./uz.gif", value: "uz" },
      {
        name: "EN",
        img: "./en.png",
        value: "en",
      },
    ],
  },
  methods: {
    ...mapMutations("auth", ["logout"]),
    smallNav() {
      document.body.classList.toggle("small-sidenav");
      if (this.windowWidth < 575) {
        document.body.classList.remove("small-sidenav");
        document.body.classList.toggle("mobile-sidenav");
      }

      this.$emit("makeFalse");
    },
    requestFullScreen() {
      const body = document.body;
      const requestMethod =
        body.requestFullScreen ||
        body.webkitRequestFullScreen ||
        body.mozRequestFullScreen ||
        body.msRequestFullScreen;

      if (requestMethod) {
        // Native full screen.
        requestMethod.call(body);
      } else if (typeof window.ActiveXObject !== "undefined") {
        // Older IE.
        const wscript = new ActiveXObject("WScript.Shell");

        if (wscript !== null) {
          wscript.SendKeys("{F11}");
        }
      }
    },
    getWindowWidth(event) {
      this.windowWidth = document.documentElement.clientWidth;
    },
    changeHandler(idx, value) {
      localStorage.setItem("locale", value);
      localStorage.setItem("localeIndex", idx);
      this.$i18n.locale = value;
      this.langIndex = idx;
      this.isLangOpen = false;
    },
  },
  mounted() {
    this.$store.dispatch("renderDate");
    document.body.addEventListener("click", (e) => {
      e.target.closest(".notification") &&
      !e.target.closest(".notification .all")
        ? (this.activeNotif = true)
        : (this.activeNotif = false);

      e.target.closest(".navbar-profile") &&
      !e.target.closest(".navbar-profile__menu > ul > li > a")
        ? (this.activeProfile = true)
        : (this.activeProfile = false);

      e.target.closest(".lang")
        ? (this.isLangOpen = true)
        : (this.isLangOpen = false);
    });

    this.$nextTick(function () {
      window.addEventListener("resize", this.getWindowWidth);

      //Init
      this.getWindowWidth();
    });
  },
  beforeDestroy() {
    this.$store.commit("CLEAR_INTERVAL");
    window.removeEventListener("resize", this.getWindowWidth);
  },
};
</script>

<style></style>
