<template>
<section>
  <div class="blc-wrapper">
    <book-large-card :post="book.post" v-if="book && book.post">
    </book-large-card>
  </div>

  <div class="form">
    <div class="form-text" v-if="!hideForm">
       <input v-model="newReview.rating" placeholder="5" />
       <input v-model="newReview.reviewText" />
    </div>
    <div class="buttons">
      <button @click="addReview" class="button-primary" v-if="!hideForm">Pridať</button>
      <button @click="toggleVisibility" class="button-secondary hide-form">Schovať formulár</button>
    </div>
  </div>

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
import { mapActions } from "vuex";

export default {
  components: { BookLargeCard },
  name: 'Book',
  data() {
      return {
        book: {},
        newReview: {},
        hideForm: false,
      }
  },
  methods: {
    ...mapActions("books", ["getBookById"]),
    toggleVisibility()
    {
      this.hideForm = !this.hideForm;
    }
  },
  computed: {
  },
  mounted() {
      this.getBookById(this.$route.params.bookId)
        .then(book => this.book = book);
  }
}
</script>