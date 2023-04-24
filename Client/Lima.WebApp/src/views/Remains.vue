<template>
  <div class="sales booking">
    <the-filter :filterItems="filterItems" :fullbar="true">
      <template #name>{{ $t("remains") }}</template>
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

    <the-table
      v-if="false"
      :head="[
        '№',
        $t('organization'),
        $t('wholesaler'),
        $t('cheque'),
        '№ ' + $t('booking'),
        $t('amount'),
        $t('prepayment'),
        $t('debt'),
      ]"
      class="remains-table"
    >
      <tr v-for="b in []" :key="b.id">
        <td>{{ b.number }}</td>
        <td>{{ b.organization }}</td>
        <td>{{ b.seller }}</td>
        <td>{{ b.numberBrone }}</td>
        <td>{{ b.check }}</td>
        <td>{{ prettify(b.sum) }}</td>
        <td>{{ prettify(b.booking) }}</td>
        <td>{{ prettify(b.left) }}</td>
        <td><table-buttons /></td>
      </tr>
    </the-table>
    <empty v-else v-show="isEmpty" :item="[]" />
    <teleport to="body">
      <modal @closeModal="isModalOpen = false" :isModalOpen="isModalOpen">
        <template #title> {{ $t("filter_remains") }}</template>

        <div class="modal-form">
          <input type="text" class="input" :placeholder="$t('number')" />
          <div class="form-select">
            <select class="select">
              <option>{{ $t("employee") }}</option>
              <option>{{ $t("employee") }}</option>
              <option>{{ $t("employee") }}</option>
            </select>
          </div>
          <div class="form-select">
            <select class="select">
              <option>{{ $t("pharmacy") }}</option>
              <option>{{ $t("pharmacy") }}</option>
              <option>{{ $t("pharmacy") }}</option>
            </select>
          </div>
          <div class="form-select">
            <select class="select">
              <option>{{ $t("wholesaler") }}</option>
              <option>{{ $t("wholesaler") }}</option>
              <option>{{ $t("wholesaler") }}</option>
            </select>
          </div>
          <input type="text" class="input" :placeholder="$t('date_h')" />
          <input type="text" class="input" :placeholder="$t('date_o')" />
          <input
            type="text"
            :placeholder="$t('total_amount_with')"
            class="input"
          />
          <input
            type="text"
            :placeholder="$t('total_amount_for')"
            class="input"
          />
          <input
            type="text"
            :placeholder="$t('amount_of_prepayment_from')"
            class="input"
          />
          <input
            type="text"
            :placeholder="$t('amount_of_prepayment_by')"
            class="input"
          />
          <input
            type="text"
            :placeholder="$t('amount_of_debt_with')"
            class="input"
          />
          <input
            type="text"
            :placeholder="$t('amount_of_debt_for')"
            class="input"
          />
          <input
            type="text"
            :placeholder="$t('amount_of_sum_left')"
            class="input"
          />
          <input type="text" :placeholder="$t('check_number')" class="input" />
          <input type="text" :placeholder="$t('intr')" class="input" />
        </div>
        <template #button>
          <button class="btn btn-red" @click="isModalOpen = false">
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
import TableUser from "@/components/TableUser.vue";
import TheFilter from "@/components/TheFilter.vue";
import Modal from "@/components/Modal.vue";
import Empty from "@/components/Empty.vue";
import Teleport from "vue2-teleport";
import { prettify } from "../use/PrettifySum.js";

export default {
  components: {
    TheTable,
    TableButtons,
    TableUser,
    TheFilter,
    Modal,
    Teleport,
    Empty,
  },
  data: () => ({
    prettify,
    isModalOpen: false,
    isAddOpen: false,
    filterItems: [],
  }),
};
</script>

<style></style>
