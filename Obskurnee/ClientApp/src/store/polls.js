import axios from "axios";

export default {
  namespaced: true,
  state: {
    polls: [],
    votes: [],
  },
  getters: {
  },
  mutations: {
    setPoll(state, poll)
    {
      state.polls[poll.pollId] = poll;
    },
    setMyVote(state, vote)
    {
      state.votes[vote.pollId] = vote.postIds;
    },
    setEmptyVote(state, pollId)
    {
      state.votes[pollId] = [];
    },
  },
  actions: {
    async getPollData({ commit, state }, pollId)
    {
      if (pollId in state.polls)
      {
        return Promise.resolve();
      }
      return axios.get("/api/polls/" + pollId)
        .then((response) =>
        {
          commit('setPoll', response.data.poll);
          if (response.data.myVote)
          {
            commit('setMyVote', response.data.myVote);
          }
          else 
          {
            commit('setEmptyVote', pollId);
          }
          return Promise.resolve(state.polls[pollId]);
        });
    },
    async sendVote({ commit }, { pollId, votes })
    {
      return axios.post(
        "/api/polls/" + pollId + "/vote",
        { postIds: votes })
        .then((response) =>
        {
          commit('setPoll', response.data.poll);
          commit('setMyVote', { pollId: pollId, postIds: votes });
          return Promise.resolve(response.data);
        });
    },
    async closePoll({ commit }, pollId)
    {
      return axios.post(
        "/api/rounds/close-poll/" + pollId)
        .then((response) =>
        {
          commit('setPoll', response.data.poll);
          return Promise.resolve(response.data);
        });
    }
  }
}