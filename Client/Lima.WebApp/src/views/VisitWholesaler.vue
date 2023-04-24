<template>
  <div class="sales page">
    <the-filter
      :filterItems="filterItems"
      @filterClicked="s.isFilterOpen = true"
      @filterAdd="s.isAddOpen = true"
      @removeItem="removeFilterItem"
    >
      <template #name>{{ $t("visit_to_the_wholesaler") }}</template>
    </the-filter>
    <div class="statistics-menu statistic-anything">
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">{{ $t("total_spent") }}</div>
          <div class="statistics-menu__num">
            {{ +visitDistributorsStatistics.compleated || 0 }}
          </div>
        </div>
      </div>
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title" style="font-size: 0.7rem">
            {{ $t("total_planned") }}
          </div>
          <div class="statistics-menu__num">
            {{ +visitDistributorsStatistics.planned || 0 }}
          </div>
        </div>
      </div>
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">{{ $t("total_overdue") }}</div>
          <div class="statistics-menu__num">
            {{ +visitDistributorsStatistics.overdue || 0 }}
          </div>
        </div>
      </div>
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">
            {{ $t("reserved_for_the_amount_of") }}
          </div>
          <div class="statistics-menu__num">
            {{ prettify(+visitDistributorsStatistics.orders) || 0 }}
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
            {{ prettify(+visitDistributorsStatistics.prepayments) || 0 }}
          </div>
        </div>
      </div>
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">
            {{ $t("balances_in_the_amount_of") }}
          </div>
          <div class="statistics-menu__num">
            {{ prettify(+visitDistributorsStatistics.remains) || 0 }}
          </div>
        </div>
      </div>
    </div>
    <spinner v-if="isLoading" />
    <div v-else>
      <the-table
        :head="[
          $t('employee'),
          $t('date'),
          $t('time'),
          $t('wholesaler'),
          $t('comment'),
          $t('product'),
          $t('total_amount'),
          $t('prepayment'),
          $t('debt'),
          $t('status'),
        ]"
        class="visit-doctor-table"
        v-if="visitDistributors.length"
      >
        <tr v-for="b in visitDistributors" :key="b.visit_id">
          <td>
            <table-user :name="b.medrep.medrep_full_name" img="no-user.jpg" />
          </td>
          <td>{{ new Date(b.date_create).toLocaleDateString("ru-RU") }}</td>
          <td>{{ new Date(b.date_create).toLocaleTimeString("ru-RU") }}</td>
          <td>
            {{
              b.organization ? b.organization.organization_name : $t("empty")
            }}
          </td>
          <td>
            <i class="icofont-eye font-eye" @click="setComment(b.comment)"></i>
          </td>
          <td>
            <i class="icofont-eye font-eye" @click="setItems(b.drugs)"></i>
          </td>
          <td>{{ prettify(b.total_sum) }}</td>
          <td>{{ prettify(b.prepayment_sum) }}</td>
          <td>{{ prettify(b.total_sum - b.prepayment_sum) }}</td>
          <td :class="sortStatusClass(b.visit_status_id)">
            {{ $t(sortStatus(b.visit_status_id)) }}
          </td>
          <td>
            <table-buttons
              v-if="b.visit_status_id !== 3"
              @edit="editHandler(b)"
              :isLocation="true"
              @locationClicked="(openLocation = true), (visit_id = b.visit_id)"
            />
          </td>
        </tr>
      </the-table>
      <empty v-else v-show="isEmpty" :item="visitDistributors" />
    </div>
    <teleport to="body">
      <!-- Filter Visit -->
      <modal
        @closeModal="(s.isFilterOpen = false), clearValues()"
        :isModalOpen="s.isFilterOpen"
        @submitHandler="submitFilterHandler"
      >
        <template #title>{{ $t("filter_visits_to_the_wholesaler") }}</template>

        <div class="modal-form mb-10">
          <div class="input-wrapper input-group">
            <input
              type="text"
              class="input"
              v-model="inputDistributorValue"
              @input="searchDistributorHandler"
              placeholder=" "
              autocomplete="off"
              id="name"
            />

            <label for="name" class="input-label">{{
              $t("organization")
            }}</label>
            <ul
              class="auto-suggest"
              v-if="
                inputDistributorValue.length && searchedDistributors.length > 0
              "
            >
              <li
                v-for="store in searchedDistributors"
                :key="store.id"
                @click="clickLpuHandler(store)"
              >
                {{ store.name }}
              </li>
            </ul>
          </div>
          <div class="form-select">
            <select class="select" @change="changeHandler" name="medrep_name">
              <option selected disabled>{{ $t("employee") }}</option>
              <option
                v-for="user in users"
                :key="user.id"
                :value="user.full_name"
              >
                {{ user.full_name }}
              </option>
            </select>
          </div>
        </div>
        <div class="modal-form">
          <div class="input-group">
            <input
              type="date"
              placeholder=" "
              class="input"
              name="dates_min"
              id="date_h"
              @input="changeHandler"
            />
            <label for="date_h" class="input-label">{{ $t("date_h") }}</label>
          </div>
          <div class="input-group">
            <input
              type="date"
              placeholder=" "
              class="input"
              name="dates_max"
              id="date_o"
              @input="changeHandler"
            />
            <label for="date_o" class="input-label">{{ $t("date_o") }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              name="sales_min"
              id="sales_min"
              @input="changeHandler"
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
              id="sales_max"
              @input="changeHandler"
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
              name="prepayments_min"
              id="prepayments_min"
              @input="changeHandler"
            />
            <label for="prepayments_min" class="input-label">{{
              $t("amount_of_prepayment_from")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              name="prepayments_max"
              id="prepayments_max"
              @input="changeHandler"
            />
            <label for="prepayments_max" class="input-label">{{
              $t("amount_of_prepayment_by")
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

          <div class="form-select">
            <select class="select" name="status" @change="changeHandler">
              <option disabled selected>{{ $t("status") }}</option>
              <option :value="1">{{ $t(sortStatus(1)) }}</option>
              <option :value="2">{{ $t(sortStatus(2)) }}</option>
              <option :value="3">{{ $t(sortStatus(3)) }}</option>
            </select>
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
      <!-- Add Visit -->
      <modal
        @closeModal="(s.isAddOpen = false), clearValues()"
        :isModalOpen="s.isAddOpen"
        @submitHandler="submitHandler"
      >
        <template #title>{{ $t("add_a_visit_to_a_wholesaler") }}</template>
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
            <div class="input-wrapper input-group">
              <input
                type="text"
                class="input"
                v-model="inputDistributorValue"
                @input="searchDistributorHandler"
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
                v-if="
                  inputDistributorValue.length &&
                  searchedDistributors.length > 0
                "
              >
                <li
                  v-for="store in searchedDistributors"
                  :key="store.id"
                  @click="clickLpuHandler(store)"
                >
                  {{ store.name }}
                </li>
              </ul>
            </div>
            <div class="input-group">
              <input
                type="text"
                placeholder=" "
                class="input"
                id="comment"
                v-model="comment"
              />
              <label for="comment" class="input-label">{{
                $t("comment")
              }}</label>
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
                $t("products")
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
            <div class="form-select">
              <select class="select" @change="chooseMarkup">
                <option value="" selected disabled>
                  {{ $t("prepayment") }}
                </option>
                <option
                  v-for="markup in markups"
                  :key="markup.id"
                  :value="markup.id"
                >
                  {{ markup.name }}
                </option>
              </select>
            </div>
            <div class="input-group">
              <input
                type="date"
                placeholder=" "
                class="input"
                id="return_period"
                v-model="return_period"
              />
              <label for="return_period" class="input-label">{{
                $t("refund_period")
              }}</label>
            </div>
          </div>
          <the-table
            :head="[
              $t('photo'),
              $t('title_p'),
              $t('quantity_short'),
              $t('amount'),
            ]"
            v-if="drugItems.length"
          >
            <tr v-for="b in drugItems" :key="b.id">
              <td><table-user img="no-user.jpg" /></td>
              <td>{{ b.drug_name }}</td>
              <td class="count">
                <i
                  class="icofont-minus-circle icon-minus"
                  @click="
                    b.drug_in_order_count > 1
                      ? b.drug_in_order_count--
                      : deleteDrug(b.id)
                  "
                ></i>
                <input
                  class="quantity-input input"
                  type="text"
                  v-model="b.drug_in_order_count"
                />
                <i
                  class="icofont-plus-circle icon-minus"
                  @click="b.drug_in_order_count++"
                ></i>
              </td>
              <td>{{ prettify(b.base_price) }}</td>
            </tr>
          </the-table>
          <the-table
            :head="[$t('total_amount'), $t('prepayment'), $t('debt')]"
            v-if="drugItems.length"
          >
            <tr>
              <td>{{ prettify(totalSum) }}</td>
              <td>
                {{ prettify(prepaymentSum(totalSum, markup)) }}
              </td>
              <td>
                {{ prettify(totalSum - prepaymentSum(totalSum, markup)) }}
              </td>
            </tr>
          </the-table>
        </div>
        <div v-else-if="topButtons[1].isActive">
          <div class="modal-form mb-10">
            <div class="form-select">
              <select class="select">
                <option>{{ $t("organization") }}</option>
                <option>{{ $t("organization") }}</option>
                <option>{{ $t("organization") }}</option>
              </select>
            </div>
          </div>
          <the-table :head="[]" v-if="false">
            <tr v-for="b in []" :key="b.id">
              <td><table-user :img="b.img" /></td>
              <td>{{ b.name }}</td>
              <td class="count">
                <i
                  class="icofont-minus-circle icon-minus"
                  @click="b.count--"
                ></i>
                {{ b.count }}
                <i
                  class="icofont-plus-circle icon-minus"
                  @click="b.count++"
                ></i>
              </td>
              <td>{{ prettify(b.sum) }}</td>
            </tr>
            <tr v-for="b in []" :key="b.id">
              <td><table-user :img="b.img" /></td>
              <td>{{ b.name }}</td>
              <td class="count">
                <i
                  class="icofont-minus-circle icon-minus"
                  @click="b.count--"
                ></i>
                {{ b.count }}
                <i
                  class="icofont-plus-circle icon-minus"
                  @click="b.count++"
                ></i>
              </td>
              <td>{{ prettify(b.sum) }}</td>
            </tr>
            <tr v-for="b in []" :key="b.id">
              <td><table-user :img="b.img" /></td>
              <td>{{ b.name }}</td>
              <td class="count">
                <i
                  class="icofont-minus-circle icon-minus"
                  @click="b.count--"
                ></i>
                {{ b.count }}
                <i
                  class="icofont-plus-circle icon-minus"
                  @click="b.count++"
                ></i>
              </td>
              <td>{{ prettify(b.sum) }}</td>
            </tr>
          </the-table>
          <the-table :head="[$t('total_amount')]" v-if="false">
            <tr>
              <td><strong>0</strong></td>
            </tr>
          </the-table>
        </div>
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
      <!-- Edit visit -->
      <modal
        @closeModal="(s.isEditOpen = false), clearValues()"
        :isModalOpen="s.isEditOpen"
        @submitHandler="submitEditHandler"
      >
        <template #title>{{ $t("edit_visit_pharmacy") }}</template>
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
            <div class="input-wrapper input-group">
              <input
                type="text"
                class="input"
                v-model="inputDistributorValue"
                @input="searchDistributorHandler"
                placeholder=" "
                autocomplete="off"
                id="name"
              />

              <label for="name" class="input-label">{{
                Object.values(distributor).length
                  ? $t("organization")
                  : $t("loading")
              }}</label>
              <ul
                class="auto-suggest"
                v-if="
                  inputDistributorValue.length &&
                  searchedDistributors.length > 0
                "
              >
                <li
                  v-for="distributor in searchedDistributors"
                  :key="distributor.id"
                  @click="clickLpuHandler(distributor)"
                >
                  {{ distributor.name }}
                </li>
              </ul>
            </div>
            <div class="form-select">
              <select class="select" @change="changeHandler" name="medrep_id">
                <option value="" disabled>
                  {{ $t("employee") }}
                </option>
                <option
                  :value="user.id"
                  v-for="user in users"
                  :key="user.id"
                  :selected="
                    Object.values(visitDistributor).length
                      ? visitDistributor.medrep.medrep_full_name === user.id
                      : false
                  "
                >
                  {{ user.full_name }}
                </option>
              </select>
            </div>
            <div class="input-group">
              <input
                type="text"
                placeholder=" "
                class="input"
                id="comment"
                v-model="visitDistributor.comment"
                @change="changeHandler"
              />
              <label for="comment" class="input-label">{{
                $t("comment")
              }}</label>
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
                $t("products")
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
            <div class="form-select">
              <select class="select" @change="chooseMarkup">
                <option value="" selected disabled>
                  {{ $t("prepayment") }}
                </option>
                <option
                  v-for="markup in markups"
                  :key="markup.id"
                  :value="markup.id"
                >
                  {{ markup.name }}
                </option>
              </select>
            </div>
            <div class="input-group">
              <input
                type="date"
                placeholder=" "
                class="input"
                id="return_period"
                v-model="return_period"
                @change="changeHandler"
              />
              <label for="return_period" class="input-label">{{
                $t("refund_period")
              }}</label>
            </div>
          </div>
          <div v-if="drugItems.length">
            <the-table
              :head="[
                $t('photo'),
                $t('title_p'),
                $t('quantity_short'),
                $t('amount'),
              ]"
            >
              <tr v-for="b in drugItems" :key="b.id">
                <td><table-user img="no-user.jpg" /></td>
                <td>{{ b.drug_name }}</td>
                <td class="count">
                  <i
                    class="icofont-minus-circle icon-minus"
                    @click="
                      b.drug_in_order_count > 1
                        ? b.drug_in_order_count--
                        : deleteDrug(b.id)
                    "
                  ></i>
                  <input
                    class="quantity-input input"
                    type="text"
                    v-model="b.drug_in_order_count"
                  />
                  <i
                    class="icofont-plus-circle icon-minus"
                    @click="b.drug_in_order_count++"
                  ></i>
                </td>
                <td>{{ prettify(b.base_price) }}</td>
              </tr>
            </the-table>
            <the-table
              :head="[$t('total_amount'), $t('prepayment'), $t('debt')]"
            >
              <tr>
                <td>{{ prettify(totalSum) }}</td>
                <td>
                  {{ prettify(prepaymentSum(totalSum, markup)) }}
                </td>
                <td>
                  {{ prettify(totalSum - prepaymentSum(totalSum, markup)) }}
                </td>
              </tr>
            </the-table>
          </div>
        </div>
        <div v-else-if="topButtons[1].isActive">
          <div class="modal-form mb-10">
            <div class="form-select">
              <select class="select">
                <option>{{ $t("organization") }}</option>
                <option>{{ $t("organization") }}</option>
                <option>{{ $t("organization") }}</option>
              </select>
            </div>
          </div>
          <the-table
            :head="[
              $t('photo'),
              $t('title_p'),
              $t('quantity_short'),
              $t('amount'),
            ]"
            v-if="false"
          >
            <tr v-for="b in []" :key="b.id">
              <td><table-user :img="b.img" /></td>
              <td>{{ b.name }}</td>
              <td class="count">
                <i
                  class="icofont-minus-circle icon-minus"
                  @click="b.count--"
                ></i>
                {{ b.count }}
                <i
                  class="icofont-plus-circle icon-minus"
                  @click="b.count++"
                ></i>
              </td>
              <td>{{ prettify(b.sum) }}</td>
            </tr>
            <tr v-for="b in []" :key="b.id">
              <td><table-user :img="b.img" /></td>
              <td>{{ b.name }}</td>
              <td class="count">
                <i
                  class="icofont-minus-circle icon-minus"
                  @click="b.count--"
                ></i>
                {{ b.count }}
                <i
                  class="icofont-plus-circle icon-minus"
                  @click="b.count++"
                ></i>
              </td>
              <td>{{ prettify(b.sum) }}</td>
            </tr>
            <tr v-for="b in []" :key="b.id">
              <td><table-user :img="b.img" /></td>
              <td>{{ b.name }}</td>
              <td class="count">
                <i
                  class="icofont-minus-circle icon-minus"
                  @click="b.count--"
                ></i>
                {{ b.count }}
                <i
                  class="icofont-plus-circle icon-minus"
                  @click="b.count++"
                ></i>
              </td>
              <td>{{ prettify(b.sum) }}</td>
            </tr>
          </the-table>
          <the-table :head="[$t('total_amount')]" v-if="false">
            <tr>
              <td><strong>4.500.000</strong></td>
            </tr>
          </the-table>
        </div>
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
      <modal-geolocation
        @getCoordinates="getCoordinates"
        :openLocation="openLocation"
        @makeFalse="(openLocation = false), (visit_id = null)"
        title="is_visit_done"
      />
      <modal-small
        :isModalOpen="isCommentOpen"
        @closeModal="(isCommentOpen = false), (modalProps = null)"
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
              <the-table
                :head="[$t('title_p'), $t('quantity_short'), $t('amount')]"
                v-if="modalProps.length"
              >
                <tr v-for="b in modalProps" :key="b.id">
                  <td>{{ b.drug_name }}</td>
                  <td>{{ b.package }}</td>
                  <td>{{ prettify(b.base_price) }}</td>
                </tr>
              </the-table>
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
import ModalGeolocation from "@/components/ModalGeolocation.vue";
import { mapGetters, mapActions } from "vuex";
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
    inputDrugValue: "",
    comment: "",
    return_period: "",
    distributor: {},
    visitDistributor: {},
    drugItems: [],
    visit_id: null,
    topButtons: [
      { id: 0, name: "booking", isActive: true },
      { id: 1, name: "remains", isActive: false },
    ],
    isCommentOpen: false,
    filterItems: [],
    openLocation: false,
    markup: {},
    prepayment: null,
    inputDistributorValue: "",
    inputDrugValue: "",
    isItemsOpen: false,
    modalProps: [],
  }),
  computed: {
    ...mapGetters("visitDistributors", ["visitDistributors", "isLoading"]),
    ...mapGetters("visitStatistics", ["visitDistributorsStatistics"]),
    ...mapGetters("distributors", ["searchedDistributors"]),
    ...mapGetters("drugs", ["searchedDrugs"]),
    ...mapGetters("markups", ["markups"]),
    ...mapGetters("users", ["users"]),
    ...mapGetters(["s"]),
    selectedDrugs() {
      return this.drugItems.map((item) => ({
        drug_id: item.id,
        package: item.drug_in_order_count,
      }));
    },
    totalSum() {
      return this.drugItems.reduce(
        (acc, prev) => (acc += prev.base_price * prev.drug_in_order_count),
        0
      );
    },
  },
  methods: {
    ...mapActions("visitDistributors", [
      "getVisitDistributors",
      "addVisitDistributors",
      "filterVisitDistributors",
      "editVisitDistributor",
    ]),
    ...mapActions("visitStatistics", ["getVisitDistributorsStatistics"]),
    ...mapActions("visitDoctors", ["completeVisit"]),
    ...mapActions("distributors", ["findDistributorsByName"]),
    ...mapActions("drugs", ["getDrugsByName"]),
    ...mapActions("markups", ["getMarkups"]),
    ...mapActions("users", ["getUsers"]),
    setItems(value) {
      this.isItemsOpen = true;
      this.modalProps = value;
    },
    async editHandler(visit) {
      this.s.isEditOpen = true;
      this.visitDistributor = visit;
      // Stores the drugStore
      await this.findDistributorsByName(visit.organization.organization_name);
      this.inputDistributorValue = visit.organization.organization_name;
      this.distributor = visit.organization;
      this.$store.commit("distributors/setSearchedDistributors", []);
      if (visit.drugs.length) {
        this.drugItems = visit.drugs.map((drug) => ({
          drug_name: drug.drug_name,
          base_price: drug.base_price,
          id: drug.drug_id,
          drug_in_order_count: drug.package,
        }));
      }
      this.return_period = visit.date_create.split("T")[0];
    },
    searchDistributorHandler(e) {
      this.distributor = {};
      this.findDistributorsByName(e.target.value);
    },
    async submitEditHandler() {
      this.s.isEditOpen = false;
      const drugs = this.drugItems.map((drug) => ({
        drug_id: drug.id,
        package: drug.drug_in_order_count,
      }));

      const payload = {
        id: this.visitDistributor.visit_id,
        organization_id:
          this.distributor.id || this.distributor.organization_id,
        comment: this.visitDistributor.comment,
        drugs,
        prepayment: this.markup.length ? this.markup[0].percent : 0,
        return_period: this.visitDistributor.date_create,
      };

      await this.editVisitDistributor(payload);
      await this.clearValues();
    },
    async submitFilterHandler() {
      this.s.isFilterOpen = false;
      let payload = {};
      if (this.distributor.name) {
        payload = {
          ...payload,
          org_name: this.distributor.name,
        };
      }
      if (this.visitDistributor.medrep_name) {
        payload = {
          ...payload,
          medrep_name: this.visitDistributor.medrep_name,
        };
      }
      if (this.visitDistributor.inn) {
        payload = {
          ...payload,
          inn: this.visitDistributor.inn,
        };
      }
      if (this.visitDistributor.status) {
        payload = {
          ...payload,
          status: this.visitDistributor.status,
        };
      }
      if (this.visitDistributor.dates_min) {
        payload = {
          ...payload,
          dates_min: this.visitDistributor.dates_min,
        };
      }
      if (this.visitDistributor.dates_max) {
        payload = {
          ...payload,
          dates_max: this.visitDistributor.dates_max,
        };
      }
      if (this.visitDistributor.sales_min) {
        payload = {
          ...payload,
          sales_min: this.visitDistributor.sales_min,
        };
      }
      if (this.visitDistributor.sales_max) {
        payload = {
          ...payload,
          sales_max: this.visitDistributor.sales_max,
        };
      }
      if (this.visitDistributor.prepayments_min) {
        payload = {
          ...payload,
          prepayments_min: this.visitDistributor.prepayments_min,
        };
      }
      if (this.visitDistributor.prepayments_max) {
        payload = {
          ...payload,
          prepayments_max: this.visitDistributor.prepayments_max,
        };
      }
      this.filterItems = Object.values(payload);
      await this.filterVisitDistributors(payload);
    },
    async removeFilterItem(item) {
      this.filterItems = this.filterItems.filter((d) => d !== item);
      let payload = {
        ...this.visitDistributor,
      };
      for (let key in payload) {
        if (payload[key].includes(item)) {
          delete payload[key];
        }
      }
      await this.filterVisitDistributors(payload);
    },
    changeHandler(e) {
      const { name, value } = e.target;
      this.visitDistributor = { ...this.visitDistributor, [name]: value };
    },
    clearValues() {
      this.inputDistributorValue = "";
      this.inputDrugValue = "";
      this.comment = "";
      this.return_period = "";
      this.distributor = {};
      this.drugItems = [];
      this.visitDistributor = {};
      this.visit_id = null;
      this.markup = {};
      this.prepayment = null;
    },
    submitHandler() {
      this.s.isAddOpen = false;
      let payload = {
        organization_id: this.distributor.id,
        comment: this.comment,
        drugs: this.selectedDrugs,
        return_period: this.return_period,
      };
      if (this.markup.length) {
        payload = { ...payload, prepayment: this.markup[0].percent };
      }
      this.addVisitDistributors(payload);
      this.s.isAddOpen = false;
      this.clearValues();
    },
    handleClick(idx) {
      this.topButtons.forEach((btn) => (btn.isActive = false));
      this.topButtons[idx].isActive = true;
    },
    chooseMarkup(e) {
      this.markup = this.markups.filter((m) => m.id === +e.target.value);
    },
    async getCoordinates(coordinates) {
      this.openLocation = false;
      await this.completeVisit({ visit_id: this.visit_id, ...coordinates });
      await this.getVisitDistributors();
      this.visit_id = null;
    },
    clickLpuHandler(store) {
      this.inputDistributorValue = store.name;
      this.$store.commit("distributors/setSearchedDistributors", []);
      this.distributor = store;
    },
    searchDrugHandler(e) {
      this.getDrugsByName(e.target.value);
    },
    clickDrugHandler(drug) {
      this.inputDrugValue = "";
      this.$store.commit("drugs/setSearchedDrugs", []);
      const found = this.drugItems.find((fitem) => fitem.id === drug.id);
      if (!found)
        this.drugItems = [
          { ...drug, drug_in_order_count: 1 },
          ...this.drugItems,
        ];
    },
    prepaymentSum(totalSum, markup) {
      let total = totalSum;
      if (markup.length) {
        total =
          totalSum + Math.floor(totalSum * (markup[0].markup_percent / 100));
        this.prepayment = Math.floor(total * (markup[0].percent / 100));
      } else {
        this.prepayment = total;
      }
      return this.prepayment;
    },
    deleteDrug(id) {
      this.drugItems = this.drugItems.filter((drug) => drug.id !== id);
    },
    setComment(comment) {
      this.isCommentOpen = true;
      this.modalProps = comment;
    },
  },
  async mounted() {
    await this.getVisitDistributorsStatistics();
    await this.getVisitDistributors();
    await this.getMarkups();
    await this.getUsers();
  },
};
</script>

<style>
.icon-minus {
  color: #336cfb;
  cursor: pointer;
}
</style>
