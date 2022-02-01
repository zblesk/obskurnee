import axios from "axios";

export default {
    namespaced: true,
    state: {
      discussions: [],
    },
    mutations: {
        setDiscussions (state, discussions) {
          state.discussions = discussions;
        },
        updateDiscussion (state, discussion) {
          let discussionIndex = state.discussions.findIndex(d => d.discussionId == discussion.discussionId);
          if (discussionIndex > -1)
          {
            state.discussions[discussionIndex] = discussion;
          }
          else
          {
            state.discussions.unshift(discussion);
          }
      },
      addPost (state, { discussionId, post }) {
        let discussionIndex = state.discussions.findIndex(d => d.discussionId == discussionId);
        if (discussionIndex > -1)
        {
          state.discussions[discussionIndex].posts.push(post);
        }
      },
      updatePost (state, { post }) {
        var discussionIndex = state.discussions.findIndex(d => d.discussionId == post.discussionId);
        if (discussionIndex > -1)
        {
          let postIndex = state.discussions[discussionIndex].posts.findIndex(p => p.postId == post.postId);
          if (postIndex > -1)
          {
            state.discussions[discussionIndex].posts[postIndex] = post;
          }
          else
          {
            state.discussions[discussionIndex].posts.push(post);
          }
        }
      }
    },
    actions: {
      async fetchDiscussionList ({ commit }) 
      {
        return axios
          .get("/api/discussions/")
          .then(response => {
            commit('setDiscussions', response.data);
          });
      },
      async getDiscussionData({ commit }, discussionId) 
      {
        return axios
          .get("/api/discussions/" + discussionId)
          .then((response) => {
            commit('updateDiscussion', response.data);
          });
      },
      async newPost({ commit }, { discussionId, newPost }) 
      {
        return axios.post(
            "/api/discussions/" + discussionId,
            newPost)
          .then((response) => {
            commit('addPost', { discussionId: discussionId, post: response.data });
          });
      },
      async updatePost({ commit }, { post }) 
      {
        return axios.patch(
            "/api/discussions/" + post.discussionId,
            post)
          .then((response) => {
            commit('updatePost', { post: response.data });
          });
      },
      async getDiscussionPost({ state, dispatch }, { discussionId, postId })
      {
        if (!state.discussions.some(d => d.discussionId == discussionId))
        {
          await dispatch('getDiscussionData', discussionId);
        }
        let discussion = state.discussions.find(d => d.discussionId == discussionId);
        return Promise.resolve(discussion.posts.find(p => p.postId == postId));
      }
  }
}