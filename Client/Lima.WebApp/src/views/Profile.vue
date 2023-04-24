<template>
  <div class="profile">
    <div class="filter-left">
      <i class="icofont-chart-bar-graph"></i>
      <div class="filter-name">{{ $t("profile") }}</div>
    </div>
    <div class="profile-blog">
      <div class="profile-wrapper">
        <div class="profile-top">
          <div class="profile-img">
            <img src="no-user.jpg" alt="profile-img" />
          </div>
        </div>
        <div class="profile-content">
          <ul class="profile-content__menu">
            <li class="profile-content__list">
              <div class="profile-content__inner">
                <i class="icofont-envelope"></i>
                <p>{{ $t("lfm") }}</p>
              </div>
              <div class="profile-content__info">{{ user.full_name }}</div>
            </li>
            <li class="profile-content__list">
              <div class="profile-content__inner">
                <i class="icofont-envelope"></i>
                <p>{{ $t("date_of_birth") }}</p>
              </div>
              <div class="profile-content__info">
                {{ new Date(user.birthday).toLocaleDateString("ru-RU") }}
              </div>
            </li>
            <li class="profile-content__list">
              <div class="profile-content__inner">
                <i class="icofont-envelope"></i>
                <p>{{ $t("telephone") }}</p>
              </div>
              <div class="profile-content__info">
                {{ user.phone || $t("empty") }}
              </div>
            </li>
            <li class="profile-content__list">
              <div class="profile-content__inner">
                <i class="icofont-envelope"></i>
                <p>{{ $t("pesidential_address") }}</p>
              </div>
              <div class="profile-content__info">
                {{ user.address || $t("empty") }}
              </div>
            </li>
            <li class="profile-content__list">
              <div class="profile-content__inner">
                <i class="icofont-envelope"></i>
                <p>{{ $t("regions") }}</p>
              </div>
              <div class="profile-content__info">{{ user.region_name }}</div>
            </li>
            <li class="profile-content__list">
              <div class="profile-content__inner">
                <i class="icofont-envelope"></i>
                <p>{{ $t("department") }}</p>
              </div>
              <div class="profile-content__info">{{ user.department }}</div>
            </li>
            <li class="profile-content__list">
              <div class="profile-content__inner">
                <i class="icofont-envelope"></i>
                <p>{{ $t("post") }}</p>
              </div>
              <div class="profile-content__info">
                {{ user.position || $t("empty") }}
              </div>
            </li>
            <li class="profile-content__list">
              <div class="profile-content__inner">
                <i class="icofont-envelope"></i>
                <p>{{ $t("login") }}</p>
              </div>
              <div class="profile-content__info">
                {{ user.login || $t("empty") }}
              </div>
            </li>
            <li class="profile-content__list">
              <div class="profile-content__inner">
                <i class="icofont-envelope"></i>
                <p>{{ $t("role") }}</p>
              </div>
              <div class="profile-content__info">
                {{ user.role_name }}
              </div>
            </li>
          </ul>
          <div class="modal-buttons" v-if="false">
            <button class="btn btn-blue">{{ $t("edit") }}</button>
            <button class="btn btn-red">Отключить</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
export default {
  computed: {
    ...mapGetters("user", ["user"]),
  },
  methods: {
    ...mapActions("user", ["getUser"]),
  },
  async mounted() {
    await this.getUser();
  },
};
</script>

<style></style>
