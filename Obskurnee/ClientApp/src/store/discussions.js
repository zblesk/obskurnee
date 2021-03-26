import axios from "axios";

export default {
    namespaced: true,
    state: {
      discussions: [],
    },
    getters: {
    },
    mutations: {
        setDiscussions (state, discussions) {
          state.discussions = discussions;
        },
        updateDiscussion (state, discussion) {
          var index = state.discussions.findIndex(d => d.discussionId == discussion.discussionId);
          if (index > -1)
          {
            state.discussions[index] = discussion;
          }
          else
          {
            state.discussions.unshift(discussion);
          }
      },
      addPost (state, { discussionId, post }) {
        var index = state.discussions.findIndex(d => d.discussionId == discussionId);
        if (index > -1)
        {
          state.discussions[index].posts.push(post);
        }
      }
    },
    actions: {
      async fetchDiscussionList ({ commit }) {
        return axios
          .get("/api/discussions/")
          .then(response => {
            commit('setDiscussions', response.data);
          })
          .catch(function (error) {
            console.log(error);
          });
      },
      async getDiscussionData({ commit }, discussionId) {
        return axios
          .get("/api/discussions/" + discussionId)
          .then((response) => {
            commit('updateDiscussion', response.data);
          })
          .catch(function (error) {
            console.log(error);
          });
      },
      async newPost({ commit }, { discussionId, newPost }) {
        return axios.post(
            "/api/discussions/" + discussionId,
            newPost)
          .then((response) => {
            commit('addPost', { discussionId: discussionId, post: response.data });
          });
    }
  }
}