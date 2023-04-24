<template>
  <div class="sales page">
    <the-filter @filterAdd="s.isAddOpen = true">
      <template #name>{{ $t("series") }}</template>
    </the-filter>
    <spinner v-if="isLoading" />
    <div v-else>
      <the-table
        v-if="series.length"
        :head="[
          $t('series_name'),
          $t('quantity_short'),
          $t('expiration_date'),
          $t('products'),
        ]"
      >
        <tr v-for="s in series" :key="s.id">
          <td>{{ s.series }}</td>
          <td>{{ s.quantity }}</td>
          <td>{{ new Date(s.expired_date).toLocaleDateString("ru-RU") }}</td>
          <td>{{ s.full_name }}</td>
          <td><table-buttons @edit="openEdit(s)" /></td>
        </tr>
      </the-table>
      <empty v-else v-show="isEmpty" :item="series" />
    </div>
    <teleport to="body">
      <!-- Add serie -->
      <modal
        :isModalOpen="s.isAddOpen"
        @closeModal="(s.isAddOpen = false), clearValue()"
        @submitHandler="submitAddSerie"
      >
        <template #title>{{ $t("add_serie") }}</template>
        <div class="modal-form">
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              name="series"
              @input="changeHandler"
              required
              id="series"
            />
            <label for="series" class="input-label">{{
              $t("serial_number")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              name="quantity"
              @input="changeHandler"
              id="quantity"
            />
            <label for="quantity" class="input-label">{{
              $t("quantity")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="date"
              placeholder=" "
              class="input"
              name="expired_date"
              @input="changeHandler"
              required
              id="expired_date"
            />
            <label for="expired_date" class="input-label">{{
              $t("expiration_date")
            }}</label>
          </div>
          <div class="form-select">
            <select class="select" name="drug_id" @change="selectDrug">
              <option value="" disabled selected>
                {{ $t("choose_product") }}
              </option>
              <option v-for="drug in drugs" :key="drug.id" :value="drug.id">
                {{ drug.drug_name }}
              </option>
            </select>
          </div>
        </div>
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
      <!-- Edit serie -->
      <modal
        :isModalOpen="s.isEditOpen"
        @closeModal="(s.isEditOpen = false), clearValue()"
        @submitHandler="submitEditHandler"
      >
        <template #title>{{ $t("edit_serie") }}</template>
        <div class="modal-form">
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              name="series"
              @input="changeHandler"
              :value="serie.series"
              required
              id="series"
            />
            <label for="series" class="input-label">{{
              $t("serial_number")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              name="quantity"
              :value="serie.quantity"
              @input="changeHandler"
              id="quantity"
            />
            <label for="quantity" class="input-label">{{
              $t("quantity")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="date"
              placeholder=" "
              class="input"
              name="expired_date"
              :value="serie.expired_date"
              required
              id="expired_date"
            />
            <label for="expired_date" class="input-label">{{
              $t("expiration_date")
            }}</label>
          </div>
          <div class="form-select">
            <select class="select" name="drug_id" @change="selectDrug">
              <option value="" disabled selected>
                {{ $t("choose_product") }}
              </option>
              <option
                v-for="drug in drugs"
                :selected="drug.id === serie.drug_id"
                :key="drug.id"
                :value="drug.id"
              >
                {{ drug.drug_name }}
              </option>
            </select>
          </div>
        </div>
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
    </teleport>
  </div>
</template>
<script>
import TheFilter from "@/components/TheFilter.vue";
import Spinner from "@/components/Spinner.vue";
import TheTable from "@/components/TheTable.vue";
import { mapActions, mapGetters } from "vuex";
import Empty from "@/components/Empty.vue";
import Teleport from "vue2-teleport";
import Modal from "@/components/Modal.vue";
import TableButtons from "@/components/TableButtons.vue";
export default {
  components: {
    TheFilter,
    Spinner,
    TheTable,
    Empty,
    Teleport,
    Modal,
    TableButtons,
  },
  data: () => ({
    serie: {},
    selectedDrug: [],
  }),
  computed: {
    ...mapGetters("series", ["series", "isLoading"]),
    ...mapGetters("drugs", ["drugs"]),
    ...mapGetters(["s"]),
  },
  methods: {
    ...mapActions("series", ["getSeries", "addSerie", "editSerie"]),
    ...mapActions("drugs", ["getDrugs"]),
    changeHandler(e) {
      const { name, value } = e.target;
      this.serie = { ...this.serie, [name]: value };
    },
    selectDrug(e) {
      this.selectedDrug = this.drugs.filter((d) => d.id === +e.target.value);
    },

    submitAddSerie() {
      this.s.isAddOpen = false;
      const payload = {
        ...this.serie,
        drug_id: this.selectedDrug[0].id,
      };
      this.addSerie(payload);
      this.clearValue();
    },
    openEdit(serie) {
      this.s.isEditOpen = true;
      this.serie = serie;
    },
    async submitEditHandler() {
      this.s.isEditOpen = false;
      const payload = {
        ...this.serie,
        drug_id: this.selectedDrug.length
          ? this.selectedDrug[0].id
          : this.serie.drug_id,
      };
      await this.editSerie(payload);
      this.clearValue();
    },
    clearValue() {
      this.serie = "";
      this.selectedDrug = [];
    },
  },
  async mounted() {
    await this.getSeries();
    await this.getDrugs();
  },
};
</script>
