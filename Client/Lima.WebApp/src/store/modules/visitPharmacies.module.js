import axios from "axios";
import queryString from "query-string";

export default {
  namespaced: true,
  state: () => ({
    visitPharmacies: [],
    isLoading: false,
  }),
  mutations: {
    setVisitPharmacies(state, visitPharmacies) {
      state.visitPharmacies = visitPharmacies;
    },
  },
  actions: {
    async getVisitPharmacies({ commit, state }) {
      state.isLoading = true;
      try {
        const res = await axios.get("visits/pharmacies", {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });
        if (res.status === 200) {
          commit("setVisitPharmacies", res.data);
        }
      } catch (err) {}

      state.isLoading = false;
    },
    async addVisitPharmacies({ dispatch }, newVisit) {
      await axios("visits/pharmacies/add", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
        data: newVisit,
      });
      await dispatch("getVisitPharmacies");
    },
    async filterVisitPharmacies({ commit }, visitPharmacie) {
      const query = queryString.stringify({
        medrep_id: visitPharmacie.medrep_id,
        dates: [visitPharmacie.dates_min, visitPharmacie.dates_max],
        status: visitPharmacie.status,
        org_id: visitPharmacie.org_id,
        inn: visitPharmacie.inn,
        sales: [visitPharmacie.sales_min, visitPharmacie.sales_max],
        prepayments: [
          visitPharmacie.prepayments_min,
          visitPharmacie.prepayments_max,
        ],
      });
      try {
        const res = await axios.get(`visits/pharmacies?${query}`, {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });
        if (res.status === 200) {
          commit("setVisitPharmacies", res.data);
        }
      } catch (error) {
        console.log(error);
      }
    },
    async editVisitPharmacy({ dispatch }, payload) {
      await axios(`visits/pharmacies/update/${payload.id}`, {
        method: "PUT",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
        data: payload,
      });
      await dispatch("getVisitPharmacies");
    },
  },
  getters: {
    visitPharmacies(state) {
      return state.visitPharmacies;
    },
    isLoading(state) {
      return state.isLoading;
    },
  },
};
