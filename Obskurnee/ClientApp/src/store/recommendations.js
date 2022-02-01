import axios from "axios";

export default {
  namespaced: true,
  state: {
    recommendations: [],
  },
  getters: {
  },
  mutations: {
    setRecommendations (state, recommendations) {
      state.recommendations = recommendations;
    },
    addRec (state, rec) {
      state.recommendations.unshift(rec);
    },
    updateRec (state, { rec }) {
      let existingRec = state.recommendations.findIndex(r => r.recommendationId == rec.recommendationId);
      if (existingRec > -1)
        state.recommendations[existingRec] = rec;
      else
        state.recommendations.unshift(rec);
    }
  },
  actions: {
    async fetchRecommendationList ({ commit }) 
    {
      return axios
        .get("/api/recommendations/")
        .then(response => {
          commit('setRecommendations', response.data);
        });
    },
    async newRecommendation({ commit }, newRec) 
    {
      return axios.post(
          "/api/recommendations/",
          newRec)
        .then((response) => {
          commit('addRec', response.data);
        });
    },
    async updateRecommendation({ commit }, { rec }) 
    {
      return axios.patch(
          "/api/recommendations/",
          rec)
        .then((response) => {
          commit('updateRec', { rec: response.data });
        });
    },
    async fetchRecommendationsFor({ dispatch, state }, userId)
    {
      if (!userId || userId.length < 1)
      {
        return Promise.resolve([]);
      }
      if (!state.recommendations?.length > 0)
      {
        await dispatch('fetchRecommendationList');
      }
      return Promise.resolve(state.recommendations.filter(r => r.ownerId == userId));
    },
    async fetchRecommendationById({ dispatch, state }, recId)
    {
      var rec = state.recommendations?.find(r => r.recommendationId == recId);
      if (!rec)
      {
        console.log('not found');
        await dispatch('fetchRecommendationList');
        rec = state.recommendations.find(r => r.recommendationId == recId);
      }
      return Promise.resolve(rec);
    }
  }
}