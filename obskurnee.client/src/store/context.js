import axios from "axios";

export default {
  namespaced: true,
  state: {
    profile: {},
    jwtToken: null
  },
  getters: {
    isAuthenticated: state => state.profile.name && state.profile.email,
    isMod: (state, getters) => state.profile.isModerator || getters.isAdmin,
    isAdmin: state => state.profile.isAdmin,
    myUserId: state => state.profile.userId,
    isMe: (_, getters) => (userId) => getters.myUserId == userId,
    currentToken: () => window.localStorage.getItem('jwtToken')
  },
  mutations: {
    setProfile(state, profile)
    {
      state.profile = profile;
    },
    setJwtToken(state, jwtToken)
    {
      state.jwtToken = jwtToken;
      if (jwtToken)
      {
        window.localStorage.setItem('jwtToken', jwtToken);
      }
      else
      {
        window.localStorage.removeItem('jwtToken');
      }
    }
  },
  actions: {
    login({ commit, dispatch }, credentials)
    {
      return axios.post('/api/accounts/login', credentials).then(res =>
      {
        const profile = res.data;
        const jwtToken = res.data.token;
        delete profile.token;
        commit('setProfile', profile);
        commit('setJwtToken', jwtToken);
        dispatch('global/loadHome', {}, { root: true});
      });
    },
    logout({ commit }) 
    {
      commit('setJwtToken', null);
      commit('setProfile', {});
    },
    restoreContext({ commit }) 
    {
      const jwtToken = window.localStorage.getItem('jwtToken');
      if (jwtToken)
      {
        commit('setJwtToken', jwtToken);
        return axios.get('/api/accounts/context').then(res =>
        {
          commit('setProfile', res.data);
        });
      }
    },
    registerFirstAdmin({ dispatch }, credentials) 
    {
      return axios.post('/api/accounts/registerfirstadmin', credentials)
        .then(() =>
        {
          dispatch('login', credentials);
          dispatch('global/loadHome', {}, { root: true});
        });
    },
    passwordResetInit(_, email)
    {
      return axios.post('/api/accounts/passwordreset/' + email);
    },
    passwordResetFinish(_, pwdData)
    {
      return axios.post(
        `/api/accounts/passwordreset/token/${encodeURIComponent(pwdData.userId)}`, 
        { resetToken: pwdData.token, password: pwdData.password });
    }
  }
}