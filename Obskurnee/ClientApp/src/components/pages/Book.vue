<template>
<section>
  <div class="blc-wrapper">
    <book-large-card :post="book.post" v-if="book && book.post">
    </book-large-card>
  </div>

  <div class="form">
    <div class="form-text" v-if="!hideForm">
       <input v-model="newReviewData.rating" placeholder="5" />
       <input v-model="newReviewData.reviewText" />
    </div>
    <div class="buttons">
      <button @click="addReview" class="button-primary" v-if="!hideForm">Pridať</button>
      <button @click="toggleVisibility" class="button-secondary hide-form"><span v-if="hideForm">Zobraziť</span><span v-else>Schovať</span> formulár</button>
    </div>
  </div>

  <review-card v-for="rev in bookReviews(currentBookId)" v-bind:key="rev.reviewId" v-bind:review="rev" ></review-card>
</section>
</template>

<style scoped>

  .blc-wrapper {
    max-width: 800px;
    margin: calc(var(--spacer) * 2) var(--spacer);
  }

  @media screen and (min-width: 840px) {
    .blc-wrapper {
      margin: calc(var(--spacer) * 2) auto;
    }
  }

</style>

<script>
import BookLargeCard from '../BookLargeCard.vue';
import { mapActions, mapGetters } from "vuex";
import ReviewCard from '../ReviewCard.vue';

export default {
  components: { BookLargeCard, ReviewCard },
  name: 'Book',
  data() {
      return {
        currentBookId: 0,
        book: {},
        newReviewData: {},
        hideForm: false,
        reviews: [],
      }
  },
  methods: {
    ...mapActions("books", ["getBookById"]),
    ...mapActions("reviews", ["fetchBookReviews", "newReview"]),
    toggleVisibility()
    {
      this.hideForm = !this.hideForm;
    },
    addReview()
    {
      this.newReview({ bookId: this.currentBookId, review: this.newReviewData})
      .then((data => this.reviews = data))
      .catch(e => this.$notifyError(e));
    }
  },
  computed: {
    ...mapGetters("reviews", ["bookReviews"]),
  },
  mounted() {
    this.currentBookId = this.$route.params.bookId;
    this.getBookById(this.currentBookId)
      .then(book => this.book = book);
    this.fetchBookReviews(this.currentBookId)
      .then(data => {
        this.reviews = data;
        console.log('b', data);
      })
      .catch(e => this.$notifyError(e));
  }
}
</script>