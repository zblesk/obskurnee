import axios from "axios";

export default {
  namespaced: true,
  state: {
    currentBook: {},
    myProfile: {},
    noticeboardHtml: '',
    currentPoll: null,
    currentDiscussion: null,
    siteName: 'Obskurnee',
  },
  getters: {
    isRepostingAllowed: state => state?.currentDiscussion?.topic == 'Books',
    activeDiscussionId: state => state?.currentDiscussion?.discussionId,
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
    setSiteName(state, siteName)
    {
      if (siteName)
      {
        window.localStorage.setItem('siteName', siteName);
        state.siteName = siteName;
      }
    },
    setCurrentDiscussion(state, discussion)
    {
      state.currentDiscussion = discussion;
    },
  },
  actions: {
    loadHome({ commit }) 
    {
      return axios
      .get('/api/home')
      .then((response) => {
          commit('setHomeData', response.data);
          commit('setSiteName', response.data.siteName);
      });
    },
    reloadSiteName({ commit })
    {
      const name = window.localStorage.getItem('siteName');
      if (name)
      {
        commit('setSiteName', name);
      }
    }
  }
}