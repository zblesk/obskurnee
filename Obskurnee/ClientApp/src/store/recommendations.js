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
      addPost (state, post) {
        state.recommendations.push(post);
      }
    },
    actions: {
      async fetchRecommendationList ({ commit }) {
        return axios
          .get("/api/recommendations/")
          .then(response => {
            commit('setRecommendations', response.data);
          })
          .catch(function (error) {
            console.log(error);
          });
      },
      async newRecommendation({ commit }, newPost) {
        return axios.post(
            "/api/recommendations/",
            newPost)
          .then((response) => {
            commit('addPost', { recommendationId: recommendationId, post: response.data });
          })
          .catch(function (error) {
            console.log(error);
          });
    }
  }
}