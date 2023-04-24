import axios from "axios";
import queryString from "query-string";

export default {
  namespaced: true,
  state: () => ({
    drugStores: [],
    searchedDrugStores: [],
    isLoading: false,
    pages: [],
  }),
  mutations: {
    setDrugStores(state, drugStores) {
      state.drugStores = drugStores;
    },
    setSearchedDrugStores(state, searchedDrugStores) {
      state.searchedDrugStores = searchedDrugStores;
    },
    setDrugStoresPage(state, pages) {
      state.pages = pages;
    },
  },
  actions: {
    async getDrugStores({ commit, state }, page) {
      state.isLoading = true;
      const res = await axios.get(
        `dict/organizations/drug-stores?page=${page}`,
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        }
      );
      if (res.status === 200) {
        commit("setDrugStores", res.data.result);
        commit("setDrugStoresPage", res.data.page);
      }
      state.isLoading = false;
    },
    async addDrugStore({ dispatch }, drugStore) {
      await axios("dict/organizations/drug-stores/add", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
        data: drugStore,
      });
      dispatch("getDrugStores");
    },
    async editDrugStore({ dispatch }, drugStore) {
      await axios(`dict/organizations/drug-stores/update/${drugStore.id}`, {
        method: "PUT",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
        data: drugStore,
      });
      dispatch("getDrugStores");
    },
    async findDrugStoresByName({ commit }, txt) {
      if (txt.length > 0) {
        const { data } = await axios.get(
          `dict/organizations/drug-stores/find/${txt}`,
          {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
            },
          }
        );
        commit("setSearchedDrugStores", data);
      }
    },
    async filterDrugStores({ commit, state }, drugStore) {
      state.isLoading = true;

      const query = queryString.stringify({
        org_name: drugStore.org_name,
        inn: drugStore.inn,
        sales: [drugStore.sales_min, drugStore.sales_max],
        orders: [drugStore.orders_min, drugStore.orders_max],
        total_sum: [drugStore.total_sum_min, drugStore.total_sum_max],
        prepay: [drugStore.prepay_min, drugStore.prepay_max],
      });

      const { data } = await axios.get(
        `dict/organizations/drug-stores?${query}`,
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        }
      );
      commit("setDrugStores", data);
      state.isLoading = false;
    },
  },
  getters: {
    drugStores(state) {
      return state.drugStores;
    },
    searchedDrugStores(state) {
      return state.searchedDrugStores;
    },
    isLoading(state) {
      return state.isLoading;
    },
    pages(state) {
      return state.pages;
    },
  },
};
