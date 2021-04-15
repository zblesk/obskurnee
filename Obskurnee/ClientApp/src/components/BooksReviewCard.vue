<template>
  <div class="book">
      <div class="book__title-wrapper">
        <!-- router link to owner -->
          <h2 class="book__title">{{ review.ownerName }}</h2>
        <!-- </router-link> -->
        <div class="book__stars" v-if="review.rating === 5">⭐⭐⭐⭐⭐</div>
        <div class="book__stars" v-if="review.rating === 4">⭐⭐⭐⭐</div>
        <div class="book__stars" v-if="review.rating === 3">⭐⭐⭐</div>
        <div class="book__stars" v-if="review.rating === 2">⭐⭐</div>
        <div class="book__stars" v-if="review.rating === 1">⭐</div>
      </div>
      <p class="book__text" v-html="review.renderedReviewText"></p>
  </div> 
</template>

<script>
export default {
    name: "UsersReviewCard",
    props: ['review'],
}
</script>

<style scoped>

.book__title-wrapper {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
}

.book__title {
  margin-top: 0;
}

.book__stars {
  margin: 0 0 0 var(--spacer);
}

</style>

