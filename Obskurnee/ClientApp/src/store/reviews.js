import axios from "axios";

export default {
    namespaced: true,
    state: {
      reviews: {},
    },
    getters: {
      bookReviews: (state) => (bookId) => 
        Object.entries(state.reviews).filter((kvp) => kvp[1].book.bookId == bookId).map((kvp) => kvp[1]),
    },
    mutations: {
      addReviews (state, reviews) {
        for (let r of reviews)
        {
          state.reviews[r.reviewId] = r;
        }
      }
    },
    actions: {
      async fetchUserReviews ({ commit }, userId) 
      {
        return axios
          .get("/api/reviews/user" + userId)
          .then(response => {
            commit('addReviews', response.data);
            return Promise.resolve(response.data);
          });
      },
      async fetchBookReviews ({ commit }, bookId) 
      {
        return axios
          .get("/api/reviews/book/" + bookId)
          .then(response => {
            commit('addReviews', response.data);
            return Promise.resolve(response.data);
          });
      },
      async newReview({ commit, state }, { bookId, review }) 
      {
        return axios.post(
            "/api/reviews/book/" + bookId,
            review)
          .then((response) => {
            commit('addReviews', [ response.data ]);
            return Promise.resolve(Object.entries(state.reviews).filter((kvp) => kvp[1].book.bookId == bookId).map((kvp) => kvp[1]));
          });
    }
  }
}