<template>
<div>
  <h1 id="tableLabel" class="page-title">{{ discussion.title }}<span class="disc-closed" v-if="discussion.isClosed">&nbsp;Uzavreté</span></h1>
  <div class="page">
    <p v-html="discussion.renderedDescription" class="disc-desc"></p>
    <div class="form" v-if="!discussion.isClosed">
      <new-post :mode="discussion.topic" :discussionId="discussion.discussionId"></new-post>
      <div></div>
    </div>
    <button @click="closeDiscussion" v-if="isMod && discussion.posts?.length" class="button-primary">Uzavri diskusiu a vytvor hlasovanie</button>

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
    ...mapActions("discussions", ["getDiscussionData"]),
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
    }
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
  gap: 20px;
  margin: 0;
  background-color: whitesmoke;
  font-family: "Raleway", sans-serif;
}

.book {
  padding: 15px;
  background-color: white;
}

.book__cover {
  width: 100%;
  max-width: 150px;
  margin: 0 auto;
}

@media screen and (min-width: 400px) {
  .book__cover {
    float: left;
    margin: 0 15px 15px 0;
  }
}

.book__cover img {
  width: 150px;
  height: auto;
}

.book__link {
  text-decoration: none;
}

.book__title {
  font-size: 20px;
}

.book__pitch,
.book__pages {
  line-height: 1.5;
}

.book__grtitle {
  font-size: 16px;
}

.book__grblurb {
  font-size: 14px;
  line-height: 1.5;
}

.book__quote {
  font-size: 14px;
  font-style: italic;
}

#HCB_comment_box {
  max-width: 800px;
  width: 100%;
  margin: 30px auto;
  padding-left: 30px;
  padding-right: 30px;
}
</style>