<template>
    <div  class="book">
      <div class="book__cover" v-if="post.imageUrl"><img :src="post.imageUrl" :alt="post.title"></div>
      <a :href="post.url" class="book__link"><h2 class="book__title">{{ post.title }}</h2></a>
      <p class="book__pages" v-if="post.pageCount"><em>{{ post.pageCount }} strán</em></p>
      <p><em>Navrhla {{ post.ownerName }}</em></p>
      <p class="book__pitch" v-html="post.renderedText"> </p>
    </div>
</template>


<script>
    export default {
        name: "BookPost",
        props: ['post']
    }
</script>


<style scoped>

*, :after, :before {
  -webkit-box-sizing: inherit;
          box-sizing: inherit;
}

body {
  margin: 0;
  background-color: whitesmoke;
  font-family: 'Raleway', sans-serif;
}

.grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(150px, 450px));
  gap: 20px;
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