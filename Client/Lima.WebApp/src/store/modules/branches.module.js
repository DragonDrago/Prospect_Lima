import axios from "axios";

export default {
  namespaced: true,
  state: () => ({
    branches: [],
    isLoading: false,
  }),
  mutations: {
    setBranches(state, branches) {
      state.branches = branches;
    },
  },
  actions: {
    async getBranches({ commit, state }) {
      state.isLoading = true;
      try {
        const res = await axios.get("settings/branches", {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });
        if (res.status === 200) {
          commit("setBranches", res.data);
        }
      } catch (error) {}
      state.isLoading = false;
    },
    async addBranch({ dispatch }, branch) {
      await axios("settings/branches/add", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
        data: branch,
      });
      dispatch("getBranches");
    },
  },
  getters: {
    branches(state) {
      return state.branches;
    },
    isLoading(state) {
      return state.isLoading;
    },
  },
};
