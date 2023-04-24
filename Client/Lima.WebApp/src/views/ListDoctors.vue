<template>
  <div class="page sales">
    <the-filter
      :filterItems="filterItems"
      @filterClicked="s.isFilterOpen = true"
      @filterAdd="s.isAddOpen = true"
      @removeItem="removeFilterItem"
    >
      <template #name>{{ $t("list_doctors") }}</template>
    </the-filter>
    <spinner v-if="isLoading" />
    <div v-else>
      <the-table
        v-if="doctors.length"
        :head="[
          $t('doctor'),
          $t('telephone'),
          $t('comment'),
          $t('specialization'),
        ]"
      >
        <tr v-for="doc in doctors" :key="doc.id">
          <td>{{ doc.full_name }}</td>
          <td>{{ doc.phone }}</td>
          <td>{{ doc.comment || $t("empty") }}</td>
          <td>{{ doc.position || $t("empty") }}</td>
          <td>
            <table-buttons
              @edit="(doctor = doc), (s.isEditOpen = true)"
              :deleteButton="false"
            />
          </td>
        </tr>
      </the-table>
      <empty :item="doctors" v-else v-show="isEmpty" />
    </div>
    <teleport to="body">
      <!-- Filter Doctors -->
      <modal
        @closeModal="(s.isFilterOpen = false), (doctor = {})"
        :isModalOpen="s.isFilterOpen"
        @submitHandler="submitFilterHandler"
      >
        <template #title>{{ $t("filter_list_of_doctors") }}</template>

        <div class="modal-form">
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              @input="changeHandler"
              name="doctor_name"
              id="doctor_name"
            />
            <label for="doctor_name" class="input-label">{{ $t("lfm") }}</label>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              @input="changeHandler"
              name="phone"
              id="phone"
            />
            <label for="phone" class="input-label">{{ $t("telephone") }}</label>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              @input="changeHandler"
              name="comment"
              id="comment"
            />
            <label for="comment" class="input-label">{{ $t("comment") }}</label>
          </div>
        </div>
        <template #button>
          <button
            class="btn btn-red"
            @click.prevent="(s.isFilterOpen = false), (doctor = {})"
          >
            {{ $t("to_close") }}
          </button>
          <button class="btn btn-blue" type="submit">{{ $t("add") }}</button>
        </template>
      </modal>
      <!-- Add Doctors -->
      <modal
        @closeModal="(s.isAddOpen = false), (doctor = {}), (inputLpuValue = '')"
        :isModalOpen="s.isAddOpen"
        @submitHandler="submitAddHandler"
      >
        <template #title>{{ $t("add_a_doctor") }}</template>
        <div class="input-wrapper input-group mb-10">
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

          <label for="name" class="input-label">{{ $t("organization") }}</label>
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
        <div class="modal-form">
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              name="full_name"
              @input="changeHandler"
              required
              id="full_name"
            />
            <label for="full_name" class="input-label">{{
              $t("lfm_doctor")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              name="position"
              @input="changeHandler"
              required
              id="position"
            />
            <label for="position" class="input-label">{{
              $t("specialization")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              name="phone"
              @input="changeHandler"
              required
              id="phone"
            />
            <label for="phone" class="input-label">{{ $t("telephone") }}</label>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              name="comment"
              @input="changeHandler"
              id="comment"
            />
            <label for="comment" class="input-label">{{ $t("comment") }}</label>
          </div>
        </div>
        <template #button>
          <button
            class="btn btn-red"
            @click.prevent="
              (s.isAddOpen = false), (doctor = {}), (inputLpuValue = '')
            "
          >
            {{ $t("to_close") }}
          </button>
          <button class="btn btn-blue" type="submit">{{ $t("add") }}</button>
        </template>
      </modal>
      <!-- Update -->
      <modal
        @closeModal="(s.isEditOpen = false), (doctor = {})"
        :isModalOpen="s.isEditOpen"
        @submitHandler="submitChangeHandler"
      >
        <template #title>{{ $t("edit_list_doctor") }}</template>
        <div class="modal-form">
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              v-model="doctor.full_name"
              name="full_name"
              @input="changeHandler"
              required
              id="full_name"
            />
            <label for="full_name" class="input-label">{{
              $t("lfm_doctor")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              v-model="doctor.position"
              name="position"
              @input="changeHandler"
              required
              id="position"
            />
            <label for="position" class="input-label">{{
              $t("specialization")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              v-model="doctor.phone"
              name="phone"
              @input="changeHandler"
              required
              id="phone"
            />
            <label for="phone" class="input-label">{{ $t("telephone") }}</label>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              v-model="doctor.comment"
              name="comment"
              @input="changeHandler"
              id="comment"
            />
            <label for="comment" class="input-label">{{ $t("comment") }}</label>
          </div>
        </div>
        <template #button>
          <button
            class="btn btn-red"
            @click.prevent="(s.isEditOpen = false), (doctor = {})"
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
import { mapActions, mapGetters } from "vuex";
import TheTable from "@/components/TheTable.vue";
import TableButtons from "@/components/TableButtons.vue";
import TheFilter from "@/components/TheFilter.vue";
import Modal from "@/components/Modal.vue";
import Teleport from "vue2-teleport";
import TableUser from "@/components/TableUser.vue";
import Spinner from "@/components/Spinner.vue";
import Empty from "@/components/Empty.vue";

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
  },
  data: () => ({
    filterItems: [],
    doctor: {},
    inputLpuValue: "",
    lpuItem: null,
  }),
  computed: {
    ...mapGetters(["s"]),
    ...mapGetters("doctors", ["doctors", "isLoading"]),
    ...mapGetters("lpus", ["searchedLpus"]),
  },
  methods: {
    ...mapActions("doctors", [
      "getDoctors",
      "editDoctors",
      "addDoctor",
      "filterDoctors",
    ]),
    ...mapActions("lpus", ["findLpuByName"]),

    changeHandler(e) {
      const { name, value } = e.target;
      this.doctor = { ...this.doctor, [name]: value.trim() };
    },
    async submitChangeHandler() {
      this.s.isEditOpen = false;
      await this.editDoctors(this.doctor).then(() => (this.doctor = {}));
    },
    async submitAddHandler() {
      this.s.isAddOpen = false;
      await this.addDoctor({
        ...this.doctor,
        organization_id: this.lpuItem.id,
      }).then(() => {
        this.doctor = {};
        this.lpuItem = null;
        this.inputLpuValue = "";
      });
    },
    async submitFilterHandler() {
      this.s.isFilterOpen = false;
      this.filterItems = Object.values(this.doctor);
      await this.filterDoctors(this.doctor);
    },
    async removeFilterItem(item) {
      this.filterItems = this.filterItems.filter((d) => d !== item);
      for (let key in this.doctor) {
        if (this.doctor[key].includes(item)) {
          delete this.doctor[key];
        }
      }
      await this.filterDoctors(this.doctor);
    },
    searchLpuHandler(e) {
      this.lpuItem = {};
      this.findLpuByName(e.target.value);
    },
    clickLpuHandler(item) {
      this.inputLpuValue = item.name;
      this.$store.commit("lpus/setSearchedLpus", []);
      this.lpuItem = item;
    },
  },
  async mounted() {
    await this.getDoctors();
  },
};
</script>

<style></style>
