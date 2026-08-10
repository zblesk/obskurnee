import axios from "axios";

export default {
  namespaced: true,
  state: {
    userStats: [],
    bookStats: [],
    // Detail stats keyed by user id/email, so several users can be cached independently.
    userDetailStats: {},
  },
  mutations: {
    setUserStats(state, userStats) {
      state.userStats = userStats;
    },
    setBookStats(state, bookStats) {
      state.bookStats = bookStats;
    },
    setUserDetailStats(state, { id, stats }) {
      state.userDetailStats = { ...state.userDetailStats, [id]: stats };
    },
  },
  getters: {
    userDetailStatsById: (state) => (id) => state.userDetailStats[id] || null,
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
    async fetchUserDetailStats({ commit }, id) {
      return axios
        .get(`/api/stats/user/${encodeURIComponent(id)}`)
        .then(response => {
          commit("setUserDetailStats", { id, stats: response.data });
          return response.data;
        });
    },
  },
}
