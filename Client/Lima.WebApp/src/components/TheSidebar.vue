<template>
  <div class="sidebar">
    <ul class="sidebar-menu">
      <li class="sidebar-menu__list">
        <router-link exact :to="{ name: 'Home' }" class="sidebar-menu__link">
          <i class="icofont-chart-histogram-alt"></i>
          <p>{{ $t("desktop") }}</p>
        </router-link>
      </li>

      <li
        class="sidebar-menu__list dropList"
        v-if="$grants.includes('SALES_VIEW')"
      >
        <div
          class="sidebar-menu__link dropdown"
          @click="$emit('clickHandler', 0)"
          :class="{
            activeDrop:
              dropdownList[0].item ||
              $route.fullPath === '/booking' ||
              $route.fullPath === '/shipping' ||
              $route.fullPath === '/remains',
          }"
        >
          <div class="sidebar-menu__content">
            <i class="icofont-id"></i>
            <p>
              {{ $t("sales") }}<i class="icofont-thin-down inner-array"></i>
            </p>
          </div>
          <i class="icofont-thin-down dropdown-array"></i>
        </div>
        <ul
          class="dropdown-menu"
          :class="{
            activeDrop:
              dropdownList[0].item ||
              $route.fullPath === '/booking' ||
              $route.fullPath === '/shipping' ||
              $route.fullPath === '/remains',
          }"
        >
          <li class="dropdown-menu__list">
            <router-link to="/booking" class="dropdown-menu__link">{{
              $t("booking")
            }}</router-link>
          </li>
          <li class="dropdown-menu__list">
            <router-link to="/shipping" class="dropdown-menu__link">{{
              $t("shipment")
            }}</router-link>
          </li>
          <li class="dropdown-menu__list">
            <router-link to="/remains" class="dropdown-menu__link">{{
              $t("remains")
            }}</router-link>
          </li>
        </ul>
      </li>
      <li
        class="sidebar-menu__list dropList"
        v-if="
          $grants.includes('VISITS_DOCTOR_VIEW') ||
          $grants.includes('VISITS_PHARMACY_DISTRIBUTORS_VIEW')
        "
      >
        <div
          class="sidebar-menu__link dropdown"
          @click="$emit('clickHandler', 1)"
          :class="{
            activeDrop:
              dropdownList[1].item ||
              $route.fullPath === '/visit-doctor' ||
              $route.fullPath === '/visit-pharmacy' ||
              $route.fullPath === '/visit-wholesaler',
          }"
        >
          <div class="sidebar-menu__content">
            <i class="icofont-location-pin"></i>
            <p>
              {{ $t("visits") }}<i class="icofont-thin-down inner-array"></i>
            </p>
          </div>
          <i class="icofont-thin-down dropdown-array"></i>
        </div>
        <ul
          class="dropdown-menu"
          :class="{
            activeDrop:
              dropdownList[1].item ||
              $route.fullPath === '/visit-doctor' ||
              $route.fullPath === '/visit-pharmacy' ||
              $route.fullPath === '/visit-wholesaler',
          }"
        >
          <li
            class="dropdown-menu__list"
            v-if="$grants.includes('VISITS_DOCTOR_VIEW')"
          >
            <router-link to="/visit-doctor" class="dropdown-menu__link">{{
              $t("to_the_doctors")
            }}</router-link>
          </li>
          <li
            class="dropdown-menu__list"
            v-if="$grants.includes('VISITS_PHARMACY_DISTRIBUTORS_VIEW')"
          >
            <router-link to="/visit-pharmacy" class="dropdown-menu__link">{{
              $t("to_the_pharmacy")
            }}</router-link>
          </li>
          <li
            class="dropdown-menu__list"
            v-if="$grants.includes('VISITS_PHARMACY_DISTRIBUTORS_VIEW')"
          >
            <router-link to="/visit-wholesaler" class="dropdown-menu__link">
              {{ $t("to_the_wholesaler") }}</router-link
            >
          </li>
        </ul>
      </li>
      <li class="sidebar-menu__list dropList">
        <div
          class="sidebar-menu__link dropdown"
          @click="$emit('clickHandler', 2)"
          :class="{
            activeDrop:
              dropdownList[2].item ||
              $route.path === '/list-doctors' ||
              $route.path === '/list-lpu' ||
              $route.path === '/list-pharmacy' ||
              $route.path === '/list-wholesalers' ||
              $route.path === '/list-goods' ||
              $route.path === '/list-series' ||
              $route.path === '/presentation',
          }"
        >
          <div class="sidebar-menu__content">
            <i class="icofont-chart-growth"></i>
            <p>
              {{ $t("references") }}
              <i class="icofont-thin-down inner-array"></i>
            </p>
          </div>
          <i class="icofont-thin-down dropdown-array"></i>
        </div>
        <ul
          class="dropdown-menu"
          :class="{
            activeDrop:
              dropdownList[2].item ||
              $route.path === '/list-doctors' ||
              $route.path === '/list-lpu' ||
              $route.path === '/list-pharmacy' ||
              $route.path === '/list-wholesalers' ||
              $route.path === '/list-goods' ||
              $route.path === '/list-series' ||
              $route.path === '/presentation',
          }"
        >
          <li
            class="dropdown-menu__list"
            v-if="$grants.includes('DOCTORS_VIEW')"
          >
            <router-link
              :to="{ name: 'list-doctors' }"
              class="dropdown-menu__link"
              >{{ $t("doctors") }}</router-link
            >
          </li>
          <li class="dropdown-menu__list" v-if="$grants.includes('LPU_VIEW')">
            <router-link
              class="dropdown-menu__link"
              :to="{ name: 'list-lpu', query: { page: 1 } }"
              >{{ $t("medical_institution") }}</router-link
            >
          </li>
          <li
            class="dropdown-menu__list"
            v-if="$grants.includes('PHARMACY_DISTRIBUTORS_VIEW')"
          >
            <router-link
              class="dropdown-menu__link"
              :to="{ name: 'list-pharmacy', query: { page: 1 } }"
              >{{ $t("pharmacies") }}</router-link
            >
          </li>

          <li
            class="dropdown-menu__list"
            v-if="$grants.includes('PHARMACY_DISTRIBUTORS_VIEW')"
          >
            <router-link
              class="dropdown-menu__link"
              :to="{ name: 'list-wholesalers', query: { page: 1 } }"
              >{{ $t("wholesalers") }}</router-link
            >
          </li>
          <li
            class="dropdown-menu__list"
            v-if="$grants.includes('PHARMACY_DISTRIBUTORS_VIEW')"
          >
            <router-link
              class="dropdown-menu__link"
              :to="{ name: 'list-series' }"
              >{{ $t("series") }}</router-link
            >
          </li>
          <li class="dropdown-menu__list" v-if="$grants.includes('DRUGS_VIEW')">
            <router-link
              class="dropdown-menu__link"
              :to="{ name: 'list-goods' }"
              >{{ $t("products") }}</router-link
            >
          </li>
          <li class="dropdown-menu__list">
            <router-link
              class="dropdown-menu__link"
              :to="{ name: 'presentation' }"
              >{{ $t("presentations") }}</router-link
            >
          </li>
        </ul>
      </li>
      <li class="sidebar-menu__list">
        <router-link
          :to="{ name: 'messages' }"
          class="sidebar-menu__link notif-wrapper"
        >
          <div class="sidebar-menu__content">
            <i class="icofont-ui-messaging"></i>
            <p>{{ $t("messages") }}</p>
          </div>

          <div class="notif">0</div>
        </router-link>
      </li>
      <li class="sidebar-menu__list">
        <router-link
          :to="{ name: 'notifications' }"
          class="sidebar-menu__link notif-wrapper"
        >
          <div class="sidebar-menu__content">
            <i class="icofont-notification"></i>
            <p>{{ $t("notifications") }}</p>
          </div>
          <div class="notif">0</div>
        </router-link>
      </li>
      <li class="sidebar-menu__list" v-if="$grants.includes('EVENTS_VIEW')">
        <router-link :to="{ name: 'calendar' }" class="sidebar-menu__link">
          <i class="icofont-calendar"></i>
          <p>{{ $t("calendar") }}</p>
        </router-link>
      </li>
      <li class="sidebar-menu__list" v-if="$grants.includes('MAP_VIEW')">
        <router-link :to="{ name: 'map' }" class="sidebar-menu__link">
          <i class="icofont-map"></i>
          <p>{{ $t("event_map") }}</p>
        </router-link>
      </li>
      <li class="sidebar-menu__list dropList">
        <div
          class="sidebar-menu__link dropdown"
          @click="$emit('clickHandler', 3)"
          :class="{
            activeDrop:
              dropdownList[3].item ||
              $route.fullPath === '/reports' ||
              $route.fullPath === '/reports-region' ||
              $route.fullPath === '/reports-branches' ||
              $route.fullPath === '/reports-staff',
          }"
        >
          <div class="sidebar-menu__content">
            <i class="icofont-settings"></i>
            <p>
              {{ $t("reports") }} <i class="icofont-thin-down inner-array"></i>
            </p>
          </div>
          <i class="icofont-thin-down dropdown-array"></i>
        </div>
        <ul
          class="dropdown-menu"
          :class="{
            activeDrop:
              dropdownList[3].item ||
              $route.fullPath === '/reports' ||
              $route.fullPath === '/reports-region' ||
              $route.fullPath === '/reports-branches' ||
              $route.fullPath === '/reports-staff',
          }"
        >
          <li class="dropdown-menu__list">
            <router-link to="/reports" class="dropdown-menu__link">
              {{ $t("general_reports") }}</router-link
            >
          </li>
          <li class="dropdown-menu__list">
            <router-link class="dropdown-menu__link" to="/reports-region">{{
              $t("reports_by_regions")
            }}</router-link>
          </li>
          <li class="dropdown-menu__list">
            <router-link class="dropdown-menu__link" to="/reports-branches">{{
              $t("reports_by_branches")
            }}</router-link>
          </li>
          <li class="dropdown-menu__list">
            <router-link class="dropdown-menu__link" to="/reports-staff">{{
              $t("by_employees")
            }}</router-link>
          </li>
        </ul>
      </li>

      <li class="sidebar-menu__list dropList">
        <div
          class="sidebar-menu__link dropdown"
          @click="$emit('clickHandler', 4)"
          :class="{ activeDrop: dropdownList[4].item }"
        >
          <div class="sidebar-menu__content">
            <i class="icofont-question"></i>
            <p>
              {{ $t("knowledge_base")
              }}<i class="icofont-thin-down inner-array"></i>
            </p>
          </div>
          <i class="icofont-thin-down dropdown-array"></i>
        </div>
        <ul
          class="dropdown-menu"
          :class="{
            activeDrop: dropdownList[4].item,
          }"
        >
          <li class="dropdown-menu__list">
            <a href="#" class="dropdown-menu__link">{{ $t("testing") }}</a>
          </li>
          <li class="dropdown-menu__list">
            <a class="dropdown-menu__link" href="#">{{
              $t("training_manuals")
            }}</a>
          </li>
        </ul>
      </li>
      <li
        class="sidebar-menu__list dropList"
        v-if="$grants.includes('SETTINGS_VIEW')"
      >
        <div
          class="sidebar-menu__link dropdown"
          @click="$emit('clickHandler', 5)"
          :class="{
            activeDrop:
              dropdownList[5].item ||
              $route.fullPath === '/settings' ||
              $route.fullPath === '/branches' ||
              $route.fullPath === '/departments' ||
              $route.fullPath === '/staff' ||
              $route.fullPath === '/roles-and-rules' ||
              $route.fullPath === '/prepayments-settings',
          }"
        >
          <div class="sidebar-menu__content">
            <i class="icofont-settings"></i>
            <p>
              {{ $t("settings") }}<i class="icofont-thin-down inner-array"></i>
            </p>
          </div>
          <i class="icofont-thin-down dropdown-array"></i>
        </div>
        <ul
          class="dropdown-menu"
          :class="{
            activeDrop:
              dropdownList[5].item ||
              $route.fullPath === '/settings' ||
              $route.fullPath === '/branches' ||
              $route.fullPath === '/departments' ||
              $route.fullPath === '/staff' ||
              $route.fullPath === '/roles-and-rules' ||
              $route.fullPath === '/prepayments-settings',
          }"
        >
          <li class="dropdown-menu__list">
            <router-link to="/settings" class="dropdown-menu__link">{{
              $t("main")
            }}</router-link>
          </li>
          <li class="dropdown-menu__list">
            <router-link to="/branches" class="dropdown-menu__link">{{
              $t("regions")
            }}</router-link>
          </li>
          <li class="dropdown-menu__list">
            <router-link to="/departments" class="dropdown-menu__link">{{
              $t("departments")
            }}</router-link>
          </li>
          <li class="dropdown-menu__list">
            <router-link :to="{ name: 'staff' }" class="dropdown-menu__link">{{
              $t("staff")
            }}</router-link>
          </li>
          <li class="dropdown-menu__list">
            <router-link
              class="dropdown-menu__link"
              :to="{ name: 'roles-and-rules' }"
              >{{ $t("roles_and_rights") }}</router-link
            >
          </li>
          <li class="dropdown-menu__list">
            <router-link
              to="/prepayments-settings"
              class="dropdown-menu__link"
              >{{ $t("prepayments_settings") }}</router-link
            >
          </li>
        </ul>
      </li>
    </ul>
    <div class="license" @click="isLicenseOpen = true">
      <p>{{ $t("license") }}</p>
    </div>
    <div class="license-icon" @click="isLicenseOpen = true">
      <img src="../assets/image/medal.png" alt="medal" />
    </div>

    <teleport to="body">
      <modal
        :isModalOpen="isLicenseOpen"
        @closeModal="closeModal"
        :class="{ 'license-open': isLicenseOpen }"
      >
        <template #top>
          <div v-if="requsetLicense">
            <div class="license-title">{{ $t("place_order") }}</div>
            <div class="modal-form license-content">
              <div class="license-content__item">{{ $t("extend_to") }}:</div>
              <div class="license-content__item">
                <div class="form-select">
                  <select class="select">
                    <option>{{ $t("duration") }}</option>
                    <option>{{ $t("duration") }}</option>
                    <option>{{ $t("duration") }}</option>
                  </select>
                </div>
              </div>
              <div class="license-content__item">
                {{ $t("number_of_employees") }}:
              </div>
              <div class="license-content__item">
                <div class="count">
                  <i
                    class="icofont-minus-circle"
                    style="color: #336cfb; cursor: pointer"
                    @click="count--"
                  ></i>
                  <input
                    v-model="count"
                    type="text"
                    class="quantity-input input"
                  />
                  <i
                    class="icofont-plus-circle"
                    style="color: #336cfb; cursor: pointer"
                    @click="count++"
                  ></i>
                </div>
              </div>
              <div class="license-content__item">{{ $t("analytics") }}:</div>
              <div class="license-content__item">
                <div class="analitics">
                  <button
                    v-for="(btn, index) in analiticsBtns"
                    :key="btn.id"
                    class="analitics-btn"
                    :class="btn.isActive ? btn.className : null"
                    @click="toggleButtons(index)"
                  >
                    {{ $t(btn.title) }}
                  </button>
                </div>
              </div>
            </div>
            <the-table :head="[$t('for_analytics'), $t('for_employees')]">
              <tr>
                <td v-for="b in licenseTableBody1" :key="b.id">
                  {{ prettify(b.price) }}
                </td>
              </tr>
            </the-table>
            <the-table :head="[$t('amount_to_be_paid')]">
              <tr>
                <td>
                  {{ prettify(2500000) }}
                </td>
              </tr>
            </the-table>
          </div>
          <div v-else>
            <div class="license-title">{{ $t("license") }}</div>
            <div class="license-content">
              <div class="license-content__item">{{ $t("company") }}:</div>
              <div class="license-content__item">
                {{ licenseInfo.company_name }}
              </div>
              <div class="license-content__item">{{ $t("intr") }}:</div>
              <div class="license-content__item">{{ licenseInfo.inn }}</div>
              <div class="license-content__item">{{ $t("license") }}:</div>
              <div class="license-content__item">
                {{ licenseInfo.license_code || $t("empty") }}
              </div>
              <div class="license-content__item">
                {{ $t("number_of_employees") }}:
              </div>
              <div class="license-content__item">
                {{ licenseInfo.employee_count }}
              </div>
              <div class="license-content__item">{{ $t("analytics") }}:</div>
              <div class="license-content__item">{{ $t("empty") }}</div>
              <div class="license-content__item">{{ $t("date_of_end") }}:</div>
              <div class="license-content__item">
                {{
                  new Date(licenseInfo.license_date).toLocaleDateString("ru-RU")
                }}
              </div>
            </div>
          </div>

          <div class="modal-buttons">
            <button class="btn btn-blue" @click="requestHandle">
              {{ $t("place_order") }}</button
            ><button @click="closeModal" class="btn btn-red">
              {{ $t("to_close") }}
            </button>
          </div>
        </template>
      </modal>
    </teleport>
  </div>
</template>

<script>
import Teleport from "vue2-teleport";
import Modal from "../components/Modal.vue";
import { prettify } from "../use/PrettifySum";
import TheTable from "../components/TheTable.vue";
import { mapGetters, mapActions } from "vuex";
export default {
  components: { Teleport, Modal, TheTable },
  props: ["dropdownList"],
  emits: ["clickHandler"],
  data() {
    return {
      prettify,
      isLicenseOpen: false,
      requsetLicense: false,
      count: 0,
      analiticsBtns: [
        {
          id: 0,
          title: "turn_on",
          className: "analitics-btn__blue",
          isActive: true,
        },
        {
          id: 1,
          title: "turn_off",
          className: "analitics-btn__red",
          isActive: false,
        },
      ],
      licenseTableHead1: ["for_analytics", "for_employees"],
      licenseTableBody1: [
        { id: 0, price: 2000000 },
        { id: 1, price: 500000 },
      ],
    };
  },
  computed: {
    ...mapGetters("license", ["licenseInfo"]),
  },
  methods: {
    ...mapActions("license", ["getLicenseInfo"]),
    requestHandle() {
      if (!this.requsetLicense) {
        this.requsetLicense = true;
      }
    },
    closeModal() {
      this.isLicenseOpen = false;
      this.requsetLicense = false;
    },
    toggleButtons(idx) {
      this.analiticsBtns.forEach((btn) => (btn.isActive = false));
      this.analiticsBtns[idx].isActive = true;
    },
  },
  async mounted() {
    await this.getLicenseInfo();
  },
};
</script>

<style lang="scss">
.license-open {
  .modal {
    z-index: 120;
  }
  .overlay {
    z-index: 100;
  }
}
</style>
