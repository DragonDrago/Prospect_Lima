import axios from "axios";

export default {
  namespaced: true,
  state: () => ({
    user: {},
  }),
  mutations: {
    setUser(state, user) {
      state.user = user;
    },
  },
  actions: {
    async getUser({ commit }) {
      const { data } = await axios.get("company/profile", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
      });
      commit("setUser", data);
    },
  },
  getters: {
    user(state) {
      return state.user;
    },
  },
};
