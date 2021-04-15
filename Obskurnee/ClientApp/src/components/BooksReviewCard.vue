<template>
  <div class="book">
      <div class="book__title-wrapper">
        <router-link :to="{ name: 'user', params: { email: userMailById(review.ownerId) } }" class="book__link">
          <h2 class="book__title">{{ review.ownerName }}</h2>
        </router-link>
        <div class="book__stars" v-if="review.rating">{{ "⭐".repeat(review.rating) }}</div>
      </div>
      <p class="book__text" v-html="review.renderedReviewText"></p>
  </div> 
</template>

<script>
import { mapGetters } from "vuex";
export default {
    name: "UsersReviewCard",
    props: ['review'],
    computed: 
    {
      ...mapGetters("users", ["userMailById"]),
    }
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

