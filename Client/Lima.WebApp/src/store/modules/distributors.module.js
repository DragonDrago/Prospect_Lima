import queryString from "query-string";
import axios from "axios";

export default {
  namespaced: true,
  state: () => ({
    distributors: [],
    searchedDistributors: [],
    isLoading: false,
    pages: [],
  }),
  mutations: {
    setDistributors(state, distributors) {
      state.distributors = distributors;
    },
    setSearchedDistributors(state, searchedDistributors) {
      state.searchedDistributors = searchedDistributors;
    },
    setDistributorsPage(state, pages) {
      state.pages = pages;
    },
  },
  actions: {
    async getDistributors({ commit, state }, page) {
      state.isLoading = true;
      const res = await axios.get(`dict/organizations/distributors?page=${0}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
      });
      if (res.status === 200) {
        commit("setDistributors", res.data.result);
        commit("setDistributorsPage", res.data.page);
      }
      state.isLoading = false;
    },
    async addDistributor({ dispatch }, distributor) {
      await axios("dict/organizations/distributors/add", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
        data: distributor,
      });
      dispatch("getDistributors");
    },
    async editDistributor({ dispatch }, distributor) {
      await axios(`dict/organizations/distributors/update/${distributor.id}`, {
        method: "PUT",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
        data: distributor,
      });
      dispatch("getDistributors");
    },
    async findDistributorsByName({ commit }, txt) {
      if (txt.length > 0) {
        const { data } = await axios.get(
          `dict/organizations/distributors/find/${txt}`,
          {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
            },
          }
        );
        commit("setSearchedDistributors", data);
      }
    },
    async filterDistributors({ commit, state }, distriburor) {
      state.isLoading = true;

      const query = queryString.stringify({
        org_name: distriburor.org_name,
        inn: distriburor.inn,
        sales: [distriburor.sales_min, distriburor.sales_max],
        orders: [distriburor.orders_min, distriburor.orders_max],
        total_sum: [distriburor.total_sum_min, distriburor.total_sum_max],
        prepay: [distriburor.prepay_min, distriburor.prepay_max],
      });

      const { data } = await axios.get(
        `dict/organizations/distributors?${query}`,
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        }
      );
      commit("setDistributors", data);
      state.isLoading = false;
    },
  },
  getters: {
    distributors(state) {
      return state.distributors;
    },
    searchedDistributors(state) {
      return state.searchedDistributors;
    },
    isLoading(state) {
      return state.isLoading;
    },
    pages(state) {
      return state.pages;
    },
  },
};
