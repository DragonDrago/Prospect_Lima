import axios from "axios";
import queryString from "query-string";

export default {
  namespaced: true,
  state: () => ({
    visitDoctors: [],
    isLoading: false,
  }),
  mutations: {
    setVisitDoctor(state, visitDoctors) {
      state.visitDoctors = visitDoctors;
    },
  },
  actions: {
    async getVisitDoctors({ commit, state }) {
      state.isLoading = true;
      try {
        const res = await axios.get("visits/doctors", {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });
        if (res.status === 200) {
          commit("setVisitDoctor", res.data);
        }
      } catch (error) {}
      state.isLoading = false;
    },
    async addVisitDoctor({ dispatch }, visitDoctor) {
      await axios("visits/doctors/add", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          "Content-Type": "application/json",
        },
        data: visitDoctor,
      });
      await dispatch("getVisitDoctors");
    },
    async completeVisit(_, complete) {
      await axios("visits/compleate", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
        data: complete,
      });
    },
    async updateVisitDoctor({ dispatch }, visitDoctor) {
      await axios(`visits/doctors/update/${visitDoctor.id}`, {
        method: "PUT",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
        data: visitDoctor,
      });
      await dispatch("getVisitDoctors");
    },
    async filterVisitDoctors({ commit }, visitDoctor) {
      const query = queryString.stringify({
        medrep_id: visitDoctor.medrep_id,
        dates: [visitDoctor.dates_min, visitDoctor.dates_max],
        status_id: visitDoctor.status_id,
        organization_id: visitDoctor.organization_id,
        doctor_id: visitDoctor.doctor_id,
        phone: visitDoctor.phone,
        doctor_position: visitDoctor.doctor_position,
      });
      try {
        const res = await axios.get(`visits/doctors?${query}`, {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });
        if (res.status === 200) {
          commit("setVisitDoctor", res.data);
        }
      } catch (error) {}
    },
  },
  getters: {
    visitDoctors(state) {
      return state.visitDoctors;
    },
    isLoading(state) {
      return state.isLoading;
    },
  },
};
