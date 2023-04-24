<template>
  <div class="login">
    <div class="login-left">
      <div class="login-logo__wrapper">
        <img class="login-logo" src="../assets/image/logo.jpg" alt="logo" />
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
      </div>

      <form class="login-form" @submit.prevent="submitHandler">
        <h2 class="login-form__title">{{ $t("entry_form") }}</h2>
        <p class="login-form__subtitle">{{ $t("log_in_to_your_account") }}</p>
        <div class="login-form__control">
          <input
            type="text"
            :placeholder="$t('login')"
            required
            v-model="user"
          />
        </div>
        <div class="login-form__control">
          <input
            type="password"
            :placeholder="$t('password')"
            required
            v-model="password"
          />
        </div>
        <label class="login-form__container">
          <input
            type="checkbox"
            class="login-form__checkbox"
            v-model="remember"
          />
          <span class="login-form__checkmark"></span>
          <p>{{ $t("remember_me") }}</p>
        </label>
        <button type="submit" class="login-form__btn">
          <i class="icofont-sign-in"></i>{{ $t("entrance") }}
        </button>
      </form>
    </div>
    <div class="login-right">
      <login-svg />
    </div>
  </div>
</template>

<script>
import LoginSvg from "@/components/LoginSvg.vue";
import { mapActions, mapGetters } from "vuex";
import moveLoginImages from "../use/LoginImages";
export default {
  components: { LoginSvg },
  data() {
    return {
      langIndex: +localStorage.getItem("localeIndex") || 0,
      isLangOpen: false,
      user: "",
      password: "",
      remember: false,
    };
  },
  computed: {
    isChecked() {
      return this.remember === true ? 1 : 0;
    },
    ...mapGetters("auth", ["loginError"]),
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
    ...mapActions("auth", ["login"]),
    changeHandler(idx, value) {
      localStorage.setItem("locale", value);
      localStorage.setItem("localeIndex", idx);
      this.$i18n.locale = value;
      this.langIndex = idx;
      this.isLangOpen = false;
    },
    async submitHandler() {
      if (this.login !== "" && this.password !== "") {
        const params = {
          login: this.user,
          password: this.password,
        };
        await this.login(params);
        this.user = "";
        this.password = "";
      }
    },
  },
  mounted() {
    localStorage.removeItem("TOKEN");
    moveLoginImages();
    document.body.addEventListener("click", (e) => {
      e.target.closest(".lang")
        ? (this.isLangOpen = true)
        : (this.isLangOpen = false);
    });
  },
};
</script>
