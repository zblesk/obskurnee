<template>
<div>
  <h1 id="tableLabel">{{ discussion.title }}<small v-if="discussion.isClosed">Uzavreté</small></h1>
  <p v-html="discussion.renderedDescription"></p>
  <div class="form" v-if="!discussion.isClosed">
    <new-post :mode="discussion.topic" :discussionId="discussion.discussionId"></new-post>
    <div></div>
    <button @click="closeDiscussion" v-if="isMod && discussion.posts?.length">UZAVRI diskusiu a vytvor hlasovanie</button>
  </div>

  <div class="form" v-if="discussion.pollId">
    <router-link :to="{ name: 'poll', params: { pollId: discussion.pollId } }" class="nav-item nav-link" style="color: orange;">Choď na hlasovanie</router-link>
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

<style>
html {
  -webkit-box-sizing: border-box;
  box-sizing: border-box;
}

*,
:after,
:before {
  -webkit-box-sizing: inherit;
  box-sizing: inherit;
}

body {
  margin: 0;
  background-color: whitesmoke;
  font-family: "Raleway", sans-serif;
}

.title {
  text-align: center;
}


.form {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(100px, 400px));
  gap: 10px;
  margin: 0;
  background-color: gray;
  font-family: "Raleway", sans-serif;
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