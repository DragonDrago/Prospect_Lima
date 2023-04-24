<template>
  <div class="sales page">
    <the-filter
      :filterItems="filterItems"
      @filterClicked="s.isFilterOpen = true"
      @filterAdd="s.isAddOpen = true"
      @removeItem="removeFilterItem"
    >
      <template #name>{{ $t("products") }}</template>
    </the-filter>
    <div class="statistics-menu statistic-visit">
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">{{ $t("best_selling") }}</div>
          <div class="statistics-menu__num">{{ $t("empty") }}</div>
        </div>
      </div>
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">{{ $t("most_unsellable") }}</div>
          <div class="statistics-menu__num">{{ $t("empty") }}</div>
        </div>
      </div>
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">{{ $t("total_bookings") }}</div>
          <div class="statistics-menu__num">0</div>
        </div>
      </div>
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">
            {{ $t("total_pending_shipment") }}
          </div>
          <div class="statistics-menu__num">0</div>
        </div>
      </div>
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">{{ $t("total_shipped") }}</div>
          <div class="statistics-menu__num">0</div>
        </div>
      </div>
    </div>
    <spinner v-if="isLoading" />
    <div v-else>
      <the-table
        v-if="drugs.length"
        :head="[
          $t('photo'),
          $t('title_p'),
          $t('manufacturer'),
          $t('a_country'),
          $t('price'),
          $t('keeping'),
          $t('quantity_short'),
          $t('umit_measurement'),
          $t('booking'),
        ]"
        class="list-goods-table"
      >
        <tr v-for="b in drugs" :key="b.id">
          <td><table-user img="no-user.jpg" /></td>
          <td>{{ b.drug_name }}</td>
          <td>{{ b.producer_name || $t("empty") }}</td>
          <td>{{ b.country_name || $t("empty") }}</td>
          <td>{{ prettify(b.base_price) }}</td>
          <td>{{ b.store_place_id }}</td>
          <td>{{ b.amount }}</td>
          <td>
            {{ b.unit_name }}
          </td>
          <td>{{ b.drug_in_order_count }}</td>
          <td><table-buttons @edit="openEdit(b)" /></td>
        </tr>
      </the-table>
      <empty v-else v-show="isEmpty" :item="drugs" />
    </div>
    <teleport to="body">
      <!-- Filter Drugs -->
      <modal
        @closeModal="(s.isFilterOpen = false), clearValue()"
        :isModalOpen="s.isFilterOpen"
        @submitHandler="submitFilterHandler"
      >
        <template #title>{{ $t("filter_the_list_of_products") }}</template>

        <div class="modal-form mb-10">
          <div class="input-group">
            <input
              type="text"
              id="name"
              class="input"
              placeholder=" "
              name="drug_name"
              @input="changeHandler"
            />
            <label for="name" class="input-label">{{ $t("title_p") }}</label>
          </div>
          <div class="form-select">
            <select class="select" @change="selectProducer($event)">
              <option :selected="selectedProducer === null" disabled value="">
                {{ $t("manufacturer") }}
              </option>
              <option
                v-for="producer in producers"
                :key="producer.id"
                :value="producer.id"
              >
                {{ producer.name }}
              </option>
            </select>
          </div>
          <div class="form-select">
            <select class="select" @change="selectProducer($event)">
              <option :selected="selectedProducer === null" disabled value="">
                {{ $t("a_country") }}
              </option>
              <option
                v-for="producer in producers"
                :key="producer.id"
                :value="producer.id"
              >
                {{ producer.country_name }}
              </option>
            </select>
          </div>
          <div class="input-group">
            <input
              type="date"
              class="input"
              placeholder=" "
              id="expire_date_month"
              @input="changeHandler"
              name="expire_date_month"
            />
            <label for="expire_date_month" class="input-label">{{
              $t("expiration_date")
            }}</label>
          </div>
          <div class="form-select">
            <select class="select" @change="changeHandler" name="unit_id">
              <option disabled selected>{{ $t("umit_measurement") }}</option>
              <option :value="unit.id" v-for="unit in units" :key="unit.id">
                {{ unit.name }}
              </option>
            </select>
          </div>
        </div>
        <div class="modal-form">
          <div class="input-group">
            <input
              type="number"
              class="input"
              placeholder=" "
              id="amount_min"
              @input="changeHandler"
              name="amount_min"
            />
            <label for="amount_min" class="input-label">{{
              $t("quantities_with")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              @input="changeHandler"
              id="amount_max"
              name="amount_max"
            />
            <label for="amount_max" class="input-label">{{
              $t("quantities_by")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              id="orders_count_min"
              name="orders_count_min"
              @input="changeHandler"
            />
            <label for="orders_count_min" class="input-label">{{
              $t("number_of_reservations_with")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              id="orders_count_max"
              name="orders_count_max"
              @input="changeHandler"
              class="input"
            />
            <label for="orders_count_max" class="input-label">{{
              $t("number_of_reservations_by")
            }}</label>
          </div>
        </div>
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
      <!-- Add Drug -->
      <modal
        @closeModal="(s.isAddOpen = false), clearValue()"
        :isModalOpen="s.isAddOpen"
        @submitHandler="submitAddHandler"
      >
        <template #title>{{ $t("add_good") }}</template>
        <div class="input-group mb-10">
          <input
            type="text"
            placeholder=" "
            id="name"
            class="input"
            @input="changeHandler"
            name="drug_name"
          />
          <label for="name" class="input-label">{{ $t("product_name") }}</label>
        </div>

        <div class="modal-form">
          <div class="input file-input">
            <p>{{ $t("choose") }}...</p>
            <button class="btn btn-blue">
              <i class="icofont-clip"></i> <span>{{ $t("file") }}</span>
            </button>
            <input type="file" />
          </div>

          <div class="form-select">
            <select class="select" @change="selectProducer($event)">
              <option :selected="selectedProducer === null" disabled value="">
                {{ $t("manufacturer") }}
              </option>
              <option
                v-for="producer in producers"
                :key="producer.id"
                :value="producer.id"
              >
                {{ producer.name }}
              </option>
            </select>
          </div>
          <div class="form-select">
            <select class="select">
              <option>{{ $t("storage_place") }}</option>
              <option>{{ $t("storage_place") }}</option>
              <option>{{ $t("storage_place") }}</option>
            </select>
          </div>

          <div class="input-group">
            <input
              type="text"
              id="country_name"
              disabled
              class="input"
              placeholder=" "
              :value="selectedProducer ? selectedProducer[0].country_name : ''"
            />
            <label for="country_name" class="input-label">{{
              $t("a_country")
            }}</label>
          </div>
          <div class="form-select">
            <select class="select" @change="selectUnit($event)">
              <option :selected="selectedUnit === null" value="" disabled>
                {{ $t("unit_of_measurement") }}
              </option>
              <option v-for="unit in units" :key="unit.id" :value="unit.id">
                {{ unit.name }}
              </option>
            </select>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              id="base_price"
              class="input"
              @input="changeHandler"
              name="base_price"
            />
            <label for="base_price" class="input-label">{{
              $t("price")
            }}</label>
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
      <!-- Edit Drugs -->
      <modal
        @closeModal="(s.isEditOpen = false), clearValue()"
        :isModalOpen="s.isEditOpen"
        @submitHandler="submitChangeHandler"
      >
        <template #title>{{ $t("edit_good") }}</template>
        <div class="input-group mb-10">
          <input
            type="text"
            placeholder=" "
            class="input mb-10"
            v-model="drug.drug_name"
            id="name"
            name="drug_name"
            @input="changeHandler"
          />
          <label for="name" class="input-label">{{ $t("product_name") }}</label>
        </div>

        <div class="modal-form">
          <div class="input file-input">
            <p>{{ $t("choose") }}...</p>
            <button class="btn btn-blue">
              <i class="icofont-clip"></i> <span>{{ $t("file") }}</span>
            </button>
            <input type="file" />
          </div>

          <div class="form-select">
            <select class="select" @change="selectProducer($event)">
              <option :selected="selectedProducer === null" disabled value="">
                {{ $t("manufacturer") }}
              </option>
              <option
                v-for="producer in producers"
                :key="producer.id"
                :value="producer.id"
                :selected="
                  selectedProducer && selectedProducer[0].id === producer.id
                "
              >
                {{ producer.name }}
              </option>
            </select>
          </div>
          <div class="form-select">
            <select class="select">
              <option>{{ $t("storage_place") }}</option>
            </select>
          </div>
          <div class="input-group">
            <input
              type="text"
              id="country_name"
              disabled
              class="input"
              placeholder=" "
              :value="selectedProducer ? selectedProducer[0].country_name : ''"
            />
            <label for="country_name" class="input-label">{{
              $t("a_country")
            }}</label>
          </div>
          <div class="form-select">
            <select class="select" @change="selectUnit($event)">
              <option selected disabled>
                {{ $t("unit_of_measurement") }}
              </option>
              <option
                v-for="unit in units"
                :key="unit.id"
                :value="unit.id"
                :selected="selectedUnit && selectedUnit[0].id === unit.id"
              >
                {{ unit.name }}
              </option>
            </select>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              id="base_price"
              class="input"
              @input="changeHandler"
              v-model="drug.base_price"
              name="base_price"
            />
            <label for="base_price" class="input-label">{{
              $t("price")
            }}</label>
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
import TheTable from "@/components/TheTable.vue";
import TableButtons from "@/components/TableButtons.vue";
import TheFilter from "@/components/TheFilter.vue";
import Modal from "@/components/Modal.vue";
import Teleport from "vue2-teleport";
import TableUser from "@/components/TableUser.vue";
import Spinner from "@/components/Spinner.vue";
import { prettify } from "@/use/PrettifySum.js";
import { mapGetters, mapActions } from "vuex";
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
    prettify,
    filterItems: [],
    drug: {},
    selectedProducer: null,
    selectedCountry: null,
    selectedUnit: null,
  }),
  computed: {
    ...mapGetters("drugs", ["drugs", "isLoading"]),
    ...mapGetters("producers", ["producers"]),
    ...mapGetters("countries", ["countries"]),
    ...mapGetters("units", ["units"]),
    ...mapGetters(["s"]),
  },
  methods: {
    ...mapActions("drugs", [
      "getDrugs",
      "updateDrugs",
      "addDrug",
      "filterDrugs",
    ]),
    ...mapActions("producers", ["getProducers"]),
    ...mapActions("units", ["getUnits"]),
    changeHandler(e) {
      const { name, value } = e.target;
      this.drug = { ...this.drug, [name]: value.trim() };
    },
    selectProducer(event) {
      this.selectedProducer = this.producers.filter(
        (branch) => branch.id === +event.target.value
      );
    },
    selectUnit(event) {
      this.selectedUnit = this.units.filter(
        (branch) => branch.id === +event.target.value
      );
    },
    openEdit(b) {
      this.s.isEditOpen = true;
      this.drug = b;
      if (b.producer_id && b.producer_id !== 0) {
        this.selectProducer({ target: { value: b.producer_id } });
      }
      if (b.unit_id && b.unit_id !== 0) {
        this.selectUnit({ target: { value: b.unit_id } });
      }
    },
    async submitChangeHandler() {
      this.s.isEditOpen = false;
      await this.updateDrugs({
        ...this.drug,
        producer_id:
          this.selectedProducer !== null ? this.selectedProducer[0].id : null,
        country_id:
          this.selectedProducer !== null
            ? this.selectedProducer[0].country_id
            : null,
        unit_id: this.selectedUnit !== null ? this.selectedUnit[0].id : null,
        store_place_id: Math.floor(Math.random() * 10),
      });
      this.clearValue();
    },
    clearValue() {
      this.drug = {};
      this.selectedProducer = null;
      this.selectedUnit = null;
    },
    async submitAddHandler() {
      this.s.isAddOpen = false;
      await this.addDrug({
        ...this.drug,
        producer_id:
          this.selectedProducer !== null ? this.selectedProducer[0].id : null,
        country_id:
          this.selectedProducer !== null
            ? this.selectedProducer[0].country_id
            : null,
        unit_id: this.selectedUnit !== null ? this.selectedUnit[0].id : null,
        store_place_id: Math.floor(Math.random() * 10),
      });
      this.clearValue();
    },
    async submitFilterHandler() {
      this.s.isFilterOpen = false;
      let payload = {
        ...this.drug,
      };
      let filterPayload = {
        ...this.drug,
      };
      if (this.selectedProducer) {
        filterPayload = {
          ...filterPayload,
          country_name: this.selectedProducer[0].country_name,
          producer_name: this.selectedProducer[0].name,
        };
        payload = {
          ...payload,
          producer_id: this.selectedProducer[0].id,
          country_id: this.selectedProducer[0].country_id,
        };
      }

      this.filterItems = Object.values(filterPayload);
      await this.filterDrugs(payload);
      this.clearValue();
    },
    async removeFilterItem(item) {
      this.filterItems = this.filterItems.filter((d) => d !== item);
      let payload = {
        ...this.drug,
      };
      if (this.selectedProducer) {
        payload = {
          ...this.drug,
          country_name: this.selectedProducer[0].country_name,
          producer_name: this.selectedProducer[0].name,
        };
      }

      for (let key in payload) {
        if (payload[key].includes(item)) {
          delete payload[key];
        }
      }
      await this.filterDrugs(payload);
    },
  },
  async mounted() {
    await this.getDrugs();
    await this.getProducers();
    await this.getUnits();
  },
};
</script>

<style></style>
