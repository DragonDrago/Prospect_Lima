<template>
  <div class="sales page">
    <the-filter
      :filterItems="filterItems"
      @filterClicked="s.isFilterOpen = true"
      @filterAdd="(s.isAddOpen = true), initialMap()"
      @removeItem="removeFilterItem"
    >
      <template #name>{{ $t("list_wholesellers") }}</template>
    </the-filter>
    <div class="statistics-menu statistic-visit">
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">{{ $t("total_sales") }}</div>
          <div class="statistics-menu__num">{{ getTotal("sale_count") }}</div>
        </div>
      </div>
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">{{ $t("total_booking") }}</div>
          <div class="statistics-menu__num">
            {{ getTotal("order_count") }}
          </div>
        </div>
      </div>
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">{{ $t("total_amount") }}</div>
          <div class="statistics-menu__num">
            {{ prettify(getTotal("sale_total_sum")) }}
          </div>
        </div>
      </div>
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">
            {{ $t("prepayments_in_the_amount_of") }}
          </div>
          <div class="statistics-menu__num">
            {{ prettify(getTotal("order_total_sum")) }}
          </div>
        </div>
      </div>
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">
            {{ $t("debts_in_the_amount_of") }}
          </div>
          <div class="statistics-menu__num">
            {{ prettify(getTotal("dept_sum")) }}
          </div>
        </div>
      </div>
    </div>
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
        v-if="distributors.length"
        :head="[
          $t('title_p'),
          $t('intr'),
          $t('address'),
          $t('sales'),
          $t('reservations'),
          $t('total_amount'),
          $t('prepayment'),
          $t('debt'),
        ]"
      >
        <tr v-for="b in distributors" :key="b.id">
          <td>{{ b.name }}</td>
          <td>{{ b.inn || $t("empty") }}</td>
          <td>{{ b.address }}</td>
          <td>{{ b.sale_count }}</td>
          <td>{{ b.order_count }}</td>
          <td>{{ prettify(b.sale_total_sum) }}</td>
          <td>{{ prettify(b.prepayment_sum) }}</td>
          <td>{{ prettify(b.dept_sum) }}</td>
          <td><table-buttons @edit="editHandler(b), initialMap()" /></td>
        </tr>
      </the-table>
      <empty v-else v-show="isEmpty" :item="distributors" />
    </div>

    <modal
      @closeModal="(s.isAddOpen = false), (distributor = {})"
      :isModalOpen="true"
      v-if="s.isAddOpen"
      @submitHandler="submitAddHandler"
    >
      <template #title>{{ $t("add_wholeseller") }}</template>

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
        <div class="form-select">
          <select class="select" @change="selectBranch($event)">
            <option :selected="selectedBranch === null" value="" disabled>
              {{ $t("choose_a_region") }}
            </option>
            <option
              v-for="branch in branches"
              :key="branch.id"
              :value="branch.id"
            >
              {{ branch.name }}
            </option>
          </select>
        </div>
      </div>
      <!-- Search Input -->
      <input
        type="search"
        id="searchInput"
        class="controls"
        :placeholder="$t('choose_place')"
      />
      <!-- Google Map -->
      <div id="map"></div>

      <p class="modal-bottom" id="place"></p>

      <template #button>
        <button
          class="btn btn-red"
          @click.prevent="(s.isAddOpen = false), (distributor = {})"
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
      <template #title>{{ $t("edit_a_pharmacy") }}</template>

      <div class="modal-form mb-10">
        <div class="input-group">
          <input
            type="text"
            placeholder=" "
            class="input"
            name="name"
            @input="changeHandler"
            :value="distributor.name"
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
            :value="distributor.inn"
            @input="changeHandler"
            id="inn"
          />
          <label for="inn" class="input-label">{{ $t("intr") }}</label>
        </div>
        <div class="form-select">
          <select class="select" @change="selectBranch($event)">
            <option :selected="selectedBranch === null" value="" disabled>
              {{ $t("choose_a_region") }}
            </option>
            <option
              v-for="branch in branches"
              :key="branch.id"
              :value="branch.id"
            >
              {{ branch.name }}
            </option>
          </select>
        </div>
      </div>
      <!-- Search Input -->
      <input
        type="search"
        id="searchInput"
        class="controls"
        :placeholder="$t('choose_place')"
        ref="inputValue"
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
        @submitHandler="filterDistributor"
      >
        <template #title>{{ $t("filter_list_wholeseller") }}</template>

        <div class="modal-form mb-10">
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
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              name="inn"
              @input="changeHandler"
              id="inn"
            />
            <label for="inn" class="input-label">{{ $t("intr") }}</label>
          </div>
        </div>
        <div class="modal-form">
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              name="sales_min"
              @input="changeHandler"
              id="sales_min"
            />
            <label for="sales_min" class="input-label">{{
              $t("total_amount_with")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              name="sales_max"
              @input="changeHandler"
              id="sales_max"
            />
            <label for="sales_max" class="input-label">{{
              $t("total_amount_for")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              name="orders_min"
              @input="changeHandler"
              id="orders_min"
            />
            <label for="orders_min" class="input-label">{{
              $t("number_of_reservations_with")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              name="orders_max"
              @input="changeHandler"
              id="orders_max"
            />
            <label for="orders_max" class="input-label">{{
              $t("number_of_reservations_by")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              name="total_sum_min"
              @input="changeHandler"
              id="total_sum_min"
            />
            <label for="total_sum_min" class="input-label">{{
              $t("sales_quantity_with")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              name="total_sum_max"
              @input="changeHandler"
              id="totaltotal_sum_max_sum2"
            />
            <label for="total_sum_max" class="input-label">{{
              $t("sales_quantity_by")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              name="prepay_min"
              @input="changeHandler"
              id="prepay_min"
            />
            <label for="prepay_min" class="input-label">{{
              $t("amount_of_prepayment_from")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              @input="changeHandler"
              name="prepay_max"
              id="prepay_max"
            />
            <label for="prepay_max" class="input-label">{{
              $t("amount_of_prepayment_by")
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
          <button class="btn btn-blue" type="submit">
            {{ $t("add") }}
          </button>
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
import { initMap } from "../use/searchMap";
import { prettify } from "@/use/PrettifySum";
import { mapGetters, mapActions } from "vuex";
import Spinner from "@/components/Spinner.vue";
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
    initMap,
    Empty,
    Pagination,
  },

  data: () => ({
    prettify,
    filterItems: [],
    distributor: {},
    selectedBranch: null,
    currentPage: 1,
  }),
  computed: {
    ...mapGetters("distributors", ["distributors", "isLoading", "pages"]),
    ...mapGetters("branches", ["branches"]),
    ...mapGetters(["s", "details"]),
  },
  methods: {
    ...mapActions("distributors", [
      "getDistributors",
      "addDistributor",
      "filterDistributors",
      "editDistributor",
    ]),
    ...mapActions("branches", ["getBranches"]),
    pageChangeHandler(page) {
      this.currentPage = page;
      this.$router.push(`${this.$route.path}?page=${page}`);
      this.getDistributors(page - 1);
    },
    initialMap() {
      setTimeout(() => {
        initMap();
      }, 200);
    },
    getTotal(key) {
      return this.distributors.reduce((acc, val) => acc + val[key], 0);
    },
    changeHandler(e) {
      const { name, value } = e.target;
      this.distributor = { ...this.distributor, [name]: value };
    },
    selectBranch(event) {
      this.selectedBranch = this.branches.filter(
        (branch) => branch.id === +event.target.value
      );
    },
    async submitAddHandler() {
      this.s.isAddOpen = false;
      await this.addDistributor({
        ...this.distributor,
        ...this.details,
        region_id: this.selectedBranch[0].id,
      });
      this.clearValue();
    },
    async filterDistributor() {
      this.s.isFilterOpen = false;
      this.filterItems = Object.values(this.distributor);
      await this.filterDistributors(this.distributor);
      this.clearValue();
    },
    async removeFilterItem(item) {
      this.filterItems = this.filterItems.filter((d) => d !== item);
      for (let key in this.distributor) {
        if (this.distributor[key].includes(item)) {
          delete this.distributor[key];
        }
      }
      await this.filterDistributors(this.distributor);
    },
    editHandler(distributor) {
      this.s.isEditOpen = true;
      setTimeout(() => {
        this.$refs.inputValue.value = distributor.address;
      }, 200);
      this.selectBranch({ target: { value: distributor.region_id } });
      this.distributor = distributor;
    },
    submitEditHandler() {
      this.s.isEditOpen = false;
      const payload = {
        ...this.distributor,
        ...this.details,
      };
      this.editDistributor(payload);
      this.clearValue();
    },
    clearValue() {
      this.distributor = {};
      this.selectedBranch = null;
    },
  },
  async mounted() {
    this.currentPage = +this.$route.query.page;
    await this.getDistributors(this.currentPage - 1);
    await this.getBranches();
  },
};
</script>
