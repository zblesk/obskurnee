<template>
  <div class="book-card">
    <router-link :to="{ name: 'book', params: { bookId: book.bookId } }">
      <div class="book-cover">
        <img :src="book.post.imageUrl" :alt="book.post.title" v-if="book.post.imageUrl">
      </div>
      <div class="book-description">
        <h2 class="book-order" v-if="this.$slots.default"><slot></slot></h2>
        <p class="book-text">{{ book.post.author }}: {{ book.post.title }}</p>
      </div>
    </router-link>
  </div> 
</template>

<script>
export default {
    name: "BookPreview",
    props: ['book'],
}
</script>

<style scoped>

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

