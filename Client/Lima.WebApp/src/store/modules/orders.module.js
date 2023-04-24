import axios from "axios";
import queryString from "query-string";

export default {
  namespaced: true,
  state: () => ({
    orders: [],
    isLoading: false,
  }),
  mutations: {
    setOrders(state, orders) {
      state.orders = orders;
    },
  },
  actions: {
    async getOrders({ commit, state }) {
      state.isLoading = true;
      try {
        const res = await axios.get("orders/all", {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });
        if (res.status === 200) {
          commit("setOrders", res.data);
        }
      } catch (error) {}
      state.isLoading = false;
    },
    async filterOrder({ commit }, order) {
      const query = queryString.stringify({
        order_id: order.order_id,
        user_id: order.user_id,
        org_id: order.org_id,
        dates: [order.dates_min, order.dates_max],
        total_sum: [order.total_sum_min, order.total_sum_max],
        prepay: [order.prepay_min, order.prepay_max],
        debt_sum: [order.debt_sum_min, order.debt_sum_max],
        remains: [order.remains_min, order.remains_max],
        check_number: order.check_number,
        inn: order.inn,
      });
      try {
        const res = await axios.get(`orders/all?${query}`, {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });
        if (res.status === 200) {
          commit("setOrders", res.data);
        }
      } catch (error) {}
    },
    async acceptOrder({ dispatch }, id) {
      try {
        await axios(`orders/accept/${id}`, {
          method: "POST",
          headers: {
            Authorization: `Bearer ${localStorage.getItem("TOKEN")}`,
          },
        });
        if (res.status === 200) {
          dispatch("getOrders");
        }
      } catch (error) {}
    },
  },
  getters: {
    orders(state) {
      return state.orders;
    },
    isLoading(state) {
      return state.isLoading;
    },
  },
};
