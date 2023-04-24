import axios from "axios";
import queryString from "query-string";

export default {
  namespaced: true,
  state: () => ({
    visitDistributors: [],
    isLoading: false,
  }),
  mutations: {
    setVisitDistributors(state, visitDistributors) {
      state.visitDistributors = visitDistributors;
    },
  },
  actions: {
    async getVisitDistributors({ commit, state }) {
      state.isLoading = true;
      const res = await axios.get("visits/distributors", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
      });
      state.isLoading = false;
      if (res.status === 200) {
        commit("setVisitDistributors", res.data);
      }
    },
    async addVisitDistributors({ dispatch }, visitDistributor) {
      await axios("visits/distributors/add", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
        data: visitDistributor,
      });
      dispatch("getVisitDistributors");
    },
    async filterVisitDistributors({ commit }, visitDistributor) {
      const query = queryString.stringify({
        medrep_name: visitDistributor.medrep_name,
        dates: [visitDistributor.dates_min, visitDistributor.dates_max],
        sales: [visitDistributor.sales_min, visitDistributor.sales_max],
        prepayments: [
          visitDistributor.prepayments_min,
          visitDistributor.prepayments_max,
        ],
        status: visitDistributor.status,
        org_name: visitDistributor.org_name,
        inn: visitDistributor.inn,
      });
      try {
        const res = await axios.get(`visits/distributors?${query}`, {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });
        if (res.status === 200) {
          commit("setVisitDistributors", res.data);
        }
      } catch (error) {}
    },
    async editVisitDistributor({ dispatch }, payload) {
      await axios(`visits/distributors/update/${payload.id}`, {
        method: "PUT",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
        data: payload,
      });
      await dispatch("getVisitDistributors");
    },
  },
  getters: {
    visitDistributors(state) {
      return state.visitDistributors;
    },
    isLoading(state) {
      return state.isLoading;
    },
  },
};
