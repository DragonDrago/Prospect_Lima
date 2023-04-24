import axios from "axios";
export default {
  namespaced: true,
  state: () => ({
    series: [],
    isLoading: false,
  }),
  mutations: {
    setSeries(state, series) {
      state.series = series;
    },
  },
  actions: {
    async getSeries({ commit, state }) {
      state.isLoading = true;
      try {
        const { data } = await axios.get("dict/drugs/series", {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });
        commit("setSeries", data);
      } catch (error) {}
      state.isLoading = false;
    },
    async addSerie({ dispatch, state }, serie) {
      state.isLoading = true;
      try {
        await axios("dict/drugs/series/add", {
          method: "POST",
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
          data: serie,
        });
      } catch (error) {}
      state.isLoading = false;
      dispatch("getSeries");
    },
    async editSerie({ dispatch, state }, serie) {
      console.log(serie);
      state.isLoading = true;
      try {
        await axios(`dict/drugs/series/update/${serie.id}`, {
          method: "PUT",
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
          data: serie,
        });
      } catch (error) {}
      state.isLoading = false;
      dispatch("getSeries");
    },
  },
  getters: {
    series(state) {
      return state.series;
    },
    isLoading(state) {
      return state.isLoading;
    },
  },
};
