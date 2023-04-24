<template>
  <div class="sales page">
    <the-filter
      :filterItems="filterItems"
      @filterClicked="s.isFilterOpen = true"
      @filterAdd="s.isAddOpen = true"
      @removeItem="removeFilterItem"
    >
      <template #name>{{ $t("visits_to_the_pharmacy") }}</template>
    </the-filter>
    <div class="statistics-menu statistic-anything">
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">{{ $t("total_spent") }}</div>
          <div class="statistics-menu__num">
            {{ visitPharmaciesStatistics.compleated || $t("empty") }}
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
            {{ visitPharmaciesStatistics.planned || $t("empty") }}
          </div>
        </div>
      </div>
      <div class="statistics-menu__card">
        <i class="icofont-user-alt-6"></i>
        <div class="statistics-menu__content">
          <div class="statistics-menu__title">{{ $t("total_overdue") }}</div>
          <div class="statistics-menu__num">
            {{ visitPharmaciesStatistics.overdue || $t("empty") }}
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
            {{ prettify(+visitPharmaciesStatistics.orders) || $t("empty") }}
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
            {{ prettify(+visitPharmaciesStatistics.prepayments) }}
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
            {{ prettify(+visitPharmaciesStatistics.overdue) }}
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
          $t('pharmacy'),
          $t('comment'),
          $t('product'),
          $t('total_amount'),
          $t('prepayment'),
          $t('debt'),
          $t('status'),
        ]"
        class="visit-doctor-table"
        v-if="visitPharmacies.length"
      >
        <tr v-for="b in visitPharmacies" :key="b.visit_id">
          <td>
            <table-user :name="b.medrep.medrep_full_name" img="no-user.jpg" />
          </td>
          <td>{{ new Date(b.date_create).toLocaleDateString("ru-RU") }}</td>
          <td>{{ new Date(b.date_create).toLocaleTimeString("ru-RU") }}</td>
          <td>
            {{ b.organization ? b.organization.organization_name : "Пусто" }}
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
      <empty v-else v-show="isEmpty" :item="visitPharmacies" />
    </div>
    <teleport to="body">
      <!-- Filter visit -->
      <modal
        @closeModal="(s.isFilterOpen = false), clearValues()"
        :isModalOpen="s.isFilterOpen"
        @submitHandler="submitFilterHandler"
      >
        <template #title>{{ $t("filter_visits_to_the_pharmacy") }}</template>

        <div class="modal-form mb-10">
          <div class="input-wrapper input-group">
            <input
              type="text"
              class="input"
              v-model="inputDrugStoreValue"
              @input="searchDrugStoreHandler"
              placeholder=" "
              autocomplete="off"
              id="name"
            />

            <label for="name" class="input-label">{{
              $t("organization")
            }}</label>
            <ul
              class="auto-suggest"
              v-if="inputDrugStoreValue.length && searchedDrugStores.length > 0"
            >
              <li
                v-for="store in searchedDrugStores"
                :key="store.id"
                @click="clickLpuHandler(store)"
              >
                {{ store.name }}
              </li>
            </ul>
          </div>
          <div class="form-select">
            <select class="select" @change="changeHandler" name="medrep_id">
              <option value="" disabled selected>{{ $t("employee") }}</option>
              <option :value="user.id" v-for="user in users" :key="user.id">
                {{ user.full_name }}
              </option>
            </select>
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
        </div>
        <div class="modal-form">
          <div class="input-group">
            <input
              type="date"
              placeholder=" "
              class="input"
              id="date_min"
              name="date_min"
              @change="changeHandler"
            />
            <label for="date_min" class="input-label">{{ $t("date_h") }}</label>
          </div>
          <div class="input-group">
            <input
              type="date"
              placeholder=" "
              class="input"
              id="date_max"
              name="date_max"
              @change="changeHandler"
            />
            <label for="date_max" class="input-label">{{ $t("date_o") }}</label>
          </div>
          <div class="input-group">
            <input
              type="number"
              placeholder=" "
              class="input"
              id="sales_min"
              name="sales_min"
              @change="changeHandler"
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
              id="sales_max"
              name="sales_max"
              @change="changeHandler"
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
              id="prepayments_min"
              name="prepayments_min"
              @change="changeHandler"
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
              id="prepayments_max"
              name="prepayments_max"
              @change="changeHandler"
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
              id="inn"
              name="inn"
              @change="changeHandler"
            />
            <label for="inn" class="input-label">{{ $t("intr") }}</label>
          </div>
          <div class="form-select">
            <select class="select" @change="changeHandler" name="status">
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
      <!-- Add visit -->
      <modal
        @closeModal="(s.isAddOpen = false), clearValues()"
        :isModalOpen="s.isAddOpen"
        @submitHandler="submitHandler"
      >
        <template #title>{{ $t("add_a_visit_to_the_pharmacy") }}</template>
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
                v-model="inputDrugStoreValue"
                @input="searchDrugStoreHandler"
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
                  inputDrugStoreValue.length && searchedDrugStores.length > 0
                "
              >
                <li
                  v-for="store in searchedDrugStores"
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
                v-model="inputDrugStoreValue"
                @input="searchDrugStoreHandler"
                placeholder=" "
                autocomplete="off"
                id="name"
              />

              <label for="name" class="input-label">{{
                Object.values(drugStore).length
                  ? $t("organization")
                  : $t("loading")
              }}</label>
              <ul
                class="auto-suggest"
                v-if="
                  inputDrugStoreValue.length && searchedDrugStores.length > 0
                "
              >
                <li
                  v-for="store in searchedDrugStores"
                  :key="store.id"
                  @click="clickLpuHandler(store)"
                >
                  {{ store.name }}
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
                    Object.values(visitPharmacie).length
                      ? visitPharmacie.medrep.medrep_full_name === user.id
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
                v-model="visitPharmacie.comment"
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
import { mapActions, mapGetters } from "vuex";
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
    inputDrugStoreValue: "",
    inputDrugValue: "",
    comment: "",
    return_period: "",
    drugStore: {},
    drugItems: [],
    visit_id: null,
    inputDoctorValue: "",
    topButtons: [
      { id: 0, name: "booking", isActive: true },
      { id: 1, name: "remains", isActive: false },
    ],
    isCommentOpen: false,
    modalProps: null,
    filterItems: [],
    openLocation: false,
    markup: [],
    prepayment: null,
    visitPharmacie: {},
    doctor: {},
    isItemsOpen: false,
    modalProps: [],
  }),
  computed: {
    ...mapGetters("visitPharmacies", ["visitPharmacies", "isLoading"]),
    ...mapGetters("visitStatistics", ["visitPharmaciesStatistics"]),
    ...mapGetters("drugStores", ["searchedDrugStores"]),
    ...mapGetters("drugs", ["searchedDrugs"]),
    ...mapGetters("markups", ["markups"]),
    ...mapGetters("doctors", ["searchedDoctors"]),
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
    ...mapActions("visitPharmacies", [
      "getVisitPharmacies",
      "addVisitPharmacies",
      "filterVisitPharmacies",
      "editVisitPharmacy",
    ]),
    ...mapActions("visitStatistics", ["getVisitPharmaciesStatistics"]),
    ...mapActions("drugStores", ["findDrugStoresByName"]),
    ...mapActions("drugs", ["getDrugsByName"]),
    ...mapActions("markups", ["getMarkups"]),
    ...mapActions("users", ["getUsers"]),
    ...mapActions("visitDoctors", ["completeVisit"]),
    ...mapActions("doctors", ["findDoctorByName"]),
    setItems(value) {
      this.isItemsOpen = true;
      this.modalProps = value;
    },
    async editHandler(visit) {
      this.s.isEditOpen = true;
      this.visitPharmacie = visit;

      // Stores the drugStore
      await this.findDrugStoresByName(visit.organization.organization_name);
      this.inputDrugStoreValue = visit.organization.organization_name;
      this.drugStore = visit.organization;
      this.$store.commit("drugStores/setSearchedDrugStores", []);
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
    async submitEditHandler() {
      this.s.isEditOpen = false;
      const drugs = this.drugItems.map((drug) => ({
        drug_id: drug.id,
        package: drug.drug_in_order_count,
      }));
      const payload = {
        id: this.visitPharmacie.visit_id,
        organization_id: this.drugStore.id || this.drugStore.organization_id,
        comment: this.visitPharmacie.comment,
        drugs,
        prepayment: this.markup.length ? this.markup[0].percent : 0,
        return_period: this.visitPharmacie.date_create,
      };
      await this.editVisitPharmacy(payload);
      await this.clearValues();
    },
    async submitFilterHandler() {
      this.s.isFilterOpen = false;
      let payload = {
        ...this.visitPharmacie,
      };
      if (Object.values(this.drugStore).length) {
        payload = {
          ...payload,
          org_id: this.drugStore.id,
        };
      }
      if (Object.values(this.doctor).length) {
        payload = {
          ...payload,
          medrep_id: this.doctor.id,
        };
      }
      this.filterItems = Object.values(payload);
      await this.filterVisitPharmacies(payload);
      this.clearValues();
    },
    async removeFilterItem(item) {
      this.filterItems = this.filterItems.filter((d) => d !== item);
      let payload = {
        ...this.visitPharmacie,
      };
      for (let key in payload) {
        if (payload[key].includes(item)) {
          delete payload[key];
        }
      }
      await this.filterVisitPharmacies(payload);
    },
    changeHandler(e) {
      const { value, name } = e.target;
      this.visitPharmacie = { ...this.visitPharmacie, [name]: value };
    },
    searchDoctorsHandler(e) {
      this.doctor = {};
      this.findDoctorByName(e.target.value);
    },
    clickDoctorHandler(doctor) {
      this.inputDoctorValue = doctor.full_name;
      this.doctor = doctor;
      this.$store.commit("doctors/setSearchedDoctors", []);
    },
    clearValues() {
      this.inputDrugStoreValue = "";
      this.inputDrugValue = "";
      this.comment = "";
      this.return_period = "";
      this.drugStore = {};
      this.drugItems = [];
      this.visit_id = null;
      this.markup = {};
      this.prepayment = null;
      this.inputDoctorValue = "";
    },
    submitHandler() {
      let payload = {
        organization_id: this.drugStore.id,
        comment: this.comment,
        drugs: this.selectedDrugs,
        return_period: this.return_period,
      };
      if (this.markup.length) {
        payload = { ...payload, prepayment: this.markup[0].percent };
      }
      this.addVisitPharmacies(payload);
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
      await this.getVisitPharmacies();
      this.visit_id = null;
    },
    searchDrugStoreHandler(e) {
      this.drugStore = {};
      this.findDrugStoresByName(e.target.value);
    },
    clickLpuHandler(store) {
      this.inputDrugStoreValue = store.name;
      this.drugStore = store;
      this.$store.commit("drugStores/setSearchedDrugStores", []);
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
    await this.getVisitPharmaciesStatistics();
    await this.getVisitPharmacies();
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
