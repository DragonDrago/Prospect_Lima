import axios from "axios";

export default {
  namespaced: true,
  state: () => ({
    markups: [],
    isLoading: false,
  }),
  mutations: {
    setMarkups(state, markups) {
      state.markups = markups;
    },
  },
  actions: {
    async getMarkups({ commit, state }) {
      state.isLoading = true;
      try {
        const res = await axios.get("settings/markups", {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });
        if (res.status === 200) {
          commit("setMarkups", res.data);
        }
      } catch (error) {}

      state.isLoading = false;
    },
    async addMarkup({ dispatch }, markup) {
      await axios("settings/markups/add", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
        data: markup,
      });
      dispatch("getMarkups");
    },
    async updateMarkup({ dispatch }, markup) {
      await axios("settings/markups/update", {
        method: "PUT",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
        data: markup,
      });
      dispatch("getMarkups");
    },
  },
  getters: {
    markups(state) {
      return state.markups;
    },
    isLoading(state) {
      return state.isLoading;
    },
  },
};
