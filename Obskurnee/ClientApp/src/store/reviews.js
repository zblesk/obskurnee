import axios from "axios";

export default {
    namespaced: true,
    state: {
      reviews: [],
    },
    getters: {
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
        await axios
          .get("/api/reviews/user" + userId)
          .then(response => {
            commit('addReviews', response.data);
            return Promise.resolve(response.data);
          });
      },
      async fetchBookReviews ({ commit }, bookId) 
      {
        await axios
          .get("/api/reviews/book/" + bookId)
          .then(response => {
            commit('addReviews', response.data);
            return Promise.resolve(response.data);
          });
      },
      async newReview({ commit, state }, { bookId, review }) 
      {
        retuawaitrn axios.post(
            "/api/reviews/book/" + bookId,
            review)
          .then((response) => {
            commit('addReviews', [ response.data ]);
            return Promise.resolve(state.reviews.filter(r => r.book.bookId == bookId));
          });
    }
  }
}