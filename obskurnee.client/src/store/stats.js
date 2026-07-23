import axios from "axios";

export default {
  namespaced: true,
  state: {
    userStats: [],
  },
  mutations: {
    setUserStats(state, userStats) {
      state.userStats = userStats;
    },
  },
  actions: {
    async fetchUserStats({ commit }) {
      return axios
        .get("/api/stats/users")
        .then(response => {
          commit("setUserStats", response.data);
        });
    },
  },
}
