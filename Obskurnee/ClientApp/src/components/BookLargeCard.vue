<template>
  <div class="card">
    <div class="cover">
      <router-link :to="{ name: 'book', params: { bookId: book.bookId } }">
        <img :src="book.post.imageUrl" :alt="book.post.title" v-if="book.post.imageUrl">
      </router-link>
    </div>

    <h1 class="title">
      <router-link :to="{ name: 'book', params: { bookId: book.bookId } }">
        {{ book.post.author }}: {{ book.post.title }}
      </router-link>
      <a :href="book.post.url" :alt="book.post.title">
        <span class="goodreads-wrapper">
          <img src="../assets/goodreads_icon_32x32.png" alt="goodreads icon" class="goodreads-icon">
        </span>
      </a>
    </h1>
    <p class="summary" v-html="book.post.renderedText"></p>
  </div>
</template>

<style scoped>

  .card {
    padding: var(--spacer);
    background-color: var(--c-bckgr-primary);
    max-width: 800px;
    overflow: hidden;
  }

  .cover {
    width: 100%;
    max-width: 250px;
    margin: 0 auto var(--spacer) auto;
  }

  @media screen and (min-width: 576px) {
    .cover {
      float: left;
      margin-right: var(--spacer);
    }
  }

  .cover img {
    max-width: 100%;
  }

  .title {
    font-size: 1.5em;
    line-height: 1.5;
    font-weight: bold;
    text-align: center;
    margin-top: var(--spacer);
    margin-bottom: var(--spacer);
  }

  .title a {
    text-decoration: none;
  }

  @media screen and (min-width: 576px) {
    .title {
      margin-top: 0;
      text-align: left;
    }
  }

  .summary {
    font-size: 1em;
  }

  .goodreads-wrapper {
    display: inline-block;
    width: 1em;
    height: 1em;
    margin-left: 0.5em;
  }

  .goodreads-icon {
    max-width: 100%;
    vertical-align: baseline;
  }

</style>


<script>
    export default {
        name: "BookLargeCard",
        props: ['book'],
    }
</script>