<template>
<section>
    <ul v-if="books">
        <li v-for="book in books" v-bind:key="book.bookId">
                  #{{book.order}}  <router-link :to="{ name: 'book', params: { bookId: book.bookId } }">{{ book.post.title }} - {{ book.post.author }}</router-link>
        </li>
    </ul>
</section>
</template>


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