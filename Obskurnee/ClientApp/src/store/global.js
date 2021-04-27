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
    language: '',
    matrixRoomLink: '',
  },
  getters: {
    isRepostingAllowed: state => state?.currentDiscussion?.topic == 'Books',
    activeDiscussionId: state => state?.currentDiscussion?.discussionId,
    getLanguage: state => state.language,
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
    setMatrixRoomLink(state, roomLink)
    {
      state.matrixRoomLink = roomLink;
      window.localStorage.setItem('matrixRoomLink', roomLink);
    },
    setCurrentDiscussion(state, discussion)
    {
      state.currentDiscussion = discussion;
    },
    setLanguage(state, language)
    {
      if (language)
      {
        window.localStorage.setItem('language', language);
        state.language = language;
      }
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
          commit('setLanguage', response.data.myProfile.language);
          commit('setMatrixRoomLink', response.data.matrixRoomLink);
      });
    },
    reloadSiteData({ commit })
    {
      const name = window.localStorage.getItem('siteName');
      if (name)
      {
        commit('setSiteName', name);
      }
      commit('setMatrixRoomLink', window.localStorage.getItem('matrixRoomLink'));
    }
  }
}