<template>
  <div class="book">
    <div class="book__cover" v-if="recommendation.imageUrl">
      <a :href="recommendation.url" class="book__link">
        <img :src="recommendation.imageUrl" :alt="recommendation.title">
      </a>
    </div>
    <a :href="recommendation.url" class="book__link">
      <h2 class="book__title">{{recommendation.author}}: {{ recommendation.title }}</h2>
    </a>
    <p class="book__pages" v-if="recommendation.pageCount">{{ recommendation.pageCount }} strán</p>
    <p class="book__owner" v-if="showName">Doporučila {{ recommendation.ownerName }}</p>
    <p class="book__text" v-html="recommendation.renderedText"> </p>
  </div> 
</template>

<script>
export default {
    name: "RecommendationCard",
    props: {
      recommendation: {
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

