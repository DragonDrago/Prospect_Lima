import axios from "axios";

export default {
  namespaced: true,
  state: () => ({
    statistics: {},
    isLoading: false,
  }),
  mutations: {
    setStatistics(state, statistics) {
      state.statistics = statistics;
    },
  },
  actions: {
    async getStatistics({ commit, state }) {
      state.isLoading = true;
      const res = await axios.get("statistics/common", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
      });
      if (res.status === 200) {
        commit("setStatistics", res.data);
      }
      state.isLoading = false;
    },
  },
  getters: {
    statistics(state) {
      return state.statistics;
    },
    isLoading(state) {
      return state.isLoading;
    },
  },
};
