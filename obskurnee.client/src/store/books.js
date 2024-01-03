import axios from "axios";

export default {
    namespaced: true,
    state: {
      books: [],
    },
    getters: {
    },
    mutations: {
        setBooks (state, books) {
          state.books = books;
        },
    },
    actions: {
      async fetchBookList ({ commit }) {
        return axios
          .get("/api/books/")
          .then(response => {
            commit('setBooks', response.data);
          })
          .catch(function (error) {
            console.log(error);
          });
      },
      async getBookByOrder({ dispatch, state }, bookOrder) {
        if (!state.books
          || !state.books.length 
          || !state.books.some(b => b.order == bookOrder))
        {
          await dispatch('fetchBookList');
        }
        return Promise.resolve(state.books.find(b => b.order == bookOrder));
      },
      async getBookById({ dispatch, state }, bookId) {
        if (!state.books
          || !state.books.length 
          || !state.books.some(b => b.bookId == bookId))
        {
          await dispatch('fetchBookList');
        }
        return Promise.resolve(state.books.find(b => b.bookId == bookId));
      },
  }
}