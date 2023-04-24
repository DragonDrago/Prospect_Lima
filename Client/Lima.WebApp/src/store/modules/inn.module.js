import axios from "axios";

export default {
  namespaced: true,
  state: () => ({
    innToken: "",
  }),
  mutations: {
    setInnToken(state, innToken) {
      console.log(innToken);
      state.innToken = innToken;
    },
  },
  actions: {
    async authInn({ commit }) {
      const res = await axios("https://account.faktura.uz/Token", {
        method: "POST",
        headers: {
          grant_type: "password",
          username: "artamonov.o.d@gmail.com",
          password: "777.@dmin",
          client_id: "procrm",
          client_secret:
            "aMGcHExhbMpERCybAfdj4JCegMfKCv2CEiv22z2zVtJvoK07HSJqCXkMxZkh",
        },
      });
      if (res.status === 200) {
        commit("setInnToken", res.data);
      }
      state.isLoading = false;
    },
  },
  getters: {},
};
