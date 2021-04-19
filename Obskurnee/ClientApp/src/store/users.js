import axios from "axios";

export default {
  namespaced: true,
  state: {
    users: [],
  },
  getters: {
    nonMods: state => state.users?.filter(u => !u.isModerator),
    mods: state => state.users?.filter(u => u.isModerator),
    totalUsers: state => state.users?.length,
    userMailById: state => userId => state.users?.find(u => u.userId == userId).email,
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
        .then(() =>
        {
          return Promise.resolve();
        });
      return dispatch('getUsers');
    }
  }
}