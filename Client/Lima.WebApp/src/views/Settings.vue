<template>
  <div class="settings">
    <div class="filter-left">
      <i class="icofont-chart-bar-graph"></i>
      <div class="filter-name">{{ $t("basic_settings") }}</div>
    </div>
    <div class="settings-page">
      <div class="settings-page__content">
        <div class="settings-page__title">{{ $t("company") }}</div>
        <div class="settings-page__inner">
          <input type="text" placeholder="PROSPEKT" class="input" />
          <div class="input file-input">
            <p>{{ $t("choose") }}...</p>
            <button class="btn btn-blue">
              <i class="icofont-clip"></i> <span>{{ $t("file") }}</span>
            </button>
            <input type="file" />
          </div>
          <input type="text" :placeholder="$t('intr')" class="input" />
          <input type="text" :placeholder="$t('a_country')" class="input" />
          <input type="text" :placeholder="$t('city')" class="input" />
          <input type="text" :placeholder="$t('regions')" class="input" />
          <input type="text" :placeholder="$t('address')" class="input" />
          <input type="text" :placeholder="$t('telephone')" class="input" />
        </div>
      </div>
      <div class="settings-page__content">
        <div class="settings-page__title">{{ $t("notifications") }}</div>
        <div class="settings-page__inner">
          <p>{{ $t("alerts_every") }}</p>
          <input type="text" placeholder="1 мин" class="input" />
          <p>{{ $t("remind_for") }}</p>
          <input type="text" placeholder="15 мин" class="input" />
        </div>
      </div>
      <div class="settings-page__content">
        <div class="settings-page__title">{{ $t("additional") }}</div>
        <div class="settings-page__inner">
          <input type="text" placeholder="Русский" class="input" />
          <input type="text" placeholder="Администратор" class="input" />
          <input type="text" placeholder="ASIA/TASHKENT" class="input" />
        </div>
      </div>
      <button class="btn btn-red" style="margin-left: auto">
        {{ $t("save") }}
      </button>
    </div>
  </div>
</template>

<script>
export default {};
</script>

<style></style>
