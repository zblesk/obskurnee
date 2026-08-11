<template>
  <div class="book">
    <div class="book__top">
      <div class="book__cover book__cover--stars" v-if="review.book.post.imageUrl">
        <router-link :to="{ name: 'book', params: { bookId: review.book.bookId } }" class="book__link">
          <img :src="review.book.post.imageUrl" :alt="review.book.post.title">
        </router-link>
        <star-rating class="book__stars" v-if="review.rating" :rating="review.rating" />
      </div>
      <router-link :to="{ name: 'book', params: { bookId: review.book.bookId } }" class="book__link">
        <h2 class="book__title">{{ review.book.post.author }}: {{ review.book.post.title }}</h2>
      </router-link>
      <p class="book__text" v-html="review.renderedReviewText"></p>
    </div>
  </div>
</template>

<script>
import StarRating from './StarRating.vue';
export default {
    name: "UsersReviewCard",
    components: { StarRating },
    props: ['review'],
}
</script>

<style scoped>
.book__stars {
  margin: calc(var(--spacer) / 2) 0 calc(var(--spacer) / 2) 0;
}

.book__cover--stars {
  display: flex;
  flex-direction: column;
  align-items: center;
}
</style>

