import axios from "axios";

export default {
  namespaced: true,
  state: () => ({
    roles: [],
    isLoading: false,
    settingsRoles: [],
  }),
  mutations: {
    setRoles(state, roles) {
      state.roles = roles;
    },
    setSettingsRoles(state, settingsRoles) {
      state.settingsRoles = settingsRoles;
    },
  },
  actions: {
    async getRoles({ commit, state }) {
      state.isLoading = true;
      try {
        const res = await axios.get("company/roles", {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });
        if (res.status === 200) {
          commit("setRoles", res.data);
        }
      } catch (error) {}
      state.isLoading = false;
    },
  },
  async getSettingsRoles({ commit, state }) {
    state.isLoading = true;
    try {
      const res = await axios.get("settings/roles", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
      });
      if (res.status === 200) {
        commit("setSettingsRoles", res.data);
      }
    } catch (error) {}
    state.isLoading = false;
  },
  getters: {
    roles(state) {
      return state.roles;
    },
    settingsRoles(state) {
      return state.settingsRoles;
    },
    isLoading(state) {
      return state.isLoading;
    },
  },
};
