<template>
  <div class="book-card">
    <router-link :to="{ name: 'book', params: { bookId: book.bookId } }">
      <div class="book-cover">
        <img :src="book.post.imageUrl" :alt="book.post.title" v-if="book.post.imageUrl">
      </div>
      <div class="book-description">
        <h2 class="book-title"><span v-if="$slots.default"><slot></slot></span> <span class="book-link">{{ book.post.title }}</span></h2>
        <p class="book-author">{{ book.post.author }}</p>
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

  .book-title {
    font-size: 1em;
    font-weight: bold;
    text-align: center;
    margin-top: var(--spacer);
    margin-bottom: calc(var(--spacer) / 2);
  }

  .book-author {
    font-size: 0.875em;
    text-align: center;
  }

  .book-card a {
    text-decoration: none;
  }

  .book-link {
    color: var(--c-accent);
  }

</style>

