import axios from "axios";

export default {
    namespaced: true,
    state: {
      profile: {},
      jwtToken: null
    },
    getters: {
      isAuthenticated: state => state.profile.name && state.profile.email,
      isMod: state => state.profile.isModerator == "true",
      isAdmin: state => state.profile.isAdmin == "true",
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
        return axios.post('/api/accounts/login', credentials).then(res => {
          const profile = res.data;
          const jwtToken = res.data.token;
          delete profile.token;
          commit('setProfile', profile);
          commit('setJwtToken', jwtToken);
        })
        .catch(err => {
          console.log(err);
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
          return axios.get('/api/accounts/context').then(res => {
            commit('setProfile', res.data);
          });
        }
      },
      registerFirstAdmin ({ dispatch }, credentials) {
        return axios.post('/api/accounts/registerfirstadmin', credentials)
        .then(() => {
          dispatch('login', credentials);
        })
        .catch(err => {
          console.log(err);
        });
      },
    }
}