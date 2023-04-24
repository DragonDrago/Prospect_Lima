import axios from "axios";

export default {
  namespaced: true,
  state: () => ({
    visitDoctorStatistics: {},
    visitPharmaciesStatistics: {},
    visitDistributorsStatistics: {},
  }),
  mutations: {
    setVisitDoctorStatistics(state, visitDoctorStatistics) {
      state.visitDoctorStatistics = visitDoctorStatistics;
    },
    setVisitPharmaciesStatistics(state, visitPharmaciesStatistics) {
      state.visitPharmaciesStatistics = visitPharmaciesStatistics;
    },
    setVisitDistributorsStatistics(state, visitDistributorsStatistics) {
      state.visitDistributorsStatistics = visitDistributorsStatistics;
    },
  },
  actions: {
    async getVisitDoctorStatistics({ commit }) {
      const res = await axios.get("visits/doctors/statistics", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
      });
      if (res.status === 200) {
        commit("setVisitDoctorStatistics", res.data);
      }
    },
    async getVisitPharmaciesStatistics({ commit }) {
      const res = await axios.get("visits/pharmacies/statistics", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
      });
      if (res.status === 200) {
        commit("setVisitPharmaciesStatistics", res.data);
      }
    },
    async getVisitDistributorsStatistics({ commit }) {
      const res = await axios.get("visits/distributors/statistics", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
      });
      if (res.status === 200) {
        commit("setVisitDistributorsStatistics", res.data);
      }
    },
  },
  getters: {
    visitDoctorStatistics(state) {
      return state.visitDoctorStatistics;
    },
    visitPharmaciesStatistics(state) {
      return state.visitPharmaciesStatistics;
    },
    visitDistributorsStatistics(state) {
      return state.visitDistributorsStatistics;
    },
  },
};
