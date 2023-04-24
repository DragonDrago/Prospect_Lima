<template>
  <div class="settings page presentation">
    <the-filter @filterAdd="s.isAddOpen = true" :isfilter="true">
      <template #name>{{ $t("settings_regions") }}</template>
    </the-filter>
    <spinner v-if="isLoading" />

    <div v-else>
      <the-table
        v-if="branches.length"
        :head="[
          $t('title_p'),
          $t('address'),
          $t('chief'),
          $t('department'),
          $t('employees'),
        ]"
        class="rules-table"
      >
        <tr v-for="b in branches" :key="b.id">
          <td>{{ b.name }}</td>
          <td>{{ b.address || "Пусто" }}</td>
          <td>{{ b.boss || "Пусто" }}</td>
          <td>{{ b.departments_count }}</td>
          <td>{{ b.employee_count }}</td>
          <td><table-buttons /></td>
        </tr>
      </the-table>
      <empty v-else v-show="isEmpty" :item="branches" />
    </div>
    <teleport to="body">
      <modal
        @closeModal="s.isAddOpen = false"
        :isModalOpen="s.isAddOpen"
        @submitHandler="submitAddHandler"
      >
        <template #title>{{ $t("add_a_region") }}</template>
        <div class="form-select mb-10">
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              name="name"
              @input="changeHandler"
              id="name"
              required
            />
            <label for="name" class="input-label">{{ $t("title_p") }}</label>
          </div>
        </div>
        <div class="modal-form mb-10">
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              name="address"
              @input="changeHandler"
              id="address"
              required
            />
            <label for="address" class="input-label">{{ $t("address") }}</label>
          </div>
          <div class="form-select">
            <select class="select" name="chief_user_id" @change="changeHandler">
              <option selected disabled>{{ $t("chief") }}</option>
              <option v-for="user in users" :key="user.id" :value="user.id">
                {{ user.full_name }}
              </option>
            </select>
          </div>
        </div>
        <template #button>
          <button class="btn btn-red" @click.prevent="s.isAddOpen = false">
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
import { mapGetters, mapActions } from "vuex";
import Spinner from "@/components/Spinner.vue";
import Empty from "@/components/Empty.vue";
export default {
  components: {
    TheFilter,
    TheTable,
    TableButtons,
    Teleport,
    Modal,
    Spinner,
    Empty,
  },
  data() {
    return {
      user: {},
    };
  },
  computed: {
    ...mapGetters("branches", ["branches", "isLoading"]),
    ...mapGetters("users", ["users"]),
    ...mapGetters(["s"]),
  },
  methods: {
    ...mapActions("branches", ["getBranches", "addBranch"]),
    ...mapActions("users", ["getUsers"]),
    changeHandler(e) {
      const { name, value } = e.target;
      this.user = { ...this.user, [name]: value };
    },
    submitAddHandler() {
      this.s.isAddOpen = false;
      this.addBranch(this.user);
      this.user = {};
    },
  },
  async mounted() {
    await this.getBranches();
    await this.getUsers();
  },
};
</script>
