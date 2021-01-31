<template>
<section>
  <book-large-card :post="book.post" v-if="book && book.post" style="margin: auto;">
  </book-large-card>
  <div>
    <h2>sem este recenzie</h2>
  </div>
</section>
</template>

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
    ...mapActions("books", ["getBookByOrder"]),
  },
  computed: {
  },
  mounted() {
      this.getBookByOrder(this.$route.params.bookOrder)
        .then(book => this.book = book);
  }
}
</script>