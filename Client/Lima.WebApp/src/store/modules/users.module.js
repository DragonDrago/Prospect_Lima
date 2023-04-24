import axios from "axios";

export default {
  namespaced: true,
  state: () => ({
    users: [],
    prefix: null,
    searchedUsers: [],
    isLoading: false,
  }),
  mutations: {
    setUsers(state, users) {
      state.users = users;
    },
    setPrefix(state, prefix) {
      state.prefix = prefix;
    },
    setSearchedUsers(state, searchedUsers) {
      state.searchedUsers = searchedUsers;
    },
  },
  actions: {
    async getUsers({ commit, state }) {
      state.isLoading = true;
      const res = await axios.get("settings/users", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
      });
      if (res.status === 200) {
        commit("setUsers", res.data);
      }
      state.isLoading = false;
    },
    async addUser({ dispatch }, user) {
      await axios("settings/users/add", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          "Content-Type": "application/json",
        },
        data: user,
      });
      dispatch("getUsers");
    },
    async getPrefix({ commit }) {
      const { data } = await axios.get("settings/company", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
      });
      commit("setPrefix", data.prefix);
    },
    async findUsersByName({ commit }, txt) {
      if (txt.length > 0) {
        const { data } = await axios.get(`company/users/find/${txt}`, {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });
        commit("setSearchedUsers", data);
      }
    },
  },
  getters: {
    users(state) {
      return state.users;
    },
    prefix(state) {
      return state.prefix;
    },
    searchedUsers(state) {
      return state.searchedUsers;
    },
    isLoading(state) {
      return state.isLoading;
    },
  },
};
