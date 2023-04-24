<template>
  <div>
    <the-filter
      :filterItems="filterItems"
      @filterClicked="s.isFilterOpen = true"
      @filterAdd="s.isAddOpen = true"
    >
      <template #name>{{ $t("map") }}</template>
    </the-filter>
    <spinner v-show="isLoading" />
    <!-- Checks if is there any event for this profile. If event exist opens first yandex map, if not opens default map.  -->
    <div v-if="!isLoading && maps.length">
      <yandex-map
        :coords="[
          maps[0].organization.organization_latitude,
          maps[0].organization.organization_longitude,
        ]"
        :zoom="10"
        map-type="map"
        ymap-class="yandexmap"
        :controls="['typeSelector', 'fullscreenControl']"
      >
        <div v-if="maps.length">
          <div v-for="map in maps" :key="map.unique_key">
            <ymap-marker
              :coords="[
                map.organization.organization_latitude,
                map.organization.organization_longitude,
              ]"
              :balloon="{
                header: map.user_name,
                body: [
                  map.organization.organization_type_name,
                  map.organization.organization_name,
                ],
                footer: [
                  new Date(map.end_date).toLocaleDateString('ru-RU'),
                  new Date(map.end_date).toLocaleTimeString('ru-RU'),
                ],
              }"
              :hint-content="`
                <h2>${map.user_name}</h2>
                <p>${map.organization.organization_type_name}</p>
                <p>${map.organization.organization_name}</p>
                <h6>${new Date(map.end_date).toLocaleDateString(
                  'ru-RU'
                )}, ${new Date(map.end_date).toLocaleTimeString('ru-RU')}</h6>  
              `"
              :markerId="map.unique_key"
              :icon="{ color: 'blue' }"
              cluster-name="1"
            />

            <ymap-marker
              v-if="map.visit"
              :coords="[map.visit.latitude, map.visit.longitude]"
              :balloon="{
                header: map.user_name,
                body: [
                  map.organization.organization_type_name,
                  map.organization.organization_name,
                  `${$t('status')}: ${map.visit.status_name}`,
                ],
                footer: [
                  new Date(map.end_date).toLocaleDateString('ru-RU'),
                  new Date(map.end_date).toLocaleTimeString('ru-RU'),
                ],
              }"
              :hint-content="`
                <h2>${map.user_name}</h2>
                <p>${map.organization.organization_type_name}</p>
                <p>${map.organization.organization_name}</p>
                <p>${$t('status')}: ${map.visit.status_name}</p>
                <h6>${new Date(map.end_date).toLocaleDateString(
                  'ru-RU'
                )}, ${new Date(map.end_date).toLocaleTimeString('ru-RU')}</h6>  
              `"
              :markerId="map.visit.visit_id"
              :icon="{ color: 'green' }"
            />
          </div>
        </div>
      </yandex-map>
    </div>
    <!-- Default map if there is no event -->
    <iframe
      v-if="isEmpty && !maps.length"
      src="https://yandex.ru/map-widget/v1/?um=constructor%3A39289e8e802d38ff942e9c1e26deb772e27ed9d4778d6e611c5eadf72a620e9d&amp;source=constructor"
      frameborder="0"
    ></iframe>
    <teleport to="body">
      <modal @closeModal="s.isFilterOpen = false" :isModalOpen="s.isFilterOpen">
        <template #title>{{ $t("eventfilter") }}</template>
        <div class="form-select mb-10">
          <select class="select">
            <option>{{ $t("employee") }}</option>
            <option>{{ $t("employee") }}</option>
            <option>{{ $t("employee") }}</option>
          </select>
        </div>
        <div class="modal-form">
          <input type="date" class="input" />
          <input type="date" class="input" />
          <input type="time" class="input" />
          <input type="time" class="input" />
        </div>
        <template #button>
          <button class="btn btn-red" @click.prevent="s.isFilterOpen = false">
            {{ $t("to_close") }}
          </button>
          <button class="btn btn-blue" type="submit">{{ $t("add") }}</button>
        </template>
      </modal>
      <modal
        @closeModal="(s.isAddOpen = false), clearValues()"
        :isModalOpen="s.isAddOpen"
        @submitHandler="submitHandler"
      >
        <template #title>{{ $t("add_an_event") }}</template>
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
                v-model="inputValue"
                @input="findHandler($event, 'drug-stores')"
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
                v-if="searchedDrugStores.length && inputValue.length"
              >
                <li
                  v-for="drug in searchedDrugStores"
                  :key="drug.id"
                  @click="selectHandler(drug, 'drug-stores')"
                >
                  {{ drug.name }}
                </li>
              </ul>
            </div>
            <div class="input-group">
              <input
                type="text"
                placeholder=" "
                class="input"
                name="comment"
                @input="changeHandler"
                id="comment"
              />
              <label for="comment" class="input-label">{{
                $t("comment")
              }}</label>
            </div>
            <div class="input-group">
              <input
                type="date"
                placeholder=" "
                class="input"
                @input="changeHandler"
                name="start_date"
                id="start_date"
              />
              <label for="start_date" class="input-label">{{
                $t("start_date")
              }}</label>
            </div>
            <div class="input-group">
              <input
                type="date"
                placeholder=" "
                class="input"
                @input="changeHandler"
                name="end_date"
                id="end_date"
              />
              <label for="end_date" class="input-label">{{
                $t("date_of_end")
              }}</label>
            </div>
            <div class="input-group">
              <input
                type="time"
                placeholder=" "
                class="input"
                name="end_date"
                id="end_date"
              />
              <label for="end_date" class="input-label">{{
                $t("time_start")
              }}</label>
            </div>
            <div class="input-group">
              <input
                type="time"
                placeholder=" "
                class="input"
                name="end_date"
                id="end_date"
              />
              <label for="end_date" class="input-label">{{
                $t("time_end")
              }}</label>
            </div>
          </div>
          <div class="input-wrapper input-group">
            <input
              type="text"
              class="input"
              v-model="inputUserValue"
              @input="findHandler($event, 'users')"
              placeholder=" "
              autocomplete="off"
              id="users"
            />

            <label for="users" class="input-label">{{
              $t("participants")
            }}</label>
            <ul
              class="auto-suggest"
              v-if="inputUserValue.length && searchedUsers.length > 0"
            >
              <li
                v-for="user in searchedUsers"
                :key="user.id"
                @click="clickUserHandler(user)"
              >
                {{ user.full_name }}
              </li>
            </ul>
          </div>
          <ul class="drug_list">
            <li v-for="user in users" :key="user.id">
              <p>{{ user.full_name }}</p>
              <span @click="deleteUser(user)">&times;</span>
            </li>
          </ul>
        </div>
        <div v-if="topButtons[1].isActive">
          <div class="modal-form mb-10">
            <div class="input-wrapper input-group">
              <input
                type="text"
                class="input"
                v-model="inputValue"
                @input="findHandler($event, 'distributors')"
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
                v-if="searchedDistributors.length && inputValue.length"
              >
                <li
                  v-for="drug in searchedDistributors"
                  :key="drug.id"
                  @click="selectHandler(drug, 'distributors')"
                >
                  {{ drug.name }}
                </li>
              </ul>
            </div>
            <div class="input-group">
              <input
                type="text"
                placeholder=" "
                class="input"
                @input="changeHandler"
                name="comment"
                id="comment"
              />
              <label for="comment" class="input-label">{{
                $t("comment")
              }}</label>
            </div>
            <div class="input-group">
              <input
                type="date"
                placeholder=" "
                class="input"
                @input="changeHandler"
                name="start_date"
                id="start_date"
              />
              <label for="start_date" class="input-label">{{
                $t("start_date")
              }}</label>
            </div>
            <div class="input-group">
              <input
                type="date"
                placeholder=" "
                class="input"
                @input="changeHandler"
                name="end_date"
                id="end_date"
              />
              <label for="end_date" class="input-label">{{
                $t("date_of_end")
              }}</label>
            </div>
            <div class="input-group">
              <input
                type="time"
                placeholder=" "
                class="input"
                name="end_date"
                id="end_date"
              />
              <label for="end_date" class="input-label">{{
                $t("time_start")
              }}</label>
            </div>
            <div class="input-group">
              <input
                type="time"
                placeholder=" "
                class="input"
                name="end_date"
                id="end_date"
              />
              <label for="end_date" class="input-label">{{
                $t("time_end")
              }}</label>
            </div>
          </div>
          <div class="input-wrapper input-group">
            <input
              type="text"
              class="input"
              v-model="inputUserValue"
              @input="findHandler($event, 'users')"
              placeholder=" "
              autocomplete="off"
              id="users"
            />

            <label for="users" class="input-label">{{
              $t("participants")
            }}</label>
            <ul
              class="auto-suggest"
              v-if="inputUserValue.length && searchedUsers.length > 0"
            >
              <li
                v-for="user in searchedUsers"
                :key="user.id"
                @click="clickUserHandler(user)"
              >
                {{ user.full_name }}
              </li>
            </ul>
          </div>
          <ul class="drug_list">
            <li v-for="user in users" :key="user.id">
              <p>{{ user.full_name }}</p>
              <span @click="deleteUser(user)">&times;</span>
            </li>
          </ul>
        </div>
        <div v-else-if="topButtons[2].isActive">
          <div class="input-wrapper input-group mb-10">
            <input
              type="text"
              class="input"
              v-model="inputValue"
              @input="findHandler($event, 'lpus')"
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
              v-if="searchedLpus.length && inputValue.length"
            >
              <li
                v-for="lpu in searchedLpus"
                :key="lpu.id"
                @click="selectHandler(lpu, 'lpus')"
              >
                {{ lpu.name }}
              </li>
            </ul>
          </div>
          <div class="modal-form mb-10">
            <div class="input-wrapper input-group">
              <input
                type="text"
                class="input"
                v-model="doctorsInputValue"
                @input="findHandler($event, 'doctors')"
                placeholder=" "
                autocomplete="off"
                required
                id="name"
              />
              <label for="name" class="input-label">{{
                $t("lfm_doctor")
              }}</label>
              <ul
                class="auto-suggest"
                v-if="searchedDoctors.length && doctorsInputValue.length"
              >
                <li
                  v-for="doctor in searchedDoctors"
                  :key="doctor.id"
                  @click="selectDoctorHandler(doctor)"
                >
                  {{ doctor.full_name }}
                </li>
              </ul>
            </div>
            <div class="input-group">
              <input
                type="text"
                placeholder=" "
                class="input"
                name="position"
                id="position"
                v-model="doctor_position"
                required
              />
              <label for="position" class="input-label">{{
                $t("specialization")
              }}</label>
            </div>
            <div class="input-group">
              <input
                type="text"
                placeholder=" "
                class="input"
                name="phone"
                id="phone"
                v-model="doctor_phone"
                required
              />
              <label for="phone" class="input-label">{{
                $t("telephone")
              }}</label>
            </div>
            <div class="input-group">
              <input
                type="text"
                placeholder=" "
                class="input"
                name="comment"
                id="comment"
                v-model="doctor_comment"
              />
              <label for="comment" class="input-label">{{
                $t("comment")
              }}</label>
            </div>
          </div>
          <div class="input-wrapper input-group mb-10">
            <input
              type="text"
              class="input"
              v-model="inputDrugValue"
              @input="findHandler($event, 'drugs')"
              placeholder=" "
              autocomplete="off"
              id="drugs"
            />

            <label for="drugs" class="input-label">{{
              $t("talking_about_the_product")
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
          <div class="modal-form mb-10">
            <div class="input-group">
              <input
                type="date"
                placeholder=" "
                class="input"
                @input="changeHandler"
                name="start_date"
                id="start_date"
              />
              <label for="start_date" class="input-label">{{
                $t("start_date")
              }}</label>
            </div>
            <div class="input-group">
              <input
                type="date"
                placeholder=" "
                class="input"
                @input="changeHandler"
                name="end_date"
                id="end_date"
              />
              <label for="end_date" class="input-label">{{
                $t("date_of_end")
              }}</label>
            </div>
            <div class="input-group">
              <input
                type="time"
                placeholder=" "
                class="input"
                name="end_date"
                id="end_date"
              />
              <label for="end_date" class="input-label">{{
                $t("time_start")
              }}</label>
            </div>
            <div class="input-group">
              <input
                type="time"
                placeholder=" "
                class="input"
                name="end_date"
                id="end_date"
              />
              <label for="end_date" class="input-label">{{
                $t("time_end")
              }}</label>
            </div>
          </div>
          <div class="input-wrapper input-group">
            <input
              type="text"
              class="input"
              v-model="inputUserValue"
              @input="findHandler($event, 'users')"
              placeholder=" "
              autocomplete="off"
              id="users"
            />

            <label for="users" class="input-label">{{
              $t("participants")
            }}</label>
            <ul
              class="auto-suggest"
              v-if="inputUserValue.length && searchedUsers.length > 0"
            >
              <li
                v-for="user in searchedUsers"
                :key="user.id"
                @click="clickUserHandler(user)"
              >
                {{ user.full_name }}
              </li>
            </ul>
          </div>

          <div class="bottom-wrapper">
            <div class="bottom-item" v-if="drugItems.length">
              <h4>{{ $t("products") }}</h4>
              <ul class="drug_list">
                <li v-for="item in drugItems" :key="item.id">
                  <p>{{ item.drug_name }}</p>
                  <span @click="deleteDrug(item)">&times;</span>
                </li>
              </ul>
            </div>
            <div class="bottom-item" v-if="users.length">
              <h4>{{ $t("participants") }}</h4>
              <ul class="drug_list">
                <li v-for="user in users" :key="user.id">
                  <p>{{ user.full_name }}</p>
                  <span @click="deleteUser(user)">&times;</span>
                </li>
              </ul>
            </div>
          </div>
        </div>
        <div v-else-if="topButtons[3].isActive">
          <input type="text" class="input mb-10" placeholder="Комментарий" />

          <div class="modal-form mb-10">
            <input type="date" class="input" />
            <input type="date" class="input" />
            <input type="time" class="input" />
            <input type="time" class="input" />
          </div>
          <div class="form-select">
            <select class="select">
              <option>{{ $t("participants") }}</option>
              <option>{{ $t("participants") }}</option>
              <option>{{ $t("participants") }}</option>
            </select>
          </div>
          <div class="modal-color modal-repeat">
            <div class="modal-color__title">{{ $t("repeat") }}</div>
            <input type="checkbox" />
            <input type="text" value="10" />
            <p>{{ $t("day") }}</p>
          </div>
        </div>
        <div class="modal-color">
          <div class="modal-color__title">{{ $t("event_color") }}</div>
          <span
            v-for="(c, i) in colors"
            :key="c.color"
            :class="`bg-${c.color} ${c.isSelected ? 'color-selected' : null}`"
            @click="chooseColor(c, i)"
          ></span>
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
    </teleport>
  </div>
</template>

<script>
import Modal from "@/components/Modal.vue";
import Teleport from "vue2-teleport";
import TheFilter from "@/components/TheFilter.vue";
import { mapActions, mapGetters } from "vuex";
import Spinner from "@/components/Spinner.vue";
import { yandexMap, ymapMarker } from "vue-yandex-maps";
export default {
  components: { Modal, Teleport, TheFilter, Spinner, yandexMap, ymapMarker },
  data() {
    return {
      options: {
        enableHighAccuracy: true,
        timeout: 5000,
        maximumAge: 0,
      },
      colors: [
        { color: "green", isSelected: true },
        { color: "blue", isSelected: false },
        { color: "grey", isSelected: false },
        { color: "yellow", isSelected: false },
        { color: "red", isSelected: false },
      ],
      topButtons: [
        { id: 0, name: "pharmacy", isActive: true },
        { id: 1, name: "wholesalers", isActive: false },
        { id: 2, name: "doctor", isActive: false },
        { id: 3, name: "task", isActive: false },
      ],
      filterItems: [],
      drugItems: [],
      drugIds: [],
      users: [],
      userIds: [],
      inputValue: "",
      color_id: 1,
      item: {},
      doctorsInputValue: "",
      doctor_position: "",
      doctor_phone: "",
      doctor_comment: "",
      inputDrugValue: "",
      inputUserValue: "",
    };
  },
  computed: {
    ...mapGetters("map", ["maps", "isLoading"]),
    ...mapGetters("drugStores", ["searchedDrugStores"]),
    ...mapGetters("distributors", ["searchedDistributors"]),
    ...mapGetters("lpus", ["searchedLpus"]),
    ...mapGetters("doctors", ["searchedDoctors"]),
    ...mapGetters("drugs", ["searchedDrugs"]),
    ...mapGetters("users", ["searchedUsers"]),
    ...mapGetters(["s"]),
  },
  methods: {
    ...mapActions("map", ["getMaps", "addEvent"]),
    ...mapActions("drugStores", ["findDrugStoresByName"]),
    ...mapActions("distributors", ["findDistributorsByName"]),
    ...mapActions("doctors", ["findDoctorByName"]),
    ...mapActions("drugs", ["getDrugsByName"]),
    ...mapActions("lpus", ["findLpuByName"]),
    ...mapActions("users", ["findUsersByName"]),
    handleClick(idx) {
      this.topButtons.forEach((btn) => {
        btn.isActive = false;
        this.clearValues();
      });
      this.topButtons[idx].isActive = true;
    },
    findHandler(e, element) {
      if (element === "distributors") {
        this.findDistributorsByName(e.target.value);
      } else if (element === "drug-stores") {
        this.findDrugStoresByName(e.target.value);
      } else if (element === "lpus") {
        this.findLpuByName(e.target.value);
      } else if (element === "doctors") {
        this.findDoctorByName(e.target.value);
      } else if (element === "drugs") {
        this.getDrugsByName(e.target.value);
      } else if (element === "users") {
        this.findUsersByName(e.target.value);
      }
    },
    selectHandler(item, element) {
      if (element === "distributors") {
        this.$store.commit("distributors/setSearchedDistributors", []);
      } else if (element === "drug-stores") {
        this.$store.commit("drugStores/setSearchedDrugStores", []);
      } else if (element === "lpus") {
        this.$store.commit("lpus/setSearchedLpus", []);
      }
      this.inputValue = item.name;
      this.item = {
        organization_id: item.id,
        type_id: item.type_id,
      };
    },
    selectDoctorHandler(doctor) {
      this.$store.commit("doctors/setSearchedDoctors", []);
      this.doctorsInputValue = doctor.full_name;
      this.doctor_position = doctor.position ?? "";
      this.doctor_phone = doctor.phone ?? "";
      this.doctor_comment = doctor.comment ?? "";
    },

    changeHandler(e) {
      const { name, value } = e.target;
      this.item = { ...this.item, [name]: value };
    },
    chooseColor(c, i) {
      this.colors.forEach((c) => (c.isSelected = false));
      c.isSelected = true;
      this.color_id = i + 1;
    },
    clickUserHandler(user) {
      this.inputUserValue = "";
      this.$store.commit("users/setSearchedUsers", []);
      const found = this.users.find((u) => u.id === user.id);
      if (!found) {
        this.users = [...this.users, user];
        this.userIds = [...this.userIds, user.id];
      }
    },
    clickDrugHandler(item) {
      this.inputDrugValue = "";
      this.$store.commit("drugs/setSearchedDrugs", []);
      const found = this.drugItems.find((fitem) => fitem.id === item.id);
      if (!found) {
        this.drugItems = [...this.drugItems, item];
        this.drugIds = [...this.drugIds, item.id];
      }
    },
    deleteDrug(item) {
      this.drugItems = this.drugItems.filter((di) => di.id !== item.id);
    },
    deleteUser(user) {
      this.users = this.users.filter((di) => di.id !== user.id);
    },
    submitHandler() {
      let payload = {
        ...this.item,
        color_id: this.color_id,
        is_task: false,
      };
      if (this.doctorsInputValue.length && this.doctor_position.length) {
        payload = {
          ...payload,
          doctor_name: this.doctorsInputValue,
          doctor_position: this.doctor_position,
        };
      }
      if (this.drugIds.length) {
        payload = {
          ...payload,
          drugs: this.drugIds,
        };
      }
      if (this.userIds.length) {
        payload = {
          ...payload,
          members: this.userIds,
        };
      }
      this.addEvent(payload);
      this.clearValues();
      this.s.isAddOpen = false;
    },
    clearValues() {
      this.inputValue = "";
      this.item = "";
      this.drugItems = [];
      this.drugIds = [];
      this.doctorsInputValue = "";
      this.doctor_position = "";
      this.doctor_phone = "";
      this.doctor_comment = "";
      this.inputDrugValue = "";
      this.users = [];
      this.userIds = "";
      this.inputUserValue = "";
    },
  },
  async mounted() {
    await this.getMaps();
  },
};
</script>

<style lang="scss">
.yandexmap,
iframe {
  display: block;
  height: 80vh;
  width: 100%;
}
.color-selected {
  border: 2px solid #222;
}
.bottom-wrapper {
  margin-top: 1rem;
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 10px;
  .drug_list {
    margin-top: 0;
  }
}
</style>
