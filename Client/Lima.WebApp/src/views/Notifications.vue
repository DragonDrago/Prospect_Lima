<template>
  <div class="notifications">
    <div class="filter-left">
      <i class="icofont-chart-bar-graph"></i>
      <div class="filter-name">{{ $t("notifications") }}</div>
    </div>
    <div class="notifications-page">
      <div class="notifications-page__title">{{ $t("today") }}</div>
      <div class="notifications-wrapper">
        <div class="notifications-list">
          <div class="notifications-list__time">18.00</div>
          <div class="notifications-list__container">
            <div class="notifications-list__text">
              {{ $t("you_have_been_assigned_a_task") }}
            </div>
            <div class="notifications-list__num">353</div>
          </div>
        </div>
        <div class="notifications-list">
          <div class="notifications-list__time">18:30</div>
          <div class="notifications-list__container">
            <div class="notifications-list__text">
              {{ $t("meeting_in_minutes") }}
            </div>
            <div class="notifications-list__num">1245</div>
          </div>
        </div>
        <div class="notifications-list">
          <div class="notifications-list__time">18:30</div>
          <div class="notifications-list__container">
            <div class="notifications-list__text">
              {{ $t("the_status_of_the_shipment_has_been_changed_to_shipped") }}
            </div>
            <div class="notifications-list__num">986</div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {};
</script>

<style></style>
