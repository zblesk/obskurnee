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
        addRound (state, round) {
          state.rounds.unshift(round);
      },
    },
    actions: {
      async fetchRounds ({ commit }) {
        return axios.get('/api/rounds').then(res => {
            commit('setRounds', res.data);
          })
          .catch(err => {
            console.log(err);
          });
      },
      async createNewRound ({ commit }, round) {
        return axios.post('api/rounds', round)
          .then(res => {
            commit('addRound', res.data);
        })
      }
    }
}