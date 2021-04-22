<template>
<div>
  <h1 id="tableLabel" class="page-title">{{ discussion.title }}<span class="disc-closed" v-if="discussion.isClosed">&nbsp;Uzavreté</span></h1>
  <div class="main">
    <p v-html="discussion.renderedDescription" class="disc-desc"></p>
    <div class="form" v-if="!discussion.isClosed">
      <new-post :mode="discussion.topic" @new-post="onNewPost">Pridať nový návrh</new-post>
      <div></div>
    </div>
    <div class="buttons">
      <button @click="closeDiscussion" v-if="isMod && discussion.posts?.length && !discussion.isClosed" class="button-secondary">Uzavřít diskuzi a vytvořit hlasování</button>
    </div>

    <div class="form" v-if="discussion.pollId">
      <router-link :to="{ name: 'poll', params: { pollId: discussion.pollId } }" class="button-primary">Jít na hlasování</router-link>
    </div>
  </div>

  <div class="grid">
    <p v-if="!discussion.title" class="alert-inline">Zatial tu nic nie je</p>

    <book-post
      v-for="post in discussion.posts"
      v-bind:key="post.postId"
      v-bind:post="post">
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
    closeDiscussion() 
    {
        axios.post(
          "/api/rounds/close-discussion/" + this.$route.params.discussionId)
        .then((response) => {
            this.pollId = response.data.discussion.pollId;
            this.IsClosed = true;
            this.$router.push({ name: "poll", params: { pollId: this.pollId} });
        })
        .catch(function (error) {
          alert(error);
        });
    },
    onNewPost(post)
    {
      const onErr = this.$notifyError;
      this.newPost({ discussionId: this.discussion.discussionId, newPost: post })
          .then(() => this.emitter.emit('clear-post'))
          .catch(function (error) {
            console.log(error);
            onErr("Nepodarilo sa pridať príspevok");
          });
    },
    async onLoad()
    {
      this.getDiscussionData(this.$route.params.discussionId);
      let query = this.$route.query;
      if (query.fromDiscussionId && query.fromPostId)
      {
        let post = await this.getDiscussionPost({ discussionId: query.fromDiscussionId, postId: query.fromPostId });
        this.emitter.emit('prefill-post', { post, parentData: { parentPostId: query.fromPostId } });
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
    font-size: 1.2em;
    margin-bottom: var(--spacer);
  }

  .form {
    margin-bottom: var(--spacer);
  }
</style>