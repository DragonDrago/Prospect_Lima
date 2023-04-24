import axios from "axios";
import queryString from "query-string";

export default {
  namespaced: true,
  state: () => ({
    drugs: [],
    searchedDrugs: [],
    isLoading: false,
  }),
  mutations: {
    setDrugs(state, drugs) {
      state.drugs = drugs;
    },
    setSearchedDrugs(state, searchedDrugs) {
      state.searchedDrugs = searchedDrugs;
    },
  },
  actions: {
    async getDrugs({ commit, state }) {
      state.isLoading = true;
      const res = await axios.get("dict/drugs/all", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
      });
      if (res.status === 200) {
        commit("setDrugs", res.data);
      }
      state.isLoading = false;
    },
    async updateDrugs({ dispatch }, drug) {
      try {
        await axios(`dict/drugs/update/${drug.id}`, {
          method: "PUT",
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
          data: drug,
        });
        dispatch("getDrugs");
      } catch (error) {}
    },
    async addDrug({ dispatch }, drug) {
      await axios("dict/drugs/add", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
        data: drug,
      });
      dispatch("getDrugs");
    },
    async filterDrugs({ commit, state }, drug) {
      state.isLoading = true;
      const query = queryString.stringify({
        drug_name: drug.drug_name,
        producer_id: drug.producer_id,
        country_id: drug.country_id,
        expire_date_month: drug.expire_date_month,
        unit_id: drug.unit_id,
        amount: [drug.amount_min, drug.amount_max],
        orders_count: [drug.orders_count_min, drug.orders_count_max],
      });
      try {
        const res = await axios(`dict/drugs/all?${query}`, {
          method: "GET",
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });
        if (res.status === 200) {
          commit("setDrugs", res.data);
        }
      } catch (error) {
        console.error(error);
      }
      state.isLoading = false;
    },
    async getDrugsByName({ commit }, txt) {
      if (txt.length > 0) {
        const { data } = await axios.get(`dict/drugs/find/${txt}`, {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });
        commit("setSearchedDrugs", data);
      }
    },
  },
  getters: {
    drugs(state) {
      return state.drugs;
    },
    isLoading(state) {
      return state.isLoading;
    },
    searchedDrugs(state) {
      return state.searchedDrugs;
    },
  },
};
