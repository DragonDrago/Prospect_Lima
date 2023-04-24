import { useParams } from "@/use/useParams";
import axios from "axios";
import router from "@/router";

export default {
  namespaced: true,
  state: () => ({
    token: localStorage.getItem("TOKEN") || "",
    loginError: "",
    grants: [],
  }),
  mutations: {
    setToken(state, token) {
      state.token = token;
    },
    logout(state) {
      localStorage.removeItem("TOKEN");
      localStorage.removeItem("GRANTS");
      state.token = "";
      router.push("/login");
    },
    setGrants(state, grants) {
      state.grants = grants;
    },
    setError(state, error) {
      state.loginError = error;
    },
  },
  actions: {
    async login({ commit }, params) {
      try {
        const res = await axios.post(`users/authenticate?${useParams(params)}`);
        if (res.status === 200) {
          commit("setToken", res.data.accessToken);
          commit("setGrants", res.data.grants);
          commit("setError", "");
          localStorage.setItem("TOKEN", res.data.accessToken);
          localStorage.setItem("GRANTS", JSON.stringify(res.data.grants));
          router.push("/", () => {
            window.location.reload();
          });
        }
      } catch (error) {
        commit("setError", error.response.data.message);
      }
    },
  },
  getters: {
    token(state) {
      return state.token;
    },
    isAuthenticated(_, getters) {
      return !!getters.token;
    },
    grants(state) {
      return state.grants;
    },
    loginError(state) {
      return state.loginError;
    },
  },
};
