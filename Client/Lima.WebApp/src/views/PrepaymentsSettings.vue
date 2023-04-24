<template>
  <div class="settings presentation">
    <the-filter @filterAdd="s.isAddOpen = true" :isfilter="true">
      <template #name>{{ $t("prepayment_and_markup_settings") }}</template>
    </the-filter>
    <spinner v-if="isLoading" />
    <div v-else>
      <the-table
        :head="[$t('title_p'), $t('percent'), $t('extra_charge')]"
        class="rules-table"
        v-if="markups.length"
      >
        <tr v-for="b in markups" :key="b.id">
          <td>{{ b.name }}</td>
          <td>{{ b.percent + "%" }}</td>
          <td>{{ b.markup_percent + "%" }}</td>
          <td><table-buttons @edit="getEdit(b)" /></td>
        </tr>
      </the-table>
      <empty v-else v-show="isEmpty" :item="markups" />
    </div>

    <teleport to="body">
      <modal
        @closeModal="s.isAddOpen = false"
        :isModalOpen="s.isAddOpen"
        @submitHandler="submitHandler"
      >
        <template #title>{{ $t("add_prepayment") }}</template>
        <div class="input-group mb-10">
          <input
            type="text"
            placeholder=" "
            class="input"
            id="name"
            name="name"
            @input="changeHandler"
          />
          <label for="name" class="input-label">{{ $t("title_p") }}</label>
        </div>
        <div class="modal-form mb-10">
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              id="percent"
              name="percent"
              @input="changeHandler"
            />
            <label for="percent" class="input-label">{{ $t("percent") }}</label>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              id="markup_percent"
              name="markup_percent"
              @input="changeHandler"
            />
            <label for="markup_percent" class="input-label">{{
              $t("extra_charge")
            }}</label>
          </div>
        </div>
        <template #button>
          <button
            class="btn btn-red"
            @click.prevent="(s.isAddOpen = false), (markup = {})"
          >
            {{ $t("to_close") }}
          </button>
          <button class="btn btn-blue" type="submit">{{ $t("add") }}</button>
        </template>
      </modal>
      <modal
        @closeModal="s.isEditOpen = false"
        :isModalOpen="s.isEditOpen"
        @submitHandler="editHandler"
      >
        <template #title>{{ $t("edit_prepayment") }}</template>
        <div class="input-group mb-10">
          <input
            type="text"
            placeholder=" "
            class="input"
            id="name"
            name="name"
            :value="markup.name"
            @input="changeHandler"
          />
          <label for="name" class="input-label">{{ $t("title_p") }}</label>
        </div>
        <div class="modal-form mb-10">
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              id="percent"
              name="percent"
              :value="markup.percent"
              @input="changeHandler"
            />
            <label for="percent" class="input-label">{{ $t("percent") }}</label>
          </div>
          <div class="input-group">
            <input
              type="text"
              placeholder=" "
              class="input"
              id="markup_percent"
              name="markup_percent"
              :value="markup.markup_percent"
              @input="changeHandler"
            />
            <label for="markup_percent" class="input-label">{{
              $t("extra_charge")
            }}</label>
          </div>
        </div>
        <template #button>
          <button
            class="btn btn-red"
            @click.prevent="(s.isEditOpen = false), (markup = {})"
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
      markup: {},
    };
  },
  computed: {
    ...mapGetters(["s"]),
    ...mapGetters("markups", ["markups", "isLoading"]),
  },
  methods: {
    ...mapActions("markups", ["getMarkups", "addMarkup", "updateMarkup"]),
    changeHandler(e) {
      const { name, value } = e.target;
      this.markup = { ...this.markup, [name]: value };
    },
    submitHandler() {
      this.s.isAddOpen = false;
      this.addMarkup(this.markup);
      this.markup = {};
    },
    getEdit(markup) {
      this.s.isEditOpen = true;
      this.markup = markup;
    },
    editHandler() {
      this.s.isEditOpen = false;
      this.updateMarkup(this.markup);
      this.markup = {};
    },
  },
  async mounted() {
    await this.getMarkups();
  },
};
</script>
