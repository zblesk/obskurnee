<template>
  <div class="book">
    <div class="book__top">
      <div class="book__title-wrapper">
        <h2 class="book__title"><user-link :userName="review.ownerName" :userId="review.ownerId" class="book__link" /></h2>
        <star-rating class="book__stars" v-if="review.rating" :rating="review.rating" />
      </div>
      <p class="book__text" v-html="review.renderedReviewText"></p>
    </div>
  </div> 
</template>

<script>
import { mapGetters } from "vuex";
import UserLink from './UserLink.vue';
import StarRating from './StarRating.vue';
export default {
  components: { UserLink, StarRating },
    name: "BooksReviewCard",
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

