<template>
  <div class="sales page booking">
    <the-filter
      :filterItems="filterItems"
      @filterClicked="s.isFilterOpen = true"
      @removeItem="removeFilterItem"
      :fullbar="true"
    >
      <template #name>{{ $t("shipment") }}</template>
    </the-filter>
    <div class="statistics-menu statistic-shipping">
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">{{ $t("total_pending") }}</div>
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
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">{{ $t("waiting_amount") }}</div>
          <div class="statistics-menu__num">{{ prettify(0) }}</div>
        </div>
      </div>
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">{{ $t("shipped_amount") }}</div>
          <div class="statistics-menu__num">{{ prettify(0) }}</div>
        </div>
      </div>
    </div>
    <spinner v-if="isLoading" />
    <div v-else>
      <the-table
        v-if="sales.length"
        :head="[
          '№',
          $t('date'),
          $t('employee'),
          $t('organization'),
          $t('wholesaler'),
          $t('cheque'),
          $t('amount'),
          $t('status'),
        ]"
        class="shipping-table"
      >
        <tr v-for="b in sales" :key="b.id">
          <td>{{ b.id }}</td>
          <td>{{ new Date(b.create_date).toLocaleDateString("ru-RU") }}</td>
          <td>
            <table-user img="no-user.jpg" :name="b.user.user_full_name" />
          </td>
          <td>
            {{
              b.organization.type_id === 1 || b.organization.type_id === 2
                ? b.organization.organization_name
                : "-"
            }}
          </td>
          <td>
            {{
              b.organization.type_id === 3
                ? b.organization.organization_name
                : "-"
            }}
          </td>
          <td>
            <p class="cheque">{{ b.order_check }}</p>
          </td>
          <td>{{ prettify(b.sale_total) }}</td>
          <td :class="sortStatusBookingClasses(b.status_id)">
            {{ $t(sortStatusBooking(b.status_id)) }}
          </td>
          <td>
            <button
              v-if="b.status_id !== 2 && b.status_id !== 3"
              class="table-btn table-btn__location table_ready"
              @click="getBookingId(b.id)"
            >
              <i class="icofont-verification-check"></i>
            </button>
          </td>
        </tr>
      </the-table>
      <empty v-else v-show="isEmpty" :item="sales" />
    </div>
    <teleport to="body">
      <!-- Filter Sales -->
      <modal
        @closeModal="(s.isFilterOpen = false), clearValue()"
        :isModalOpen="s.isFilterOpen"
        @submitHandler="submitFilterHandler"
      >
        <template #title>{{ $t("filter_shipping") }}</template>
        <div class="modal-form mb-10">
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              name="sale_id"
              id="sale_id"
              @input="changeHandler"
            />
            <label for="sale_id" class="input-label">{{ $t("title_p") }}</label>
          </div>
          <div class="input-wrapper input-group">
            <input
              type="text"
              class="input"
              v-model="usersValue"
              @input="searchUsersHandler"
              placeholder=" "
              autocomplete="off"
              id="organization"
            />

            <label for="organization" class="input-label">{{
              $t("employee")
            }}</label>
            <ul
              class="auto-suggest"
              v-if="usersValue.length && searchedUsers.length > 0"
            >
              <li
                v-for="user in searchedUsers"
                :key="user.id"
                @click="clickUsersHandler(user)"
              >
                {{ user.full_name }}
              </li>
            </ul>
          </div>
          <div class="input-wrapper input-group">
            <input
              type="text"
              class="input"
              v-model="inputOrgValue"
              @input="searchOrgHandler"
              placeholder=" "
              autocomplete="off"
              id="organization"
            />

            <label for="organization" class="input-label">{{
              $t("organization")
            }}</label>
            <ul
              class="auto-suggest"
              v-if="inputOrgValue.length && organizations.length > 0"
            >
              <li
                v-for="item in organizations"
                :key="item.id"
                @click="clickLpuHandler(item)"
              >
                {{ item.name }}
              </li>
            </ul>
          </div>
        </div>
        <div class="modal-form">
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
              $t("date_h")
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
              $t("date_o")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              name="sale_sum_min"
              id="sale_sum_min"
              @input="changeHandler"
            />
            <label for="sale_sum_min" class="input-label">{{
              $t("amount_of_sales_from")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              name="sale_sum_max"
              id="sale_sum_max"
              @input="changeHandler"
            />
            <label for="sale_sum_max" class="input-label">{{
              $t("amount_of_sales_from")
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
      <modal-geolocation
        @getCoordinates="processBooking"
        :openLocation="isConfirmOpen"
        @makeFalse="(isConfirmOpen = false), (visit_id = null)"
        title="accept_sales"
      />
    </teleport>
  </div>
</template>

<script>
import TheTable from "@/components/TheTable.vue";
import TableButtons from "@/components/TableButtons.vue";
import TableUser from "@/components/TableUser.vue";
import TheFilter from "@/components/TheFilter.vue";
import Modal from "@/components/Modal.vue";
import Teleport from "vue2-teleport";
import { prettify } from "../use/PrettifySum";
import { mapGetters, mapActions } from "vuex";
import Spinner from "@/components/Spinner.vue";
import Empty from "@/components/Empty.vue";
import { sortStatusBooking, sortStatusBookingClasses } from "../use/SortStatus";
import ModalGeolocation from "@/components/ModalGeolocation.vue";

export default {
  components: {
    TheTable,
    TableButtons,
    TableUser,
    TheFilter,
    Modal,
    Teleport,
    Spinner,
    Empty,
    ModalGeolocation,
  },
  data: () => ({
    prettify,
    sortStatusBooking,
    sortStatusBookingClasses,
    filterItems: [],
    sale: {},
    inputOrgValue: "",
    organization: {},
    isChequeOpen: false,
    isConfirmOpen: false,
    visit_id: null,
    usersValue: "",
    user: {},
  }),
  computed: {
    ...mapGetters("sales", ["sales", "isLoading", "organizations"]),
    ...mapGetters("users", ["searchedUsers"]),
    ...mapGetters(["s"]),
  },
  methods: {
    ...mapActions("sales", [
      "getSales",
      "filterSales",
      "getOrganizations",
      "acceptSales",
    ]),
    ...mapActions("users", ["findUsersByName"]),
    getBookingId(id) {
      this.isConfirmOpen = true;
      this.visit_id = id;
    },
    async processBooking() {
      this.isConfirmOpen = false;
      await this.acceptSales(this.visit_id);
      await this.getSales();
    },
    changeHandler(e) {
      const { name, value } = e.target;
      this.sale = { ...this.sale, [name]: value };
    },
    async searchOrgHandler(e) {
      await this.getOrganizations(e.target.value);
    },
    clickLpuHandler(item) {
      this.inputOrgValue = item.name;
      this.$store.commit("sales/setOrganizations", []);
      this.organization = item;
    },
    async submitFilterHandler() {
      this.s.isFilterOpen = false;
      let filterPayload = {
        ...this.sale,
      };
      if (Object.values(this.organization).length) {
        filterPayload = {
          ...filterPayload,
          org_id: this.organization.id,
        };
      }
      if (Object.values(this.user).length) {
        filterPayload = {
          ...filterPayload,
          user_id: this.user.id,
        };
      }
      this.filterItems = Object.values(filterPayload);
      await this.filterSales(filterPayload);
      this.clearValue();
    },
    async removeFilterItem(item) {
      this.filterItems = this.filterItems.filter((d) => d !== item);
      for (let key in this.sale) {
        if (this.sale[key].includes(item)) {
          delete this.sale[key];
        }
      }
      await this.filterSales(this.sale);
    },
    async searchUsersHandler(e) {
      await this.findUsersByName(e.target.value);
    },
    clickUsersHandler(user) {
      this.user = user;
      this.usersValue = user.full_name;
      this.$store.commit("users/setSearchedUsers", []);
    },
    clearValue() {
      this.inputOrgValue = "";
      this.usersValue = "";
    },
  },
  async mounted() {
    await this.getSales();
  },
};
</script>

<style></style>
