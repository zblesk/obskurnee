<template>
  <div class="book">
    <div class="book__cover" v-if="post.imageUrl">
      <a :href="post.url" class="book__link">
        <img :src="post.imageUrl" :alt="post.title">
      </a>
    </div>
    <a :href="post.url" class="book__link">
      <h2 class="book__title">{{post.author}}: {{ post.title }}</h2>
    </a>
    <p class="book__pages" v-if="post.pageCount">{{ post.pageCount }} strán</p>
    <p class="book__owner" v-if="showName">Doporučila {{ post.ownerName }}</p>
    <p class="book__text" v-html="post.renderedText"> </p>
  </div> 
</template>

<script>
export default {
    name: "RecommendationCard",
    props: {
      post: {
        type: Object,
        required: true,
      },
      showName: {
        type: Boolean,
        required: false,
        default: true,
      }
    }
}
</script>

<style scoped>


</style>

