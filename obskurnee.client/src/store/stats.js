import axios from "axios";

export default {
  namespaced: true,
  state: {
    userStats: [],
    bookStats: [],
  },
  mutations: {
    setUserStats(state, userStats) {
      state.userStats = userStats;
    },
    setBookStats(state, bookStats) {
      state.bookStats = bookStats;
    },
  },
  actions: {
    async fetchUserStats({ commit }) {
      return axios
        .get("/api/stats/bookclub")
        .then(response => {
          commit("setUserStats", response.data.userStats);
          commit("setBookStats", response.data.bookStats);
        });
    },
  },
}
