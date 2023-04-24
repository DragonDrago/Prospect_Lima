<template>
  <div>
    <the-filter
      :filterItems="filterItems"
      @filterClicked="isModalOpen = true"
      @filterAdd="isAddOpen = true"
    >
      <template #name>{{ $t("events") }}</template>
    </the-filter>
    <div class="calendar">
      <FullCalendar :options="calendarOptions" />
    </div>
    <teleport to="body">
      <modal @closeModal="isModalOpen = false" :isModalOpen="isModalOpen">
        <template #title>{{ $t("eventfilter") }}</template>
        <div class="form-select mb-10">
          <select class="select">
            <option>{{ $t("employee") }}</option>
            <option>{{ $t("employee") }}</option>
            <option>{{ $t("employee") }}</option>
          </select>
        </div>
        <div class="modal-form">
          <input type="date" class="input" />
          <input type="date" class="input" />
          <input type="time" class="input" />
          <input type="time" class="input" />
        </div>
        <template #button>
          <button class="btn btn-red" @click.prevent="isModalOpen = false">
            {{ $t("to_close") }}
          </button>
          <button class="btn btn-blue" type="submit">{{ $t("add") }}</button>
        </template>
      </modal>
      <modal @closeModal="isAddOpen = false" :isModalOpen="isAddOpen">
        <template #title>{{ $t("add_an_event") }}</template>
        <template #top>
          <div class="modal-top">
            <button
              v-for="(btn, index) in topButtons"
              :key="btn.id"
              class="btn"
              :class="{ 'btn-blue': btn.isActive }"
              @click="handleClick(index)"
            >
              {{ $t(btn.name) }}
            </button>
          </div>
        </template>
        <div v-if="topButtons[0].isActive">
          <div class="modal-form mb-10">
            <div class="form-select">
              <select class="select">
                <option>{{ $t("organization") }}</option>
                <option>{{ $t("organization") }}</option>
                <option>{{ $t("organization") }}</option>
              </select>
            </div>
            <input type="text" :placeholder="$t('comment')" class="input" />
            <input type="date" class="input" />
            <input type="date" class="input" />
            <input type="time" class="input" />
            <input type="time" class="input" />
          </div>
          <div class="form-select">
            <select class="select">
              <option>{{ $t("participants") }}</option>
              <option>{{ $t("participants") }}</option>
              <option>{{ $t("participants") }}</option>
            </select>
          </div>
        </div>
        <div v-if="topButtons[1].isActive">
          <div class="modal-form mb-10">
            <div class="form-select">
              <select class="select">
                <option>{{ $t("organization") }}</option>
                <option>{{ $t("organization") }}</option>
                <option>{{ $t("organization") }}</option>
              </select>
            </div>
            <input type="text" :placeholder="$t('comment')" class="input" />
            <input type="date" class="input" />
            <input type="date" class="input" />
            <input type="time" class="input" />
            <input type="time" class="input" />
          </div>
          <div class="form-select">
            <select class="select">
              <option>{{ $t("participants") }}</option>
              <option>{{ $t("participants") }}</option>
              <option>{{ $t("participants") }}</option>
            </select>
          </div>
        </div>
        <div v-else-if="topButtons[2].isActive">
          <div class="form-select mb-10">
            <select class="select">
              <option>{{ $t("organization") }}</option>
              <option>{{ $t("organization") }}</option>
              <option>{{ $t("organization") }}</option>
            </select>
          </div>
          <div class="modal-form mb-10">
            <input type="text" :placeholder="$t('lfm_doctor')" class="input" />
            <input
              type="text"
              :placeholder="$t('specialization')"
              class="input"
            />
            <input type="text" :placeholder="$t('telephone')" class="input" />
            <input type="text" :placeholder="$t('comment')" class="input" />
          </div>
          <div class="form-select mb-10">
            <select class="select">
              <option>{{ $t("talking_about_the_product") }}</option>
              <option>{{ $t("talking_about_the_product") }}</option>
              <option>{{ $t("talking_about_the_product") }}</option>
            </select>
          </div>
          <div class="modal-form mb-10">
            <input type="date" class="input" />
            <input type="date" class="input" />
            <input type="time" class="input" />
            <input type="time" class="input" />
          </div>
          <div class="form-select">
            <select class="select">
              <option>{{ $t("participants") }}</option>
              <option>{{ $t("participants") }}</option>
              <option>{{ $t("participants") }}</option>
            </select>
          </div>
        </div>
        <div v-else-if="topButtons[3].isActive">
          <input type="text" class="input mb-10" :placeholder="$t('comment')" />

          <div class="modal-form mb-10">
            <input type="date" class="input" />
            <input type="date" class="input" />
            <input type="time" class="input" />
            <input type="time" class="input" />
          </div>
          <div class="form-select">
            <select class="select">
              <option>{{ $t("participants") }}</option>
              <option>{{ $t("participants") }}</option>
              <option>{{ $t("participants") }}</option>
            </select>
          </div>
          <div class="modal-color modal-repeat">
            <div class="modal-color__title">{{ $t("repeat") }}</div>
            <input type="checkbox" />
            <input type="text" value="10" />
            <p>{{ $t("day") }}</p>
          </div>
        </div>
        <div class="modal-color">
          <div class="modal-color__title">{{ $t("event_color") }}</div>
          <span class="bg-green"></span><span class="bg-blue"></span
          ><span class="bg-grey"></span><span class="bg-yellow"></span
          ><span class="bg-red"></span>
        </div>
        <template #button>
          <button class="btn btn-red" @click="isAddOpen = false">
            {{ $t("to_close") }}
          </button>
          <button class="btn btn-blue" type="submit">{{ $t("add") }}</button>
        </template>
      </modal>
    </teleport>
  </div>
</template>

<script>
import FullCalendar from "@fullcalendar/vue";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";
import interactionPlugin from "@fullcalendar/interaction";
import listPlugin from "@fullcalendar/list";
import ruLocale from "@fullcalendar/core/locales/ru";
import enLocale from "@fullcalendar/core/locales/en-au";
import uzLocale from "@fullcalendar/core/locales/uz";
import Modal from "@/components/Modal.vue";
import Teleport from "vue2-teleport";
import TheFilter from "@/components/TheFilter.vue";
export default {
  components: { FullCalendar, Modal, Teleport, TheFilter },
  data() {
    return {
      topButtons: [
        { id: 0, name: "pharmacy", isActive: true },
        { id: 1, name: "wholesalers", isActive: false },
        { id: 2, name: "doctor", isActive: false },
        { id: 3, name: "task", isActive: false },
      ],
      calendarOptions: {
        plugins: [dayGridPlugin, timeGridPlugin, interactionPlugin, listPlugin],
        headerToolbar: {
          left: "prev,next today",
          center: "title",
          right: "dayGridMonth,timeGridWeek,timeGridDay,listWeek",
        },
        initialView: "dayGridMonth",
        editable: true,
        selectable: true,
        selectMirror: true,
        dayMaxEvents: true,
        weekends: true,
        select: this.handleDateSelect,
        eventClick: this.handleEventClick,
        eventsSet: this.handleEvents,
        locale: ruLocale,
        height: 650,
      },
      eventGuid: 0,
      title: "",
      isModalOpen: false,
      isAddOpen: false,
      filterItems: ["Азиза"],
    };
  },
  methods: {
    handleWeekendsToggle() {
      this.calendarOptions.weekends = !this.calendarOptions.weekends; // update a property
    },

    handleDateSelect(selectInfo) {
      let title = prompt(
        "Пожалуйста, введите новое название для вашего события"
      );
      let calendarApi = selectInfo.view.calendar;
      calendarApi.unselect();

      if (title) {
        calendarApi.addEvent({
          id: this.createEventId(),
          title,
          start: selectInfo.startStr,
          end: selectInfo.endStr,
          allDay: selectInfo.allDay,
        });
      }
    },

    handleEventClick(clickInfo) {
      if (
        confirm(
          `Вы уверены, что хотите удалить событие '${clickInfo.event.title}'`
        )
      ) {
        clickInfo.event.remove();
      }
    },

    handleEvents(events) {
      this.currentEvents = events;
    },
    createEventId() {
      return String(this.eventGuid++);
    },
    handleClick(idx) {
      this.topButtons.forEach((btn) => (btn.isActive = false));
      this.topButtons[idx].isActive = true;
    },
  },
};
</script>
