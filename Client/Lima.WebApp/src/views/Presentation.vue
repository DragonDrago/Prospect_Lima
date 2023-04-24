<template>
  <div class="settings presentation">
    <the-filter @filterAdd="isAddOpen = true" :isfilter="true">
      <template #name>{{ $t("presentations") }}</template>
    </the-filter>
    <the-table :head="[$t('title_p'), $t('files')]" class="rules-table">
      <tr v-for="b in tableBody" :key="b.id">
        <td>{{ b.name }}</td>
        <td><i style="font-size: 1.2rem" class="icofont-eye"></i></td>
        <td><table-buttons /></td>
      </tr>
    </the-table>
    <teleport to="body">
      <modal @closeModal="isAddOpen = false" :isModalOpen="isAddOpen">
        <template #title>{{ $t("add_a_presentation") }}</template>
        <input type="text" :placeholder="$t('title_p')" class="input mb-10" />
        <div class="modal-form" style="align-items: center">
          <div class="input file-input" style="">
            <p>{{ $t("choose") }}...</p>
            <button class="btn btn-blue">
              <i class="icofont-clip"></i> <span>{{ $t("file") }}</span>
            </button>
            <input type="file" />
          </div>
          <p style="color: #336cfb; font-weight: 600">+{{ $t("else") }}</p>
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
import TheFilter from "../components/TheFilter.vue";
import TheTable from "../components/TheTable.vue";
import TableButtons from "../components/TableButtons.vue";
import Teleport from "vue2-teleport";
import Modal from "@/components/Modal.vue";

export default {
  components: { TheFilter, TheTable, TableButtons, Teleport, Modal },
  data() {
    return {
      tableBody: [{ id: 0, name: "Продукция фарм" }],
      isAddOpen: false,
    };
  },
};
</script>
