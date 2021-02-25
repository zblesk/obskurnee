<template>
 <div class="card">
  <div class="container">
    <div class="book__cover" v-if="post.imageUrl"><img :src="post.imageUrl" :alt="post.title"></div>
    <a :href="post.url" class="book__link"><h2 class="book__title">{{ post.title }}</h2></a>
    <p class="book__pages" v-if="post.pageCount"><em>{{ post.pageCount }} strán</em></p>
    <p><em>Navrhla {{ post.ownerName }}</em></p>
    <p class="book__pitch" v-html="post.renderedText"> </p>
  </div>
</div> 
</template>

<script>
export default {
    name: "RecommendationCard",
    props: ['post'],
}
</script>

<style scoped>
.card {
  box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
  transition: 0.3s;
  max-width:  220px;
  padding: 0 1ex 0 1ex;
}

.card:hover {
  box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);
}

.container {
  padding: 2px 16px;
}
</style>

