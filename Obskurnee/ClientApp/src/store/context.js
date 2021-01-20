import axios from "axios";

export default {
    namespaced: true,
    state: {
      profile: {},
      jwtToken: null
    },
    getters: {
      isAuthenticated: state => state.profile.name && state.profile.email,
      isMod: state => state.profile.isModerator == "true"
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
        return axios.post('/api/account/login', credentials).then(res => {
          console.log('lgin', res.data);
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
        console.log('restroring');
        const jwtToken = window.localStorage.getItem('jwtToken');
        console.log('restroring tok', jwtToken);
        if (jwtToken) {
          commit('setJwtToken', jwtToken);
        }
        return axios.get('/api/account/context').then(res => {
          commit('setProfile', res.data);
        });
      },
      registerFirstAdmin ({ dispatch }, credentials) {
        return axios.post('/api/account/registerfirstadmin', credentials)
        .then(res => {
          const profile = res.data;
          console.log('registered! Got:', profile);
          dispatch('login', credentials);
        })
        .catch(err => {
          console.log(err);
        });
      },
    }
}