import axios from "axios";

export default {
  namespaced: true,
  state: () => ({
    units: [],
  }),
  mutations: {
    setUnits(state, units) {
      state.units = units;
    },
  },
  actions: {
    async getUnits({ commit }) {
      const res = await axios.get("dict/drugs/units", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
      });
      if (res.status === 200) {
        commit("setUnits", res.data);
      }
    },
  },
  getters: {
    units(state) {
      return state.units;
    },
  },
};
