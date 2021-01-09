export default {
    namespaced: true,
    state: {
      profile: {}
    },
    getters: {
      isAuthenticated: state => state.profile.name && state.profile.email
    },
    mutations: {
      setProfile (state, profile) {
        state.profile = profile
      },
    },
    actions: {
      login ({ commit }, credentials) {
        return axios.post('api/ucet/login', credentials)
          .then(res => {
            commit('setProfile', res.data)
        })
      },
      logout ({ commit }) {
        return axios.post('api/ucet/logout').then(() => {
          commit('setProfile', {})
        })
      }
    }
}