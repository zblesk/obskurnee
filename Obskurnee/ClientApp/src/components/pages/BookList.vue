<template>
<section>
    <h1 class="page-title">Knihy</h1>
    <div class="books-grid" v-if="books">
        <book-preview v-bind:post="book.post" v-for="book in books" v-bind:key="book.bookId">Kniha #{{ book.order }}</book-preview>
    </div>
</section>
</template>

<style scoped>

    .books-grid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
        gap: var(--spacer);
        padding: 0 var(--spacer);
    }

</style>


<script>
import { mapActions, mapState } from "vuex";
import BookPreview from '../BookPreview.vue';
export default {
  components: { BookPreview },
name: "BookList",
computed: {
    ...mapState("books", ["books"]),
},
methods: {
...mapActions("books", ["fetchBookList"]),
},
mounted() {
    this.fetchBookList();
}
}
</script>