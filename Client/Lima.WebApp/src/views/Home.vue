<template>
  <div class="page statistics">
    <spinner v-if="isLoading" />
    <div v-else>
      <div class="statistics-menu" v-if="Object.values(statistics).length">
        <div class="statistics-menu__card" @click="$router.push('/map')">
          <i class="icofont-cart"></i>
          <div class="statistics-menu__content">
            <div class="statistics-menu__title">{{ $t("events") }}</div>
            <div class="statistics-menu__num">{{ statistics.events }}</div>
          </div>
        </div>
        <div class="statistics-menu__card" @click="$router.push('/calendar')">
          <i class="icofont-id"></i>
          <div class="statistics-menu__content">
            <div class="statistics-menu__title">{{ $t("tasks") }}</div>
            <div class="statistics-menu__num">749</div>
          </div>
        </div>
        <div class="statistics-menu__card" @click="$router.push('/list-lpu')">
          <i class="icofont-network"></i>
          <div class="statistics-menu__content">
            <div class="statistics-menu__title">
              {{ $t("medical_institution") }}
            </div>
            <div class="statistics-menu__num">{{ statistics.lpu }}</div>
          </div>
        </div>
        <div class="statistics-menu__card" @click="$router.push('/remains')">
          <i class="icofont-users-alt-3"></i>
          <div class="statistics-menu__content">
            <div class="statistics-menu__title">{{ $t("remains") }}</div>
            <div class="statistics-menu__num">
              {{ statistics.drug_balance }}
            </div>
          </div>
        </div>
        <div
          class="statistics-menu__card"
          @click="$router.push('/list-wholesalers')"
        >
          <i class="icofont-sign-in"></i>
          <div class="statistics-menu__content">
            <div class="statistics-menu__title">{{ $t("wholesalers") }}</div>
            <div class="statistics-menu__num">
              {{ statistics.distributors }}
            </div>
          </div>
        </div>
        <div
          class="statistics-menu__card"
          @click="$router.push('/list-pharmacy')"
        >
          <i class="icofont-hospital"></i>
          <div class="statistics-menu__content">
            <div class="statistics-menu__title">{{ $t("pharmacies") }}</div>
            <div class="statistics-menu__num">{{ statistics.pharmacies }}</div>
          </div>
        </div>
        <div class="statistics-menu__card" @click="$router.push('/list-goods')">
          <i class="icofont-pay"></i>
          <div class="statistics-menu__content">
            <div class="statistics-menu__title">{{ $t("products") }}</div>
            <div class="statistics-menu__num">{{ statistics.drugs }}</div>
          </div>
        </div>
        <div class="statistics-menu__card" @click="$router.push('/booking')">
          <i class="icofont-tasks"></i>
          <div class="statistics-menu__content">
            <div class="statistics-menu__title">{{ $t("reserve") }}</div>
            <div class="statistics-menu__num">{{ statistics.orders }}</div>
          </div>
        </div>
        <div
          class="statistics-menu__card"
          @click="$router.push('/list-doctors')"
        >
          <i class="icofont-doctor"></i>
          <div class="statistics-menu__content">
            <div class="statistics-menu__title">{{ $t("doctors") }}</div>
            <div class="statistics-menu__num">{{ statistics.doctors }}</div>
          </div>
        </div>
        <div class="statistics-menu__card" @click="$router.push('/staff')">
          <i class="icofont-people"></i>
          <div class="statistics-menu__content">
            <div class="statistics-menu__title">{{ $t("employees") }}</div>
            <div class="statistics-menu__num">{{ statistics.employees }}</div>
          </div>
        </div>
        <div
          class="statistics-menu__card"
          @click="$router.push('/visit-pharmacy')"
        >
          <i class="icofont-contacts"></i>
          <div class="statistics-menu__content">
            <div class="statistics-menu__title">
              {{ $t("visits_to_pharmacies") }}
            </div>
            <div class="statistics-menu__num">
              {{ statistics.visits_to_pharmacies }}
            </div>
          </div>
        </div>
        <div
          class="statistics-menu__card"
          @click="$router.push('/visit-doctor')"
        >
          <i class="icofont-medicine"></i>
          <div class="statistics-menu__content">
            <div class="statistics-menu__title">
              {{ $t("visits_to_doctors") }}
            </div>
            <div class="statistics-menu__num">
              {{ statistics.visits_to_doctors }}
            </div>
          </div>
        </div>
      </div>

      <!-- Chart component-->
      <chart :chart_title="$t('monthly_sales')" />

      <!-- Rating -->
      <div class="rating">
        <rating :data="ratingBestData">
          <template #title>{{ $t("the_best_indicators") }}</template>
        </rating>
        <rating :data="ratingWorstData">
          <template #title>{{ $t("bad_performance") }}</template>
        </rating>
      </div>
      <div class="rating">
        <rating>
          <template #title>{{
            $t("the_orders_this_month_by_region")
          }}</template>
          <pie-chart>
            <template #text> 60% </template>
          </pie-chart>
        </rating>
        <rating>
          <template #title>{{ $t("items_sold_this_month") }}</template>
          <pie-chart>
            <template #text> 90% </template>
          </pie-chart>
        </rating>
      </div>
      <!-- The Table component -->
      <the-table
        v-if="lastLpuVisits.length"
        :isAction="true"
        :head="[
          $t('employee'),
          $t('date'),
          $t('time'),
          $t('organization'),
          $t('doctor'),
          $t('post'),
          $t('telephone'),
          $t('comment'),
          $t('product'),
          $t('status'),
        ]"
      >
        <template #title>{{ $t("recent_visits_to_doctors") }}</template>
        <tr v-for="b in lastLpuVisits" :key="b.visit_id">
          <td>
            <table-user
              :name="b.medrep.medrep_full_name"
              :img="'no-user.jpg'"
            />
          </td>
          <td>{{ new Date(b.date_create).toLocaleDateString("ru-RU") }}</td>
          <td>{{ new Date(b.date_create).toLocaleTimeString("ru-RU") }}</td>
          <td>{{ b.organization.organization_name }}</td>
          <td>{{ b.doctor ? b.doctor.doctor_name : $t("empty") }}</td>
          <td>{{ b.doctor ? b.doctor.doctor_position : $t("empty") }}</td>
          <td>{{ b.doctor ? b.doctor.doctor_phone : $t("empty") }}</td>
          <td>
            <i class="icofont-eye font-eye" @click="setComment(b.comment)"></i>
          </td>
          <td>
            <i
              class="icofont-eye font-eye"
              @click="setItems(b.talked_about_drugs)"
            ></i>
          </td>
          <td :class="sortStatusClass(b.visit_status_id)">
            {{ b.visit_status }}
          </td>
        </tr>
      </the-table>

      <!-- The Table component -->
      <the-table
        :head="[
          $t('employee'),
          $t('date'),
          $t('time'),
          $t('organization'),
          $t('wholesaler'),
          $t('sales_p'),
          $t('status'),
        ]"
      >
        <template #title>{{
          $t("recent_visits_to_pharmacies_and_wholesalers")
        }}</template>
        <tr v-for="b in visitToPharmacy" :key="b.id">
          <td>
            <table-user :img="b.img" :name="b.name" />
          </td>
          <td>{{ b.date }}</td>
          <td>{{ b.time }}</td>
          <td>{{ b.organization }}</td>
          <td>{{ b.client }}</td>
          <td>{{ b.salesNumber }}</td>
          <td>{{ b.status }}</td>
          <td>
            <table-buttons />
          </td>
        </tr>
        <template #pagination>
          <pagination />
        </template>
      </the-table>
    </div>

    <teleport to="body">
      <!-- Comment modal -->
      <modal-small
        :isModalOpen="isCommentOpen"
        @closeModal="(isCommentOpen = false), (modalProps = [])"
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
              <li v-for="item in modalProps" :key="item.drug_id">
                {{ item.drug_name }}
              </li>
            </template>
            <li v-else>{{ $t("empty") }}</li>
          </ul>
        </template>
      </modal-small>
    </teleport>
  </div>
</template>

<script>
import Chart from "@/components/Chart.vue";
import TheTable from "@/components/TheTable.vue";
import TheNavbar from "@/components/TheNavbar.vue";
import Pagination from "@/components/Pagination.vue";
import TableButtons from "@/components/TableButtons.vue";
import TableUser from "@/components/TableUser.vue";
import Rating from "@/components/Rating.vue";
import PieChart from "@/components/PieChart.vue";
import { sortStatusClass, sortStatus } from "../use/SortStatus";
import { mapActions, mapGetters } from "vuex";
import Spinner from "@/components/Spinner.vue";
import Teleport from "vue2-teleport";
import ModalSmall from "@/components/ModalSmall.vue";
export default {
  components: {
    Chart,
    TheTable,
    TheNavbar,
    Pagination,
    TableButtons,
    TableUser,
    Rating,
    PieChart,
    Spinner,
    ModalSmall,
    Teleport,
  },
  data() {
    return {
      visitToPharmacy: [
        {
          id: 0,
          img: "/img/profile.46a4cc36.jpg",
          name: "Азиза",
          date: "01.01.22",
          time: "12:30",
          organization: "Аптека Таблетка",
          client: "Аптека Таблетка",
          status: "Успешно",
          salesNumber: "№ чека",
        },
        {
          id: 1,
          img: "/img/profile.46a4cc36.jpg",
          name: "Азиза",
          date: "01.01.22",
          time: "12:30",
          organization: "Аптека Таблетка",
          client: "Аптека Таблетка",
          status: "Успешно",
          salesNumber: "№ чека",
        },
        {
          id: 2,
          img: "/img/profile.46a4cc36.jpg",
          name: "Азиза",
          date: "01.01.22",
          time: "12:30",
          organization: "Аптека Таблетка",
          client: "Аптека Таблетка",
          status: "Успешно",
          salesNumber: "№ чека",
        },
      ],
      ratingBestData: {
        region: "Хорезм",
        doctor: "Азаматов З.",
        by_month: "Февраль",
        organization: "Клиника №5",
        by_profit: 1500200,
        stuff: "Тишаева М.",
      },
      ratingWorstData: {
        region: "Фергана",
        doctor: "Тулкунов П.",
        by_month: "Январь",
        organization: "Клиника №6",
        by_profit: 520000,
        stuff: "Мухиддинов Ф.",
      },
      sortStatusClass,
      sortStatus,
      isCommentOpen: false,
      modalProps: [],
      isItemsOpen: false,
      isLoading: true,
    };
  },
  computed: {
    ...mapGetters("statistics", ["statistics"]),
    ...mapGetters("lpus", ["lastLpuVisits"]),
  },
  methods: {
    ...mapActions("statistics", ["getStatistics"]),
    ...mapActions("lpus", ["getLastLpuVisits"]),
    setComment(value) {
      this.isCommentOpen = true;
      this.modalProps = value;
    },
    setItems(value) {
      this.isItemsOpen = true;
      this.modalProps = value;
    },
  },
  async mounted() {
    await this.getStatistics();
    await this.getLastLpuVisits();
  },
  created() {
    setTimeout(() => (this.isLoading = false), 500);
  },
};
</script>
