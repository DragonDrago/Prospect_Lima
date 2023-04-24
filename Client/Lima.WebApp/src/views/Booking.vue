<template>
  <div class="sales booking page">
    <the-filter
      :filterItems="filterItems"
      @filterClicked="s.isFilterOpen = true"
      :fullbar="true"
      @removeItem="removeFilterItem"
    >
      <template #name>{{ $t("booking") }}</template>
    </the-filter>
    <div class="statistics-menu statistic-page">
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">
            {{ $t("reserved_for_the_amount_of") }}
          </div>
          <div class="statistics-menu__num">{{ prettify(0) }}</div>
        </div>
      </div>
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">
            {{ $t("prepayments_in_the_amount_of") }}
          </div>
          <div class="statistics-menu__num">{{ prettify(0) }}</div>
        </div>
      </div>
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">
            {{ $t("balances_in_the_amount_of") }}
          </div>
          <div class="statistics-menu__num">{{ prettify(0) }}</div>
        </div>
      </div>
    </div>
    <spinner v-if="isLoading" />
    <div v-else>
      <the-table
        v-if="orders.length"
        :head="[
          '№',
          $t('date'),
          $t('employee'),
          $t('organization'),
          $t('wholesaler'),
          $t('cheque'),
          $t('amount'),
          $t('prepayment'),
          $t('debt'),
          $t('status'),
        ]"
        class="booking-table"
      >
        <tr v-for="b in orders" :key="b.id">
          <td>{{ b.id }}</td>
          <td>{{ new Date(b.date_create).toLocaleDateString("ru-RU") }}</td>
          <td>
            <table-user img="no-user.jpg" :name="b.user_name" />
          </td>
          <td>
            {{ b.organization ? b.organization.organization_name : "-" }}
          </td>
          <td>{{ b.distributor ? b.distributor.organization_name : "-" }}</td>
          <td>
            <p @click="chequeHandler(b)" class="cheque">{{ b.check }}</p>
          </td>
          <td>{{ prettify(b.order_sum) }}</td>
          <td>{{ prettify(b.prepayment_sum) }}</td>
          <td>{{ prettify(b.remainder_sum) }}</td>
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
      <empty v-show="isEmpty" :item="orders" v-else />
    </div>

    <teleport to="body">
      <!-- Filter -->
      <modal
        @closeModal="(s.isFilterOpen = false), clearValue()"
        :isModalOpen="s.isFilterOpen"
        @submitHandler="submitFilterHandler"
      >
        <template #title> {{ $t("filter_booking") }}</template>

        <div class="modal-form">
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              name="order_id"
              id="order_id"
              @input="changeHandler"
            />
            <label for="order_id" class="input-label">{{ $t("number") }}</label>
          </div>
          <div class="input-wrapper input-group">
            <input
              type="text"
              class="input"
              v-model="usersValue"
              @input="searchHandler($event, 'user')"
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
              v-model="drugStoreValue"
              @input="searchHandler($event, 'drugStores')"
              placeholder=" "
              autocomplete="off"
              id="organization"
            />

            <label for="organization" class="input-label">{{
              $t("pharmacy")
            }}</label>
            <ul
              class="auto-suggest"
              v-if="drugStoreValue.length && searchedDrugStores.length > 0"
            >
              <li
                v-for="org in searchedDrugStores"
                :key="org.id"
                @click="clickOrgHandler(org, 'drugStore')"
              >
                {{ org.name }}
              </li>
            </ul>
          </div>
          <div class="input-wrapper input-group">
            <input
              type="text"
              class="input"
              v-model="distributorValue"
              @input="searchHandler($event, 'distributors')"
              placeholder=" "
              autocomplete="off"
              id="organization"
            />

            <label for="organization" class="input-label">{{
              $t("wholesaler")
            }}</label>
            <ul
              class="auto-suggest"
              v-if="distributorValue.length && searchedDistributors.length > 0"
            >
              <li
                v-for="org in searchedDistributors"
                :key="org.id"
                @click="clickOrgHandler(org, 'distributor')"
              >
                {{ org.name }}
              </li>
            </ul>
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
              name="total_sum_min"
              id="total_sum_min"
              @input="changeHandler"
            />
            <label for="total_sum_min" class="input-label">{{
              $t("total_amount_with")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              name="total_sum_max"
              id="total_sum_max"
              @input="changeHandler"
            />
            <label for="total_sum_max" class="input-label">{{
              $t("total_amount_for")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              name="prepay_min"
              id="prepay_min"
              @input="changeHandler"
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
              name="prepay_max"
              id="prepay_max"
              @input="changeHandler"
            />
            <label for="prepay_max" class="input-label">{{
              $t("amount_of_prepayment_by")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              name="debt_sum_min"
              id="debt_sum_min"
              @input="changeHandler"
            />
            <label for="debt_sum_min" class="input-label">{{
              $t("amount_of_debt_with")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              name="debt_sum_max"
              id="debt_sum_max"
              @input="changeHandler"
            />
            <label for="debt_sum_max" class="input-label">{{
              $t("amount_of_debt_for")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              name="remains_min"
              id="remains_min"
              @input="changeHandler"
            />
            <label for="remains_min" class="input-label">{{
              $t("amount_of_sum_to")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              name="remains_max"
              id="remains_max"
              @input="changeHandler"
            />
            <label for="remains_max" class="input-label">{{
              $t("amount_of_sum_left")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              name="check_number"
              id="check_number"
              @input="changeHandler"
            />
            <label for="check_number" class="input-label">{{
              $t("check_number")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              name="inn"
              id="inn"
              @input="changeHandler"
            />
            <label for="inn" class="input-label">{{ $t("intr") }}</label>
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
      <modal-small
        @closeModal="(isChequeOpen = false), clearValue()"
        :isModalOpen="isChequeOpen && order"
        class="modal-check"
      >
        <template #top>
          <div class="modal-check__wrapper">
            <div class="modal-check__title">
              <span>Организация: </span
              >{{
                order.organization
                  ? order.organization.organization_name
                  : order.distributor
                  ? order.distributor.organization_name
                  : $t("empty")
              }}
            </div>
            <div v-if="order.order_detailings.length">
              <the-table
                :head="[
                  $t('photo'),
                  $t('title_p'),
                  $t('quantity_short'),
                  $t('amount'),
                ]"
              >
                <tr v-for="o in order.order_detailings" :key="o.id">
                  <td>
                    <div class="table-user">
                      <img src="no-user.jpg" alt="no-user" />
                    </div>
                  </td>
                  <td>{{ o.drug_name }}</td>
                  <td>{{ o.amount }}</td>
                  <td>{{ o.sale_price }}</td>
                </tr>
              </the-table>
            </div>
          </div>
        </template>
      </modal-small>
      <modal-geolocation
        @getCoordinates="processBooking"
        :openLocation="isConfirmOpen"
        @makeFalse="(isConfirmOpen = false), (visit_id = null)"
        title="accept_reservation"
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
import ModalSmall from "@/components/ModalSmall.vue";
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
    ModalSmall,
    ModalGeolocation,
  },
  data: () => ({
    prettify,
    sortStatusBooking,
    sortStatusBookingClasses,
    filterItems: [],
    order: {},
    isChequeOpen: false,
    isConfirmOpen: false,
    visit_id: null,
    usersValue: "",
    user: {},
    organization: {},
    drugStoreValue: "",
    distributorValue: "",
  }),
  computed: {
    ...mapGetters("orders", ["orders", "isLoading"]),
    ...mapGetters("users", ["searchedUsers"]),
    ...mapGetters("drugStores", ["searchedDrugStores"]),
    ...mapGetters("distributors", ["searchedDistributors"]),
    ...mapGetters(["s"]),
  },
  methods: {
    ...mapActions("orders", ["getOrders", "filterOrder", "acceptOrder"]),
    ...mapActions("users", ["findUsersByName"]),
    ...mapActions("drugStores", ["findDrugStoresByName"]),
    ...mapActions("distributors", ["findDistributorsByName"]),
    changeHandler(e) {
      const { name, value } = e.target;
      this.order = { ...this.order, [name]: value };
    },
    getBookingId(id) {
      this.isConfirmOpen = true;
      this.visit_id = id;
    },
    async processBooking() {
      this.isConfirmOpen = false;
      await this.acceptOrder(this.visit_id);
      await this.getOrders();
    },
    async submitFilterHandler() {
      this.s.isFilterOpen = false;
      let filterPayload = {
        ...this.order,
      };
      if (Object.values(this.user).length) {
        filterPayload = {
          ...filterPayload,
          user_id: this.user.id,
        };
      }
      if (Object.values(this.organization).length) {
        filterPayload = {
          ...filterPayload,
          org_id: this.organization.id,
        };
      }
      this.filterItems = Object.values(filterPayload);
      await this.filterOrder(filterPayload);
      this.clearValue();
    },
    async removeFilterItem(item) {
      this.filterItems = this.filterItems.filter((d) => d !== item);
      for (let key in this.order) {
        if (this.order[key].includes(item)) {
          delete this.order[key];
        }
      }
      await this.filterOrder(this.order);
    },
    chequeHandler(order) {
      this.isChequeOpen = true;
      this.order = order;
    },
    clearValue() {
      this.order = {};
      this.usersValue = "";
      this.drugStoreValue = "";
      this.distributorValue = "";
    },
    async searchHandler(e, value) {
      if (value === "user") {
        await this.findUsersByName(e.target.value);
      } else if (value === "drugStores") {
        this.organization = {};
        await this.findDrugStoresByName(e.target.value);
      } else if (value === "distributors") {
        this.organization = {};
        await this.findDistributorsByName(e.target.value);
      }
    },
    clickOrgHandler(org, value) {
      if (value === "drugStore") {
        this.drugStoreValue = org.name;
        this.organization = org;
        this.$store.commit("drugStores/setSearchedDrugStores", []);
      } else if (value === "distributor") {
        this.distributorValue = org.name;
        this.organization = org;
        this.$store.commit("distributors/setSearchedDistributors", []);
      }
    },
    clickUsersHandler(user) {
      this.user = user;
      this.usersValue = user.full_name;
      this.$store.commit("users/setSearchedUsers", []);
    },
  },
  async mounted() {
    await this.getOrders();
  },
};
</script>
