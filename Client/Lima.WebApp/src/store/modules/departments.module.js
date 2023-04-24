import axios from "axios";

export default {
  namespaced: true,
  state: () => ({
    departments: [],
  }),
  mutations: {
    setDepartments(state, departments) {
      state.departments = departments;
    },
  },
  actions: {
    async getDepartments({ commit }) {
      const res = await axios.get("settings/departments", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
      });
      if (res.status === 200) {
        commit("setDepartments", res.data);
      }
    },
  },
  getters: {
    departments(state) {
      return state.departments;
    },
  },
};
