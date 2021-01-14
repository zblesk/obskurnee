import axios from "axios";

export default {
    namespaced: true,
    state: {
      profile: {},
      jwtToken: null
    },
    getters: {
      isAuthenticated: state => state.profile.name && state.profile.email
    },
    mutations: {
      setProfile (state, profile) {
        state.profile = profile;
      },  
      setJwtToken (state, jwtToken) {
        state.jwtToken = jwtToken;
        if (jwtToken) { 
          window.localStorage.setItem('jwtToken', jwtToken);
        }
        else {
          window.localStorage.removeItem('jwtToken');
        }
      }
    },
    actions: {
      login ({ commit }, credentials) {
        return axios.post('/api/account/login', credentials.creds).then(res => {
          const profile = res.data;
          const jwtToken = res.data.token;
          delete profile.token;
          commit('setProfile', profile);
          commit('setJwtToken', jwtToken);
        });
      },
      logout ({ commit }) {
        commit('setJwtToken', null);
        commit('setProfile', {});
      },
      restoreContext ({ commit}) {
        const jwtToken = window.localStorage.getItem('jwtToken');
        if (jwtToken) {
          commit('setJwtToken', jwtToken);
        }
        return axios.get('/api/account/context').then(res => {
          commit('setProfile', res.data);
        });
      }
    }
}