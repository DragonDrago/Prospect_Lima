<template>
  <div class="settings presentation">
    <the-filter @filterAdd="isAddOpen = true" :isfilter="true">
      <template #name>{{ $t("settings_departments") }}</template>
    </the-filter>
    <the-table
      :head="[$t('department'), $t('chief'), $t('employees')]"
      class="rules-table"
    >
      <tr v-for="b in tableBody" :key="b.id">
        <td>{{ b.otdel }}</td>
        <td>{{ b.boss }}</td>
        <td>{{ b.stuff }}</td>
        <td><table-buttons /></td>
      </tr>
    </the-table>
    <teleport to="body">
      <modal @closeModal="isAddOpen = false" :isModalOpen="isAddOpen">
        <template #title>{{ $t("add_a_department") }}</template>
        <div class="form-select mb-10">
          <select class="select">
            <option>{{ $t("title_p") }}</option>
            <option>{{ $t("title_p") }}</option>
            <option>{{ $t("title_p") }}</option>
          </select>
        </div>
        <div class="modal-form mb-10">
          <input type="text" placeholder="Регион" class="input" />
          <div class="form-select">
            <select class="select">
              <option>{{ $t("chief") }}</option>
              <option>{{ $t("chief") }}</option>
              <option>{{ $t("chief") }}</option>
            </select>
          </div>
        </div>
        <template #button>
          <button class="btn btn-red" @click="isAddOpen = false">
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
import TheTable from "@/components/TheTable.vue";
import TableButtons from "@/components/TableButtons.vue";
import Teleport from "vue2-teleport";
import Modal from "@/components/Modal.vue";
export default {
  components: { TheFilter, TheTable, TableButtons, Teleport, Modal },
  data() {
    return {
      tableBody: [
        {
          id: 0,
          otdel: "Продажи",
          boss: "Ниязов Алишер",
          stuff: 5,
        },
        {
          id: 1,
          otdel: "Маркетинг",
          boss: "Акрамов Лазиз",
          stuff: 11,
        },
      ],
      isAddOpen: false,
    };
  },
};
</script>
