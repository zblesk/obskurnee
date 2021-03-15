import axios from "axios";

export default {
  namespaced: true,
  state: {
    currentBook: {},
    myProfile: {},
    noticeboardHtml: '',
    currentPoll: null,
    currentDiscussion: null,
  },
  mutations: {
    setHomeData(state, data)
    {
        if (data.books.length) {
            state.currentBook = data.books.shift();
        }
        state.noticeboardHtml = data.notice;
        state.currentPoll = data.currentPoll;
        state.currentDiscussion = data.currentDiscussion;
        state.myProfile = data.myProfile;
    },
  },
  actions: {
      loadHome({ commit }) 
      {
        return axios
        .get('/api/home')
        .then((response) => {
            commit('setHomeData', response.data);
        });
      }
  }
}