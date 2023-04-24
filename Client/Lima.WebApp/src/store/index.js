import Vue from "vue";
import Vuex from "vuex";
// Modules
import auth from "./modules/auth.module";
import statistics from "./modules/statistics.module";
import countries from "./modules/countries.module";
import distributors from "./modules/distributors.module";
import doctors from "./modules/doctors.module";
import drugs from "./modules/drugs.module";
import lpus from "./modules/lpu.module";
import drugStores from "./modules/drugStores.module";
import map from "./modules/map.module";
import users from "./modules/users.module";
import inn from "./modules/inn.module";
import visitDoctors from "./modules/visitDoctors.module";
import visitDistributors from "./modules/visitDistributors.module";
import visitPharmacies from "./modules/visitPharmacies.module";
import visitStatistics from "./modules/visitStatistics.module";
import branches from "./modules/branches.module";
import departments from "./modules/departments.module";
import roles from "./modules/roles.module";
import producers from "./modules/producers.module";
import units from "./modules/units.module";
import markups from "./modules/markups.module";
import series from "./modules/series.module";
import orders from "./modules/orders.module";
import sales from "./modules/sales.module";
import license from "./modules/license.module";
import user from "./modules/user.module";

Vue.use(Vuex);

export default new Vuex.Store({
  modules: {
    auth,
    statistics,
    countries,
    distributors,
    doctors,
    drugs,
    lpus,
    drugStores,
    map,
    user,
    users,
    inn,
    visitDoctors,
    visitDistributors,
    visitPharmacies,
    visitStatistics,
    branches,
    departments,
    roles,
    producers,
    units,
    markups,
    series,
    orders,
    sales,
    license,
  },
  state: {
    currentTime: null,
    interval: null,
    s: {
      isFilterOpen: false,
      isAddOpen: false,
      isEditOpen: false,
    },
    details: {},
  },
  getters: {
    s(state) {
      return state.s;
    },
    details(state) {
      return state.details;
    },
  },
  mutations: {
    SET_DATE(state, date) {
      state.currentTime = date;
    },
    CLEAR_INTERVAL(state) {
      clearInterval(state.interval);
    },
    SET_DETAILS(state, details) {
      state.details = details;
    },
  },
  actions: {
    renderDate({ commit, state }) {
      const zeros = (i) => {
        return i < 10 ? "0" + i : i;
      };
      const setDate = (date) => {
        return `${zeros(date.getHours())}:${zeros(date.getMinutes())}:${zeros(
          date.getSeconds()
        )}`;
      };
      commit("SET_DATE", setDate(new Date()));
      state.interval = setInterval(() => {
        commit("SET_DATE", setDate(new Date()));
      }, 1000);
    },
  },
});
