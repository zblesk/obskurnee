<template>
<section>
    <h1 class="page-title">Knihy</h1>
    <div class="books-grid" v-if="books">
        <book-preview v-bind:post="book.post" v-for="book in books" v-bind:key="book.bookId">Kniha {{ book.order }}</book-preview>
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

    .book-card {
        background-color: var(--c-bckgr-primary);
        padding: var(--spacer);
    }

    .book-cover img {
        max-width: 100%;
        width: auto;
        height: auto;
        max-height: 250px;
        display: block;
        margin: 0 auto;
    }

    .book-order {
        font-weight: bold;
        margin-top: var(--spacer);
        margin-bottom: var(--spacer);
    }

    .book-order,
    .book-text {
        font-size: 1em;
        text-align: center;
    }

    .book-card a {
        text-decoration: none;
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