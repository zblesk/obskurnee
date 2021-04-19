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
    }

  }
}