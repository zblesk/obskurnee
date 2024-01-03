<template>
<div>
  <h1 id="tableLabel" class="page-title">{{ discussion.title }}<span class="disc-closed" v-if="discussion.isClosed">&nbsp;{{$t('messages.closed')}}</span></h1>
  <div class="main">
    <p v-if="discussion.renderedDescription" v-html="discussion.renderedDescription" class="disc-desc"></p>
    <div class="form" v-if="!discussion.isClosed">
      <new-post :mode="discussion.topic" @new-post="onNewPost">{{$t('discussion.addNewPitch')}}</new-post>
      <div></div>
    </div>
    <div class="buttons">
      <button @click="closeDiscussion" v-if="isMod && discussion.posts?.length && !discussion.isClosed" class="button-secondary">{{$t('discussion.closeCreatePoll')}}</button>
    </div>

    <div class="form" v-if="discussion.pollId">
      <div class="buttons">
      <router-link :to="{ name: 'poll', params: { pollId: discussion.pollId } }" class="button-primary">{{$t('discussion.goToPoll')}}</router-link>
      </div>
    </div>
  </div>

  <div class="grid">
    <p v-if="!discussion.title" class="alert-inline">{{$t('discussion.noneSoFar')}}</p>

    <book-post
      v-for="post in discussion.posts"
      v-bind:key="post.postId"
      v-bind:post="post"
      v-bind:topic="discussion.topic">
    </book-post>
  </div> 
</div>
</template>


<script>
import axios from "axios";
import BookPost from "../BookPost.vue";
import { mapGetters,mapActions, mapState } from "vuex";
import NewPost from '../NewPost.vue';

export default {
  name: "Discussion",
  components: { BookPost, NewPost },
  computed: {
    ...mapGetters("context", ["isMod"]),
    ...mapState("discussions", ["discussions"]),
    discussion: function () { return this.discussions.find(d => d.discussionId == this.$route.params.discussionId) ?? { posts: [] }; },
  },
  watch: {
    '$route' (to) {
      if (to.name == 'discussion')
      {
        this.onLoad();
      }
    }
  },
  methods: {
    ...mapActions("discussions", ["getDiscussionData", "newPost", "getDiscussionPost"]),
    ...mapActions("recommendations", ["fetchRecommendationById"]),
    closeDiscussion() 
    {
      if (confirm(this.$t('discussion.confirmClose')))
      {
        axios.post(
          "/api/rounds/close-discussion/" + this.$route.params.discussionId)
        .then((response) => {
            this.pollId = response.data.discussion.pollId;
            this.IsClosed = true;
            this.$router.push({ name: "poll", params: { pollId: this.pollId} });
        })
        .catch(this.$handleApiError);
      }
    },
    onNewPost(post)
    {
      const onErr = this.$notifyError;
      this.newPost({ discussionId: this.discussion.discussionId, newPost: post })
          .then(() => this.emitter.emit('clear-post'))
          .catch(function (error) {
            console.log(error);
            onErr(this.$t('discussion.failedToAddPost'));
          });
    },
    async onLoad()
    {
      this.getDiscussionData(this.$route.params.discussionId);
      let query = this.$route.query;
      if (query.fromDiscussionId && query.parentPostId)
      {
        let post = await this.getDiscussionPost({ discussionId: query.fromDiscussionId, postId: query.parentPostId });
        this.emitter.emit('prefill-post', { post, parentData: { parentPostId: query.parentPostId } });
      }
      else if (query.parentRecommendationId)
      {
        let post = await this.fetchRecommendationById(query.parentRecommendationId);
        this.emitter.emit('prefill-post', { post, parentData: { parentRecommendationId: query.parentRecommendationId } });
      }
    }
  },
  mounted() {
    this.onLoad();
  },
};
</script>

<style scoped>
  .disc-closed {
    font-size: 0.65em;
    color: var(--c-accent);
    text-transform: uppercase;
    vertical-align: super;
  }

  .disc-desc {
    font-size: 1.2rem;
    margin-bottom: var(--spacer);
  }

  .form:not(:last-child) {
    margin-bottom: var(--spacer);
  }
</style>