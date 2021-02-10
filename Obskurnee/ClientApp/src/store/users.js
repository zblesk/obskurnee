import axios from "axios";

export default {
  namespaced: true,
  state: {
    users: [],
  },
  getters: {
    nonMods: state => state.users?.filter(u => !u.isModerator),
    mods: state => state.users?.filter(u => u.isModerator),
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
      return axios.get("/api/users")
        .then((response) =>
        {
          commit('setUsers', response.data);
          return Promise.resolve();
        })
        .catch(function (error)
        {
          console.log(error);
          return Promise.resolve(error);
        });
    },
    async getUser({ dispatch, state }, email)
    {
      if (!state.users 
        || !state.users.length 
        || !state.users.some(u => u.email == email))
      {
        await dispatch('getUsers');
      }
      return Promise.resolve(state.users.find(u => u.email == email));
    },
    async updateUser({ dispatch }, userData)
    {
      await axios.post("/api/users/" + userData.email, userData)
        .then((response) =>
        {
          console.log(response.data);
          return Promise.resolve();
        })
        .catch(function (error)
        {
          console.log(error);
          return Promise.resolve(error);
        });
      return dispatch('getUsers');
    }
  }
}