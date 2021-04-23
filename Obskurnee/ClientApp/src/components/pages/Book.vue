<template>
<section>
  <div class="blc-wrapper">
    <book-large-card :book="book" v-if="book && book.post">
    </book-large-card>
  </div>

  <div class="main">
    <div class="form-field" v-if="!hideForm">
      <label for="review-stars">Počet hvězdiček (1–5)</label>
      <input type="number" min="1" max="5" v-model="newReviewData.rating" id="review-stars" required />
    </div>
    <div class="form-field" v-if="!hideForm">
      <div class="label-md-wrapper">
        <label for="review-text">Text review</label>
        <div class="mo-md">
          <div class="mo-md-pic">
            <img src="../../assets/Markdown-mark.svg" alt="markdown logo">
          </div>
          <div class="mo-md-link">
            <markdown-help-link></markdown-help-link>
          </div>
        </div>
      </div>
      <textarea id="review-text" v-model="newReviewData.reviewText" required placeholder="Mozes pouzit Markdown na jednoduche formatovanie textu. Ak by si sa stratila, klikni na link hore.

Medzi zaklady patri napriklad: 
# Najvacsi nadpis
## mensi nadpis 
**tucny textt** alebo _kurziva_ 
- necislovany zoznam
1. cislovany zoznam
> takto sa pise citat
Mozes lahko pridat aj [link](https://google.sk)."></textarea>
    </div>
    <div class="buttons" v-if="hideForm">
      <button @click="toggleVisibility" class="button-primary">Pridať recenziu</button>
    </div>
    <div class="buttons" v-if="!hideForm">
      <button @click="addReview" class="button-primary">Pridať</button>
      <button @click="toggleVisibility" class="button-secondary button-hide">Skryť</button>
    </div>
  </div>

  <div class="grid">
    <books-review-card v-for="rev in bookReviews(currentBookId)" v-bind:key="rev.reviewId" v-bind:review="rev" ></books-review-card>
  </div>
</section>
</template>

<style scoped>

  .blc-wrapper {
    max-width: 800px;
    margin: calc(var(--spacer) * 2) var(--spacer) var(--spacer) var(--spacer);
  }

  @media screen and (min-width: 840px) {
    .blc-wrapper {
      margin: calc(var(--spacer) * 2) auto var(--spacer) auto;
    }
  }

  .form-field input {
    max-width: 380px;
  }

  .button-hide {
    margin-top: var(--spacer);
  }

  @media screen and (min-width: 576px) {
  .button-hide {
    margin-top: 0;
    margin-left: var(--spacer);
  }
}

</style>

<script>
import BookLargeCard from '../BookLargeCard.vue';
import { mapActions, mapGetters } from "vuex";
import BooksReviewCard from '../BooksReviewCard.vue';
import MarkdownHelpLink from '../MarkdownHelpLink.vue';

export default {
  components: { BookLargeCard, BooksReviewCard, MarkdownHelpLink },
  name: 'Book',
  data() {
      return {
        currentBookId: 0,
        book: {},
        newReviewData: {},
        hideForm: true,
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
      if (this.newReviewData?.rating > 5 || this.newReviewData?.rating < 0)
      {
        this.$notifyError("Prepáč, hodnotenie musí byť 0-5.");
        return;
      }
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
      .catch(e => this.$notifyError(e));
  }
}
</script>