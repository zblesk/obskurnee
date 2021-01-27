<template>
<div>
  <h1 id="tableLabel">{{ discussion.title }}<small v-if="discussion.isClosed">Uzavreté</small></h1>
  <div class="form" v-if="!discussion.isClosed">
    <div>
      <span>Pridaj novú knihu!</span>
      <span style="color: cyan" v-if="fetchInProgress">Kamo pockaj, LOADUJEM</span>
    </div>
    <input v-model="newpost.url" placeholder="https://www.goodreads.com/book..." @change="linkChange"/>
    <input v-model="newpost.title" placeholder="Meno knihy" required />
    <input v-model="newpost.author" placeholder="Autor" />
    <textarea v-model="newpost.text" placeholder="Komentár k návrhu" required></textarea>
    <button @click="postNewBook">Pridaj</button>
    <img :src="newpost.imageUrl" v-if="newpost.imageUrl" />
    <div></div>
    <button @click="closeDiscussion" v-if="isMod">UZAVRI diskusiu a vytvor hlasovanie</button>
  </div>

  <div class="form" v-if="discussion.pollId">
    <router-link :to="{ name: 'poll', params: { pollId: discussion.pollId } }" class="nav-item nav-link" style="color: orange;">Choď na hlasovanie</router-link>
  </div>

  <div class="grid">
    <p v-if="!discussion.title"><em>Zatial tu nic nie je</em></p>

    <book-recommendation
      v-for="post in discussion.posts"
      v-bind:key="post.postId"
      v-bind:post="post">
    </book-recommendation>
  </div> 
</div>
</template>


<script>
import axios from "axios";
import BookRecommendation from "./BookRecommendation.vue";
import { mapGetters,mapActions, mapState } from "vuex";

export default {
  name: "Discussion",
 components: { BookRecommendation },
  data() {
    return {
      newpost: {},
      fetchInProgress: false,
    };
  },
  computed: {
    ...mapGetters("context", ["isMod"]),
    ...mapState("discussions", ["discussions"]),
    discussion: function () { return this.discussions.find(d => d.discussionId == this.$route.params.discussionId) ?? { posts: [] }; },
  },
  methods: {
    ...mapActions("discussions", ["getDiscussionData", "newPost"]),
    postNewBook() {
      console.log('qwewqeqwewqe', this.newpost);
          this.newPost({ discussionId: this.$route.params.discussionId, newPost: this.newpost })
            .then(() => {
              this.newpost = {};
            });
    },
    linkChange() {
      if (!this.newpost.url 
        || !this.newpost.url.startsWith("https://www.goodreads.com"))
      {
        console.log("Not fetching ", this.newpost.url);
        return;
      }
      this.fetchInProgress = true;
        axios.get(
          "/api/scrape/?goodreadsUrl=" + this.newpost.url)
        .then((response) => {
          this.newpost.author = response.data.author;
          this.newpost.title = response.data.name;
          this.newpost.text = "\n\nPopis na Goodreads: \n\n" + response.data.description;
          this.newpost.imageUrl = response.data.imageUrl;
          this.fetchInProgress = false;
          console.log(response.data, this.newpost);
        })
        .catch(function (error) {
          alert(error);
          this.fetchInProgress = false;
        });
    },
    closeDiscussion() 
    {
        axios.get(
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