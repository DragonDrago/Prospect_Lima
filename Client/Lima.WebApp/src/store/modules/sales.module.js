import axios from "axios";
import queryString from "query-string";

export default {
  namespaced: true,
  state: () => ({
    sales: [],
    organizations: [],
    isLoading: false,
  }),
  mutations: {
    setSales(state, sales) {
      state.sales = sales;
    },
    setOrganizations(state, organizations) {
      state.organizations = organizations;
    },
  },
  actions: {
    async getSales({ commit, state }) {
      state.isLoading = true;
      try {
        const res = await axios.get("sales/all", {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });
        if (res.status === 200) {
          commit("setSales", res.data);
        }
      } catch (error) {}
      state.isLoading = false;
    },
    async filterSales({ commit }, sale) {
      const query = queryString.stringify({
        sale_id: sale.sale_id,
        user_id: sale.user_id,
        org_id: sale.org_id,
        dates: [sale.dates_min, sale.dates_max],
        sale_sum: [sale.sale_sum_min, sale.sale_sum_max],
      });
      try {
        const res = await axios.get(`sales/all?${query}`, {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });
        if (res.status === 200) {
          commit("setSales", res.data);
        }
      } catch (error) {}
    },
    async getOrganizations({ commit }, query) {
      try {
        if (query.length > 0) {
          const { data } = await axios.get(
            `/dict/organizations/find/${query}`,
            {
              headers: {
                Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
              },
            }
          );
          commit("setOrganizations", data);
        }
      } catch (error) {}
    },
    async acceptSales({ dispatch }, id) {
      try {
        await axios(`sales/accept/${id}`, {
          method: "PUT",
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });
        if (res.status === 200) {
          dispatch("getSales");
        }
      } catch (error) {}
    },
  },
  getters: {
    sales(state) {
      return state.sales;
    },
    organizations(state) {
      return state.organizations;
    },
    isLoading(state) {
      return state.isLoading;
    },
  },
};
