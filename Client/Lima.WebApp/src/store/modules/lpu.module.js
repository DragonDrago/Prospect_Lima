import axios from "axios";
import queryString from "query-string";

export default {
  namespaced: true,
  state: () => ({
    lpus: [],
    isLoading: false,
    searchedLpus: [],
    lastLpuVisits: [],
    pages: {},
  }),
  mutations: {
    setLpus(state, lpus) {
      state.lpus = lpus;
    },
    setSearchedLpus(state, searchedLpus) {
      state.searchedLpus = searchedLpus;
    },
    setLpuPage(state, pages) {
      state.pages = pages;
    },
    setLastLpuVisits(state, lastLpuVisits) {
      state.lastLpuVisits = lastLpuVisits;
    },
  },
  actions: {
    async getLpus({ commit, state }, page) {
      state.isLoading = true;
      const res = await axios.get(
        `dict/organizations/health-care-facility?page=${page}`,
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        }
      );
      if (res.status === 200) {
        commit("setLpus", res.data.result);
        commit("setLpuPage", res.data.page);
      }
      state.isLoading = false;
    },
    async filterLpus({ commit, state }, lpu) {
      const query = queryString.stringify(lpu);
      state.isLoading = true;
      const res = await axios.get(
        `dict/organizations/health-care-facility?${query}`,
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        }
      );
      if (res.status === 200) {
        commit("setLpus", res.data);
      }
      state.isLoading = false;
    },
    async createLpus({ dispatch }, lpu) {
      await axios("dict/organizations/health-care-facility/add", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
        data: lpu,
      });
      dispatch("getLpus");
    },
    async findLpuByName({ commit }, txt) {
      if (txt.length > 0) {
        const { data } = await axios.get(
          `dict/organizations/health-care-facility/find/${txt}`,
          {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
            },
          }
        );
        commit("setSearchedLpus", data);
      }
    },
    async editLpus({ dispatch }, lpu) {
      await axios(
        `/dict/organizations/health-care-facility/update/${lpu.organization.id}`,
        {
          method: "PUT",
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
          data: lpu,
        }
      );
      dispatch("getLpus");
    },
    async getLastLpuVisits({ commit }) {
      try {
        const res = await axios.get(`visits/doctors/last`, {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });
        if (res.status === 200) {
          commit("setLastLpuVisits", res.data);
        }
      } catch (error) {}
    },
  },
  getters: {
    lpus(state) {
      return state.lpus;
    },
    isLoading(state) {
      return state.isLoading;
    },
    searchedLpus(state) {
      return state.searchedLpus;
    },
    pages(state) {
      return state.pages;
    },
    lastLpuVisits(state) {
      return state.lastLpuVisits;
    },
  },
};
