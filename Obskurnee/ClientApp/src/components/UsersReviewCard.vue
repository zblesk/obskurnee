<template>
  <div class="book">
    <div class="book__cover" v-if="review.book.post.imageUrl">
      <router-link :to="{ name: 'book', params: { bookId: review.book.bookId } }" class="book__link">
        <img :src="review.book.post.imageUrl" :alt="review.book.post.title">
      </router-link>
      <div class="book__stars" v-if="review.rating">{{ "⭐".repeat(review.rating) }}</div>
    </div>
    <router-link :to="{ name: 'book', params: { bookId: review.book.bookId } }" class="book__link">
      <h2 class="book__title">{{ review.book.post.author }}: {{ review.book.post.title }}</h2>
    </router-link>
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
.book__stars {
  margin: calc(var(--spacer) / 2) 0 calc(var(--spacer) / 2) var(--spacer);
}
</style>

