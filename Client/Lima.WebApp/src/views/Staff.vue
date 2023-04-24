<template>
  <div class="settings page presentation">
    <the-filter @filterAdd="s.isAddOpen = true" :isfilter="true">
      <template #name>{{ $t("staff") }}</template>
    </the-filter>
    <spinner v-if="isLoading" />
    <div v-else>
      <the-table
        :head="[
          $t('photo'),
          $t('lfm'),
          $t('date_of_birth'),
          $t('telephone'),
          $t('pesidential_address'),
          $t('post'),
          $t('login'),
          $t('role'),
        ]"
        class="staff-table"
        v-if="users.length"
      >
        <tr v-for="b in users" :key="b.id">
          <td><table-user img="no-user.jpg" /></td>
          <td>{{ b.full_name }}</td>
          <td>{{ new Date(b.birthday).toLocaleDateString("ru-RU") }}</td>
          <td>{{ b.phone || $t("empty") }}</td>
          <td>{{ b.address || $t("empty") }}</td>
          <td>{{ b.position || $t("empty") }}</td>
          <td>{{ b.login }}</td>
          <td>{{ b.role_name }}</td>
          <td><table-buttons /></td>
        </tr>
      </the-table>
      <empty v-else v-show="isEmpty" :item="users" />
    </div>
    <teleport to="body">
      <modal
        @closeModal="(s.isAddOpen = false), (user = {})"
        :isModalOpen="s.isAddOpen"
        @submitHandler="submitAddHandler"
      >
        <template #title>{{ $t("add_an_employee") }}</template>
        <div class="input-group mb-10">
          <input
            type="text"
            placeholder=" "
            class="input"
            @input="changeHandler"
            name="full_name"
            id="full_name"
          />
          <label for="full_name" class="input-label">{{ $t("lfm") }}</label>
        </div>
        <div class="modal-form">
          <div class="input file-input">
            <p>{{ $t("choose") }}...</p>
            <button class="btn btn-blue">
              <i class="icofont-clip"></i>
              <span>{{ $t("file") }}</span>
            </button>
            <input type="file" />
          </div>

          <div class="input-group">
            <input
              type="date"
              placeholder=" "
              class="input"
              @input="changeHandler"
              name="birthday"
              id="birthday"
            />
            <label for="birthday" class="input-label">{{
              $t("date_of_birth")
            }}</label>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              @input="changeHandler"
              name="phone"
              id="phone"
            />
            <label for="phone" class="input-label">{{ $t("telephone") }}</label>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              @input="changeHandler"
              name="address"
              id="address"
            />
            <label for="address" class="input-label">{{ $t("address") }}</label>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              @input="changeHandler"
              name="position"
              id="position"
            />
            <label for="position" class="input-label">{{ $t("post") }}</label>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              @input="changeHandler"
              name="login"
              id="login"
            />
            <label for="login" class="input-label">{{ $t("login") }}</label>
          </div>
          <div class="input-group">
            <input
              type="password"
              placeholder=" "
              class="input"
              @input="changeHandler"
              name="password"
              id="password"
            />
            <label for="password" class="input-label">{{
              $t("password")
            }}</label>
          </div>
          <div class="form-select">
            <select class="select">
              <option value="" selected disabled>{{ $t("regions") }}</option>
              <option v-for="branch in branches" :key="branch.id">
                {{ branch.name }}
              </option>
            </select>
          </div>
          <div class="form-select">
            <select class="select">
              <option>{{ $t("department") }}</option>
              <option>{{ $t("department") }}</option>
              <option>{{ $t("department") }}</option>
            </select>
          </div>
          <div class="form-select">
            <select class="select" @change="changeHandler" name="role_id">
              <option value="" selected disabled>{{ $t("role") }}</option>
              <option v-for="role in roles" :key="role.id" :value="role.id">
                {{ role.desc }}
              </option>
            </select>
          </div>
        </div>
        <template #button>
          <button
            class="btn btn-red"
            @click.prevent="(s.isAddOpen = false), (user = {})"
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
import TheFilter from "@/components/TheFilter.vue";
import TheTable from "@/components/TheTable.vue";
import TableButtons from "@/components/TableButtons.vue";
import TableUser from "@/components/TableUser.vue";
import Teleport from "vue2-teleport";
import Modal from "@/components/Modal.vue";
import { mapActions, mapGetters } from "vuex";
import Spinner from "@/components/Spinner.vue";
import Empty from "@/components/Empty.vue";
export default {
  components: {
    TheFilter,
    TheTable,
    TableButtons,
    TableUser,
    Teleport,
    Modal,
    Spinner,
    Empty,
  },
  computed: {
    ...mapGetters("users", ["users", "prefix", "roles", "isLoading"]),
    ...mapGetters("branches", ["branches"]),
    ...mapGetters("departments", ["departments"]),
    ...mapGetters("roles", ["roles"]),
    ...mapGetters(["s"]),
  },
  methods: {
    ...mapActions("users", ["getUsers", "addUser", "getPrefix", "getRoles"]),
    ...mapActions("branches", ["getBranches"]),
    ...mapActions("departments", ["getDepartments"]),
    ...mapActions("roles", ["getRoles"]),
    changeHandler(e) {
      const { name, value } = e.target;
      this.user = { ...this.user, [name]: value };
    },
    async submitChangeHandler() {
      this.s.isEditOpen = false;
      await this.editDoctors(this.doctor).then(() => (this.doctor = {}));
    },
    async submitAddHandler() {
      this.s.isAddOpen = false;

      await this.addUser({
        ...this.user,
        login: `${this.prefix}_${this.user.login}`,
      }).then(() => (this.user = {}));
    },
  },
  async mounted() {
    await this.getUsers();
    await this.getPrefix();
    await this.getRoles();
    await this.getBranches();
    // await this.getDepartments();
  },
};
</script>

<style></style>
