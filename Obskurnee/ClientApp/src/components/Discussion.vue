<template>
    <h1 id="tableLabel">{{ title }}</h1>
    <div class="grid">

    <p v-if="!title"><em>Cakaj, nacitavam</em></p>

        <book-recommendation  v-for="post in posts" v-bind:key="post.id" v-bind:post="post"></book-recommendation>
    </div>
</template>


<script>
    import axios from 'axios'
    import BookRecommendation from './BookRecommendation.vue'
    
    export default {
        name: "Discussion",
        components: { BookRecommendation },
        data() {
            return {
                posts: [],
                title: ""
            }
        },
        methods: {
            getDiscussionData() {
                axios.get('/api/discussions/' + this.$route.params.id + '/posts')
                    .then((response) => {
                        this.posts =  response.data.posts;
                        this.title = response.data.discussion.title;
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            }
        },
        mounted() {
            this.getDiscussionData();
        }
    }
</script>

<style>
html {
  -webkit-box-sizing: border-box;
          box-sizing: border-box;
}

*, :after, :before {
  -webkit-box-sizing: inherit;
          box-sizing: inherit;
}


body {
  margin: 0;
  background-color: whitesmoke;
  font-family: 'Raleway', sans-serif;
}

.title {
  text-align: center;
}

.grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(150px, 450px));
  gap: 20px;
  margin: 0;
  background-color: whitesmoke;
  font-family: 'Raleway', sans-serif;
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