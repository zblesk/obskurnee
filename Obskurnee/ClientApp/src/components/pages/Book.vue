<template>
<section>
  <book-large-card :post="book.post" v-if="book && book.post">
  </book-large-card>

    <h2 class="todo-l">sem este recenzie</h2>

</section>
</template>

<style scoped>

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