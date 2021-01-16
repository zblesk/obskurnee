<template>
<section>
  <book-preview :post="currentBook.post" style="margin: auto;" v-if="currentBook && currentBook.post"><h3>Teraz čítame: </h3></book-preview>
  <div>
  <div class="grid" v-if="books">
    <book-preview v-for="book in books" v-bind:key="book.bookId" v-bind:post="book.post">Kniha #{{book.order}}</book-preview>
  </div>
  </div>
</section>
</template>

<script>
import BookPreview from './BookPreview.vue';
import axios from 'axios';
export default {
  components: { BookPreview },
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
  margin: auto;
  padding: 4em;
}
</style>