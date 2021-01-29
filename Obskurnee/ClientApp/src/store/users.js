import axios from "axios";

export default {
  namespaced: true,
  state: {
    users: [],
  },
  getters: {
  },
  mutations: {
    setUsers(state, users)
    {
      state.users = users;
    },
  }, 
  actions: {
    async getUsers({ commit })
    {
      return axios.get("/api/accounts")
        .then((response) =>
        {
          console.log(response.data);
          commit('setUsers', response.data);
          return Promise.resolve();
        })
        .catch(function (error)
        {
          console.log(error);
          return Promise.resolve(error);
        });
    },
  }
}