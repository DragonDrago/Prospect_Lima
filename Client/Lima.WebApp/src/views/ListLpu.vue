<template>
  <div class="sales page">
    <the-filter
      :filterItems="filterItems"
      @filterClicked="s.isFilterOpen = true"
      @filterAdd="(s.isAddOpen = true), initialMap()"
      @removeItem="removeFilterItem"
    >
      <template #name>{{ $t("list_medical_institution") }}</template>
    </the-filter>
    <paginate
      v-if="pages.total_pages"
      :page-count="pages.total_pages"
      :click-handler="pageChangeHandler"
      prev-text="<i class='icofont-simple-left'></i>"
      next-text="<i class='icofont-simple-right'></i>"
      container-class="pagination"
      v-model="currentPage"
    />
    <spinner v-if="isLoading" />
    <div v-else>
      <the-table
        :head="[
          $t('title_p'),
          $t('type'),
          $t('address'),
          $t('form_organization'),
          $t('intr'),
        ]"
      >
        <tr v-for="b in lpus || 10" :key="b.id">
          <td>{{ b.name }}</td>
          <td>
            {{
              b.type_id === 1
                ? $t("pharmacy")
                : b.type_id === 2
                ? $t("medical_institution")
                : $t("wholesaler")
            }}
          </td>
          <td>{{ b.address }}</td>
          <td>
            {{
              b.health_care_facility_type_id === 1
                ? $t("private")
                : b.health_care_facility_type_id === 2
                ? $t("state")
                : $t("empty")
            }}
          </td>
          <td>{{ b.inn || $t("empty") }}</td>
          <td><table-buttons @edit="editHandler(b), initialMap()" /></td>
        </tr>
      </the-table>
      <empty v-show="isEmpty" v-if="!lpus.length" :item="lpus" />
    </div>
    <!-- Add Lpu -->
    <modal
      @closeModal="(s.isAddOpen = false), clearValue()"
      :isModalOpen="s.isAddOpen"
      @submitHandler="submitAddHandler"
    >
      <template #title>{{ $t("add_lpu") }}</template>
      <div class="modal-form mb-10">
        <div class="input-group">
          <input
            type="text"
            placeholder=" "
            class="input"
            name="name"
            @input="changeHandler"
            id="name"
          />
          <label for="name" class="input-label">{{ $t("name_lpu") }}</label>
        </div>
        <div class="input-group">
          <input
            type="text"
            placeholder=" "
            class="input"
            name="inn"
            @input="changeHandler"
            id="inn"
          />
          <label for="inn" class="input-label">{{ $t("intr") }}</label>
        </div>
      </div>

      <div class="modal-form mb-10">
        <div class="form-select">
          <select class="select" name="type_id">
            <option value="" disabled selected>
              {{ $t("choose_the_type") }}
            </option>
            <option :value="1">{{ $t("pharmacy") }}</option>
            <option :value="2">{{ $t("medical_institution") }}</option>
            <option :value="3">{{ $t("wholesaler") }}</option>
          </select>
        </div>
        <div class="form-select">
          <select class="select" @change="selectBranch($event)">
            <option value="" :selected="selectedBranch === null" disabled>
              {{ $t("choose_a_region") }}
            </option>
            <option
              v-for="branch in branches"
              :value="branch.id"
              :key="branch.id"
            >
              {{ branch.name }}
            </option>
          </select>
        </div>
      </div>
      <div class="input-wrapper input-group">
        <input
          type="text"
          class="input"
          v-model="inputDoctorValue"
          @input="searchDoctorHandler"
          placeholder=" "
          autocomplete="off"
          id="drugs"
        />

        <label for="drugs" class="input-label">{{
          $t("choose_a_doctor")
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
      <ul
        v-if="searchedDoctors.length"
        class="drug_list"
        style="margin: 5px 0 10px"
      >
        <li v-for="doctor in selectedDoctors" :key="doctor.id">
          <p>{{ doctor.full_name }}</p>
          <span @click="deleteDoctor(doctor.id)">&times;</span>
        </li>
      </ul>

      <!-- Search Input -->
      <input
        type="search"
        class="controls"
        id="searchInput"
        :placeholder="$t('choose_place')"
        ref="inputValue"
      />
      <!-- Google Map -->
      <div id="map"></div>

      <p class="modal-bottom" id="place"></p>

      <template #button>
        <button
          class="btn btn-red"
          @click.prevent="(s.isAddOpen = false), clearValue()"
        >
          {{ $t("to_close") }}
        </button>
        <button class="btn btn-blue" type="submit">{{ $t("add") }}</button>
      </template>
    </modal>
    <modal
      @closeModal="(s.isEditOpen = false), clearValue()"
      :isModalOpen="true"
      v-if="s.isEditOpen"
      @submitHandler="submitEditHandler"
    >
      <template #title>{{ $t("add_lpu") }}</template>
      <div class="modal-form mb-10">
        <div class="input-group">
          <input
            type="text"
            placeholder=" "
            class="input"
            name="name"
            v-model="lpu.name"
            @input="changeHandler"
            id="name"
          />
          <label for="name" class="input-label">{{ $t("name_lpu") }}</label>
        </div>
        <div class="input-group">
          <input
            type="text"
            placeholder=" "
            class="input"
            name="inn"
            v-model="lpu.inn"
            @input="changeHandler"
            id="inn"
          />
          <label for="inn" class="input-label">{{ $t("intr") }}</label>
        </div>
      </div>

      <div class="modal-form mb-10">
        <div class="form-select">
          <select class="select" name="type_id">
            <option value="" disabled :selected="lpu.type_id === null">
              {{ $t("choose_the_type") }}
            </option>
            <option :selected="lpu.type_id === 1" :value="1">
              {{ $t("pharmacy") }}
            </option>
            <option :selected="lpu.type_id === 2" :value="2">
              {{ $t("medical_institution") }}
            </option>
            <option :selected="lpu.type_id === 3" :value="3">
              {{ $t("wholesaler") }}
            </option>
          </select>
        </div>
        <div class="form-select">
          <select class="select" @change="selectBranch($event)">
            <option value="" :selected="selectedBranch === null" disabled>
              {{ $t("choose_a_region") }}
            </option>
            <option
              v-for="branch in branches"
              :value="branch.id"
              :key="branch.id"
            >
              {{ branch.name }}
            </option>
          </select>
        </div>
      </div>
      <div class="input-wrapper input-group">
        <input
          type="text"
          class="input"
          v-model="inputDoctorValue"
          @input="searchDoctorHandler"
          placeholder=" "
          autocomplete="off"
          id="drugs"
        />

        <label for="drugs" class="input-label">{{
          $t("choose_a_doctor")
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
      <ul
        v-if="searchedDoctors.length"
        class="drug_list"
        style="margin: 5px 0 10px"
      >
        <li v-for="doctor in selectedDoctors" :key="doctor.id">
          <p>{{ doctor.full_name }}</p>
          <span @click="deleteDoctor(doctor.id)">&times;</span>
        </li>
      </ul>

      <!-- Search Input -->
      <input
        type="search"
        class="controls"
        id="searchInput"
        ref="inputValue"
        :placeholder="$t('choose_place')"
      />
      <!-- Google Map -->
      <div id="map"></div>

      <p class="modal-bottom" id="place"></p>

      <template #button>
        <button
          class="btn btn-red"
          @click.prevent="(s.isEditOpen = false), clearValue()"
        >
          {{ $t("to_close") }}
        </button>
        <button class="btn btn-blue" type="submit">{{ $t("add") }}</button>
      </template>
    </modal>
    <teleport to="body">
      <modal
        @closeModal="(s.isFilterOpen = false), clearValue()"
        :isModalOpen="s.isFilterOpen"
        @submitHandler="filterLpu"
      >
        <template #title>{{ $t("filter_list_lpu") }}</template>

        <div class="modal-form">
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              name="org_name"
              @input="changeHandler"
              id="org_name"
            />
            <label for="org_name" class="input-label">{{
              $t("title_p")
            }}</label>
          </div>
          <div class="input-wrapper input-group">
            <input
              type="text"
              class="input"
              v-model="inputDoctorValue"
              @input="searchDoctorHandler"
              placeholder=" "
              autocomplete="off"
              id="drugs"
            />

            <label for="drugs" class="input-label">{{
              $t("choose_a_doctor")
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
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              name="inn"
              @input="changeHandler"
              id="inn"
            />
            <label for="inn" class="input-label">{{ $t("intr") }}</label>
          </div>
        </div>
        <ul
          v-if="searchedDoctors.length"
          class="drug_list"
          style="margin: 5px 0 10px"
        >
          <li v-for="doctor in selectedDoctors" :key="doctor.id">
            <p>{{ doctor.full_name }}</p>
            <span @click="deleteDoctor(doctor.id)">&times;</span>
          </li>
        </ul>
        <template #button>
          <button
            class="btn btn-red"
            @click.prevent="(s.isFilterOpen = false), clearValue()"
          >
            {{ $t("to_close") }}
          </button>
          <button class="btn btn-blue" type="submit">{{ $t("add") }}</button>
        </template>
      </modal>
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
import Spinner from "@/components/Spinner.vue";
import { initMap } from "../use/searchMap";
import { mapGetters, mapActions } from "vuex";
import Empty from "@/components/Empty.vue";
import Pagination from "@/components/Pagination.vue";

export default {
  components: {
    TheTable,
    TableButtons,
    TheFilter,
    Modal,
    Teleport,
    TableUser,
    Spinner,
    Empty,
    Pagination,
  },
  data: () => ({
    initMap,
    filterItems: [],
    isModalOpen: false,
    lpu: {},
    selectedBranch: null,
    inputDoctorValue: "",
    selectedDoctors: [],
    selectedDoctorsIds: [],
    currentPage: 1,
  }),
  computed: {
    ...mapGetters("lpus", ["lpus", "isLoading", "pages"]),
    ...mapGetters("branches", ["branches"]),
    ...mapGetters("doctors", ["searchedDoctors"]),
    ...mapGetters(["s", "details"]),
  },
  methods: {
    ...mapActions("lpus", ["getLpus", "createLpus", "editLpus", "filterLpus"]),
    ...mapActions("doctors", ["findDoctorByName"]),
    ...mapActions("branches", ["getBranches"]),
    pageChangeHandler(page) {
      this.currentPage = page;
      this.$router.push(`${this.$route.path}?page=${page}`);
      this.getLpus(page - 1);
    },
    changeHandler(e) {
      const { name, value } = e.target;
      this.lpu = { ...this.lpu, [name]: value };
    },
    async submitAddHandler() {
      this.s.isAddOpen = false;

      await this.createLpus({
        organization: {
          ...this.lpu,
          ...this.details,
          region_id: this.selectedBranch[0].id,
          doctor_ids: this.selectedDoctorsIds,
        },
      });
      this.clearValue();
    },
    selectBranch(event) {
      this.selectedBranch = this.branches.filter(
        (branch) => branch.id === +event.target.value
      );
    },
    searchDoctorHandler(e) {
      this.findDoctorByName(e.target.value);
    },
    clickDoctorHandler(doctor) {
      const found = this.selectedDoctors.find(
        (selectedDoctor) => selectedDoctor.id === doctor.id
      );
      if (!found) {
        this.selectedDoctors = [...this.selectedDoctors, doctor];
        this.selectedDoctorsIds = this.selectedDoctors.map((d) => d.id);
      }
      this.inputDoctorValue = "";
    },
    deleteDoctor(id) {
      this.selectedDoctors = this.selectedDoctors.filter((d) => d.id !== id);
      this.selectedDoctorsIds = this.selectedDoctors.map((d) => d.id);
    },
    initialMap() {
      setTimeout(() => {
        initMap();
      }, 200);
    },
    editHandler(lpu) {
      this.s.isEditOpen = true;
      setTimeout(() => {
        this.$refs.inputValue.value = this.lpu.address;
      }, 200);
      this.lpu = lpu;
    },
    submitEditHandler() {
      this.s.isEditOpen = false;
      this.editLpus({
        organization: {
          ...this.lpu,
          ...this.details,
        },
      });
      this.clearValue();
    },
    async filterLpu() {
      this.s.isFilterOpen = false;
      this.filterItems = Object.values(this.lpu);
      await this.filterLpus(this.lpu);
    },
    async removeFilterItem(item) {
      this.filterItems = this.filterItems.filter((d) => d !== item);
      for (let key in this.lpu) {
        if (this.lpu[key].includes(item)) {
          delete this.lpu[key];
        }
      }
      await this.filterLpus(this.lpu);
    },
    clearValue() {
      this.$store.commit("SET_DETAILS", {});
      this.selectedBranch = null;
      this.lpu = {};
      this.inputDoctorValue = "";
      this.selectedDoctors = [];
      this.selectedDoctorsIds = [];
    },
  },
  async mounted() {
    this.currentPage = +this.$route.query.page;
    await this.getLpus(this.currentPage - 1);
    await this.getBranches();
  },
};
</script>
