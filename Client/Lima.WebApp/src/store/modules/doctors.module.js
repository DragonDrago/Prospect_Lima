import axios from "axios";
import queryString from "query-string";

export default {
  namespaced: true,
  state: () => ({
    doctors: [],
    searchedDoctors: [],
    isLoading: false,
  }),
  mutations: {
    setDoctors(state, doctors) {
      state.doctors = doctors;
    },
    setSearchedDoctors(state, searchedDoctors) {
      state.searchedDoctors = searchedDoctors;
    },
  },
  actions: {
    async getDoctors({ commit, state }) {
      state.isLoading = true;
      const res = await axios.get("dict/doctors/all", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
      });
      if (res.status === 200) {
        commit("setDoctors", res.data);
      }
      state.isLoading = false;
    },
    async editDoctors({ dispatch }, doctor) {
      await axios(`dict/doctors/update/${doctor.id}`, {
        method: "PUT",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
        data: doctor,
      });
      dispatch("getDoctors");
    },
    async addDoctor({ dispatch }, doctor) {
      await axios("dict/doctors/add", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
        data: doctor,
      });
      dispatch("getDoctors");
    },
    async filterDoctors({ commit, state }, doctor) {
      state.isLoading = true;
      const query = queryString.stringify(doctor);
      try {
        const res = await axios(`dict/doctors/all?${query}`, {
          method: "GET",
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });

        if (res.status === 200) {
          commit("setDoctors", res.data);
        }
      } catch (error) {
        console.error(error);
      }
      state.isLoading = false;
    },
    async findDoctorByName({ commit }, txt) {
      if (txt.length > 0) {
        const { data } = await axios.get(`dict/doctors/find/${txt}`, {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });
        commit("setSearchedDoctors", data);
      }
    },
  },
  getters: {
    doctors(state) {
      return state.doctors;
    },
    searchedDoctors(state) {
      return state.searchedDoctors;
    },
    isLoading(state) {
      return state.isLoading;
    },
  },
};
