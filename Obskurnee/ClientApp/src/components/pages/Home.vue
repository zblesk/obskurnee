<template>
<section>
  <book-large-card :post="currentBook.post" style="margin: auto;" v-if="currentBook && currentBook.post">
      <h5><em>Momentálne čítame knihu #{{ currentBook.order }}</em></h5>
  </book-large-card>
  <div class="grid" v-if="books && isAuthenticated">
    <book-preview v-for="book in books" v-bind:key="book.bookId" v-bind:post="book.post">Kniha #{{book.order}}</book-preview>
  </div>
</section>
</template>

<script>
import BookPreview from '../BookPreview.vue';
import axios from 'axios';
import BookLargeCard from '../BookLargeCard.vue';
import { mapGetters } from "vuex";

export default {
  components: { BookPreview, BookLargeCard },
  name: 'Home',
  data() {
      return {
        books: [],
        currentBook: {},
      }
  },
  methods: {
    getBooks() {
        axios.get('/api/books')
            .then((response) => {
              if (response.data.length)
              {
                this.books = response.data;
                this.currentBook = this.books.shift();
              }
            })
            .catch(function (error) {
                alert(error);
            });
    },
  },
  computed: {
      ...mapGetters("context", ["isAuthenticated"])
  },
  mounted() {
      this.getBooks();
  }
}
</script>

<style scoped>
.grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(100px, 220px));
  gap: 10px;
  font-family: "Raleway", sans-serif;
  padding: 4em;
}
</style>