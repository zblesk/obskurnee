<template>
<section>
    <h1 class="page-title">Přečtené knihy</h1>
    <div class="books-grid" v-if="books">
        <div class="book-card" v-for="book in books" v-bind:key="book.bookId">
            <router-link :to="{ name: 'book', params: { bookId: book.bookId } }">
                <div class="book-cover">
                    <img :src="book.post.imageUrl" :alt="book.post.title" v-if="book.post.imageUrl">
                </div>
                <div class="book-description">
                    <h2 class="book-order">Kniha #{{book.order}}</h2>
                    <p class="book-text">{{ book.post.author }}: {{ book.post.title }}</p>
                </div>
            </router-link>
        </div>
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
export default {
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