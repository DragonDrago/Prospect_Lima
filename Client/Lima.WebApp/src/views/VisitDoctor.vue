<template>
  <div class="sales page">
    <the-filter
      :filterItems="filterItems"
      @filterClicked="s.isFilterOpen = true"
      @filterAdd="s.isAddOpen = true"
      @removeItem="removeFilterItem"
    >
      <template #name>{{ $t("visit_doctors") }}</template>
    </the-filter>
    <div class="statistics-menu statistic-visit">
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">{{ $t("total_spent") }}</div>
          <div class="statistics-menu__num">
            {{ visitDoctorStatistics.compleated || 0 }}
          </div>
        </div>
      </div>
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">{{ $t("total_planned") }}</div>
          <div class="statistics-menu__num">
            {{ visitDoctorStatistics.planned || 0 }}
          </div>
        </div>
      </div>
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">{{ $t("total_overdue") }}</div>
          <div class="statistics-menu__num">
            {{ visitDoctorStatistics.overdue || 0 }}
          </div>
        </div>
      </div>
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">{{ $t("more_talk_about") }}</div>
          <div class="statistics-menu__num">
            {{ visitDoctorStatistics.most_tell || $t("empty") }}
          </div>
        </div>
      </div>
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">{{ $t("less_talk_about") }}</div>
          <div class="statistics-menu__num">
            {{ visitDoctorStatistics.talk_less || $t("empty") }}
          </div>
        </div>
      </div>
    </div>
    <spinner v-if="isLoading" />
    <div v-else>
      <the-table
        v-if="visitDoctors.length"
        :head="[
          $t('employee'),
          $t('date'),
          $t('time'),
          $t('organization'),
          $t('doctor'),
          $t('post'),
          $t('telephone'),
          $t('comment'),
          $t('product'),
          $t('status'),
        ]"
        class="visit-doctor-table"
      >
        <tr v-for="b in visitDoctors" :key="b.visit_id">
          <td>
            <table-user :name="b.medrep.medrep_full_name" img="no-user.jpg" />
          </td>
          <td>{{ new Date(b.date_create).toLocaleDateString("ru-RU") }}</td>
          <td>{{ new Date(b.date_create).toLocaleTimeString("ru-RU") }}</td>
          <td>{{ b.organization.organization_name }}</td>
          <td>{{ b.doctor.doctor_name || $t("empty") }}</td>
          <td>{{ b.doctor.doctor_position || $t("empty") }}</td>
          <td>{{ b.doctor.doctor_phone || $t("empty") }}</td>
          <td>
            <i class="icofont-eye font-eye" @click="setComment(b.comment)"></i>
          </td>
          <td>
            <i
              class="icofont-eye font-eye"
              @click="setItems(b.talked_about_drugs)"
            ></i>
          </td>
          <td :class="sortStatusClass(b.visit_status_id)">
            {{ $t(sortStatus(b.visit_status_id)) }}
          </td>
          <td>
            <table-buttons
              v-if="b.visit_status_id !== 3"
              :isLocation="true"
              @locationClicked="(openLocation = true), (visit_id = b.visit_id)"
              @edit="openEdit(b)"
            />
          </td>
        </tr>
      </the-table>
      <empty v-else v-show="isEmpty" :item="visitDoctors" />
    </div>

    <teleport to="body">
      <!-- Filter visit modal -->
      <modal
        @closeModal="(s.isFilterOpen = false), clearValues()"
        :isModalOpen="s.isFilterOpen"
        @submitHandler="submitFilterHandler"
      >
        <template #title>{{ $t("filter_visits_to_doctors") }}</template>

        <div class="modal-form">
          <div class="form-select">
            <select class="select" @change="changeHandler" name="medrep_id">
              <option selected disabled>{{ $t("employee") }}</option>
              <option v-for="user in users" :key="user.id" :value="user.id">
                {{ user.full_name }}
              </option>
            </select>
          </div>
          <div class="input-wrapper input-group">
            <input
              type="text"
              class="input"
              v-model="inputLpuValue"
              @input="searchLpuHandler"
              placeholder=" "
              autocomplete="off"
              id="name"
            />

            <label for="name" class="input-label">{{
              $t("organization")
            }}</label>
            <ul
              class="auto-suggest"
              v-if="inputLpuValue.length && searchedLpus.length > 0"
            >
              <li
                v-for="item in searchedLpus"
                :key="item.id"
                @click="clickLpuHandler(item)"
              >
                {{ item.name }}
              </li>
            </ul>
          </div>
          <div class="input-wrapper input-group">
            <input
              type="text"
              class="input"
              v-model="inputDoctorValue"
              @input="searchDoctorsHandler"
              ref="doctor_name"
              placeholder=" "
              autocomplete="off"
              id="doctor"
            />

            <label for="doctor" class="input-label">{{ $t("doctor") }}</label>
            <ul
              class="auto-suggest"
              v-if="inputDoctorValue.length && searchedDoctors.length > 0"
            >
              <li
                v-for="doctor in searchedDoctors"
                :key="doctor.id"
                @click="clickDoctorHandler(doctor)"
              >
                {{ doctor.full_name }}
              </li>
            </ul>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              name="phone"
              id="phone"
              @input="changeHandler"
            />
            <label for="phone" class="input-label">{{ $t("telephone") }}</label>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              name="doctor_position"
              id="doctor_position"
              @input="changeHandler"
            />
            <label for="doctor_position" class="input-label">{{
              $t("position")
            }}</label>
          </div>
          <div class="form-select">
            <select class="select" @change="changeHandler" name="status_id">
              <option disabled selected>{{ $t("status") }}</option>
              <option :value="1">{{ $t(sortStatus(1)) }}</option>
              <option :value="2">{{ $t(sortStatus(2)) }}</option>
              <option :value="3">{{ $t(sortStatus(3)) }}</option>
            </select>
          </div>
          <div class="input-group">
            <input
              type="date"
              placeholder=" "
              class="input"
              name="dates_min"
              id="dates_min"
              @input="changeHandler"
            />
            <label for="dates_min" class="input-label">{{
              $t("start_date")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="date"
              placeholder=" "
              class="input"
              name="dates_max"
              id="dates_max"
              @input="changeHandler"
            />
            <label for="dates_max" class="input-label">{{
              $t("date_of_end")
            }}</label>
          </div>
        </div>
        <template #button>
          <button
            class="btn btn-red"
            @click.prevent="(s.isFilterOpen = false), clearValues()"
          >
            {{ $t("to_close") }}
          </button>
          <button class="btn btn-blue" type="submit">{{ $t("add") }}</button>
        </template>
      </modal>
      <!-- Add new visit modal -->
      <modal
        @closeModal="(s.isAddOpen = false), clearValues()"
        :isModalOpen="s.isAddOpen"
        @submitHandler="submitAddHandler"
      >
        <template #title>{{ $t("add_a_doctors_visit") }}</template>
        <div class="modal-form mb-10">
          <div class="input-wrapper input-group">
            <input
              type="text"
              class="input"
              v-model="inputLpuValue"
              @input="searchLpuHandler"
              placeholder=" "
              autocomplete="off"
              required
              id="name"
            />

            <label for="name" class="input-label">{{
              $t("organization")
            }}</label>
            <ul
              class="auto-suggest"
              v-if="inputLpuValue.length && searchedLpus.length > 0"
            >
              <li
                v-for="item in searchedLpus"
                :key="item.id"
                @click="clickLpuHandler(item)"
              >
                {{ item.name }}
              </li>
            </ul>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              name="present"
              id="present"
            />
            <label for="present" class="input-label">{{ $t("gift") }}</label>
          </div>
          <div class="input-wrapper input-group">
            <div v-if="!toggleDoctorMethod">
              <input
                type="text"
                class="input"
                v-model="inputDoctorValue"
                @input="searchDoctorsHandler"
                ref="doctor_name"
                placeholder=" "
                autocomplete="off"
                required
                id="doctor"
              />

              <label for="doctor" class="input-label">{{
                $t("doctor_search")
              }}</label>
              <ul
                class="auto-suggest"
                v-if="inputDoctorValue.length && searchedDoctors.length > 0"
              >
                <li
                  v-for="doctor in searchedDoctors"
                  :key="doctor.id"
                  @click="clickDoctorHandler(doctor)"
                >
                  {{ doctor.full_name }}
                </li>
              </ul>
            </div>
            <div v-else>
              <input
                type="text"
                placeholder=" "
                class="input"
                name="doctor_name"
                id="doctor_name"
                v-model="inputDoctorValue"
                ref="doctor_name"
                required
              />
              <label for="doctor_name" class="input-label">{{
                $t("add_a_doctor")
              }}</label>
            </div>
            <span
              class="input-element"
              @click="toggleDoctorMethod = !toggleDoctorMethod"
              >+</span
            >
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              id="doctor_position"
              v-model="doctor.position"
              ref="doctor_position"
            />
            <label for="doctor_position" class="input-label">{{
              $t("specialization")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              id="doctor_phone"
              v-model="doctor.phone"
              ref="doctor_phone"
              required
            />
            <label for="doctor_phone" class="input-label">{{
              $t("telephone")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              id="comment"
              v-model="doctor.comment"
              ref="comment"
            />
            <label for="comment" class="input-label">{{ $t("comment") }}</label>
          </div>
        </div>
        <div class="input-wrapper input-group">
          <input
            type="text"
            class="input"
            v-model="inputDrugValue"
            @input="searchDrugHandler"
            placeholder=" "
            autocomplete="off"
            id="drugs"
          />

          <label for="drugs" class="input-label">{{
            $t("talking_about_the_product")
          }}</label>
          <ul
            class="auto-suggest"
            v-if="inputDrugValue.length && searchedDrugs.length > 0"
          >
            <li
              v-for="item in searchedDrugs"
              :key="item.id"
              @click="clickDrugHandler(item)"
            >
              {{ item.drug_name }}
            </li>
          </ul>
        </div>
        <ul v-if="drugItems.length" class="drug_list">
          <li v-for="item in drugItems" :key="item.id">
            <p>{{ item.drug_name }}</p>
            <span @click="deleteDrug(item)">&times;</span>
          </li>
        </ul>
        <template #button>
          <button
            class="btn btn-red"
            @click.prevent="(s.isAddOpen = false), clearValues()"
          >
            {{ $t("to_close") }}
          </button>
          <button class="btn btn-blue" type="submit">{{ $t("add") }}</button>
        </template>
      </modal>
      <!-- Edit visit modal -->
      <modal
        @closeModal="(s.isEditOpen = false), clearValues()"
        :isModalOpen="s.isEditOpen"
        @submitHandler="submitEditHandler"
      >
        <template #title>{{ $t("edit_visit_doctor") }}</template>
        <div class="modal-form mb-10">
          <div class="input-wrapper input-group">
            <input
              type="text"
              class="input"
              v-model="inputLpuValue"
              @input="searchLpuHandler"
              placeholder=" "
              autocomplete="off"
              required
              id="name"
            />

            <label for="name" class="input-label">{{
              $t("organization")
            }}</label>
            <ul
              class="auto-suggest"
              v-if="inputLpuValue.length && searchedLpus.length > 0"
            >
              <li
                v-for="item in searchedLpus"
                :key="item.id"
                @click="clickLpuHandler(item)"
              >
                {{ item.name }}
              </li>
            </ul>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              name="present"
              id="present"
            />
            <label for="present" class="input-label">{{ $t("gift") }}</label>
          </div>
          <div class="input-wrapper input-group">
            <div v-if="!toggleDoctorMethod">
              <input
                type="text"
                class="input"
                v-model="inputDoctorValue"
                @input="searchDoctorsHandler"
                ref="doctor_name"
                placeholder=" "
                autocomplete="off"
                required
                id="doctor"
              />

              <label for="doctor" class="input-label">{{
                $t("doctor_search")
              }}</label>
              <ul
                class="auto-suggest"
                v-if="inputDoctorValue.length && searchedDoctors.length > 0"
              >
                <li
                  v-for="doctor in searchedDoctors"
                  :key="doctor.id"
                  @click="clickDoctorHandler(doctor)"
                >
                  {{ doctor.full_name }}
                </li>
              </ul>
            </div>
            <div v-else>
              <input
                type="text"
                placeholder=" "
                class="input"
                name="doctor_name"
                id="doctor_name"
                v-model="inputDoctorValue"
                ref="doctor_name"
                required
              />
              <label for="doctor_name" class="input-label">{{
                $t("add_a_doctor")
              }}</label>
            </div>
            <span
              class="input-element"
              @click="toggleDoctorMethod = !toggleDoctorMethod"
              >+</span
            >
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              id="doctor_position"
              v-model="doctor.position"
              ref="doctor_position"
            />
            <label for="doctor_position" class="input-label">{{
              $t("specialization")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              id="doctor_phone"
              v-model="doctor.phone"
              ref="doctor_phone"
              required
            />
            <label for="doctor_phone" class="input-label">{{
              $t("telephone")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              id="comment"
              v-model="doctor.comment"
              ref="comment"
            />
            <label for="comment" class="input-label">{{ $t("comment") }}</label>
          </div>
        </div>
        <div class="input-wrapper input-group">
          <input
            type="text"
            class="input"
            v-model="inputDrugValue"
            @input="searchDrugHandler"
            placeholder=" "
            autocomplete="off"
            id="drugs"
          />

          <label for="drugs" class="input-label">{{
            $t("talking_about_the_product")
          }}</label>
          <ul
            class="auto-suggest"
            v-if="inputDrugValue.length && searchedDrugs.length > 0"
          >
            <li
              v-for="item in searchedDrugs"
              :key="item.id"
              @click="clickDrugHandler(item)"
            >
              {{ item.drug_name }}
            </li>
          </ul>
        </div>
        <ul v-if="drugItems.length" class="drug_list">
          <li v-for="drug in drugItems" :key="drug.id">
            <p>{{ drug.full_name || drug.drug_name }}</p>
            <span @click="deleteDrug(drug)">&times;</span>
          </li>
        </ul>
        <template #button>
          <button
            class="btn btn-red"
            @click.prevent="(s.isEditOpen = false), clearValues()"
          >
            {{ $t("to_close") }}
          </button>
          <button class="btn btn-blue" type="submit">{{ $t("add") }}</button>
        </template>
      </modal>
      <!-- Modal for confirm whether send current location or not -->
      <modal-geolocation
        @getCoordinates="getCoordinates"
        :openLocation="openLocation"
        @makeFalse="(openLocation = false), (visit_id = {})"
        title="is_visit_done"
      />
      <!-- Comment modal -->
      <modal-small
        :isModalOpen="isCommentOpen"
        @closeModal="(isCommentOpen = false), (modalProps = [])"
      >
        <template #title>{{ $t("comment") }}</template>
        <template #top>
          <ul>
            <li>
              {{ modalProps || $t("empty") }}
            </li>
          </ul>
        </template>
      </modal-small>
      <!-- Drugs list modal -->
      <modal-small
        :isModalOpen="isItemsOpen"
        @closeModal="(isItemsOpen = false), (modalProps = [])"
      >
        <template #title>{{ $t("talking_about_the_product") }}</template>
        <template #top>
          <ul>
            <template v-if="modalProps.length">
              <li v-for="item in modalProps" :key="item.drug_id">
                {{ item.drug_name }}
              </li>
            </template>
            <li v-else>{{ $t("empty") }}</li>
          </ul>
        </template>
      </modal-small>
    </teleport>
  </div>
</template>

<script>
import TheTable from "@/components/TheTable.vue";
import TableButtons from "@/components/TableButtons.vue";
import TheFilter from "@/components/TheFilter.vue";
import Modal from "@/components/Modal.vue";
import Teleport from "vue2-teleport";
import TableUser from "@/components/TableUser.vue";
import { sortStatus, sortStatusClass } from "../use/SortStatus";
import { prettify } from "../use/PrettifySum";
import { mapGetters, mapActions } from "vuex";
import ModalGeolocation from "@/components/ModalGeolocation.vue";
import Spinner from "@/components/Spinner.vue";
import ModalSmall from "@/components/ModalSmall.vue";
import Empty from "@/components/Empty.vue";

export default {
  components: {
    TheTable,
    TableButtons,
    TheFilter,
    Modal,
    Teleport,
    TableUser,
    ModalGeolocation,
    Spinner,
    ModalSmall,
    Empty,
  },
  data: () => ({
    sortStatus,
    sortStatusClass,
    prettify,
    filterItems: [],
    openLocation: false,
    visit_id: null,
    inputLpuValue: "",
    inputDrugValue: "",
    lpuItem: {},
    drugItems: [],
    visitDoctor: {},
    drugIds: [],
    isCommentOpen: false,
    isItemsOpen: false,
    ids: [],
    modalProps: null,
    isListOpen: false,
    inputDoctorValue: "",
    doctor: {},
    toggleDoctorMethod: false,
    drugItem: {},
  }),
  computed: {
    ...mapGetters("visitDoctors", ["visitDoctors", "isLoading"]),
    ...mapGetters("lpus", ["searchedLpus"]),
    ...mapGetters("drugs", ["searchedDrugs"]),
    ...mapGetters("visitStatistics", ["visitDoctorStatistics"]),
    ...mapGetters("doctors", ["searchedDoctors"]),
    ...mapGetters("users", ["users"]),
    ...mapGetters(["s"]),
  },
  methods: {
    ...mapActions("visitDoctors", [
      "getVisitDoctors",
      "addVisitDoctor",
      "completeVisit",
      "updateVisitDoctor",
      "filterVisitDoctors",
    ]),
    ...mapActions("lpus", ["findLpuByName"]),
    ...mapActions("drugs", ["getDrugsByName"]),
    ...mapActions("visitStatistics", ["getVisitDoctorStatistics"]),
    ...mapActions("doctors", ["findDoctorByName"]),
    ...mapActions("users", ["getUsers"]),
    // Gets coordinates to complete visit
    async getCoordinates(coordinates) {
      this.openLocation = false;
      await this.completeVisit({ visit_id: this.visit_id, ...coordinates });
      await this.getVisitDoctors();
      this.visit_id = null;
    },
    // Searches for Lpus by name
    searchLpuHandler(e) {
      this.lpuItem = {};
      this.findLpuByName(e.target.value);
    },
    // Searches for doctors by name
    searchDoctorsHandler(e) {
      this.doctor = {};
      this.findDoctorByName(e.target.value);
    },
    // Sets single Lpu to variable and sets lpu name to input lpu value. At the same time clears the found lpus array after searching
    clickLpuHandler(item) {
      this.inputLpuValue = item.name;
      this.$store.commit("lpus/setSearchedLpus", []);
      this.lpuItem = item;
    },
    // Sets single doctor to variable and sets doctor name to input doctor value. At the same time clears the found doctors array after searching
    clickDoctorHandler(doctor) {
      this.inputDoctorValue = doctor.full_name;
      this.$store.commit("doctors/setSearchedDoctors", []);
      this.doctor = doctor;
    },
    // Searches for drugs by name
    searchDrugHandler(e) {
      this.getDrugsByName(e.target.value);
    },
    // Collects searched and selected drug items to show user. First clears the input value and searched drug array from store. Also, checks if is there any similar drug or not. If is there any similar drug with the same id this function doesn't add to array. Finally, collects drug ids to send backend
    clickDrugHandler(item) {
      this.inputDrugValue = "";
      this.$store.commit("drugs/setSearchedDrugs", []);
      const found = this.drugItems.find((fitem) => fitem.id === item.id);
      if (!found) this.drugItems = [...this.drugItems, item];
      this.drugIds = [...this.drugIds, item.id];
    },
    // Gets the selected drug items to change
    clickEditDrugHandler(item) {
      this.inputDrugValue = "";
      this.$store.commit("drugs/setSearchedDrugs", []);
      const res = {
        id: item.id,
        full_name: item.drug_name,
      };
      const found = this.drugItems.find((fitem) => fitem.id === res.id);
      if (!found) this.drugItems = [...this.drugItems, res];
      this.drugIds = [...this.drugIds, res.id];
    },
    // Deletes selected drug
    deleteDrug(item) {
      const drugId = item.id || item.drug_id;
      this.drugItems = this.drugItems.filter((di) => di.drug_id !== drugId);
      this.ids = this.ids.filter((di) => di.drug_id !== drugId);
    },
    // Submits to add new visit
    submitAddHandler() {
      this.s.isAddOpen = false;
      this.addVisitDoctor({
        doctor_name: this.$refs.doctor_name.value,
        doctor_position: this.$refs.doctor_position.value,
        doctor_phone: this.$refs.doctor_phone.value,
        comment: this.$refs.comment.value,
        organization_id: this.lpuItem.id,
        talked_about_the_drugs_ids: [...new Set(this.drugIds)],
      });
      this.clearValues();
    },
    // Clears all values
    clearValues() {
      this.toggleDoctorMethod = false;
      this.inputDoctorValue = "";
      this.inputLpuValue = "";
      this.inputDrugValue = "";
      this.visitDoctor = {};
      this.doctor = {};
      this.lpuItem = {};
      this.drugItems = [];
      this.drugItem = {};
      this.$store.commit("lpus/setSearchedLpus", []);
      this.$store.commit("doctors/setSearchedDoctors", []);
      this.$store.commit("drugs/setSearchedDrugs", []);
    },
    // Opens comment modal and sets its value
    setComment(value) {
      this.isCommentOpen = true;
      this.modalProps = value;
    },
    // Opens drug list modal and sets its value
    setItems(value) {
      this.isItemsOpen = true;
      this.modalProps = value;
    },
    // Opens edit modal and sets selected or given value to modal
    openEdit(visitDoctor) {
      this.s.isEditOpen = true;
      this.inputDoctorValue = visitDoctor.doctor.doctor_name;
      this.inputLpuValue = visitDoctor.organization.organization_name;
      this.doctor = {
        comment: visitDoctor.comment,
        name: visitDoctor.doctor.doctor_name,
        position: visitDoctor.doctor.doctor_position,
        phone: visitDoctor.doctor.doctor_phone,
      };
      this.lpuItem = visitDoctor.organization;
      this.visitDoctor = visitDoctor;
      this.drugItems = this.visitDoctor.talked_about_drugs;
    },
    // Sends edited visit to backend
    submitEditHandler() {
      this.s.isEditOpen = false;
      this.ids = this.drugItems.map((item) => item.id || item.drug_id);
      const payload = {
        id: this.visitDoctor.visit_id,
        organization_id:
          this.lpuItem.id || this.visitDoctor.organization.organization_id,
        talked_about_the_drugs_ids: this.ids,
        doctor_name: this.visitDoctor.doctor.doctor_name,
        doctor_position: this.visitDoctor.doctor.doctor_position,
        doctor_phone: this.visitDoctor.doctor.doctor_phone,
        comment: this.visitDoctor.comment,
      };
      this.updateVisitDoctor(payload);
      this.clearValues();
    },
    changeHandler(e) {
      const { value, name } = e.target;
      this.visitDoctor = { ...this.visitDoctor, [name]: value };
    },
    async submitFilterHandler() {
      this.s.isFilterOpen = false;
      let payload = {};
      let filterPayload = {};
      if (this.visitDoctor.medrep_id) {
        payload = {
          ...payload,
          medrep_id: this.visitDoctor.medrep_id,
        };
        filterPayload = {
          ...filterPayload,
          medrep_id: this.visitDoctor.medrep_id,
        };
      }
      if (this.visitDoctor.dates_min || this.visitDoctor.dates_max) {
        payload = {
          ...payload,
          dates: [this.visitDoctor.dates_min, this.visitDoctor.dates_max],
        };
        filterPayload = {
          ...filterPayload,
          dates_min: this.visitDoctor.dates_min,
          dates_max: this.visitDoctor.dates_max,
        };
      }
      if (this.visitDoctor.status_id) {
        payload = {
          ...payload,
          status_id: this.visitDoctor.status_id,
        };
        filterPayload = {
          ...filterPayload,
          status_id: this.$t(sortStatus(+this.visitDoctor.status_id)),
        };
      }
      if (this.visitDoctor.phone) {
        payload = {
          ...payload,
          phone: this.visitDoctor.phone,
        };
        filterPayload = {
          ...filterPayload,
          phone: this.visitDoctor.phone,
        };
      }
      if (this.doctor.id || this.doctor.full_name) {
        payload = {
          ...payload,
          doctor_id: this.doctor.id,
        };
        filterPayload = {
          ...filterPayload,
          doctor_id: this.doctor.full_name,
        };
      }
      if (this.doctor.doctor_position) {
        payload = {
          ...payload,
          doctor_position: this.doctor.position,
        };
        filterPayload = {
          ...filterPayload,
          doctor_position: this.doctor.position,
        };
      }
      if (this.lpuItem.id || this.lpuItem.name) {
        payload = {
          ...payload,
          organization_id: this.lpuItem.id,
        };
        filterPayload = {
          ...filterPayload,
          organization_id: this.lpuItem.name,
        };
      }
      this.filterItems = Object.values(filterPayload);
      await this.filterVisitDoctors(payload);
      this.clearValues();
    },
    async removeFilterItem(item) {
      this.filterItems = this.filterItems.filter((d) => d !== item);
      let payload = {
        ...this.visitDoctor,
      };
      for (let key in payload) {
        if (payload[key].includes(item)) {
          delete payload[key];
        }
      }
      await this.filterVisitDoctors(payload);
    },
  },
  async mounted() {
    // Fetchs all visit doctor statistics
    await this.getVisitDoctorStatistics();
    // Fetchs all visit doctors
    await this.getVisitDoctors();
    await this.getUsers();
  },
};
</script>
