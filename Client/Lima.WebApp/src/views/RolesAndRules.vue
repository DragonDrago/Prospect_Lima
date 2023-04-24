<template>
  <div class="settings page presentation">
    <the-filter @filterAdd="isAddOpen = true" :isfilter="true">
      <template #name>{{ $t("role_settings_and_permissions") }}</template>
    </the-filter>
    <spinner v-if="isLoading" />
    <div v-else>
      <the-table
        :head="[$t('role'), $t('staff')]"
        class="rules-table"
        v-if="roles.length"
      >
        <tr v-for="b in roles" :key="b.id">
          <td>{{ b.desc }}</td>
          <td>{{ 5 }}</td>
          <td><table-buttons /></td>
        </tr>
      </the-table>
      <empty v-else v-show="isEmpty" :item="roles" />
    </div>
    <teleport to="body">
      <modal
        @closeModal="isAddOpen = false"
        :isModalOpen="isAddOpen"
        class="rules-modal"
      >
        <template #title>{{ $t("add_a_role_and_rights") }}</template>
        <div class="modal-form mb-10">
          <input type="text" :placeholder="$t('title_p')" class="input" />
          <div class="form-select">
            <select class="select">
              <option>{{ $t("employee") }}</option>
              <option>{{ $t("employee") }}</option>
              <option>{{ $t("employee") }}</option>
            </select>
          </div>
        </div>
        <div class="rules-modal__wrapper">
          <div class="rules-modal__box">
            <div class="modal-title rules-title">{{ $t("sales") }}</div>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("view") }}</p>
            </label>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("editing") }}</p>
            </label>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("deleting") }}</p>
            </label>
          </div>
          <div class="rules-modal__box">
            <div class="modal-title rules-title">{{ $t("map") }}</div>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("view") }}</p>
            </label>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("editing") }}</p>
            </label>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("deleting") }}</p>
            </label>
          </div>
          <div class="rules-modal__box">
            <div class="modal-title rules-title">{{ $t("products") }}</div>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("view") }}</p>
            </label>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("editing") }}</p>
            </label>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("deleting") }}</p>
            </label>
          </div>
          <div class="rules-modal__box">
            <div class="modal-title rules-title">
              {{ $t("pharmacies") }}/{{ $t("wholesalers") }}
            </div>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("view") }}</p>
            </label>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("editing") }}</p>
            </label>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("deleting") }}</p>
            </label>
          </div>
          <div class="rules-modal__box">
            <div class="modal-title rules-title">
              {{ $t("visits_to_pharmacies") }}/{{ $t("visits_to_wholesalers") }}
            </div>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("view") }}</p>
            </label>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("editing") }}</p>
            </label>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("deleting") }}</p>
            </label>
          </div>
          <div class="rules-modal__box">
            <div class="modal-title rules-title">
              {{ $t("visits_to_the_medical_institution") }}/{{
                $t("visits_to_doctors")
              }}
            </div>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("view") }}</p>
            </label>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("editing") }}</p>
            </label>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("deleting") }}</p>
            </label>
          </div>
          <div class="rules-modal__box">
            <div class="modal-title rules-title">{{ $t("events") }}</div>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("view") }}</p>
            </label>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("editing") }}</p>
            </label>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("deleting") }}</p>
            </label>
          </div>
          <div class="rules-modal__box">
            <div class="modal-title rules-title">{{ $t("doctors") }}</div>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("view") }}</p>
            </label>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("editing") }}</p>
            </label>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("deleting") }}</p>
            </label>
          </div>
          <div class="rules-modal__box">
            <div class="modal-title rules-title">
              {{ $t("medical_institution") }}
            </div>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("view") }}</p>
            </label>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("editing") }}</p>
            </label>
            <label class="login-form__container">
              <input type="checkbox" class="login-form__checkbox" />
              <span class="login-form__checkmark"></span>
              <p>{{ $t("deleting") }}</p>
            </label>
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
      isAddOpen: false,
    };
  },
  computed: {
    ...mapGetters("roles", ["roles", "isLoading"]),
  },
  methods: {
    ...mapActions("roles", ["getRoles"]),
  },
  async mounted() {
    await this.getRoles();
  },
};
</script>
