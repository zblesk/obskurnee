<template>
<section>
    <h1 class="page-title">{{$t('menus.books')}}</h1>
    <div class="books-grid" v-if="books">
        <book-preview v-bind:book="book" v-for="book in books" v-bind:key="book.bookId">#{{ book.order }} </book-preview>
    </div>
</section>
</template>

<style scoped>

    .books-grid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
        gap: var(--spacer);
        padding: var(--spacer);
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