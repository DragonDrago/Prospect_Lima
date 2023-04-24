import axios from "axios";

export default {
  namespaced: true,
  state: () => ({
    licenseInfo: {},
  }),
  mutations: {
    setLicenseInfo(state, licenseInfo) {
      state.licenseInfo = licenseInfo;
    },
  },
  actions: {
    async getLicenseInfo({ commit }) {
      try {
        const res = await axios.get("company/license-info", {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });
        if (res.status === 200) {
          commit("setLicenseInfo", res.data);
        }
      } catch (error) {}
    },
  },
  getters: {
    licenseInfo(state) {
      return state.licenseInfo;
    },
  },
};
