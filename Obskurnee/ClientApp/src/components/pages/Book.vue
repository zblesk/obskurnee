<template>
<section>
  <div class="blc-wrapper">
    <book-large-card :post="book.post" v-if="book && book.post">
    </book-large-card>
  </div>

    <h2 class="todo-l">sem este recenzie</h2>

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
      }
  },
  methods: {
    ...mapActions("books", ["getBookById"]),
  },
  computed: {
  },
  mounted() {
      this.getBookById(this.$route.params.bookId)
        .then(book => this.book = book);
  }
}
</script>