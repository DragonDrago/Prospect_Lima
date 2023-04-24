import axios from "axios";

export default {
  namespaced: true,
  state: () => ({
    producers: [],
  }),
  mutations: {
    setProducers(state, producers) {
      state.producers = producers;
    },
  },
  actions: {
    async getProducers({ commit }) {
      const res = await axios.get("dict/producers", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
      });
      if (res.status === 200) {
        commit("setProducers", res.data);
      }
    },
  },
  getters: {
    producers(state) {
      return state.producers;
    },
  },
};
