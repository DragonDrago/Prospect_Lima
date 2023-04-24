import axios from "axios";

export default {
  namespaced: true,
  state: () => ({
    countries: {},
  }),
  mutations: {
    setCountries(state, countries) {
      state.countries = countries;
    },
  },
  actions: {
    async getCountries({ commit }) {
      const res = await axios.get("dict/countries", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
      });
      if (res.status === 200) {
        commit("setCountries", res.data);
      }
    },
  },
  getters: {
    countries(state) {
      return state.countries;
    },
  },
};
