import axios from "axios";

export default {
    namespaced: true,
    state: {
        rounds: [],
    },
    getters: {
    },
    mutations: {
        setRounds (state, rounds) {
        state.rounds = rounds;
      },
    },
    actions: {
      async fetchRounds ({ commit }) {
        return axios.get('/api/rounds').then(res => {
            console.log(res.data);
            commit('setRounds', res.data);
          })
          .catch(err => {
            console.log(err);
          });
      }
    }
}