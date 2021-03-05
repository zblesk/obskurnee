<template>
<div>
  <h1 id="tableLabel" class="page-title">{{ discussion.title }}<span class="disc-closed" v-if="discussion.isClosed">&nbsp;Uzavreté</span></h1>
  <div class="page">
    <p v-html="discussion.renderedDescription" class="disc-desc"></p>
    <div class="form" v-if="!discussion.isClosed">
      <new-post :mode="discussion.topic" @new-post="onNewPost"></new-post>
      <div></div>
    </div>
    <button @click="closeDiscussion" v-if="isMod && discussion.posts?.length && !discussion.isClosed" class="button-primary button-close">Uzavri diskusiu a vytvor hlasovanie</button>

    <div class="form" v-if="discussion.pollId">
      <router-link :to="{ name: 'poll', params: { pollId: discussion.pollId } }" class="button-primary">Choď na hlasovanie</router-link>
    </div>
  </div>

  <div class="grid">
    <p v-if="!discussion.title"><em>Zatial tu nic nie je</em></p>

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
  methods: {
    ...mapActions("discussions", ["getDiscussionData", "newPost"]),
    closeDiscussion() 
    {
        axios.post(
          "/api/rounds/close-discussion/" + this.$route.params.discussionId)
        .then((response) => {
          console.log(response.data);
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
      this.newPost({ discussionId: this.discussion.discussionId, newPost: post });
    },
  },
  mounted() {
    this.getDiscussionData(this.$route.params.discussionId);//.then(r => console.log(r));
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

  .page {
    max-width: 800px;
    background-color: var(--c-bckgr-primary);
    margin: var(--spacer);
    padding: calc(2* var(--spacer));
  }

  @media screen and (min-width: 840px) {
    .page {
      margin: var(--spacer) auto;
    }
  }


  .form {
    margin-bottom: var(--spacer);
  }


  .grid {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(150px, 450px));
    gap: var(--spacer);
    margin: 0;
    padding: var(--spacer);
  }

  .button-close {
    display: block;
    width: 100%;
  }

  @media screen and (min-width: 576px) {
    .button-close {
      display: inline-block;
      width: auto;
    }
  }

</style>