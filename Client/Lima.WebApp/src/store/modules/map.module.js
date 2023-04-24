import axios from "axios";
import { uid } from "uid";

export default {
  namespaced: true,
  state: () => ({
    maps: [],
    isLoading: false,
  }),
  mutations: {
    setMaps(state, maps) {
      state.maps = maps;
    },
  },
  actions: {
    async getMaps({ commit, state }) {
      state.isLoading = true;
      const res = await axios.get("events/map", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
      });
      state.isLoading = false;
      if (res.status === 200) {
        commit("setMaps", res.data);
      }
    },
    async addEvent({ dispatch }, event) {
      await axios("events/add", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
        },
        data: event,
      });
      dispatch("getMaps");
    },
  },
  getters: {
    maps(state) {
      return state.maps.map((map) => ({
        ...map,
        unique_key: uid(),
      }));
    },
    isLoading(state) {
      return state.isLoading;
    },
  },
};
